using System;
using System.Collections.Generic;
using Xceed.Words.NET;
using Xceed.Document.NET;
using Microsoft.Office.Interop.Word;
using ReportCreater.ViewModels;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace ReportCreater.Models
{
    public static class DocxCreater
    {
        public delegate void ShowMessage(string message);
        public static ShowMessage Message;

        public static async void CreateGeneralMonthReportAsync(List<Client> clients, string month)
        {
            await System.Threading.Tasks.Task.Run(() => CreateGeneralMonthReport(clients,month));
        }
        private static void CreateGeneralMonthReport(List<Client> clients,string month)
        {
            DocX document = DocX.Create($"C:\\Users\\Acer\\Desktop\\Отчеты\\{month}.docx");
            document.InsertParagraph("Рабочая таблица").FontSize(12).Bold().Alignment = Alignment.center;
            document.InsertParagraph($"{month} {DateTime.Now.Year}").FontSize(12).Bold().Alignment = Alignment.center;
            var table = document.AddTable(1, 7);
            table.Rows[0].Cells[0].Paragraphs[0].Append("Дата").Bold().Alignment = Alignment.center;
            table.Rows[0].Cells[1].Paragraphs[0].Append("Клиент").Bold().Alignment = Alignment.center;
            table.Rows[0].Cells[2].Paragraphs[0].Append("Вопрос").Bold().Alignment = Alignment.center;
            table.Rows[0].Cells[3].Paragraphs[0].Append("Время").Bold().Alignment = Alignment.center;
            table.Rows[0].Cells[4].Paragraphs[0].Append("Фикс. р-та").Bold().Alignment = Alignment.center;
            table.Rows[0].Cells[5].Paragraphs[0].Append("Руб/ч").Bold().Alignment = Alignment.center;
            table.Rows[0].Cells[6].Paragraphs[0].Append("Цена").Bold().Alignment = Alignment.center;
            var i = 1;
            foreach (var client in clients)
            {
                foreach (var clientInfo in client.ClientInfoCollection)
                {
                    table.InsertRow();
                    table.Rows[i].Cells[1].Paragraphs[0].Append(client.Name).Alignment = Alignment.center;
                    table.Rows[i].Cells[0].Paragraphs[0].Append(clientInfo.Date).Alignment = Alignment.center;
                    table.Rows[i].Cells[2].Paragraphs[0].Append(clientInfo.Question).Alignment = Alignment.left;
                    table.Rows[i].Cells[3].Paragraphs[0].Append($"{clientInfo.HourCount} ч. {clientInfo.MinuteCount} м.").Alignment = Alignment.center;
                    table.Rows[i].Cells[4].Paragraphs[0].Append(clientInfo.StaticWorkPrice.ToString()).Alignment = Alignment.center;
                    table.Rows[i].Cells[5].Paragraphs[0].Append(client.OneHourePrice.ToString()).Alignment = Alignment.center;
                    table.Rows[i].Cells[6].Paragraphs[0].Append(Math.Round(clientInfo.StaticWorkPrice + (clientInfo.HourCount * 60+clientInfo.MinuteCount) *client.OneHourePrice/60,2).ToString()).Alignment = Alignment.center;
                    i++;
                }
            }
            document.InsertParagraph().InsertTableAfterSelf(table);
            document.InsertParagraph("ИТОГО:").FontSize(14).Alignment = Alignment.left;
            document.InsertParagraph();
            foreach (var client in clients)
                document.InsertParagraph($"{client.Name} - {client.TotalPrice} р").FontSize(12).Bold().Alignment = Alignment.left;
            document.Save();
           Message($"Отчет за {month} сформирован");
        }

        public static async void CreateClientReportAsync(Client client)
        {
            await System.Threading.Tasks.Task.Run(()=>CreateClientReport(client)); 
        }

        private static void CreateClientReport(Client client) 
        {
            DocX document = DocX.Create($"C:\\Users\\Acer\\Desktop\\Отчеты\\{client.Name}.docx");
            document.MarginTop = 60;
            var p = document.InsertParagraph("ЮАБП");
            p.FontSize(39).Bold().Color(Color.SkyBlue);
            p.Alignment = Alignment.center;
            p.IndentationAfter = 350;
            p.IndentationBefore = -20;
            var p1 = document.InsertParagraph("ООО «Юридическое Агентство");
            p1.FontSize(8).Bold();
            p1.Alignment = Alignment.center;
            p1.IndentationAfter = 350;
            p1.IndentationBefore = -20;
            var p2 = document.InsertParagraph("«БИЗНЕС - ПРОФИ»");
            p2.FontSize(12).Bold();
            p2.Alignment = Alignment.center;
            p2.IndentationAfter = 350;
            p2.IndentationBefore = -20;
            p2.LineSpacingAfter = 50;
            var p3 = document.InsertParagraph("Отчет о выполненной работе по клиенту ");
            p3.FontSize(16).Bold();
            p3.Alignment = Alignment.center;
            var p4 = document.InsertParagraph($"«{client.Name}»");
            p4.FontSize(16).Bold();
            p4.Alignment = Alignment.center;
            p4.LineSpacingAfter = 30;
            var p5 = document.InsertParagraph($"Руб/Час – {client.OneHourePrice}");
            p5.FontSize(12).Highlight(Highlight.yellow).Alignment = Alignment.right;
            p5.AppendLine($"Руб/Мин – {Math.Round(client.OneHourePrice / 60, 3)}").Highlight(Highlight.yellow).FontSize(12);
            var table = document.AddTable(1, 4);
            table.SetColumnWidth(0, 85);
            table.SetColumnWidth(1, 250);
            table.SetColumnWidth(2, 85);
            table.SetColumnWidth(3, 81);
            table.Design = TableDesign.MediumList1Accent5;
            table.Rows[0].Cells[0].Paragraphs[0].Append("Дата").FontSize(14).Bold().Alignment = Alignment.left;
            table.Rows[0].Cells[1].Paragraphs[0].Append("Выполненные работы").FontSize(14).Bold().Alignment = Alignment.left;
            table.Rows[0].Cells[2].Paragraphs[0].Append("Часы").FontSize(14).Bold().Alignment = Alignment.left;
            table.Rows[0].Cells[3].Paragraphs[0].Append("Цена").FontSize(14).Bold().Alignment = Alignment.left;
            var i = 1;
            foreach (var clientInfo in client.ClientInfoCollection)
            {
                table.InsertRow();
                table.Rows[i].Cells[0].Paragraphs[0].Append(clientInfo.Date).FontSize(13).Alignment = Alignment.left;
                table.Rows[i].Cells[1].Paragraphs[0].Append(clientInfo.Question).FontSize(13).Alignment = Alignment.left;
                table.Rows[i].Cells[2].Paragraphs[0].Append($"{clientInfo.HourCount} ч. {clientInfo.MinuteCount} мин.").FontSize(13).Alignment = Alignment.left;
                table.Rows[i].Cells[3].Paragraphs[0].Append(Math.Round((clientInfo.StaticWorkPrice + (clientInfo.HourCount * 60 + clientInfo.MinuteCount) * client.OneHourePrice / 60),3).ToString()).FontSize(13).Alignment = Alignment.left;
                i++;
            }
            document.InsertParagraph().InsertTableAfterSelf(table);
            var p6 = document.InsertParagraph($"ИТОГО: {client.TotalPrice} р");
            p6.LineSpacingBefore = 50;
            p6.FontSize(16).Bold().Color(Color.Red);
            p6.Alignment = Alignment.left;
            document.Save();
            document.Dispose();
            ConvertToPdf(client);
            Message($"Отчет по клиенту {client.Name} сформирован");
        }
        private static void ConvertToPdf(Client client)
        {
            Application word = new Application();
            var document1 = word.Documents.Open($"C:\\Users\\Acer\\Desktop\\Отчеты\\{client.Name}.docx");
            document1.ExportAsFixedFormat($"C:\\Users\\Acer\\Desktop\\Отчеты\\{client.Name}.pdf", WdExportFormat.wdExportFormatPDF);
            word.Quit();
        }
    }
}
