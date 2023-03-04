using DDL.Resources.Interface;
using Microsoft.Data.SqlClient;


namespace DDL;
public class MsSqlDdl
{
    internal class DefinitionDictionary : IDataDefinitionDictionary
    {

        public DefinitionDictionary(string nameDictionary)
        {
            throw new NotImplementedException();
        }
        public int CreateDictionary(string name)
        {
            throw new NotImplementedException();
        }

        public int DeleteDictionary(string name)
        {
            throw new NotImplementedException();
        }

        public int RefreshDictionary(string name)
        {
            throw new NotImplementedException();
        }
    }
}
