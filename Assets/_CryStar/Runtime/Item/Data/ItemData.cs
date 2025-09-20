using CryStar.Item.Enums;

namespace CryStar.Item.Data
{
    /// <summary>
    /// アイテムのデータクラス
    /// </summary>
    public class ItemData
    {
        #region Private Fields
        
        private int _id;
        private string _name;
        private string _description;
        private RarityType _rarity;
        private string _iconPath;
        private int _maxStackCount;
        private int _sortOrder;
        private int _sellPrice;
        private int _categoryId;
        private int _subCategoryId;
        private bool _useInBattle;
        private bool _useInField;

        #endregion
        
        /// <summary>
        /// アイテムID
        /// </summary>
        public int Id => _id;
        
        /// <summary>
        /// アイテム名
        /// </summary>
        public string Name => _name;
        
        /// <summary>
        /// アイテムの説明文
        /// </summary>
        public string Description => _description;
        
        /// <summary>
        /// レアリティ
        /// </summary>
        public RarityType Rarity => _rarity;
        
        /// <summary>
        /// アイコン画像のパス
        /// </summary>
        public string IconPath => _iconPath;
        
        /// <summary>
        /// 最大スタック数
        /// </summary>
        public int MaxStackCount => _maxStackCount;
        
        /// <summary>
        /// ソート用の値
        /// </summary>
        public int SortOrder => _sortOrder;
        
        /// <summary>
        /// 売却価格
        /// </summary>
        public int SellPrice => _sellPrice;
        
        /// <summary>
        /// このアイテムが属しているカテゴリのID
        /// </summary>
        public int CategoryId => _categoryId;
        
        /// <summary>
        /// このアイテムが属しているサブカテゴリのID
        /// </summary>
        public int SubCategoryId => _subCategoryId;
        
        /// <summary>
        /// 戦闘中に使用可能か
        /// </summary>
        public bool UseInBattle => _useInBattle;
        
        /// <summary>
        /// フィールドで使用可能か
        /// </summary>
        public bool UseInField => _useInField;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ItemData(int id, string name, string description, RarityType rarity, string iconPath,
            int maxStackCount, int sortOrder, int sellPrice, int categoryId, int subCategoryId, 
            bool useInBattle, bool useInField)
        {
            _id = id;
            _name = name;
            _description = description;
            _rarity = rarity;
            _iconPath = iconPath;
            _maxStackCount = maxStackCount;
            _sortOrder = sortOrder;
            _sellPrice = sellPrice;
            _categoryId = categoryId;
            _subCategoryId = subCategoryId;
            _useInBattle = useInBattle;
            _useInField = useInField;
        }
    }
}