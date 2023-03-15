using DDL;

//namespace DefinitionDictionary.Test;


public class DefinitionDictionaryTest
{
    [Fact]
    public async void Test1()
    {
        // Arrage
        DefinitionDictionary definitionDictionary = new DefinitionDictionary("UnitTestDB");
        var actual = await definitionDictionary.CreateDictionary(DefinitionDB._nameSpCreateRoot,"Test");

        //Act
        var expected = 0;

        //Assert
        Assert.Equal(expected, actual);
    }
}