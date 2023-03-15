using System.Runtime.CompilerServices;
using DDL;
using DDL.Resources.Interface;

[assembly: InternalsVisibleTo("UI")]

namespace BL;


internal class BysLogic
{

    public BysLogic()
    {
        mode = Mode.Viewer;
    }

    public BysLogic(ICreateDB _createDB)
    {
        createDB = _createDB;
        mode = Mode.CreateDataBase;
    }

    public BysLogic(IDataDefinitionDictionary _updateDB)
    {
        updateDB = _updateDB;
        mode = Mode.CreateDictionary;
    }

    enum Mode
    {
        CreateDataBase,
        CreateDictionary,
        Viewer
    }
    private readonly Mode mode;
    private readonly ICreateDB createDB = null!;

    private readonly IDataDefinitionDictionary updateDB = null!;

    internal async ValueTask<int> CreateNewDB()
    {
        var retval = -1;

        if(mode == Mode.CreateDataBase)
        {
            retval = 1;

            if(await createDB.CreateDB() == 0)
            {
                retval = 0;
            }  
              
        }

        return retval;
    }
    internal async ValueTask<int> CreateNewDictionary(string nameSP, string nameDictionary)
    {
        var retval = -1;

        if((mode == Mode.CreateDictionary) && (nameSP != null) && (nameDictionary != null))
        {
            retval = 1;

            if(await updateDB.CreateDictionary(nameSP, nameDictionary) == 0)
            {
                retval = 0;
            }  
              
        }

        return retval;
    }
    
}
