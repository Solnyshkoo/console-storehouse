using System;
namespace Vegetables
{
    public class Box
    {
        /// <summary>
        /// Масса ящика в кг.
        /// </summary>
        public double Weight { get; private set; }
        /// <summary>
        /// Цена за кг.
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Полная стоимость ящика.
        /// </summary>
        public double TotalPrice { get; }
        /// <summary>
        /// Конструктор ящиков.
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="price"></param>
        public Box(double weight, double price)
        {
            Weight = weight;
            Price = price;
            TotalPrice = price * weight;
        }
    }
}
