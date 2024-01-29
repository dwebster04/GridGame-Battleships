using System.Drawing;
using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace GridGame_Battleships
{
    partial class GAME
    {
        private ShipControl[,] ships;

        private System.ComponentModel.IContainer components = null;

        private Button btnSubmit; // Declare the Submit button

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
            btnSubmit = new Button();
            btnSubmit.Text = "Submit";
            btnSubmit.Size = new Size(100, 50);
            btnSubmit.Location = new Point(375, 25); // Adjust the location as needed
            btnSubmit.Click += new EventHandler(this.btnSubmit_Click); // Associate click event
            Controls.Add(btnSubmit); // Add the Submit button to the form
        }

        private void CreateButtons()
        {
            Button[,] btn = new Button[7, 7];

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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Logic to check which buttons on the grid the ships are most on top of
            // You can iterate over the ships and compare their positions with the grid buttons
            // Perform the necessary logic here
            Debug.WriteLine("Submit button clicked!");
        }

        private void GAME_Load(object sender, EventArgs e) // REQUIRED
        {
            // Your GAME_Load code goes here
        }
    }
}