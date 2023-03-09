using DDL;

//namespace DefinitionDictionary.Test;


public class DefinitionDictionaryTest
{
    [Fact]
    public async void Test1()
    {
        // Arrage
        DefinitionDictionary definitionDictionary = new DefinitionDictionary("UnitTest");
        var actual = await definitionDictionary.CreateDictionary("Test");

        //Act
        var expected = 0;

        //Assert
        Assert.Equal(expected, actual);
    }
}