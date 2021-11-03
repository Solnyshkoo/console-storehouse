using System;
using System.IO;

namespace Vegetables
{
    class Program
    {
        static void Main()
        {
            Console.Clear();
            Console.WriteLine("Привет, начинающий фермер. Готов(-а) поработать?) " + Environment.NewLine);
            int input;
            do
            {
                PrintChoosingInput();
                Console.Write("Выбери как будем работать: ");
                input = ParseInput();
            } while (ChooseInput(input));
            Console.WriteLine("Пока, пока. Приятно было поработать. Хорошего дня)");

        }
        /// <summary>
        /// Выбор способа ввода данных.
        /// </summary>
        /// <param name="input"> команда, выбранная пользователем </param>
        /// <returns> повтор решения </returns>
        public static bool ChooseInput(int input)
        {
            switch (input)
            {
                case 1:
                    ConsoleInput();
                    return true;
                case 2:
                    FileInput();
                    return true;
                case 3:
                    return false;
                default:
                    Console.WriteLine("Такой команды нет.");
                    return true;
            }
        }
        /// <summary>
        /// Реализация ввода данных через консоль.
        /// </summary>
        public static void ConsoleInput()
        {
            Console.Clear();
            Console.WriteLine(Environment.NewLine + "Начнём)" + Environment.NewLine);
            Storage Warehouse = DataForStorage();
            do
            {
                Console.WriteLine();
                Console.WriteLine("_________________________________________");
                PrintRules();
                Console.Write("Выбери команду: ");
            } while (Query(Console.ReadLine(), ref Warehouse));

        }

