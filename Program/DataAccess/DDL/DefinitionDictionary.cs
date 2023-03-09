using DDL.Resources.Interface;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DefinitionDictionary.Test")]
[assembly: InternalsVisibleTo("UI")]
[assembly: InternalsVisibleTo("BL")]

namespace DDL;
public class DefinitionDictionary : IDataDefinitionDictionary
{

    public DefinitionDictionary(string _nameDB)
    {
        NameDB = _nameDB;
        ExistsConnection += SourceConnection + ";database=" + NameDB + ";";  
    }

    public DefinitionDictionary(string _nameDB, string _nameSpCreateDictionary):this(_nameDB)
    {
        nameSpCreateDictionary = _nameSpCreateDictionary;
    }

    private readonly string SourceConnection = @"Data Source=.\SQLEXPRESS;Integrated security=True;TrustServerCertificate=true;MultipleActiveResultSets=True";
    private readonly string ExistsConnection = null!;
    private readonly string NameDB;
    private string nameSpCreateDictionary = "Example";
    public string NameSpCreateDictionary 
    {
        get => nameSpCreateDictionary;
        set {nameSpCreateDictionary = value; }
    }
    private string NameDictionary;
    public async ValueTask<int> CreateDictionary(string nameDict)
    {

        NameDictionary = nameDict;

        var retval = 1;

            try
            {
                using(SqlConnection sqlConnection = new SqlConnection(ExistsConnection))
                {
                    await sqlConnection.OpenAsync();

                    SqlCommand sqlCommand = new SqlCommand(nameSpCreateDictionary, sqlConnection);

                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    SqlParameter name = new SqlParameter()
                    {
                        ParameterName = "@nameTable",
                        Value = nameDict
                    };

                    sqlCommand.Parameters.Add(name);

                    await sqlCommand.ExecuteScalarAsync();

                    retval = 0;
                }
            }
            
            catch
            {
                retval = 2;
            }

            return retval;
    }

    public int DeleteDictionary(string name)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<int> RefreshDictionary(string insertWord, int quantityWords)
    {
        var retval = 1;

        if ((insertWord.Length >= 3) && (insertWord.Length <=15) )
        {
            int resultCheckExists;

            //Check exist node trie lvl-1 
            string existNode = "SELECT COUNT(*) FROM sys.sysobjects WHERE name=@substring";
            //Check exists word in dictionary
            string existWord = "SELECT countWord FROM " + insertWord.Substring(0,2) + " WHERE word = '" + insertWord + "'";

            string createRecord = "INSERT INTO " + insertWord.Substring(0,2) + " (node, word, countWord) VALUES ('" + insertWord.Substring(0,1) + "', " + "'"  + insertWord + "', " + Convert.ToString(quantityWords) + ")";

            using (SqlConnection sqlConnection = new SqlConnection(ExistsConnection))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(existNode, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@substring", insertWord.Substring(0,2));

                    using (SqlDataReader reader1 = sqlCommand.ExecuteReader())
                    {
                        reader1.Read();

                        resultCheckExists = (int)reader1.GetValue(0);
                        reader1.Close();  
                        sqlConnection.Close();
                    }
 
                }
                 
            }   

            // Create new node if noexists
            if (resultCheckExists == 0)
            {
                CreateTable(insertWord.Substring(0,2));
            }

            //Update dictionary
            using (SqlConnection sqlConnection = new SqlConnection(ExistsConnection))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(existWord, sqlConnection))
                {
                    using (SqlDataReader reader1 = sqlCommand.ExecuteReader())
                    {
                        if (reader1.Read())
                        {
                            int count = Convert.ToInt32(reader1.GetValue(0));
                            string updateDickt = "UPDATE " + insertWord.Substring(0,2) + " SET countWord = " + Convert.ToString(count + quantityWords) +
                                " WHERE Word = '" + insertWord + "'";
                            System.Console.WriteLine($"Exists {count}");
                            
                            using (SqlConnection connection = new SqlConnection(ExistsConnection))
                            {
                                connection.Open();

                                using (SqlCommand commandUpdate = new SqlCommand(updateDickt, connection))
                                {
                                    commandUpdate.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            using (SqlConnection connection = new SqlConnection(ExistsConnection))
                            {
                                connection.Open();

                                using (SqlCommand commandUpdate = new SqlCommand(createRecord, connection))
                                {
                                    commandUpdate.ExecuteNonQuery();
                                }

                            
                            }

                        }
                        reader1.Close();  
                        sqlConnection.Close();
                    }
 
                }
                 
            }  

            //debug delete
            System.Console.WriteLine(resultCheckExists);
            
            
        }   retval = 0;

        return retval;

    }

    private int  CreateTable(string nameTable)
        {
            var retval = 1;

            try
            {
                using(SqlConnection sqlConnection = new SqlConnection(ExistsConnection))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("sp_add_table", sqlConnection);

                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    SqlParameter name = new SqlParameter()
                    {
                        ParameterName = "@substring",
                        Value = nameTable
                    };

                    sqlCommand.Parameters.Add(name);

                    SqlParameter parent = new SqlParameter()
                    {
                        ParameterName = "@parentTable",
                        Value = NameDictionary
                    };

                    sqlCommand.Parameters.Add(parent);

                    sqlCommand.ExecuteScalar();

                    retval = 0;
                }
            }
            
            catch
            {
                retval = 2;
            }

            return retval;
        }

    
}
