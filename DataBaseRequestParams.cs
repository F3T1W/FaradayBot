namespace FaradayBot
{
    public readonly struct DataBaseRequestParams
    {
        public readonly string databasePath;
        public readonly string tableName;
        public readonly string fieldName;
        public readonly string orderByField;

        public DataBaseRequestParams(string _databasePath, string _tableName, string _fieldName, string _orderByField)
        {
            databasePath = _databasePath;
            tableName = _tableName;
            fieldName = _fieldName;
            orderByField = _orderByField;
        }
    }
}
