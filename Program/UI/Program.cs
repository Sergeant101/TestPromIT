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
            if(args.Length == 1)
            {
                Console.Clear();
                switch (args[0])
                {
                    case "-u":
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
                    break;

                    case "-c":
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
                    break;

                    case "-d":
                    {
                        WriteLine("===========================================================================================================");
                        WriteLine("Режим создания словаря данных");
                        WriteLine("Введите название базы данных: ");
                        var nameDB = ReadLine();
                        WriteLine("===========================================================================================================");
                        WriteLine("Введите название создаваемого словаря: ");
                        var nameNewDictionary = ReadLine();

                        if ((nameDB == null) || (nameDB == ""))
                        {
                            WriteLine("Не задано имя базы данных");
                            break;
                        }

                        if ((nameNewDictionary == null) || (nameNewDictionary == ""))
                        {
                            WriteLine("Не задано имя словаря данных");
                            break;
                        }

                        DefinitionDictionary definitionDictionary = new DefinitionDictionary(nameDB);
                        BysLogic creater = new BysLogic(definitionDictionary);
                        if ( await creater.CreateNewDictionary(DefinitionDB._nameSpCreateRoot, nameNewDictionary) == 0)
                        {
                            WriteLine("Словарь успешно создан");
                        }
                        else
                        {
                            WriteLine("Что-то пошло не так");
                        }
                    }
                    break;
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
}


