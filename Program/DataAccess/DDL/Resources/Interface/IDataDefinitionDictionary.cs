namespace DDL.Resources.Interface
{
    public interface IDataDefinitionDictionary
    {
        public ValueTask<int> CreateDictionary(string name);

        public ValueTask<int> RefreshDictionary(string name, int quantityWords);

        public int DeleteDictionary(string name);

        public string NameSpCreateDictionary {get; set; }
    }
}