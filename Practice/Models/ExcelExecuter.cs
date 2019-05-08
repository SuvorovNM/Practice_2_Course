using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML;
using ClosedXML.Excel;

namespace Practice.Models
{
    public class ExcelExecuter
    {
        public static void XLOutput_NamePlusCountBooks(string path, List<NamePlusCountBooks> data)
        //Вывод таблицы data в файл с путем path
        //path - путь к файлу
        //data - названия столбцов и данные
        //Rows - количество строк
        //Cols - количество столбцов
        {
            //Проверка, что data - не пустой массив
            if (data== null)
                return;
            //Создание страницы Excel с названием Library_Report
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Library_Report");
            //Перенос значений из массива data в страницу Excel                        
            worksheet.Cell(1, 1).Value = "ФИО";
            worksheet.Cell(1, 2).Value = "Чит. билет";
            worksheet.Cell(1, 3).Value = "Телефон";
            worksheet.Cell(1, 4).Value = "Email";
            worksheet.Cell(1, 5).Value = "Дата регистрации";
            worksheet.Cell(1, 6).Value = "Количество книг";
            for (int i = 1; i <= 6; i++)
            {
                worksheet.Cell(1, i).Style.Font.Bold = true;
                worksheet.Column(i).Style.Border.SetRightBorder(XLBorderStyleValues.Double);
                //worksheet.Column(i).Width = 50;                
            }
            for (int i = 0; i < data.Count(); i++)
            {
                worksheet.Cell(i + 2, 1).Value = "'" + data[i].FIO;
                worksheet.Cell(i + 2, 2).Value = "'" + data[i].Library_Card;
                worksheet.Cell(i + 2, 3).Value = "'" + data[i].Phone_Number;
                worksheet.Cell(i + 2, 4).Value = "'" + data[i].Email;
                worksheet.Cell(i + 2, 5).Value = "'" + data[i].Registration_Date;
                worksheet.Cell(i + 2, 6).Value = "'" + data[i].Count;
            }
            for (int i = 1; i <= 6; i++)
                worksheet.Column(i).AdjustToContents();
            //Сохранение созданного документа Excel по пути path
            workbook.SaveAs(path);
        }
        public static void XLOutput_BooksPop(string path, List<BooksPopularity> data)
        //Вывод таблицы data в файл с путем path
        //path - путь к файлу
        //data - названия столбцов и данные
        //Rows - количество строк
        //Cols - количество столбцов
        {
            //Проверка, что data - не пустой массив
            if (data == null)
                return;
            //Создание страницы Excel с названием Library_Report
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Library_Report");
            //Перенос значений из массива data в страницу Excel                        
            worksheet.Cell(1, 1).Value = "Название";
            worksheet.Cell(1, 2).Value = "Автор";
            worksheet.Cell(1, 3).Value = "Издание";
            worksheet.Cell(1, 4).Value = "Год";
            worksheet.Cell(1, 5).Value = "Количество выдач";
            for (int i = 1; i <= 5; i++)
            {
                worksheet.Cell(1, i).Style.Font.Bold = true;
                worksheet.Column(i).Style.Border.SetRightBorder(XLBorderStyleValues.Double);
                //worksheet.Column(i).Width = 50;                
            }
            for (int i = 0; i < data.Count(); i++)
            {
                worksheet.Cell(i + 2, 1).Value = "'" + data[i].Name;
                worksheet.Cell(i + 2, 2).Value = "'" + data[i].Author;
                worksheet.Cell(i + 2, 3).Value = "'" + data[i].Publisher_Name;
                worksheet.Cell(i + 2, 4).Value = "'" + data[i].Year;
                worksheet.Cell(i + 2, 5).Value = "'" + data[i].Count;
            }
            for (int i = 1; i <= 5; i++)
                worksheet.Column(i).AdjustToContents();
            //Сохранение созданного документа Excel по пути path
            workbook.SaveAs(path);
        }
        public static void XLOutput_PenaltyDebts(string path, List<PenaltyDebtors> data)
        //Вывод таблицы data в файл с путем path
        //path - путь к файлу
        //data - названия столбцов и данные
        //Rows - количество строк
        //Cols - количество столбцов
        {
            //Проверка, что data - не пустой массив
            if (data == null)
                return;
            //Создание страницы Excel с названием Library_Report
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Library_Report");
            //Перенос значений из массива data в страницу Excel                        
            worksheet.Cell(1, 1).Value = "ФИО";
            worksheet.Cell(1, 2).Value = "Дата рождения";
            worksheet.Cell(1, 3).Value = "Телефон";
            worksheet.Cell(1, 4).Value = "Email";
            worksheet.Cell(1, 5).Value = "Сумма долга";
            for (int i = 1; i <= 5; i++)
            {
                worksheet.Cell(1, i).Style.Font.Bold = true;
                worksheet.Column(i).Style.Border.SetRightBorder(XLBorderStyleValues.Double);
                //worksheet.Column(i).Width = 50;                
            }
            for (int i = 0; i < data.Count(); i++)
            {
                worksheet.Cell(i + 2, 1).Value = "'" + data[i].FIO;
                worksheet.Cell(i + 2, 2).Value = "'" + data[i].Bday;
                worksheet.Cell(i + 2, 3).Value = "'" + data[i].Phone;
                worksheet.Cell(i + 2, 4).Value = "'" + data[i].Email;
                worksheet.Cell(i + 2, 5).Value = "'" + data[i].Sum;
            }
            for (int i = 1; i <= 5; i++)
                worksheet.Column(i).AdjustToContents();
            //Сохранение созданного документа Excel по пути path
            workbook.SaveAs(path);
        }
        public static void XLOutput_BooksByDate(string path, List<BooksByDate> data)
        //Вывод таблицы data в файл с путем path
        //path - путь к файлу
        //data - названия столбцов и данные
        //Rows - количество строк
        //Cols - количество столбцов
        {
            //Проверка, что data - не пустой массив
            if (data == null)
                return;
            //Создание страницы Excel с названием Library_Report
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Library_Report");
            //Перенос значений из массива data в страницу Excel                        
            worksheet.Cell(1, 1).Value = "Чит. билет";
            worksheet.Cell(1, 2).Value = "ФИО читателя";
            worksheet.Cell(1, 3).Value = "Код книги";
            worksheet.Cell(1, 4).Value = "Название книги";
            worksheet.Cell(1, 5).Value = "Автор";
            worksheet.Cell(1, 6).Value = "ФИО библиотекаря";
            worksheet.Cell(1, 7).Value = "Выдано до";
            for (int i = 1; i <= 7; i++)
            {
                worksheet.Cell(1, i).Style.Font.Bold = true;
                worksheet.Column(i).Style.Border.SetRightBorder(XLBorderStyleValues.Double);
                //worksheet.Column(i).Width = 50;                
            }
            for (int i = 0; i < data.Count(); i++)
            {
                worksheet.Cell(i + 2, 1).Value = "'" + data[i].LibTicket;
                worksheet.Cell(i + 2, 2).Value = "'" + data[i].Reader_FIO;
                worksheet.Cell(i + 2, 3).Value = "'" + data[i].Book_ID;
                worksheet.Cell(i + 2, 4).Value = "'" + data[i].Book_Name;
                worksheet.Cell(i + 2, 5).Value = "'" + data[i].Book_Author;
                worksheet.Cell(i + 2, 6).Value = "'" + data[i].LibFIO;
                worksheet.Cell(i + 2, 7).Value = "'" + data[i].ExpRetDate;
            }
            for (int i = 1; i <= 7; i++)
                worksheet.Column(i).AdjustToContents();
            //Сохранение созданного документа Excel по пути path
            workbook.SaveAs(path);
        }
    }
}