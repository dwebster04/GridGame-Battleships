using System;
using System.Drawing;
using System.Windows.Forms;

// ShipControl.cs
// Responsible for the ship objects and how they are interacted with by the user

namespace GridGame_Battleships
{
    public class ShipControl : Button
    {
        private bool isDragging = false;
        private Point offset;
        private Color originalColor;
        public int[,] boardLocation = new int[7, 7];

        public ShipControl()
        {
            // Set default properties for the ship control
            this.Size = new Size(100, 40);
            this.BackColor = Color.Gray;
            this.originalColor = this.BackColor; // store the original color
            this.Text = "Ship"; // default text
            this.MouseDown += ShipControl_MouseDown;
            this.MouseMove += ShipControl_MouseMove;
            this.MouseUp += ShipControl_MouseUp;
            this.KeyDown += ShipControl_KeyDown; // add key-down event
            this.Click += ShipControl_Click; // add click event
        }

        // what happens when mouse is pressed down on a ship
        private void ShipControl_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true; // set draggin to true

            offset = new Point(Width / 2, Height / 2); 

            // adjust the location so the ship is centered on the mouse
            this.Location = new Point(e.X - offset.X, e.Y - offset.Y);

            // change color to pink while dragging
            this.BackColor = Color.MediumOrchid;
        }

        // when the mouse moves
        private void ShipControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging) // if its dragging/mouse held down keep the ship on the mouse position
            {
                Point newLocation = ((Control)sender).PointToScreen(new Point(e.X, e.Y));
                newLocation.Offset(-offset.X, -offset.Y);

                this.Location = this.Parent.PointToClient(newLocation);
            }
        }

        // when the mouse click stops being held
        private void ShipControl_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false; // set dragging to false

            // revert to the original color when the mouse is released
            this.BackColor = originalColor;
        }

        // used to find closest point on the grid
        public static int CalculateDistance(Point point1, Point point2)
        {
            int dx = point1.X - point2.X;
            int dy = point1.Y - point2.Y;
            return (int)Math.Sqrt(dx * dx + dy * dy);
        }

        // when dragging and r is pressed rotate the ships
        private void ShipControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (isDragging)
            {
                if (e.KeyCode == Keys.R)
                {
                    // swap height and width on 'R' key press
                    int temp = this.Width;
                    this.Width = this.Height;
                    this.Height = temp;

                    offset = new Point(Width / 2, Height / 2);
      
                    // adjust the location so the ship is centered on the mouse
                    this.Location = new Point(MousePosition.X - offset.X, MousePosition.Y - offset.Y);
                }
            }
        }

        // NOT NEEDED
        private void ShipControl_Click(object sender, EventArgs e)
        {
            // handle click event if needed
        }
    }
}