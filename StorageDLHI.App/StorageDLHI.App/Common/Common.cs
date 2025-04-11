using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.Common
{
    public static class Common
    {
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
