using System;
using System.Collections.Generic;

namespace Vegetables
{
    public class Container
    {
        public List<Box> Boxes { get; }

        readonly Random rnd = new Random();
        /// <summary>
        /// Кол-во ящиков
        /// </summary>
        public int Size { get; }
        /// <summary>
        /// Повреждённость контейнера.
        /// </summary>
        public double Harm { get; }
        /// <summary>
        /// Вместимость контейнера.
        /// </summary>
        public int MaxWeight { get; }
        /// <summary>
        /// Добавление ящика в контейнер через консоль.
        /// </summary>
        public void AddBox()
        {
            double weight = Program.ParseDouble();
            Console.Write("Введите его стоимость за килограмм: ");
            double price = Program.ParseDouble();
            Box box = new Box(weight, price);
            Boxes.Add(box);
            Program.Colour("Ящик добавлен.", ConsoleColor.DarkGreen);
        }
        /// <summary>
        /// Добавление ящика в контейнер через файл.
        /// </summary>
        /// <param name="weight">масса ящика</param>
        /// <param name="price">цена за кг</param>
        public void AddBox(double weight, double price)
        {           
            Box box = new Box(weight, price);
            Boxes.Add(box);
            Program.Colour("Ящик добавлен.", ConsoleColor.DarkGreen);

        }
        /// <summary>
        /// Текущий вес контейнера.
        /// </summary>
        public double CurrentWeight
        {
            get
            {
                double sum = 0;
                foreach (var x in Boxes)
                {
                    sum += x.Weight;
                }
                return sum;
            }
        }

        public double FullPrice
        {

            get
            {
                double sum = 0;
                foreach (var i in Boxes)
                {
                    sum += i.TotalPrice * (1 - Harm);
                }
                return sum;
            }
        }
        /// <summary>
        /// Конструктор контейнера.
        /// </summary>
        /// <param name="size">кол-во ящиков</param>
        public Container(int size)
        {
            MaxWeight = rnd.Next(50, 1001);
            Console.WriteLine($"Максимально допустимая вместимость контейнера: {MaxWeight}");
            this.Size = size;
            Harm = Math.Round(rnd.NextDouble() % 0.5, 2);
            Console.WriteLine($"Повреждённость: {Harm}");
            Boxes = new List<Box>();
        }   
    }
}
