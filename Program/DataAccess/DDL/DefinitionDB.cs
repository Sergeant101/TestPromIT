using DDL.Resources.Interface;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using static System.Console;

[assembly: InternalsVisibleTo("UI")]
[assembly: InternalsVisibleTo("BL")]
[assembly: InternalsVisibleTo("DDL.Test")]
[assembly: InternalsVisibleTo("DefinitionDB.Test")]

namespace DDL
{
    internal partial class DefinitionDB: ICreateDB 
    {
        public DefinitionDB(string _name)
        {
            NameDB = _name;
            connection += SourceConnection + StartConnection;
            ExistsConnection += SourceConnection + ";database=" + NameDB + ";";            

            //Debug Delete
            WriteLine(connection);
            WriteLine(ExistsConnection);
        }

        public DefinitionDB(string _path, string _name):this(_name)
        {
            PathDB = _path;
        }

        private readonly string PathDB = "C:\\Temp\\";
        private readonly string NameDB;
        public string GetName {get => NameDB; }
        private readonly string connection = null!;
        private readonly string SourceConnection = @"Data Source=.\SQLEXPRESS;Integrated security=True;TrustServerCertificate=true;MultipleActiveResultSets=True";
        private readonly string StartConnection = ";database=master;";
        private readonly string ExistsConnection = null!;
        
        public async ValueTask<int> CreateDB()
        {
            var retval = 1;

            try
            {
                using(SqlConnection ConnCreateDB = new SqlConnection(connection))
                {
                    SqlCommand create = new SqlCommand(GetConnectionString(PathDB, NameDB), ConnCreateDB);


                    ConnCreateDB.Open();
                    create.ExecuteNonQuery();
                    ConnCreateDB.Close();

                    retval = 0;
                }
                
                await SpCreate(spCreateRoot);
                await SpCreate(spAddTable);
                await SpCreate(spSelectWord);

            }

            catch(Microsoft.Data.SqlClient.SqlException ex)
            {          
                switch (ex.Number)
                {
                    case  102: //Broken command SQL string
                        retval = 5;
                        break;
                    case 1801: //Database allready exists
                        retval = 4;
                        break;
                    case 5133: //Wrong path to DB
                        retval = 6;
                        break; 
                    default:
                        retval = 3;
                        break;
                }                    
            }

            catch
            {
                retval = 2;
            }

        return retval;
        
        }

        internal int DeleteDB()
        {
            var retval = 1;

                try
                {
                    using(SqlConnection ConnCreateDB = new SqlConnection(connection))
                    {
                        var sqlCommandText  = $"DROP DATABASE {NameDB}"; 
                        var sqlCommand      = new SqlCommand(sqlCommandText, ConnCreateDB);

                        ConnCreateDB.Open();
                        sqlCommand.ExecuteNonQuery();
                        ConnCreateDB.Close();

                        retval = 0;
                    }
                }
                catch
                {
                    retval = 2;
                }


            return retval;
        }

        private string GetConnectionString(string path, string name)
        {
            var lit1 = String.Concat("CREATE DATABASE ", name, " ON PRIMARY ");
            var lit2 = String.Concat("(NAME = ", name, "_Data, ");
            var lit3 = String.Concat("FILENAME = '", path, name, "Data.mdf', ");
            var lit4 = String.Concat("SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%) ",  "LOG ON (NAME = ", name, "_Log, " );
            var lit5 = String.Concat("FILENAME = '", PathDB, name, "Log.ldf', ", "SIZE = 1MB, MAXSIZE = 5MB, FILEGROWTH = 10%)");

            var paramEnvironment        = String.Concat(lit1, lit2, lit3, lit4);
            paramEnvironment            = String.Concat(paramEnvironment, lit5);

            return paramEnvironment;
        }
    }
}