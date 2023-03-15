namespace DDL.Resources.Interface
{
    public interface IDataDefinitionDictionary
    {
        public ValueTask<int> CreateDictionary(string nameSP, string NameDictionary);

        public ValueTask<int> RefreshDictionary(string nameDictionary, string name, int quantityWords);

        public int DeleteDictionary(string name);
    }
}