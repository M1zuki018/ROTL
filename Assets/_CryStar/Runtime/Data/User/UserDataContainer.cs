namespace CryStar.Data.User
{
    /// <summary>
    /// 実行中利用するユーザーデータをまとめたコンテナ
    /// </summary>
    public class UserDataContainer
    {
        private readonly FieldUserData _fieldUserData;
        private readonly StoryUserData _storyUserData;
        private readonly CharacterUserData _characterUserData;
        private readonly GameEventUserData _gameEventUserData;
        private readonly InventoryUserData _inventoryUserData;
    
        /// <summary>
        /// フィールドのユーザーデータ
        /// </summary>
        public FieldUserData FieldUserData => _fieldUserData;
        
        /// <summary>
        /// ストーリーのユーザーデータ
        /// </summary>
        public StoryUserData StoryUserData => _storyUserData;
        
        /// <summary>
        /// キャラクターのパラメーターのユーザーデータ
        /// </summary>
        public CharacterUserData CharacterUserData => _characterUserData;
        
        /// <summary>
        /// ゲームイベントのユーザーデータ
        /// </summary>
        public GameEventUserData GameEventUserData => _gameEventUserData;
        
        /// <summary>
        /// アイテムインベントリのユーザーデータ
        /// </summary>
        public InventoryUserData InventoryUserData => _inventoryUserData;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserDataContainer(int userId)
        {
            _fieldUserData = new FieldUserData(userId);
            _storyUserData = new StoryUserData(userId);
            _characterUserData = new CharacterUserData(userId);
            _gameEventUserData = new GameEventUserData(userId);
            _inventoryUserData = new InventoryUserData(userId);
        }

        /// <summary>
        /// 全データの保存時刻を更新
        /// </summary>
        public void UpdateAllSaveTimes()
        {
            _fieldUserData?.UpdateSaveTime();
            _storyUserData?.UpdateSaveTime();
            _characterUserData?.UpdateSaveTime();
            _gameEventUserData?.UpdateSaveTime();
            _inventoryUserData?.UpdateSaveTime();
        }
    }
}