using System;
using System.Collections.Generic;
using System.IO;

namespace Vegetables
{
    public class Storage
    {
        int id = 0;
        /// <summary>
        /// Кол-во контейнеров.
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }

        int count;
        /// <summary>
        /// Цена хранения одного контейнера
        /// </summary>
        public double Price
        {
            get
            {
                return price;
            }
        }

        double price;

        List<Container> containers = new List<Container>();
        /// <summary>
        /// Конструктор склада.
        /// </summary>
        /// <param name="count">кол-во контейнеров</param>
        /// <param name="price">цена хранения одного контейнера</param>
        public Storage(int count, double price)
        {
            this.count = count;
            this.price = price;
        }
        /// <summary>
        /// Добавление контейнера на склад через консоль.
        /// </summary>
        public void AddContainer()
        {
            Console.Write("Введите кол-во ящиков: ");
            int size = Program.ParseInt();
            Container container = new Container(size);
            for (int i = 0; i < size; i++)
            {
                Console.Write($"Введите массу {i + 1} ящика: ");
                container.AddBox();
            }
            if (container.CurrentWeight < container.MaxWeight && container.FullPrice > price)
            {
                if (containers.Count == Count)
                {
                    containers[id] = container;
                    id++;
                    if (id == Count)
                    {
                        id = id % Count;
                    }
                }
                else
                {
                    containers.Add(container);
                }
                Program.Colour("Контейнер добавлен.", ConsoleColor.DarkGreen);
            }
            else
            {
                Program.Colour("Контейнер не добавлен, так как не соответствует условию.", ConsoleColor.DarkRed);
                Console.WriteLine("Его хранение должно быть рентабильным и " + Environment.NewLine +
                    "суммарная масса ящиков не может превосходить максимально допустимую массу контейнера.");
            }

        }
        /// <summary>
        /// Выврд информации о складе в консоль.
        /// </summary>
        public void PrintStorage()
        {
            Console.WriteLine($"Кол-во контенеров: {Count}{Environment.NewLine}" +
                $"Цена хранений: {Price}{Environment.NewLine}");

        }
        /// <summary>
        /// Вывод информации о контейнерах в консоль.
        /// </summary>
        public void PrintContainers()
        {
            if (containers.Count == 0)
                Console.WriteLine("Склад пуст.");

            else
            {
                int i = 0;
                foreach (var item in containers)
                {
                    Program.Colour($"Контейнер №{++i}: {Environment.NewLine}", ConsoleColor.Magenta);
                    Console.WriteLine($"Цена контейнера: {Math.Round(item.FullPrice, 2)} {Environment.NewLine}" +
                        $"Вместимость по весу: {item.MaxWeight} {Environment.NewLine}" +
                        $"Занято: {item.CurrentWeight} {Environment.NewLine}" +
                        $"Свободно: {item.MaxWeight - item.CurrentWeight}{Environment.NewLine}" +
                        $"Повреждённость: {item.Harm}{Environment.NewLine}" +
                        $"Кол-во ящиков: {item.Boxes.Count}{Environment.NewLine}{Environment.NewLine}");
                }
            }
            
        }
        /// <summary>
        /// Запись информации о складе в файл.
        /// </summary>
        public void WriteToFile()
        {
            string res = "";
            if (containers.Count == 0)
                res = "Склад пуст.";

            else
            {
                int i = 0;
                foreach (var item in containers)
                {
                    res += $"Контейнер №{++i}: {Environment.NewLine}" +
                        $"Цена контейнера: {Math.Round(item.FullPrice, 2)} {Environment.NewLine}" +
                        $"Вместимость по весу: {item.MaxWeight} {Environment.NewLine}" +
                        $"Занято: {item.CurrentWeight} {Environment.NewLine}" +
                        $"Свободно: {item.MaxWeight - item.CurrentWeight}{Environment.NewLine}" +
                        $"Повреждённость: {item.Harm}{Environment.NewLine}" +
                        $"Кол-во ящиков: {item.Boxes.Count}{Environment.NewLine}{Environment.NewLine}";
                }
             }
            File.WriteAllText("result.txt", res);
            res = "";
        }
        /// <summary>
        /// Удаление контейнера на складе через консоль.
        /// </summary>
        public void DeleteContainer()
        {
            int numberContainer = Program.ParseInt();
            if (containers.Count == 0)
            {
                Console.WriteLine("Склад пуст.");
                return;
            }
            if (containers.Count < numberContainer)
            {
                Console.WriteLine("Такого контейнера нет(");
                return;
            }
            containers.RemoveAt(numberContainer - 1);
            Program.Colour("Контенер был удалён.", ConsoleColor.DarkGreen);

        }
        /// <summary>
        /// Удаление контейнера на складе через файл.
        /// </summary>
        /// <param name="number">номер контейнера</param>
        public void DeleteContainer(int number)
        {
            if (containers.Count == 0)
            {
                Console.WriteLine("Склад пуст.");
                return;
            }
            if (containers.Count < number)
            {
                Console.WriteLine("Такого контейнера нет(");
                return;
            }
            containers.RemoveAt(number - 1);
            Program.Colour("Контенер был удалён.", ConsoleColor.DarkGreen);

        }
        /// <summary>
        /// Добавление контейнера на склад через файл.
        /// </summary>
        /// <param name="size">кол-во ящиков</param>
        /// <param name="data">параметры ящика</param>
        public void AddContainer(int size, string[] data)
        {
            Container container = new Container(size);
            for (int i = 0; 2 + i < data.Length; i += 2)
            {

                container.AddBox(double.Parse(data[1 + i]), double.Parse(data[2 + i]));
               
            }
            if (container.CurrentWeight < container.MaxWeight && container.FullPrice > price)
            {
                if (containers.Count == Count)
                {
                    containers[id] = container;
                    id++;
                    if (id == Count)
                    {
                        id = id % Count;
                    }
                }
                else
                {
                    containers.Add(container);
                }
                Program.Colour("Контейнер добавлен.", ConsoleColor.DarkGreen);
            }
            else
            {
                Program.Colour("Контейнер не добавлен, так как не соответствует условию.", ConsoleColor.DarkRed);
                Console.WriteLine("Его хранение должно быть рентабильным и " + Environment.NewLine +
                    "суммарная масса ящиков не может превосходить максимально допустимую массу контейнера.");

            }

        }
    }
}
