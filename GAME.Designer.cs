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
        public int[,] occupied = new int[7, 7];
        public ShipControl[,] ships;

        private String errorMessage = "";
        private System.ComponentModel.IContainer components = null;
        private bool ValidShips = false;
        private Button btnCheck; // Declare the Check button
        private Button btnSubmit; // Declare the Submit button
        private Button btnReset; // Declare the Reset button

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
            
            // Create and initialize the Check button
            btnCheck = new Button();
            btnCheck.Text = "Check";
            btnCheck.Size = new Size(100, 50);
            btnCheck.Location = new Point(375, 25); // Adjust the location as needed
            btnCheck.Click += new EventHandler(this.btnCheck_Click); // Associate click event
            Controls.Add(btnCheck); // Add the Submit button to the form

            // Create and initialize the Submit button
            btnSubmit = new Button();
            btnSubmit.Text = "Submit";
            btnSubmit.Size = new Size(100, 50);
            btnSubmit.Location = new Point(375, 100); // Adjust the location as needed
            btnSubmit.Click += new EventHandler(this.btnSubmit_Click); // Associate click event
            Controls.Add(btnSubmit); // Add the Submit button to the form

            // Create and initialize the Reset button
            btnReset = new Button();
            btnReset.Text = "Reset";
            btnReset.Size = new Size(100, 50);
            btnReset.Location = new Point(375, 175); // Adjust the location as needed
            btnReset.Click += new EventHandler(this.btnReset_Click); // Associate click event
            Controls.Add(btnReset); // Add the Submit button to the form

            // Call the method to create buttons
            CreateButtons();

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
            Debug.WriteLine("Check button clicked!");
            check();

            if (ValidShips)
            {
                btnCheck.BackColor = Color.Green; // Set to green if ships are valid
            }
            else
            {
                btnCheck.BackColor = Color.Red; // Set to red if ships are not valid
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Reset button clicked!");
            for (int i = 0; i < 4; i++)
            {
                
                ships[i, 0].Location = new Point(10, 100 + i * 60); // Adjust the location as needed

            }
        }

        private void check()
        {      
            ValidShips = true;

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    occupied[i, j] = 0;
                }
            }

            for (int i = 0; i < 4; i++)
            {

                Button btnClosest = new Button();
                int distance = 0;
                int closest = 1000000;
                int closestX = 0; int closestY = 0;
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
                            closestX = x; closestY = y;
                        }
                    }


                }

                ships[i, 0].Location = btnClosest.Location;
                ShipControl checkShip = ships[i, 0];
                if (checkShip.Height > checkShip.Width) // if ships is vertical
                {
                    // check to see if running off the grid
                    int HeightValue = checkShip.Height / 50;
                    if (closestY + HeightValue > 7)
                    {
                        Debug.Print("INVALID SHIP OF LENGTH: {0}", HeightValue);
                        errorMessage = $"The ship that is {HeightValue} units long is not fully on the grid";
                        ValidShips = false;
                    }

                    // if occupied is greater than one ships will be overlapping
                    else
                    {
                        for (int j = 0; j < HeightValue; j++)
                        {
                            occupied[closestX, closestY + j] += 1;
                        }
                    }
                }
                else // if ships is horizontal
                {
                    // check to see if running off the grid
                    int WidthValue = checkShip.Width / 50;
                    if (closestX + WidthValue > 7)
                    {
                        Debug.Print("INVALID SHIP OF LENGTH: {0}", WidthValue);
                        errorMessage = $"The ship that is {WidthValue} units long is not fully on the grid";
                        ValidShips = false;
                    }

                    // if occupied is greater than one ships will be overlapping
                    else
                    {
                        for (int j = 0; j < WidthValue; j++)
                        {
                            occupied[closestX + j, closestY] += 1;
                        }
                    }
                }

                for (int c = 0; c < 7; c++)
                {
                    for (int d = 0; d < 7; d++)
                    {
                        if (occupied[c, d] > 1)
                        {
                            ValidShips = false;
                            Debug.Print("INVALID SHIPS ARE OVERLAPPING AT {0} , {1}", c + 1, d + 1);
                            errorMessage = $"The ships overlap at ({c+1} , {c+1})";

                        }
                    }
                }
            }

            for (int innerCounter = 0; innerCounter < 7; innerCounter++)
            {
                Debug.Print("{0} , {1}, {2}, {3}, {4} , {5} , {6}",
                occupied[0, innerCounter], occupied[1, innerCounter], occupied[2, innerCounter],
                occupied[3, innerCounter], occupied[4, innerCounter], occupied[5, innerCounter],
                occupied[6, innerCounter]);
            }
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Submit button clicked!");
            check();
            if (ValidShips)
            {
                // go to gameplay against computer
            }
            else
            {
                // Display a pop-up window saying invalid ships
                MessageBox.Show(errorMessage, "Invalid Ships", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnCheck.BackColor = Color.Red; // Set to red if ships are not valid
            }
        }


        private void GAME_Load(object sender, EventArgs e) // REQUIRED
        {
            // Your GAME_Load code goes here
        }
    }
}