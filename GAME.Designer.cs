using System.Drawing;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Sockets;

namespace GridGame_Battleships
{
    partial class GAME
    {
        public Button[,] btn = new Button[7, 7];
        public ShipControl[,] ships;

        private System.ComponentModel.IContainer components = null;

        private Button btnCheck; // Declare the Submit button

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 375);
            this.Text = "GAME";

            // Call the method to create buttons
            CreateButtons();

            // Create and initialize the Submit button
            btnCheck = new Button();
            btnCheck.Text = "Check";
            btnCheck.Size = new Size(100, 50);
            btnCheck.Location = new Point(375, 25); // Adjust the location as needed
            btnCheck.Click += new EventHandler(this.btnCheck_Click); // Associate click event
            Controls.Add(btnCheck); // Add the Submit button to the form
        }

        private void CreateButtons()
        {
            

            // Create ship controls on the side of the form and add them to the form
            ships = new ShipControl[5, 1]; // 5 ships, 1 column
            for (int i = 0; i < 4; i++)
            {
                ships[i, 0] = new ShipControl();
                ships[i, 0].Location = new Point(10, 100 + i * 60); // Adjust the location as needed
                ships[i, 0].Size = new Size((i + 2) * 50, 50);
                Controls.Add(ships[i, 0]); // Add ship control to the form's controls
            }

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    btn[x, y] = new Button();
                    btn[x, y].SetBounds(0 + (50 * x), 0 + (50 * y), 50, 50);
                    btn[x, y].BackColor = Color.PowderBlue;
                    btn[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    btn[x, y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(btn[x, y]);
                }
            }

            
        }

        private void btnEvent_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text);
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            // Logic to check which buttons on the grid the ships are most on top of
            // You can iterate over the ships and compare their positions with the grid buttons
            // Perform the necessary logic here
            Debug.WriteLine("Submit button clicked!");

            for (int i = 0; i < 4; i++)
            {
                Button btnClosest = new Button();
                int distance = 0;
                int closest = 1000000;
                for (int x = 0; x < 7; x++)
                {
                    for (int y = 0; y < 7; y++)
                    {
                        Point shipPos = new Point(ships[i, 0].Left, ships[i, 0].Top);
                        Point btnPos = new Point(btn[x, y].Left, btn[x, y].Top);
                        distance = ShipControl.CalculateDistance(shipPos, btnPos);

                        if (distance < closest)
                        {
                            closest = distance;
                            btnClosest = btn[x, y];
                        }
                    }

                   
                }
                ships[i, 0].Location = btnClosest.Location;
            }
}

        private void GAME_Load(object sender, EventArgs e) // REQUIRED
        {
            // Your GAME_Load code goes here
        }
    }
}