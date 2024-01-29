﻿using System;
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
            this.KeyDown += ShipControl_KeyDown; // Add key-down event
            this.Click += ShipControl_Click; // Add click event
        }

        private void ShipControl_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            offset = new Point(Width / 2, Height / 2);
            // Setting the offset to half of the width and height to center the ship control

            // Adjust the location so the ship is centered on the mouse
            this.Location = new Point(e.X - offset.X, e.Y - offset.Y);
        }

        private void ShipControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newLocation = ((Control)sender).PointToScreen(new Point(e.X, e.Y));
                newLocation.Offset(-offset.X, -offset.Y);

                this.Location = this.Parent.PointToClient(newLocation);
            }
        }

        private void ShipControl_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void ShipControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (isDragging)
            {
                if (e.KeyCode == Keys.R)
                {
                    // Swap height and width on 'R' key press
                    int temp = this.Width;
                    this.Width = this.Height;
                    this.Height = temp;

                    offset = new Point(Width / 2, Height / 2);
                    // Setting the offset to half of the width and height to center the ship control

                    // Adjust the location so the ship is centered on the mouse
                    this.Location = new Point(MousePosition.X - offset.X, MousePosition.Y - offset.Y);
                }
            }
        }

        private void ShipControl_Click(object sender, EventArgs e)
        {
            // Handle click event if needed
        }
    }
}