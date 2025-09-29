using System.Collections.Generic;
using System.Linq;
using CryStar.CommandBattle.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace CryStar.CommandBattle.UI
{
    /// <summary>
    /// バトルのベースとなるUIを管理するクラス
    /// </summary>
    public class BattleView : MonoBehaviour
    {
        /// <summary>
        /// キャラクターアイコンのPrefab
        /// TODO: 管理方法は検討中
        /// </summary>
        [SerializeField]
        private CharacterIconContents _unitIconPrefab;
        
        [SerializeField]
        private Transform _unitIconParent;

        /// <summary>
        /// エネミーアイコンのPrefab
        /// </summary>
        [SerializeField] 
        private EnemyIconContents _enemyIconPrefab;

        [SerializeField] 
        private Transform _enemyIconParent;
        
        /// <summary>
        /// コマンドアイコンのPrefab
        /// </summary>
        [SerializeField]
        private CommandIcon _commandIconPrefab;
        
        [SerializeField]
        private Transform _commandIconParent;
        
        /// <summary>
        /// ダメージテキストのオブジェクトプールを管理するクラス
        /// </summary>
        [SerializeField]
        private DamageTextPool _damageTextPool;
        
        /// <summary>
        /// 行動ログを管理するクラス
        /// </summary>
        [SerializeField]
        private BattleLogContents _logContents;
        
        /// <summary>
        /// 表示中のアイコンのリスト
        /// </summary>
        private List<CommandIcon> _activeIcons = new List<CommandIcon>();
        
        private List<CharacterIconContents> _icons;
        private List<EnemyIconContents> _enemyIcons;
        private IObjectPool<CommandIcon> _commandIconPool;

        #region Life cycle

        private void Start()
        {
            // コマンドのアイコン表示のセットアップを行う
            SetupCommandIconPool();
        }

        #endregion
        
        /// <summary>
        /// キャラクターのアイコンを設定する
        /// </summary>
        public async UniTask SetupIcons(IReadOnlyList<BattleUnitData> unitData, IReadOnlyList<BattleUnitData> enemyData)
        {
            // リストを新規作成
            _icons = new List<CharacterIconContents>(unitData.Count);
            
            for (int i = 0; i < unitData.Count; i++)
            {
                var icon = Instantiate(_unitIconPrefab, _unitIconParent);
                _icons.Add(icon);
                icon.Setup(_damageTextPool, unitData[i]);
                await icon.SetIcon(unitData[i].UserData.IconPath);
            }
            
            SubscribeToUnitEvents(unitData);

            _enemyIcons = new List<EnemyIconContents>(enemyData.Count);
            
            for (int i = 0; i < enemyData.Count; i++)
            {
                var icon = Instantiate(_enemyIconPrefab, _enemyIconParent);
                _enemyIcons.Add(icon);
                icon.Setup(_damageTextPool);
                await icon.SetIcon(enemyData[i].UserData.IconPath);
            }
            
            SubscribeToEnemyEvents(enemyData);
        }

        /// <summary>
        /// コマンドアイコンを生成する
        /// </summary>
        public async UniTask AddCommandIcon(BattleCommandEntryData commandData)
        {
            var icon = _commandIconPool.Get();
            _activeIcons.Add(icon);
            
            //TODO: コマンドデータが持っている情報から適切なPathを渡す
            await icon.OnGet(commandData);
        }

        /// <summary>
        /// キャラクターアイコンを表示する
        /// </summary>
        public void IsActiveCharacterIcon(bool isActive)
        {
            _unitIconParent.gameObject.SetActive(isActive);
        }
        
        /// <summary>
        /// コマンドアイコンを実行順に並び変える
        /// </summary>
        public void SortingCommandIcons()
        {
            // アクティブなアイコンを優先度順にソート
            _activeIcons = _activeIcons
                .OrderBy(icon => icon.EntryData.Priority)
                .ThenByDescending(icon => icon.EntryData.Executor.Speed)
                .ToList();
    
            // Transform内の順序を更新
            for (int i = 0; i < _activeIcons.Count; i++)
            {
                _activeIcons[i].transform.SetSiblingIndex(i);
            }
        }

        /// <summary>
        /// リストから削除する
        /// </summary>
        public async UniTask RemoveCommandIcon(BattleCommandEntryData commandData)
        {
            var icon = _activeIcons.FirstOrDefault(icon => icon.EntryData.Equals(commandData));
            if (icon != null)
            {
                _activeIcons.Remove(icon);
                _commandIconPool.Release(icon);
            }
        }

        /// <summary>
        /// 全てのアイコン表示をクリア
        /// </summary>
        public async UniTask AllRemoveCommandIcons()
        {
            foreach (var icon in _activeIcons)
            {
                // 全てをオブジェクトプールに返却
                _commandIconPool.Release(icon);
            }
            _activeIcons.Clear();
        }

        /// <summary>
        /// ログを設定する
        /// </summary>
        public void SetLog(string logMessage)
        {
            if (_logContents != null)
            {
                _logContents.SetLog(logMessage);
            }
        }

        /// <summary>
        /// ログの表示をリセットする
        /// </summary>
        public void ResetLogs()
        {
            if (_logContents != null)
            {
                _logContents.Reset();
            }
        }

        #region Private Methods
        
        /// <summary>
        /// キャラクターのHP・SP変動アクションを購読する
        /// </summary>
        private void SubscribeToUnitEvents(IReadOnlyList<BattleUnitData> unitData)
        {
            // 味方ユニットのイベント購読
            for (int i = 0; i < unitData.Count; i++)
            {
                var unit = unitData[i];
                var index = i; // ローカル変数でキャプチャ
                
                unit.OnHpChanged += (currentHp, maxHp, damage) => UpdatePlayer(index, currentHp, maxHp, damage);
                //unit.OnSpChanged += (newSp) => UpdatePlayer(index);
                unit.OnDeath += () => DisEnableIcon(index, true);
            }
        }
        
        /// <summary>
        /// エネミーのHP変動アクションを購読する
        /// </summary>
        private void SubscribeToEnemyEvents(IReadOnlyList<BattleUnitData> enemyData)
        {
            // エネミーのイベント購読
            for (int i = 0; i < enemyData.Count; i++)
            {
                var unit = enemyData[i];
                var index = i; // ローカル変数でキャプチャ
                
                unit.OnHpChanged += (currentHp, maxHp, damage) => UpdateEnemy(index, damage);
                unit.OnDeath += () => DisEnableIcon(index, false);
            }
        }

        /// <summary>
        /// ダメージを受けた時のUI更新
        /// </summary>
        private void UpdatePlayer(int index, int value, int maxValue, int damage)
        {
            // HPバーを更新
            _icons[index].SetHpSlider(value, maxValue);
            
            // ダメージ量の表記を行う
            _icons[index].SetDamageText(damage).Forget();
        }

        /// <summary>
        /// 敵がダメージを受けた時のUI更新
        /// </summary>
        private void UpdateEnemy(int index, int damage)
        {
            _enemyIcons[index].SetDamageText(damage).Forget();
        }

        /// <summary>
        /// 死亡時にアイコンを非表示にする
        /// </summary>
        private void DisEnableIcon(int index, bool isPlayer)
        {
            // TODO: コマンドアイコンを非表示にしたりもする
            
            if (isPlayer)
            {
                _icons[index].Hide();
            }
            else
            {
                _enemyIcons[index].Hide();
            }
        }
        
        #endregion

        #region Object Pool

        /// <summary>
        /// コマンドのアイコン表示のセットアップ
        /// </summary>
        private void SetupCommandIconPool()
        {
            _commandIconPool = new ObjectPool<CommandIcon>(
                createFunc: InstantiateCommandIcon,
                actionOnGet: icon => GetCommandIcon(icon),
                actionOnRelease: ReleaseCommandIcon,
                actionOnDestroy: DestroyCommandIcon,
                defaultCapacity: 5,
                maxSize: 20);
        }

        /// <summary>
        /// 生成
        /// </summary>
        private CommandIcon InstantiateCommandIcon()
        {
            // 親を指定して生成
            return Instantiate(_commandIconPrefab, _commandIconParent);
        }

        private CommandIcon GetCommandIcon(CommandIcon icon)
        {
            return icon;
        }

        /// <summary>
        /// Release
        /// </summary>
        private void ReleaseCommandIcon(CommandIcon icon)
        {
            icon.OnRelease();
        }
        
        private void DestroyCommandIcon(CommandIcon obj)
        {
            Destroy(obj);
        }
        
        #endregion
    }
}
