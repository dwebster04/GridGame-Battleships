using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridGame_Battleships
{
    public class ShipControl : Button
    {
        private bool isDragging = false;
        private Point offset;

        public ShipControl()
        {
            // Set default properties for the ship control
            this.Size = new Size(100, 40);
            this.BackColor = Color.Gray;
            this.Text = "Ship";
            this.MouseDown += ShipControl_MouseDown;
            this.MouseMove += ShipControl_MouseMove;
            this.MouseUp += ShipControl_MouseUp;
        }

        private void ShipControl_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            offset = new Point(e.X, e.Y);
        }

        private void ShipControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newLocation = ((Control)sender).PointToScreen(new Point(e.X, e.Y));
                this.Location = this.Parent.PointToClient(newLocation);
            }
        }

        private void ShipControl_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }
}