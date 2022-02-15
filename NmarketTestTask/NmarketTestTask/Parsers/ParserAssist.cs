using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NmarketTestTask.Models;

namespace NmarketTestTask.Parsers
{
    /// <summary>
    /// Вспомогательный класс с методами, потенциально общими для парсеров
    /// </summary>
    public static class ParserAssist
    {
        /// <summary>
        /// Проверка, является ли указанный файл файлом Excel
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>True or False</returns>
        internal static bool CheckIsFileCorrect(string path, string extension)
        {
            if (path == null || !Path.GetExtension(path).Contains(extension))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Метод для получения цифр из строки
        /// </summary>
        /// <param name="input">Строка с любыми символами</param>
        /// <returns>Строка, состоящая только из цифр</returns>
        internal static string GetNumByString(string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Метод сюртирует дома и квартиры, а так же
        /// удаляет дома, в которых нет квартир
        /// </summary>
        /// <param name="houseList">Список домов</param>
        internal static void SortAndClear(List<House> houseList)
        {
            houseList.Sort();
            foreach (var house in houseList)
            {
                if (house.Flats.Any())
                {
                    house.Flats.Sort();
                }
                else
                {
                    // если нет квартир - удаляем дом из списка
                    houseList.Remove(house);
                }
            }
        }
    }
}