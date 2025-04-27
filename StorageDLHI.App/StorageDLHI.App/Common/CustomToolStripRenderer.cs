using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.Common
{
    public class CustomToolStripRenderer : ToolStripProfessionalRenderer
    {
        public ToolStripButton ActiveButton { get; set; }
        public ToolStripButton HoveredButton { get; set; }


        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            ToolStripButton button = e.Item as ToolStripButton;

            int w = button.Bounds.Width;
            int h = button.Bounds.Height;
            int cornerRadius = 14;
            int sideCurveRadius = h / 2;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Determine color
            Color fillColor;
            if (button == ActiveButton)
                fillColor = Color.FromArgb(183, 219, 255); // Active: 
            else if (button == HoveredButton)
                fillColor = Color.FromArgb(220, 240, 255); // Hover: light blue
            else
                fillColor = Color.White;

            using (GraphicsPath path = new GraphicsPath())
            {
                // Top-left corner
                path.AddArc(0, 0, cornerRadius * 2, cornerRadius * 2, 180, 90);
                // Top edge
                path.AddLine(cornerRadius, 0, w - cornerRadius, 0);
                // Top-right corner
                path.AddArc(w - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2, 270, 90);

                // Right convex curve (semicircle)
                path.AddArc(w - sideCurveRadius, h / 2 - sideCurveRadius, sideCurveRadius * 2, sideCurveRadius * 2, 270, 180);

                // Left convex curve (semicircle)
                path.AddArc(-sideCurveRadius, h / 2 - sideCurveRadius, sideCurveRadius * 2, sideCurveRadius * 2, 90, 180);

                path.CloseFigure();

                // Fill background
                using (SolidBrush brush = new SolidBrush(fillColor))
                    g.FillPath(brush, path);

                // Draw border
                using (Pen borderPen = new Pen(Color.White))
                    g.DrawPath(borderPen, path);
            }

            // Draw text
            TextRenderer.DrawText(
                g,
                button.Text,
                button.Font,
                button.Bounds,
                button.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(Color.White)) // Dark blue/purple
            {
                e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
        }
    }
}
