using System.Data;
using Microsoft.Data.SqlClient;

namespace DDL
{
    internal partial class DefinitionDB
    {
  
        private readonly static string nameSelectWord = "sp_select_word";

        public readonly static string _nameSpCreateRoot = "sp_init";
        private readonly string spCreateRoot = 
        @"CREATE PROCEDURE [dbo].["+ _nameSpCreateRoot + @"]

            @nameTable nvarchar(20)

            AS

			DECLARE @cmd nvarchar(100), @initString nvarchar(100), @count INT = 1, @temp nvarchar(100) , @surceStringSql nvarchar(100)

			set @cmd = 'CREATE TABLE ' + @nameTable + ' (node CHAR PRIMARY KEY, countWord INT DEFAULT 0)'

			print @cmd  

		    EXEC(@cmd)
            
            SET @initString ='абвгдеёжзийклмнопрстуфхцчшщэюяabcdefghijklmnpqrstuvwxyz'

			SET @surceStringSql  = 'INSERT INTO ' + @nameTable + ' (node) VALUES ' 
            
			WHILE @count <= LEN(@initString)
            BEGIN

				SET @temp = @surceStringSql + '(' + '''' + (SUBSTRING(@initString,@count,1)) + '''' + ')'  

				EXEC(@temp)

				SET @temp = ''

				SET @count = @count + 1
            END";

        public static readonly string _nameSpAddTable = "sp_add_table";
        private readonly string spAddTable = 
             @"CREATE PROCEDURE [dbo].["+ _nameSpAddTable + @"]
                @parentTable NVARCHAR(50),
				@substring NVARCHAR(10)
                AS

				DECLARE @sqlCommand NVARCHAR(MAX)

				SET @sqlCommand = 'CREATE TABLE ' + @substring + ' (node CHAR, word NVARCHAR(15)  PRIMARY KEY, countWord INT DEFAULT 0, parent CHAR, FOREIGN KEY (parent) REFERENCES ' + @parentTable + ')'

				EXEC(@sqlCommand)
             ";

        private readonly string spSelectWord =
            @"CREATE PROCEDURE [dbo].["+ nameSelectWord + @"]
                @parentTable NVARCHAR(50),
				@substring NVARCHAR(10)
                AS

				DECLARE @sqlCommand NVARCHAR(MAX)

				SET @sqlCommand = 'CREATE TABLE ' + @substring + ' (node CHAR PRIMARY KEY, word NVARCHAR(15), countWord INT DEFAULT 0, parent CHAR, FOREIGN KEY (parent) REFERENCES ' + @parentTable + ')'

				EXEC(@sqlCommand)
            ";

        private async ValueTask<int>  SpCreate(string sqlString)
        {
            var retval = 1;

            try
            {
                using(SqlConnection sqlConnection = new SqlConnection(ExistsConnection))
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    await sqlCommand.ExecuteNonQueryAsync();
                    await sqlConnection.CloseAsync();

                    retval = 0;
                }
            }
            catch
            {
                retval = 2;
            }

            return retval;
        }

        // private async ValueTask<int> SpAddendum(string addWord, int quantityWord)
        // {
        //     var retval = 1;



        //     return retval;
        // }
    }
}