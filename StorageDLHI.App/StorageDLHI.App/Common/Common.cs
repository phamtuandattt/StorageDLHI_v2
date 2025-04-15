using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
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
