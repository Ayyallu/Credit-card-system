using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ClosedXML.Excel;

namespace CC_Ingenio.Manager
{
    /// <summary>
    /// Reads the "transaction pulling" workbook that BillingManager.ImportFromExcel
    /// loads into [TR_TransactionPulling]. Replaces the legacy
    /// <c>select ... from [Sheet1$]</c> through the Microsoft.ACE.OLEDB.12.0
    /// provider (HDR=Yes), which is a separately installed, Windows-only,
    /// bitness-sensitive driver. Same contract: sheet "Sheet1", first row is
    /// the header, the same 11 columns come back in the same order (the bulk
    /// copy maps them to the destination table by ordinal position), and cell
    /// values keep their native types (number, date, text, bool) the way the
    /// OLEDB reader returned them.
    /// </summary>
    public static class TransactionPullingWorkbookReader
    {
        public const string SheetName = "Sheet1";

        public static readonly string[] Columns =
        {
            "AuthCode", "CardNumber", "IncomingTransTypeId", "Amount", "BatchNo",
            "TransactionDate", "MerchantCode", "MCCCode", "ARNNo", "IsPulled", "RefNo"
        };

        public static DataTable Read(string path)
        {
            using (var workbook = new XLWorkbook(path))
            {
                if (!workbook.TryGetWorksheet(SheetName, out IXLWorksheet sheet))
                {
                    throw new InvalidDataException("Workbook has no sheet named '" + SheetName + "'.");
                }

                var table = new DataTable("TransactionPulling");
                foreach (var column in Columns)
                {
                    table.Columns.Add(column, typeof(object));
                }

                var header = sheet.FirstRowUsed();
                if (header == null)
                {
                    return table;
                }

                var columnNumbers = MapColumns(header);

                foreach (var row in sheet.RowsUsed())
                {
                    if (row.RowNumber() <= header.RowNumber())
                    {
                        continue;
                    }

                    var values = new object[Columns.Length];
                    var hasData = false;
                    for (var i = 0; i < Columns.Length; i++)
                    {
                        var value = ToValue(row.Cell(columnNumbers[i]).Value);
                        hasData |= value != null;
                        values[i] = value ?? DBNull.Value;
                    }

                    // The OLEDB reader never produced rows for fully blank lines.
                    if (hasData)
                    {
                        table.Rows.Add(values);
                    }
                }

                return table;
            }
        }

        private static int[] MapColumns(IXLRow header)
        {
            var byName = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var cell in header.CellsUsed())
            {
                byName[cell.GetString().Trim()] = cell.Address.ColumnNumber;
            }

            var numbers = new int[Columns.Length];
            var missing = new List<string>();
            for (var i = 0; i < Columns.Length; i++)
            {
                if (!byName.TryGetValue(Columns[i], out numbers[i]))
                {
                    missing.Add(Columns[i]);
                }
            }

            if (missing.Count > 0)
            {
                throw new InvalidDataException(
                    "Sheet '" + SheetName + "' is missing column(s): " + string.Join(", ", missing));
            }

            return numbers;
        }

        private static object ToValue(XLCellValue value)
        {
            switch (value.Type)
            {
                case XLDataType.Blank:
                    return null;
                case XLDataType.Boolean:
                    return value.GetBoolean();
                case XLDataType.Number:
                    return value.GetNumber();
                case XLDataType.DateTime:
                    return value.GetDateTime();
                case XLDataType.TimeSpan:
                    return value.GetTimeSpan();
                case XLDataType.Text:
                    return value.GetText();
                default:
                    return value.ToString();
            }
        }
    }
}
