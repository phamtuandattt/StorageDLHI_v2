using ComponentFactory.Krypton.Toolkit;
using OfficeOpenXml;
using StorageDLHI.App.Enums;
using StorageDLHI.BLL.ImportDAO;
using StorageDLHI.BLL.MaterialDAO;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.BLL.PoDAO;
using StorageDLHI.BLL.ProductDAO;
using StorageDLHI.BLL.SupplierDAO;
using StorageDLHI.BLL.WarehouseDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor;
using StorageDLHI.Infrastructor.Caches;
using StorageDLHI.Infrastructor.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.Common
{
    public static class DictionaryKey
    {
        public const string MPR_NO = "<<MPR_NO>>";
        public const string PO_NO = "<<PO_NO>>";
        public const string WO_NO = "<<WO_NO>>";
        public const string PROJECT_NAME = "<<PROJECT_NAME>>";
        public const string DATE_EXPORT = "<<DATE_EXPORT>>";
        public const string PREPARED = "<<PREPARED>>";
        public const string REVIEWED = "<<REVIEWED>>";
        public const string BUYER = "<<BUYER>>";
        public const string SUPPLIER_NAME = "<<SUPPLIER_NAME>>";
        public const string SUPPLIER_TEL = "<<SUPPLIER_TEL>>";
        public const string PAYMENT_TERM = "<<PAYMENT_TERM>>";
        public const string AGREEMENT = "<<AGGREMENT>>";
        public const string APPROVED = "<<APPROVED>>";
        public const string ROW_START = "<<ROWS_STAR>>";
        public const string SUPPLIER_EMAIL = "<<SUPPLIER_EMAIL>>";
        public const string SUPPLIER_CERT = "<<SUPPLIER_CERT>>";
    }

    public static class PathManager
    {
        public const string MPR_TEMPLATE_FILE_NAME = "mpr_template_v2.xlsx";
        public const string PO_TEMPLATE_FILE_NAME = "po_temp.xlsx";

        public static string GetPathTemplate(string templateFile)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string templateFileName = templateFile;  // your template file
            string fullPath = Path.Combine(baseDirectory, "TemplateExport", templateFileName);

            // Check if file exists
            if (!File.Exists(fullPath))
            {
                LoggerConfig.Logger.Info($"Template file not found: {fullPath} by {ShareData.UserName}");
            }
            else
            {
                return fullPath;
            }
            return "";
        }
    }

    public static class Common
    {
        public static async void ReloadAllCache()
        {
            CacheManager.ClearCache();
            CacheManager.Add(CacheKeys.IMPORT_PRODUCT_DATATABLE_ALL, await ImportProductDAO.GetImportProducts());
            CacheManager.Add(CacheKeys.POS_DATETABLE_GET_ALL_PO_FOR_IMPORT_PROD, await PoDAO.GetPosForImportProduct());
            CacheManager.Add(CacheKeys.POS_DATETABLE_GET_ALL_PO_FOR_IMPORT_PROD, await PoDAO.GetPosForImportProduct());
            CacheManager.Add(CacheKeys.ORIGIN_DATATABLE_ALLORIGIN, await MaterialDAO.GetOrigins());
            CacheManager.Add(CacheKeys.MATERIAL_TYPE_DATATABLE_ALLTYPE, await MaterialDAO.GetMaterialTypes());
            CacheManager.Add(CacheKeys.STANDARD_DATATABLE_ALLSTANDARD, await MaterialDAO.GetMaterialStandards());
            CacheManager.Add(CacheKeys.TAX_DATATABLE_ALLTAX, await MaterialDAO.GetTaxs());
            CacheManager.Add(CacheKeys.UNIT_DATATABLE_ALLUNIT, await MaterialDAO.GetUnits());
            CacheManager.Add(CacheKeys.COST_DATATABLE_ALLCOST, await MaterialDAO.GetCosts());
            CacheManager.Add(CacheKeys.PRODUCT_DATATABLE_ALL_PRODS_FOR_EPR, await ProductDAO.GetProductsForCreateMPR());
            CacheManager.Add(CacheKeys.MPRS_DATATABLE_ALL_MPRS, await MprDAO.GetMprs());
            CacheManager.Add(CacheKeys.SUPPLIER_DATATABLE_ALL_SUPPLIER, await SupplierDAO.GetSuppliers());
            CacheManager.Add(CacheKeys.POS_DATATABLE_ALL_PO, await PoDAO.GetPOs());
            CacheManager.Add(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS, await MprDAO.GetMprsForMakePO());
            CacheManager.Add(CacheKeys.WAREHOUSE_DATATABLE_ALL, await ShowDialogManager.WithLoader(() => WarehouseDAO.GetWarehouses()));

        }

        public static int ToInt(this KryptonTextBox textBox)
        {
            string raw = CleanInput(textBox.Text);
            return int.TryParse(raw, out var value) ? value : 0;
        }

        public static decimal ToDecimal(this KryptonTextBox textBox)
        {
            string raw = CleanInput(textBox.Text);
            return decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var value)
                ? value
                : 0;
        }

        private static string CleanInput(string input)
        {
            return input.Replace(",", "").Trim();
        }

        public static void FormatDecimalsInputTextChange(object sender)
        {
            KryptonTextBox txt = sender as KryptonTextBox;

            if (string.IsNullOrWhiteSpace(txt.Text)) return;

            // Save cursor position
            int selectionStart = txt.SelectionStart;
            int lengthBefore = txt.Text.Length;

            // Remove existing commas
            string unformatted = txt.Text.Replace(",", "");

            // Validate number
            if (!decimal.TryParse(unformatted, out decimal number)) return;

            // Count how many decimals were typed (to preserve precision)
            int decimalPlaces = 0;
            int indexOfDot = unformatted.IndexOf('.');
            if (indexOfDot >= 0)
                decimalPlaces = unformatted.Length - indexOfDot - 1;

            // Format using N* depending on how many decimals user entered
            string formatString = "N" + decimalPlaces;
            string formatted = number.ToString(formatString);

            // Update text and cursor position
            txt.Text = formatted;
            int lengthAfter = txt.Text.Length;
            txt.SelectionStart = selectionStart + (lengthAfter - lengthBefore);
        }

        public static void FormatDecimalsInputKeyPress(object sender, KeyPressEventArgs e)
        {
            KryptonTextBox txt = sender as KryptonTextBox;

            // Allow control characters (e.g., backspace)
            if (char.IsControl(e.KeyChar))
                return;

            // Allow digits
            if (char.IsDigit(e.KeyChar))
                return;

            // Allow one dot (.)
            if (e.KeyChar == '.' && !txt.Text.Contains("."))
                return;

            // Block all others
            e.Handled = true;
        }

        public static void FormatIntInputsTextChange(object sender, EventArgs e)
        {
            KryptonTextBox txt = sender as KryptonTextBox;

            if (string.IsNullOrWhiteSpace(txt.Text)) return;

            // Save cursor position
            int cursorPos = txt.SelectionStart;
            int lengthBefore = txt.Text.Length;

            // Remove commas
            string unformatted = txt.Text.Replace(",", "");

            // If input is not a valid number, skip formatting
            if (!decimal.TryParse(unformatted, out decimal number)) return;

            // Format with thousands separator (no decimal places)
            string formatted = string.Format("{0:N0}", number);

            // Reassign text and restore cursor position
            txt.Text = formatted;
            int lengthAfter = formatted.Length;
            txt.SelectionStart = cursorPos + (lengthAfter - lengthBefore);
        }

        public static double EvaluateExpression(string expression, Dictionary<string, double> variables)
        {
            string evaluableExpression = expression;

            // Replace variables with their values
            foreach (var variable in variables)
            {
                evaluableExpression = evaluableExpression.Replace(variable.Key, variable.Value.ToString());
            }

            // Use DataTable.Compute to evaluate
            DataTable table = new DataTable();
            var result = table.Compute(evaluableExpression, null);
            return Convert.ToDouble(result);
        }

        public static void ShowNoDataPanel(DataGridView dgvMain, Panel pnlNoData)
        {
            int headerHeight = dgvMain.ColumnHeadersHeight;
            int gridTop = dgvMain.Top;
            int gridLeft = dgvMain.Left;

            pnlNoData.SetBounds(
                gridLeft,
                gridTop + headerHeight,
                dgvMain.Width,
                dgvMain.Height - headerHeight
            );

            pnlNoData.BringToFront();
            pnlNoData.Visible = true;
        }

        public static void HideNoDataPanel(Panel pnlNoData)
        {
            pnlNoData.Visible = false;
        }

        public static DataView SearchDateFrom(DateTime fDate, DataTable dtSource, string property)
        {
            DataView dv = dtSource.DefaultView;
            dv.RowFilter = $"{property} >= '{fDate:dd/MM/yyyy}' ";

            return dv;
        }

        public static DataView SearchDate(DateTime FromDate, DateTime ToDate, DataTable dtSource, List<string> lstProperties)
        {
            DataView dv = dtSource.DefaultView;
            string filter = "";
            foreach (var item in lstProperties)
            {
                if (string.IsNullOrEmpty(filter))
                {
                    filter = $"{item} >= '{FromDate:dd/MM/yyyy}' ";
                }
                else
                {
                    filter += $"AND {item} <= '{ToDate:dd/MM/yyyy}' ";
                }
            }

            dv.RowFilter = filter;

            return dv;
        }

        public static DataView Search(string search, DataTable dtSource, List<string> lstProperty)
        {
            DataView dv = dtSource.DefaultView;
            string filter = "";
            foreach (var item in lstProperty)
            {
                if (string.IsNullOrEmpty(filter))
                {
                    filter = $"{item} LIKE '%{search}%' ";
                }
                else
                {
                    filter += $"OR {item} LIKE '%{search}%' ";
                }
            }
            dv.RowFilter = filter;

            return dv;
        }

        public static void AutoCompleteComboboxValidating(KryptonComboBox sender, CancelEventArgs e)
        {
            var cb = sender as KryptonComboBox;
            string typedText = cb.Text?.Trim();

            if (string.IsNullOrEmpty(typedText))
            {
                cb.SelectedIndex = 0;
                return;
            }

            bool matched = false;
            string displayMember = cb.DisplayMember;

            foreach (var item in cb.Items)
            {
                if (item is DataRowView drv)
                {
                    string value = drv[displayMember]?.ToString();

                    if (value != null && value.Equals(typedText, StringComparison.OrdinalIgnoreCase))
                    {
                        cb.SelectedItem = item;
                        matched = true;
                        break;
                    }
                }
            }

            //if (!matched &&
            //    cb.SelectedItem is DataRowView selected &&
            //    selected[displayMember]?.ToString() != typedText)
            //{
            //    cb.SelectedIndex = 0;
            //}
            if (!matched)
            {
                cb.SelectedIndex = 0;
            }
        }

        public static void StyleFooterCell(DataGridViewCell cellCustom)
        {
            var cell = cellCustom;
            cell.Style.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            cell.Style.ForeColor = Color.Black;
            cell.Style.BackColor = Color.White; // Optional
            cell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        public static void InitializeFooterGrid(DataGridView dgvMain, DataGridView dgvFooter)
        {
            // Clone columns from main grid
            foreach (DataGridViewColumn col in dgvMain.Columns)
            {
                var footerCol = (DataGridViewColumn)col.Clone();
                footerCol.Width = col.Width;
                dgvFooter.Columns.Add(footerCol);
            }

            // Add a single row
            dgvFooter.Rows.Add();

            // Styling
            dgvFooter.ReadOnly = true;
            dgvFooter.AllowUserToAddRows = false;
            dgvFooter.AllowUserToDeleteRows = false;
            dgvFooter.AllowUserToResizeRows = false;
            dgvFooter.AllowUserToResizeColumns = false;
            dgvFooter.RowHeadersVisible = true;
            dgvFooter.ColumnHeadersVisible = false;
            dgvFooter.ScrollBars = ScrollBars.None;
            dgvFooter.DefaultCellStyle.BackColor = Color.LightYellow;

            dgvFooter.ScrollBars = ScrollBars.Horizontal;
            dgvFooter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        }

        public static bool HasHorizontalScrollBar(DataGridView dgv)
        {
            int totalColumnWidth = 0;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                totalColumnWidth += col.Width;
            }

            return totalColumnWidth > dgv.ClientSize.Width;
        }

        public static void AdjustFooterScrollbar(DataGridView dgvMain, DataGridView dgvFooter)
        {
            bool mainHasScroll = HasHorizontalScrollBar(dgvMain);
            bool footerHasScroll = HasHorizontalScrollBar(dgvFooter);

            if (mainHasScroll && !footerHasScroll)
            {
                // Add a dummy column or reduce the height of the footer to simulate space
                dgvFooter.Padding = new Padding(0, 0, 0, SystemInformation.HorizontalScrollBarHeight);
            }
            else
            {
                dgvFooter.Padding = new Padding(0);
            }
        }

        public static void RenderNumbering(object sender, DataGridViewRowPostPaintEventArgs e, Font font)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        public static void HideZeroValueColumns(DataGridView dgv)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                bool allZeros = true;

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!row.IsNewRow && row.Cells[col.Index].Value != null)
                    {
                        if (decimal.TryParse(row.Cells[col.Index].Value.ToString(), out decimal value))
                        {
                            if (value != 0)
                            {
                                allZeros = false;
                                break; // No need to keep checking
                            }
                        }
                        else
                        {
                            // Non-numeric value found; keep the column
                            allZeros = false;
                            break;
                        }
                    }
                }

                // Hide the column if all values are 0
                col.Visible = !allZeros;
            }
        }

        public static async void ExportToExcelTemplate(string templatePath, string outputPath,
            DataTable dtExport, Dictionary<string, string> placeholders, ExportToExcel exportToExcel)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; 

            using (var package = new ExcelPackage(new FileInfo(templatePath)))
            {
                var ws = package.Workbook.Worksheets[0];

                foreach (var cell in ws.Cells[ws.Dimension.Address])
                {
                    if (cell.Value != null && cell.Value is string text)
                    {
                        foreach (var key in placeholders.Keys)
                        {
                            if (text.Contains(key))
                            {
                                text = text.Replace(key, placeholders[key]);
                                cell.Value = text;
                            }
                        }
                    }
                }

                int markerRow = FindMarkerRow(ws, DictionaryKey.ROW_START);

                if (markerRow == 0)
                {
                    MessageBoxHelper.ShowWarning($"Marker '{markerRow}' not found in sheet.");
                    return;
                }

                switch((int)exportToExcel)
                {
                    case 1: InsertProductDataMPRs(ws, markerRow, dtExport); break;
                    case 2: InsertProductDataPOs(ws, markerRow, dtExport); break;
                }

                await ShowDialogManager.WithLoader(() => package.SaveAsAsync(new FileInfo(outputPath)));

                if (File.Exists(outputPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = outputPath,
                        UseShellExecute = true // Required for .xlsx files to open with default app (Excel)
                    });
                }
            }
        }

        private static void InsertProductDataMPRs(ExcelWorksheet ws, int startRow, DataTable dataTable)
        {
            int sampleRowIndex = startRow + 1; // The sample data row with formatting
            int footerStartRow = sampleRowIndex + 1;

            // Insert rows to make room for data
            if (dataTable.Rows.Count > 1)
            {
                ws.InsertRow(footerStartRow, dataTable.Rows.Count, sampleRowIndex);
            }

            // Fill in product data
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                int row = startRow + 1 + i;
                var item = dataTable.Rows[i];

                ws.Cells[row, 1].Value = i + 1;
                ws.Cells[row, 2].Value = item[3].ToString().Trim(); // Name
                ws.Cells[row, 3].Value = item[4].ToString().Trim(); // Des 2
                ws.Cells[row, 4].Value = item[5].ToString().Trim(); // Material
                ws.Cells[row, 5].Value = item[6].ToString().Trim(); // A
                ws.Cells[row, 6].Value = item[7].ToString().Trim(); // B
                ws.Cells[row, 7].Value = item[8].ToString().Trim();
                ws.Cells[row, 8].Value = item[9].ToString().Trim();
                ws.Cells[row, 9].Value = item[10].ToString().Trim();
                ws.Cells[row, 10].Value = item[11].ToString().Trim(); // F
                ws.Cells[row, 11].Value = item[12].ToString().Trim(); // Usage
                ws.Cells[row, 12].Value = item[13].ToString().Trim(); // MPS
                ws.Cells[row, 13].Value = item[14].ToString().Trim(); // Rev
                ws.Cells[row, 14].Value = item[15].ToString().Trim(); // Dwg
                ws.Cells[row, 15].Value = item[16].ToString().Trim(); // Issue
                ws.Cells[row, 16].Value = item[17].ToString().Trim(); // Unit
                ws.Cells[row, 17].Value = Int32.Parse(item[18].ToString().Trim()).ToString("N0"); // Qty
                ws.Cells[row, 18].Value = item[19].ToString().Trim(); // G_Weight
                ws.Cells[row, 19].Value = item[20].ToString().Trim(); // Remarks
            }

            // remove the start row tag row
            ws.DeleteRow(startRow);
        }

        private static void InsertProductDataPOs(ExcelWorksheet ws, int startRow, DataTable dataTable)
        {
            int sampleRowIndex = startRow + 1; // The sample data row with formatting
            int footerStartRow = sampleRowIndex + 1;

            // Insert rows to make room for data
            if (dataTable.Rows.Count > 1)
            {
                ws.InsertRow(footerStartRow, dataTable.Rows.Count, sampleRowIndex);
            }

            // Fill in product data
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                int row = startRow + 1 + i;
                var item = dataTable.Rows[i];

                ws.Cells[row, 1].Value = i + 1;
                ws.Cells[row, 2].Value = item[0].ToString().Trim(); // Code - New
                ws.Cells[row, 3].Value = item[1].ToString().Trim(); // Name
                ws.Cells[row, 4].Value = item[2].ToString().Trim(); // Material
                ws.Cells[row, 5].Value = item[3].ToString().Trim(); // A
                ws.Cells[row, 6].Value = item[4].ToString().Trim(); // B
                ws.Cells[row, 7].Value = item[5].ToString().Trim(); // C
                ws.Cells[row, 8].Value = Int32.Parse(item[6].ToString().Trim()).ToString("N0"); // Qty
                ws.Cells[row, 9].Value = item[7].ToString().Trim();
                ws.Cells[row, 10].Value = item[8].ToString().Trim();
                ws.Cells[row, 11].Value = item[9].ToString().Trim();
                ws.Cells[row, 12].Value = item[10].ToString().Trim(); // 
                ws.Cells[row, 13].Value = item[11].ToString().Trim(); // 
                ws.Cells[row, 14].Value = Int32.Parse(item[12].ToString().Trim()).ToString("N0"); // Price
                ws.Cells[row, 15].Value = Int32.Parse(item[13].ToString().Trim()).ToString("N0"); // Amount
                ws.Cells[row, 16].Value = item[14].ToString().Trim(); // 
                ws.Cells[row, 17].Value = item[15].ToString().Trim(); // Issue
            }
            ws.DeleteRow(startRow);
        }

        private static int FindMarkerRow(ExcelWorksheet ws, string marker)
        {
            foreach (var cell in ws.Cells[ws.Dimension.Address])
            {
                if (cell.Value?.ToString().Trim() == marker)
                {
                    return cell.Start.Row;
                }
            }
            LoggerConfig.Logger.Info($"Marker '{marker}' not found in sheet.");
            return 0;
        }
    }
}
