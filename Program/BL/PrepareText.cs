using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL
{
    public class PrepareText
    {
        // Путь к файлу
        private string Path = null;

        // Признак завершения считывания данных
        private bool Complete = false;

        // Хранилище распарсенных слов
        Dictionary<string, int> HashWord = new Dictionary<string, int>();

        public PrepareText(string _path)
        {
            // Получаем путь к файлу для парсинга
            Path = _path;
            Console.WriteLine("\tОбработка файла.");
            ReadFile();
        }


        // =========================================================================================================
        // Читаем потрочно из файла
        private void ReadFile()
        {
            List<string> WordsFromTxt = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(Path))
                {
                    // строка для чтения данных из файла
                    string line;

                    // читаем в цикле пока не закончатся строки
                    while ((line = sr.ReadLine()) != null)
                    {
                        WordsFromTxt.Clear();
                        // получаем список слов из считанной строки
                        ParseLine(line, WordsFromTxt);
                        // добавляем полученные слова в хэш таблицу
                        AddToHash(WordsFromTxt, HashWord);
                    }                    
                    Complete = true;
                }
            }
            catch
            {
                Console.WriteLine("\tОшибка чтения файла");
            }
            finally
            {
                Console.WriteLine("\tОбработка файла завершена");
            }
        }


        // =========================================================================================================
        // Парсим строку
        private void ParseLine(string lineSource, List<String> WordParce)
        {
            // слова выделяем делая проход слева направо 
            // добавим в самый конец пробел чтобы он там точно был
            lineSource = lineSource + " ";

            // нижняя граница длины слова
            int LoLimit = 3;

            // верхняя граница длины слова
            int HiLimit = 20;

            // список с позициями разделителей слов
            List<int> posDelimiter = new List<int>(16);


            do
            {
                // бит завершает цикл если не осталось символов в строке
                bool StopLoop = true;

                // ближайший символ, который может ограничивать слово
                int NextDelimiter = 2147483647;

                // получаем позиции разделителей слов
                GetDelimiterPos(lineSource, posDelimiter);

                // заканчивает парсинг строки если в строке не найдено ни одного
                // символа, который может ограничивать слово 
                // т.е. все позиции делимитеров равны -1
                posDelimiter.ForEach(delegate (int i)
                {
                    if (i > 0)
                    {
                        // если хотя бы один разделитель слов из списка найден, то
                        StopLoop = false;
                    }
                    if ((i < NextDelimiter) && (i >= 0))
                    {
                        NextDelimiter = i;
                    }
                });

                if (StopLoop)
                {
                    break;
                }

                // проверяем вхождение выделенного слова в заданные границы
                if ((NextDelimiter < LoLimit) || (NextDelimiter > HiLimit))
                {
                    // если полученное слово/символ не входят в границы
                    // удаляем из строки
                    lineSource = lineSource.Remove(0, NextDelimiter + 1);
                }
                else
                {
                    // получаем слово из строки
                    string WordFromString = lineSource.Substring(0, NextDelimiter);
                    // обрезаем по делимитер
                    lineSource = lineSource.Remove(0, NextDelimiter + 1);
                    // делаем все прописными и в список
                    WordParce.Add(WordFromString.ToLower());

                }

            } while (true);
        }


        // =========================================================================================================
        // возвращаем позиции разделителей слов
        private void GetDelimiterPos(string WorkLine, List<int> Delimiters)
        {
            Delimiters.Clear();
            // 1 адрес первого пробела с начала строки
            Delimiters.Insert(0, WorkLine.IndexOf(' '));
            // 2 точки
            Delimiters.Insert(1, WorkLine.IndexOf('.'));
            // 3 запятой
            Delimiters.Insert(2, WorkLine.IndexOf(','));
            // 4 отрывающей скобки
            Delimiters.Insert(3, WorkLine.IndexOf('('));
            // 5 закрывающей скобки
            Delimiters.Insert(4, WorkLine.IndexOf(')'));
            // 6 двоеточия
            Delimiters.Insert(5, WorkLine.IndexOf(':'));
            // 7 точки с запятой
            Delimiters.Insert(6, WorkLine.IndexOf(';'));
            // 8 тире
            Delimiters.Insert(7, WorkLine.IndexOf('-'));
            // 9 обратный слэш
            Delimiters.Insert(8, WorkLine.IndexOf('/'));
            // 10 прямой слэш
            Delimiters.Insert(9, WorkLine.IndexOf('\\'));
            // 11 восклицательный знак
            Delimiters.Insert(10, WorkLine.IndexOf('!'));
            // 12 вопросительный знак
            Delimiters.Insert(11, WorkLine.IndexOf('?'));
            // 13 одинарные ковычки
            Delimiters.Insert(12, WorkLine.IndexOf('\''));
            // 14 двойные ковычки
            Delimiters.Insert(13, WorkLine.IndexOf('"'));
            // 15 треугольные влево
            Delimiters.Insert(14, WorkLine.IndexOf('<'));
            // 16 треугольные вправо
            Delimiters.Insert(15, WorkLine.IndexOf('>'));
        }


        // =========================================================================================================
        // Проверяем чтобы слова состояли только из русских и английских символов
        // пока отложим на перспективу !!!
        private bool CheckWord(string sourceWord)
        {

            bool CharRus = true;

            // делаем проход по кирилице, если всё ок оставляем
            // в маркере true
            foreach (char ch in sourceWord)
            {
                if ((ch >= 'а' && ch <= 'я') || (ch == 'ё') || (ch == 'Ё'))
                {
                    
                }
                else
                {
                    CharRus = false;
                    break;
                }
            }

            return true;
        }


        // =========================================================================================================
        // Добавляем слова из распарсенной строки в словарь
        private void AddToHash(List<string> ToHash, Dictionary<string, int> hash)
        {
            ToHash.ForEach(delegate (string word)
            {
                try
                {
                    hash.Add(word, 1);
                }
                catch
                {
                    int count = hash[word];
                    hash[word] = ++count;
                }
            });
        }


        // возвращаем данные о количестве слов в файле
        public Dictionary<string, int> GetHash
        {
            
            get
            {
                Dictionary<string, int> HashTemp = new Dictionary<string, int>();
                foreach(KeyValuePair<string, int> h in HashWord)
                {
                    if (h.Value >= 4)
                    {
                        HashTemp.Add(h.Key, h.Value);
                    }
                }
                return HashTemp;
            }
        }


        // возвращаем признак завершения обработки файла
        public bool IsComplete
        {
            get
            {
                return Complete;
            }
        }
    }
}