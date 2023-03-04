
namespace DDL.Resources.Interface
{
    public interface IDataDefinitionDictionary
    {
        public int CreateDictionary(string name);

        public int RefreshDictionary(string name);

        public int DeleteDictionary(string name);
    }
}