        /// <summary>
        /// Реализация ввода данных через файлы. 
        /// </summary>
        public static void FileInput()
        {
            bool flag;
            do
            {
                Console.Clear();
                try
                {
                    FileInformationInput(1); 
                    Storage WareHouse = FileStorage();
                    FileInformationInput(2);
                    string[] action = File.ReadAllLines("actions.txt"); // Cчитывание файла в массив.
                    FileInformationInput(3);
                    string[] container = File.ReadAllLines("containers.txt");
                    flag = false;
                    if (action.Length != container.Length) // Проверка данных на корректность с выбрасыванием исключений.
                    {
                        throw new FileReadException("У файлов разная длинна.");
                    }
                    for (int i = 0; i < action.Length; i++) // Проверка данных на коррекность с выбрасыванием исключений.
                    {
                        FileAction(WareHouse, action, container, i);
                    }
                    WareHouse.PrintContainers(); // Вывод информации о контенерых в консоль.
                    Colour("Данные записаны в файл result.txt"+ Environment.NewLine, ConsoleColor.DarkGreen);
                    WareHouse.WriteToFile();

                }
                catch (FileReadException ex)
                {
                    Console.WriteLine(ex.Message);
                    flag = true;
                }

                catch (Exception)
                {
                    Console.WriteLine("Формат файла не корректен попробуйте снова.");
                    flag = true;
                }

            } while (flag && RepeatGame());

        }
        /// <summary>
        /// Проверка данных в файле на корректность.
        /// </summary>
        /// <param name="WareHouse"> Ссылка на склад </param>
        /// <param name="action"> Массив с указаниями действий от пользователя </param>
        /// <param name="container"> Массив с данными о контенерах </param>
        /// <param name="i">параментр цикла</param>
        private static void FileAction(Storage WareHouse, string[] action, string[] container, int i)
        {
            if (action[i].ToLower().Trim() == "add")
            {
                string[] dataContainers = container[i].Split(' ');
                int size = int.Parse(dataContainers[0]);
                for (int y = 1; y < container.Length; y++)
                {
                    if (!(double.TryParse(dataContainers[i], out double data) && data > 0))
                    {
                        throw new FileReadException("Число некорректно.");
                    }
                }

                if (size <= 0)
                {
                    throw new FileReadException("Число некорректно.");
                }
                if (size * 2 != dataContainers.Length - 1)
                {
                    throw new FileReadException("Данные некорректны.");
                }

                WareHouse.AddContainer(size, dataContainers);
            }
            else if (action[i].ToLower().Trim() == "remove")
            {

                if (int.TryParse(container[i].Split(' ')[0], out int number) && number > 0)
                {
                    WareHouse.DeleteContainer(number);
                }
                else
                {
                    throw new FileReadException("В файле некорректное число, попробуйте заново");
                }

            }
            else
            {
                throw new FileReadException("Такой команды нет, напишите add или remove.");
            }
        }
        /// <summary>
        /// Вывод инструкции о правильной записи информации в файл.
        /// </summary>
        /// <param name="numberFile"> индификатор каждого из файлов: storage.txt, action.txt, containers.txt. </param>
        public static void FileInformationInput(int numberFile)
        {
            switch (numberFile)
            {
                case 1:
                    Colour("Все файлы (с примером ввода) храняться в папке с исполняемым модулем. " + Environment.NewLine +
                            "Если данные некоректны, исправьте их в файле и нажмите любую клавишу для продолжения."
                            + Environment.NewLine, ConsoleColor.Blue);
                    Console.WriteLine("Введите данные о контейнере в storage.txt. Каждый параметр с новой строки." + Environment.NewLine +
                          "{1} размер склада" + Environment.NewLine + "{2} стоимость хранения одного контейнера" + Environment.NewLine);
                    break;
                case 2:
                    Console.WriteLine("Введите в action.txt команды add и remove каждую с новой строки" + Environment.NewLine +
                          "Пример записи в файле:" + Environment.NewLine +
                          "add" + Environment.NewLine +
                          "add" + Environment.NewLine +
                          "remove" + Environment.NewLine);
                    break;
                case 3:
                    Console.WriteLine("Введите в containers.txt данные о котейнере. " +
                          "Каждому действию соответствует одна строка в данных." + Environment.NewLine +
                          "Для удаления (remove) нужно ввести номер удаляемого контейнера, " +
                          "для добавления (add) нужно ввести кол-во ящиков и " + Environment.NewLine +
                          "для каждого ящика его массу и стоимость за кг." + Environment.NewLine +
                          "Пример записи в файл:" + Environment.NewLine +
                          "1 3 4" + Environment.NewLine +
                          "2 3 4 5 6" + Environment.NewLine +
                          "1" + Environment.NewLine);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Считывание данных о складе из файла.
        /// </summary>
        /// <returns> возвращает ссылку на склад </returns>
        public static Storage FileStorage()
        {
            bool flag;
            int count = 0;
            double price = 0;
            do
            {
                try
                {
                    string[] storageInput = File.ReadAllLines("storage.txt");
                    count = int.Parse(storageInput[0]);
                    price = double.Parse(storageInput[1]);
                    flag = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Формат файла не корректен попробуйте снова.");
                    flag = true;
                }
            } while (flag && RepeatGame());
            Colour("Склад создан.", ConsoleColor.DarkGreen);
            return new Storage(count, price);
        }
        /// <summary>
        /// Повтор решения
        /// </summary>
        /// <returns>Выбор пользователя</returns>
        static bool RepeatGame()
        {
            Console.WriteLine(Environment.NewLine + "Для выхода нажми - Escape. " +
                              "Для продолжения любую клавишу.");
            return Console.ReadKey(true).Key != ConsoleKey.Escape;
        }
        /// <summary>
        /// Меню выбор ввода данных
        /// </summary>
        public static void PrintChoosingInput()
        {
            Console.WriteLine("1. Ввод данных через консоль. " + Environment.NewLine +
                "2. Ввод данных с помощью файлов (формат .txt)." + Environment.NewLine +
                "3. Выход" + Environment.NewLine);
        }
        /// <summary>
        /// Вывод полной инструкции.
        /// </summary>
        public static void Instruction()
        {
            Console.WriteLine($"Привет, всё очень просто){Environment.NewLine}{Environment.NewLine}" +
                $"У тебя есть склад, он является хранилищем определённого количества{Environment.NewLine}" +
                $"(при создании склада, ты выбираешь это число сам(-а)) контейнеров.{Environment.NewLine}" +
                $"Содержимое склада может меняться - туда могут поступать новые контейнеры{Environment.NewLine}" +
                $"(у них есть максимально допустимая масса, случайное число) и удаляться старые.{Environment.NewLine}{Environment.NewLine}" +
                $"Если происходит добавление нового контейнера в заполненный склад,{Environment.NewLine}" +
                $"то он заменяет собой тот, что был добавлен не позднее всех остальных.{Environment.NewLine}" +
                $"Также склад взимает фиксированную плату за хранение контейнера(выбираешь её сам(-а)).{Environment.NewLine}{Environment.NewLine}" +
                $"Хранение контейнера может быть нерентабельно. При поступлении контейнера на склад{Environment.NewLine}" +
                $"рассчитывается степень его повреждения(случайное число) - именно такую долю стоимости теряет каждый̆ ящик.{Environment.NewLine}" +
                $"Если суммарная стоимость содержимого контейнера после этого не превосходит стоимость хранения,{Environment.NewLine}" +
                $"то такой̆ контейнер на склад не помещается.{Environment.NewLine}{Environment.NewLine}" +
                $"Потом добавляются ящики, если добавление ящика приводит к превышению{Environment.NewLine}" +
                $"максимально допустимой̆ массы контейнера, то он не добавляется в контейнер.{Environment.NewLine}");
        }

        /// <summary>
        /// Вывод правил игры
        /// </summary>
        public static void PrintRules()
        {
            Colour("0. Смена настроек ввода." + Environment.NewLine +
                "1. Вывод полной инструкции." + Environment.NewLine +
                "2. Создать новый склад (все контейнеры удалятся)." + Environment.NewLine +
                "3. Добавление контейнера на склад." + Environment.NewLine +
                "4. Удаление контейнера со склада." + Environment.NewLine +
                "5. Вывод информации о контейнерах." + Environment.NewLine +
                "6. Вывoд настроек склада.", ConsoleColor.Yellow);
        }

        /// <summary>
        /// Реализация выбранной команды пользователя.
        /// </summary>
        /// <param name="query">выбор пользователя</param>
        /// <param name="Warehouse">ссылка на склад</param>
        /// <returns>повтор решения</returns>
        public static bool Query(string query, ref Storage Warehouse)
        {
            Console.Clear();
            if (!int.TryParse(query, out int queryNum))
            {
                Console.WriteLine("А так нельзя) Нужно ввести номер команды!");
                return true;
            }
            switch (queryNum)
            {
                case 0:
                    return false;
                case 1:
                    Instruction();
                    return true;
                case 2:
                    Warehouse = DataForStorage();
                    return true;
                case 3:
                    Warehouse.AddContainer();
                    return true;
                case 4:
                    Console.Write("Введите номер контейнера для удаления: ");
                    Warehouse.DeleteContainer();
                    return true;
                case 5:
                    Warehouse.PrintContainers();
                    return true;
                case 6:
                    Warehouse.PrintStorage();
                    return true;
                default:
                    Console.WriteLine("Такой команды нет, выбери из доступных.");
                    return true;
            }
        }

        /// <summary>
        /// Сбор и проверка на корректность данных для склада
        /// </summary>
        /// <returns> ссылку склад </returns>
        private static Storage DataForStorage()
        {
            Console.WriteLine("Создание нового склада:");
            Console.Write("Введи кол-во контейнеров на складе: ");
            int count = ParseInt();
            Console.Write("Введите стоимость храненя одного контейнера: ");
            double price = ParseDouble();
            Colour("Склад создан.", ConsoleColor.DarkGreen);
            return new Storage(count, price);


        }
        /// <summary>
        /// Раскрашивает текст в выбранныц цвет.
        /// </summary>
        /// <param name="text">текст, который надо окрасить </param>
        /// <param name="color">цвет, в который надо окрасить </param>
        public static void Colour(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        /// <summary>
        /// Проверка числа на int.
        /// </summary>
        /// <returns>число int</returns>
        public static int ParseInt()
        {
            int amount;
            while (!(int.TryParse(Console.ReadLine(), out amount) && amount > 0))
            {
                Console.Write("Число должно быть натуральным (не по мнению Дашкова): ");
            }
            return amount;
        }
        /// <summary>
        /// Проверка числа на double.
        /// </summary>
        /// <returns> число double </returns>
        public static double ParseDouble()
        {
            double data;
            while (!(double.TryParse(Console.ReadLine(), out data) && data > 0))
            {
                Console.Write("Число должно быть положительным: ");
            }
            return data;
        }
        /// <summary>
        /// Проверка на корректность ввёдённой команды
        /// </summary>
        /// <returns></returns>
        public static int ParseInput()
        {
            int number;
            while (!(int.TryParse(Console.ReadLine(), out number) && number >= 1 && number <= 3))
            {
                Console.Write("Такой команды нет( Выбери 1 или 2 или 3: ");
            }
            return number;
        }

    }
}
