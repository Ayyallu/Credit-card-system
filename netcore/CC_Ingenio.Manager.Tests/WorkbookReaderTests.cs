using System;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CC_Ingenio.Manager.Tests
{
    [TestClass]
    public class WorkbookReaderTests
    {
        private string _path;

        [TestInitialize]
        public void Setup()
        {
            _path = Path.Combine(Path.GetTempPath(), "pulling_" + Guid.NewGuid().ToString("N") + ".xlsx");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
        }

        private static readonly string[] Headers =
        {
            "AuthCode", "CardNumber", "IncomingTransTypeId", "Amount", "BatchNo",
            "TransactionDate", "MerchantCode", "MCCCode", "ARNNo", "IsPulled", "RefNo"
        };

        private void WriteWorkbook(string sheetName, string[] headers, Action<IXLWorksheet> rows)
        {
            using (var workbook = new XLWorkbook())
            {
                var sheet = workbook.AddWorksheet(sheetName);
                for (var i = 0; i < headers.Length; i++)
                {
                    sheet.Cell(1, i + 1).Value = headers[i];
                }
                rows(sheet);
                workbook.SaveAs(_path);
            }
        }

        [TestMethod]
        public void Returns_The_Eleven_Columns_In_Bulk_Copy_Order_With_Native_Types()
        {
            var date = new DateTime(2025, 3, 14, 0, 0, 0);
            WriteWorkbook("Sheet1", Headers, sheet =>
            {
                sheet.Cell(2, 1).Value = "A123";
                sheet.Cell(2, 2).Value = "4111111111111111";
                sheet.Cell(2, 3).Value = 13;
                sheet.Cell(2, 4).Value = 1250.75;
                sheet.Cell(2, 5).Value = "B9";
                sheet.Cell(2, 6).Value = date;
                sheet.Cell(2, 7).Value = "MER001";
                sheet.Cell(2, 8).Value = "5411";
                sheet.Cell(2, 9).Value = "ARN-1";
                sheet.Cell(2, 10).Value = false;
                sheet.Cell(2, 11).Value = "R-7";
            });

            var table = TransactionPullingWorkbookReader.Read(_path);

            CollectionAssert.AreEqual(Headers, new[]
            {
                table.Columns[0].ColumnName, table.Columns[1].ColumnName, table.Columns[2].ColumnName,
                table.Columns[3].ColumnName, table.Columns[4].ColumnName, table.Columns[5].ColumnName,
                table.Columns[6].ColumnName, table.Columns[7].ColumnName, table.Columns[8].ColumnName,
                table.Columns[9].ColumnName, table.Columns[10].ColumnName
            });
            Assert.AreEqual(1, table.Rows.Count);

            var row = table.Rows[0];
            Assert.AreEqual("A123", row["AuthCode"]);
            Assert.AreEqual(13d, row["IncomingTransTypeId"]);
            Assert.AreEqual(1250.75d, row["Amount"]);
            Assert.AreEqual(date, row["TransactionDate"]);
            Assert.AreEqual(false, row["IsPulled"]);
            Assert.AreEqual("R-7", row["RefNo"]);
        }

        [TestMethod]
        public void Finds_Columns_By_Header_Name_Regardless_Of_Order_Or_Extra_Columns()
        {
            var shuffled = new[] { "Extra", "RefNo", "Amount", "AuthCode", "CardNumber", "IncomingTransTypeId",
                "BatchNo", "TransactionDate", "MerchantCode", "MCCCode", "ARNNo", "IsPulled" };
            WriteWorkbook("Sheet1", shuffled, sheet =>
            {
                sheet.Cell(2, 1).Value = "ignored";
                sheet.Cell(2, 2).Value = "ref";
                sheet.Cell(2, 3).Value = 5;
                sheet.Cell(2, 4).Value = "auth";
            });

            var table = TransactionPullingWorkbookReader.Read(_path);

            Assert.AreEqual(1, table.Rows.Count);
            Assert.AreEqual("auth", table.Rows[0]["AuthCode"]);
            Assert.AreEqual("ref", table.Rows[0]["RefNo"]);
            Assert.AreEqual(5d, table.Rows[0]["Amount"]);
            Assert.AreEqual(DBNull.Value, table.Rows[0]["CardNumber"]);
        }

        [TestMethod]
        public void Header_Match_Ignores_Case_And_Surrounding_Whitespace()
        {
            var odd = new[] { " authcode ", "CARDNUMBER", "incomingtranstypeid", "amount", "batchno",
                "transactiondate", "merchantcode", "mcccode", "arnno", "ispulled", "refno" };
            WriteWorkbook("Sheet1", odd, sheet => sheet.Cell(2, 1).Value = "x");

            var table = TransactionPullingWorkbookReader.Read(_path);

            Assert.AreEqual("x", table.Rows[0]["AuthCode"]);
        }

        [TestMethod]
        public void Skips_Fully_Blank_Rows()
        {
            WriteWorkbook("Sheet1", Headers, sheet =>
            {
                sheet.Cell(2, 1).Value = "first";
                sheet.Cell(4, 1).Value = "third";
            });

            var table = TransactionPullingWorkbookReader.Read(_path);

            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual("first", table.Rows[0]["AuthCode"]);
            Assert.AreEqual("third", table.Rows[1]["AuthCode"]);
        }

        [TestMethod]
        public void Header_Only_Sheet_Yields_No_Rows_But_Keeps_Columns()
        {
            WriteWorkbook("Sheet1", Headers, sheet => { });

            var table = TransactionPullingWorkbookReader.Read(_path);

            Assert.AreEqual(0, table.Rows.Count);
            Assert.AreEqual(11, table.Columns.Count);
        }

        [TestMethod]
        public void Missing_Column_Is_Reported_By_Name()
        {
            var missing = new[] { "AuthCode", "CardNumber", "Amount" };
            WriteWorkbook("Sheet1", missing, sheet => sheet.Cell(2, 1).Value = "x");

            var ex = Assert.ThrowsException<InvalidDataException>(() => TransactionPullingWorkbookReader.Read(_path));

            StringAssert.Contains(ex.Message, "IncomingTransTypeId");
            StringAssert.Contains(ex.Message, "RefNo");
        }

        [TestMethod]
        public void Wrong_Sheet_Name_Is_Reported()
        {
            WriteWorkbook("Data", Headers, sheet => sheet.Cell(2, 1).Value = "x");

            var ex = Assert.ThrowsException<InvalidDataException>(() => TransactionPullingWorkbookReader.Read(_path));

            StringAssert.Contains(ex.Message, "Sheet1");
        }
    }
}
