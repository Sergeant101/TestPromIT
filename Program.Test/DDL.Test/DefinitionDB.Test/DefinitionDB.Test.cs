namespace DefinitionDB.Test;

using DDL;

public class DefinitionDBTest
{
    private string nameTestDB = "UnitTestDB";
    private string pathTestDB = "C:\\Temp\\";
    private string wrongPathTestDB = "C:\\Tamp\\";

    [Fact]
    public void TestCreateDB()
    {
        //Arrage
        DefinitionDB definitionDB = new DefinitionDB(pathTestDB, nameTestDB);
        var retval = definitionDB.CreateDB();

        //Act
        var Ok = 0;

        //Assert
        Assert.Equal(Ok, retval);
    }

    [Fact]
    public void TestExistsDB()
    {
        //Arrage
        DefinitionDB definitionDB = new DefinitionDB(pathTestDB, nameTestDB);
        var retval = definitionDB.CreateDB();

        //Act
        var Ok = 4;

        //Assert
        Assert.Equal(Ok, retval);
    }

    [Fact]
    public void TestWrongPathDB()
    {
        //Arrage
        DefinitionDB definitionDB = new DefinitionDB(wrongPathTestDB, nameTestDB);
        var retval = definitionDB.CreateDB();

        //Act
        var Ok = 6;

        //Assert
        Assert.Equal(Ok, retval);
    }
    
    [Fact]
    public void TestDeleteDB()
    {
        //Arrage
        DefinitionDB definitionDB = new DefinitionDB(pathTestDB, nameTestDB);
        var retval = definitionDB.DeleteDB();

        //Act
        var Ok = 0;

        //Assert
        Assert.Equal(Ok, retval);
    }

}