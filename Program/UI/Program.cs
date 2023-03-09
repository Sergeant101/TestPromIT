using System;
using static System.Console;
using DDL;
using BL;

namespace UserInterface
{

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.CursorVisible = false;
            if(args.Length <= 1)
            {
                Console.Clear();

                if ((args.Length == 1) && (args[0] == "-u") )
                {

                    WriteLine("===========================================================================================================");
                    WriteLine("Режим наполнения словаря");
                    WriteLine("===========================================================================================================");
                    WriteLine("Введите путь к базе данных: ");
                    var pathToDB = ReadLine();
                    WriteLine("Введите название базы данных: ");
                    var nameDB = ReadLine();
                    WriteLine($"{pathToDB}   {nameDB}");

                }
               
                if ((args.Length == 1) && (args[0] == "-c") )
                {
                    WriteLine("===========================================================================================================");
                    WriteLine("Режим создания базы данных");
                    WriteLine("===========================================================================================================");
                    WriteLine("Введите путь к создаваемой базе данных: ");
                    var pathToDB = ReadLine();
                    WriteLine("Введите название базы данных: ");
                    var nameDB = ReadLine();
                    WriteLine($"{pathToDB}   {nameDB}");

                    DefinitionDB definitionDB = new DefinitionDB(pathToDB, nameDB);
                    BysLogic creater = new BysLogic(definitionDB);
                    
                    if(await creater.CreateNewDB() == 0)
                    {
                        WriteLine("База успешно создана");
                    }
                    else
                    {
                        WriteLine("Что-то пошло не так");
                    }
                }

                if ((args.Length == 1) && (args[0] == "-d"))
                {
                    WriteLine("===========================================================================================================");
                    WriteLine("Режим создания словаря данных");
                    WriteLine("Введите название базы данных: ");
                    var nameDB = ReadLine();
                    WriteLine("===========================================================================================================");
                    WriteLine("Введите название создаваемого словаря: ");
                    var nameNewDictionary = ReadLine();

                    DefinitionDictionary definitionDictionary = new DefinitionDictionary(nameDB);
                    BysLogic creater = new BysLogic(definitionDictionary);

                    if(await creater.CreateNewDictionary(nameNewDictionary) == 0)
                      {
                        WriteLine("Словарь успешно создан");
                    }
                    else
                    {
                        WriteLine("Что-то пошло не так");
                    }
                }

            }
            else
            {
                WriteLine("Программа завершена.");
                WriteLine("Причина: количество аргументов больше одного.");
                ReadKey();
            }
        }
    }

// DefinitionDB definitionDB = new DefinitionDB("Test2");

// System.Console.WriteLine($"{definitionDB.CreateDB()}");

// DefinitionDictionary definitionDictionary = new DefinitionDictionary(definitionDB.GetName, definitionDB.NameSpCreateRoot);
// await definitionDictionary.CreateDictionary("test2");
// await definitionDictionary.RefreshDictionary("конвергенция", 100);
}


