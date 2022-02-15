using ClosedXML.Excel;
using NmarketTestTask.Models;
using System.Collections.Generic;
using System.Linq;

namespace NmarketTestTask.Parsers
{
    /// <summary>
    /// Класс парсера Excel файлов
    /// </summary>
    public class ExcelParser : IParser
    {
        private IXLWorksheet _sheet;

        /// <summary>
        /// Метод получения домов из Excel файла
        /// </summary>
        /// <param name="path">Путь до Excel файла</param>
        /// <returns>Список домов с перечислением квартир и цен на них</returns>
        public IList<House> GetHouses(string path)
        {
            if (!ParserAssist.CheckIsFileCorrect(path, ".xls")) return null;

            var workbook = new XLWorkbook(path);
            _sheet = workbook.Worksheets.First();

            // если лист пустой, освобождаем память и возвращаем null
            if (_sheet.RangeUsed() == null)
            {
                _sheet.Dispose();
                workbook.Dispose();
                return null;
            }

            var houseCells = _sheet.Cells().Where(c => c.GetValue<string>().Contains("Дом")).ToList();

            var houseList = GetHouseList(houseCells);

            // освобождаем память
            _sheet.Dispose();
            workbook.Dispose();

            ParserAssist.SortAndClear(houseList);
            return houseList;
        }

        /// <summary>
        /// Метод получения списка домов
        /// </summary>
        /// <param name="houseCells">Список ячеек для анализа</param>
        /// <returns>Список домов</returns>
        private List<House> GetHouseList(List<IXLCell> houseCells)
        {
            var houseList = new List<House>();

            // последний столбец и последняя строка, чтобы в дальнейшем минимизировать просматриваемый диапазон
            int lastRow = _sheet.RangeUsed().RowCount() + 1;
            int lastColumn = _sheet.RangeUsed().ColumnCount() + 1;

            for (int i = 0; i < houseCells.Count; i++)
            {
                houseList.Add(new House()
                {
                    Name = ParserAssist.GetNumByString(houseCells[i].GetValue<string>()),
                });

                int houseStartRowNum = houseCells[i].Address.RowNumber;
                int nextStartHouseRowNum = i + 1 < houseCells.Count ? houseCells[i + 1].Address.RowNumber - 1 : lastRow;

                var houseCellsRange = _sheet.Range(houseStartRowNum, 1, nextStartHouseRowNum, lastColumn);
                var flatCells = houseCellsRange.Cells().Where(c => c.GetValue<string>().Contains("№")).ToList();

                houseList[i].Flats = GetFlatList(flatCells);
            }

            return houseList;
        }

        /// <summary>
        /// Метод получения списка квартир
        /// </summary>
        /// <param name="flatCells">Диапазон ячеек для анализа</param>
        /// <returns>Список квартир</returns>
        private List<Flat> GetFlatList(List<IXLCell> flatCells)
        {
            var flatList = new List<Flat>();
            foreach (var flatCell in flatCells)
            {
                flatList.Add(new Flat()
                {
                    Number = ParserAssist.GetNumByString(flatCell.GetValue<string>()),
                    Price = _sheet.Cell(flatCell.Address.RowNumber + 1, flatCell.Address.ColumnNumber).GetValue<string>()
                });
            }

            return flatList;
        }
    }
}