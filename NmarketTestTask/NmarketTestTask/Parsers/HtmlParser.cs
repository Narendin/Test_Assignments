using HtmlAgilityPack;
using NmarketTestTask.Models;
using System.Collections.Generic;
using System.Linq;

namespace NmarketTestTask.Parsers
{
    /// <summary>
    /// Класс парсера Html файлов
    /// </summary>
    public class HtmlParser : IParser
    {
        /// <summary>
        /// Метод получения домов из Html файла
        /// </summary>
        /// <param name="path">Путь до Html файла</param>
        /// <returns>Список домов с перечислением квартир и цен на них</returns>
        public IList<House> GetHouses(string path)
        {
            if (!ParserAssist.CheckIsFileCorrect(path, ".html")) return null;

            var doc = new HtmlDocument();
            doc.Load(path);

            var houseList = new List<House>();

            var body = doc.DocumentNode.SelectSingleNode("//tbody");
            var trNodes = body?.SelectNodes(".//tr");
            if (trNodes == null) return null;

            int houseCount = 0;

            foreach (var trNode in trNodes)
            {
                var tdNodes = trNode.SelectNodes(".//td");
                if (tdNodes == null) return null;

                string houseName = tdNodes.First(c => c.GetClasses().Contains("house")).InnerText;
                string flatNum = tdNodes.First(c => c.GetClasses().Contains("number")).InnerText;
                string flatPrice = tdNodes.First(c => c.GetClasses().Contains("price")).InnerText;

                if (houseList.All(house => house.Name != houseName))
                {
                    houseList.Add(new House()
                    {
                        Name = houseName,
                        Flats = new List<Flat>()
                    });
                    if (houseList.Count > 1) houseCount++;
                }

                houseList[houseCount].Flats.Add(new Flat()
                {
                    Number = flatNum,
                    Price = flatPrice
                });
            }

            ParserAssist.SortAndClear(houseList);
            return houseList;
        }
    }
}