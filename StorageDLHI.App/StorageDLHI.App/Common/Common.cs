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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
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
        public const string MPR_TEMPLATE_PATH = "C:\\Users\\TUAN DAT\\Desktop\\Template\\mpr_temp.xlsx";
        public const string PO_TEMAPLATE_PATH = "C:\\Users\\TUAN DAT\\Desktop\\Template\\po_temp.xlsx";

    }

    public static class Common
    {
        public static async void ReloadAllCache()
        {
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
            KryptonComboBox cb = sender as KryptonComboBox;
            string typed = cb.Text;

            bool match = false;
            foreach (object item in cb.Items)
            {
                if (item.ToString().Equals(typed, StringComparison.OrdinalIgnoreCase))
                {
                    cb.SelectedItem = item;
                    match = true;
                    break;
                }
            }

            if (!match)
            {
                cb.SelectedIndex = 0; // Default if no match
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
                ws.Cells[row, 2].Value = item[3]; // Name
                ws.Cells[row, 3].Value = item[4]; // Des 2
                ws.Cells[row, 4].Value = item[5]; // Material
                ws.Cells[row, 5].Value = item[6]; // A
                ws.Cells[row, 6].Value = item[7]; // B
                ws.Cells[row, 7].Value = item[8];
                ws.Cells[row, 8].Value = item[9];
                ws.Cells[row, 9].Value = item[10];
                ws.Cells[row, 10].Value = item[11]; // F
                ws.Cells[row, 11].Value = item[12]; // Usage
                ws.Cells[row, 12].Value = item[13]; // MPS
                ws.Cells[row, 13].Value = item[14]; // Rev
                ws.Cells[row, 14].Value = item[15]; // Dwg
                ws.Cells[row, 15].Value = item[16]; // Issue
                ws.Cells[row, 16].Value = item[17]; // Unit
                ws.Cells[row, 17].Value = item[18]; // Qty
                ws.Cells[row, 18].Value = item[19]; // G_Weight
                ws.Cells[row, 19].Value = item[20]; // Remarks
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
                ws.Cells[row, 2].Value = item[0]; // Name
                ws.Cells[row, 3].Value = item[1]; // Des 2
                ws.Cells[row, 4].Value = item[2]; // Material
                ws.Cells[row, 5].Value = item[3]; // A
                ws.Cells[row, 6].Value = item[4]; // B
                ws.Cells[row, 7].Value = item[5];
                ws.Cells[row, 8].Value = item[6];
                ws.Cells[row, 9].Value = item[7];
                ws.Cells[row, 10].Value = item[8];
                ws.Cells[row, 11].Value = item[9]; // G
                ws.Cells[row, 12].Value = item[10]; // Qty
                ws.Cells[row, 13].Value = item[11]; // Usage
                ws.Cells[row, 14].Value = item[12]; // Issue.
                ws.Cells[row, 15].Value = item[13]; // Issue
                ws.Cells[row, 16].Value = item[14]; // Issue
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
            throw new Exception($"Marker '{marker}' not found in sheet.");
        }
    }
}
