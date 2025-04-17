using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.BLL.ImportDAO;
using StorageDLHI.BLL.MaterialDAO;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.BLL.PoDAO;
using StorageDLHI.BLL.ProductDAO;
using StorageDLHI.BLL.SupplierDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Caches;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.Common
{
    public static class Common
    {
        public static void ReloadAllCache()
        {
            CacheManager.Add(CacheKeys.IMPORT_PRODUCT_DATATABLE_ALL, ImportProductDAO.GetImportProducts());
            CacheManager.Add(CacheKeys.POS_DATETABLE_GET_ALL_PO_FOR_IMPORT_PROD, PoDAO.GetPosForImportProduct());
            CacheManager.Add(CacheKeys.POS_DATETABLE_GET_ALL_PO_FOR_IMPORT_PROD, PoDAO.GetPosForImportProduct());
            CacheManager.Add(CacheKeys.ORIGIN_DATATABLE_ALLORIGIN, MaterialDAO.GetOrigins());   
            CacheManager.Add(CacheKeys.MATERIAL_TYPE_DATATABLE_ALLTYPE, MaterialDAO.GetMaterialTypes());
            CacheManager.Add(CacheKeys.STANDARD_DATATABLE_ALLSTANDARD, MaterialDAO.GetMaterialStandards());
            CacheManager.Add(CacheKeys.TAX_DATATABLE_ALLTAX, MaterialDAO.GetTaxs());
            CacheManager.Add(CacheKeys.UNIT_DATATABLE_ALLUNIT, MaterialDAO.GetUnits());
            CacheManager.Add(CacheKeys.COST_DATATABLE_ALLCOST, MaterialDAO.GetCosts());
            CacheManager.Add(CacheKeys.PRODUCT_DATATABLE_ALL_PRODS_FOR_EPR, ProductDAO.GetProductsForCreateMPR());
            CacheManager.Add(CacheKeys.MPRS_DATATABLE_ALL_MPRS, MprDAO.GetMprs());
            CacheManager.Add(CacheKeys.SUPPLIER_DATATABLE_ALL_SUPPLIER, SupplierDAO.GetSuppliers());
            CacheManager.Add(CacheKeys.POS_DATATABLE_ALL_PO, PoDAO.GetPOs());
            CacheManager.Add(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS, MprDAO.GetMprsForMakePO());
            
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
    }
}
