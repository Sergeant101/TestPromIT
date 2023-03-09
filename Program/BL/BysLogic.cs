using System.Runtime.CompilerServices;
using DDL;
using DDL.Resources.Interface;

[assembly: InternalsVisibleTo("UI")]

namespace BL;


internal class BysLogic
{

    public BysLogic()
    {
        mode = "v";
    }

    public BysLogic(ICreateDB _createDB)
    {
        createDB = _createDB;
        mode = "c";
    }

    public BysLogic(IDataDefinitionDictionary _updateDB)
    {
        updateDB = _updateDB;
        mode = "d";
    }

    private readonly string mode;
    private readonly ICreateDB createDB;

    private readonly IDataDefinitionDictionary updateDB;

    internal async ValueTask<int> CreateNewDB()
    {
        var retval = -1;

        if(mode == "c")
        {
            retval = 1;

            if(await createDB.CreateDB() == 0)
            {
                retval = 0;
            }  
              
        }

        return retval;
    }


    internal async ValueTask<int> CreateNewDictionary(string nameDictionary)
    {
        var retval = -1;

        if(mode == "d")
        {
            retval = 1;

            if(await updateDB.CreateDictionary(nameDictionary) == 0)
            {
                retval = 0;
            }  
              
        }

        return retval;
    }
    
}
