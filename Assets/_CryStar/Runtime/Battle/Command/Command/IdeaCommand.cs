using CryStar.CommandBattle.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CryStar.CommandBattle.Command
{
    /// <summary>
    /// アイデアコマンド
    /// </summary>
    public class IdeaCommand : IBattleCommand
    {
        // 優先順は特に設定なし
        public int Priority => 50;
        public string DisplayName => "アイデア";
        
        public bool CanExecute(BattleUnitData executor)
        {
            // 使用者が生きていれば実行可能
            return executor.IsAlive;
        }
        
        public async UniTask<BattleCommandResultData> ExecuteAsync(BattleUnitData executor, BattleUnitData[] targets)
        {
            if (targets.Length == 0)
            {
                // 敵がいない場合はコマンド失敗としてリザルトを作成
                return new BattleCommandResultData(false, "対象が存在しません");
            }
            
            // 単体攻撃
            var target = targets[0];
            
            // ダメージ計算を行う
            int damage = CalculateDamage(executor, target);
            
            // クリティカル判定を行う
            bool isCritical = CheckCritical(executor.UserData.CriticalRate);
            if (isCritical)
            {
                // NOTE: クリティカルダメージは100%といった単位で渡されるので、0.01fをかける
                damage = Mathf.RoundToInt(damage * executor.UserData.CriticalDamage * 0.01f);
            }

            // 演出を実行する
            await PlayAttackEffectAsync(target, isCritical);
            
            // ダメージ適用
            target.TakeDamage(damage);
            
            // エフェクトデータ作成
            var effects = new BattleCommandEffectData[]
            {
                new BattleCommandEffectData
                {
                    Target = target,
                    Damage = damage,
                    EffectName = "Idea",
                    IsCritical = isCritical
                }
            };
            
            // ログに表示するメッセージを作成
            string message = isCritical ? 
                $"会心の一撃！{target.Name}に{damage}のダメージ！" :
                $"{target.Name}に{damage}のダメージ！";
            
            return new BattleCommandResultData(true, message, effects);
        }
        
        /// <summary>
        /// ダメージ計算を行う
        /// </summary>
        private int CalculateDamage(BattleUnitData attacker, BattleUnitData defender)
        {
            // 攻撃力 * 知力 // TODO: 更にスキルごとの倍率をかける
            int baseDamage = (int)(attacker.Attack * ((100 + attacker.SkillMultiplier) * 0.01f));
            // 実効防御力: ディフェンダーの物理防御 × (1 - アタッカーの防御無視率)
            int defense = (int)(defender.Defense * (1 - attacker.ArmorPenetration / 100f));
            // 最終物理ダメージ = 物理攻撃 × (100 / (100 + 実効防御力))
            int damage = Mathf.Max(1, (int)(baseDamage * (100f / (100f + defense))));
            
            // ランダム要素を追加（±10%）
            float randomFactor = Random.Range(0.9f, 1.1f);
            damage = Mathf.RoundToInt(damage * randomFactor);
            
            return damage;
        }
        
        /// <summary>
        /// クリティカル攻撃か抽選を行う
        /// </summary>
        private bool CheckCritical(float criticalLate)
        {
            return Random.Range(0f, 1f) < criticalLate;
        }
        
        /// <summary>
        /// 攻撃演出を実行する
        /// </summary>
        private async UniTask PlayAttackEffectAsync(BattleUnitData target, bool isCritical)
        {
            // 現在は最小限の待機のみ
            // TODO: 将来的にエフェクト演出、アニメーション、サウンドなどを追加
            await UniTask.DelayFrame(1);
        }
    }
}