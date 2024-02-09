using System.Drawing;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Sockets;

// GAME.Designer.cs
// This is form is where the user chooses where to place there ships

namespace GridGame_Battleships
{
    partial class GAME
    {
        public Button[,] btn = new Button[7, 7];
        public int[,] occupied = new int[7, 7];
        public string[] shipNames = new string[] { "Destroyer", "Submarine", "Battleship", "Carrier" };
        public ShipControl[,] ships;

        private String errorMessage = "";
        private System.ComponentModel.IContainer components = null;
        private bool ValidShips = false;
        private Button btnCheck; // declare the Check button
        private Button btnSubmit; // declare the Submit button
        private Button btnReset; // declare the Reset button
        private Button btnControls; // declare the Controls button
        private Button btnExit; // declare the Exit button

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
            this.ClientSize = new System.Drawing.Size(525, 400);
            this.Text = "GAME";
            
            // btnCheck

            btnCheck = new Button();
            btnCheck.Text = "Check";
            btnCheck.Size = new Size(100, 50);
            btnCheck.Location = new Point(400, 25); 
            btnCheck.Click += new EventHandler(this.btnCheck_Click);
            btnCheck.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnCheck.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnCheck.BackColor = Color.DarkOrchid;
            Controls.Add(btnCheck);

            // btnSubmit

            btnSubmit = new Button();
            btnSubmit.Text = "Submit";
            btnSubmit.Size = new Size(100, 50);
            btnSubmit.Location = new Point(400, 100); 
            btnSubmit.Click += new EventHandler(this.btnSubmit_Click); 
            btnSubmit.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnSubmit.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnSubmit.BackColor = Color.DarkOrchid;
            Controls.Add(btnSubmit); 

            // btnReset

            btnReset = new Button();
            btnReset.Text = "Reset";
            btnReset.Size = new Size(100, 50);
            btnReset.Location = new Point(400, 175); 
            btnReset.Click += new EventHandler(this.btnReset_Click);
            btnReset.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnReset.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnReset.BackColor = Color.DarkOrchid;
            Controls.Add(btnReset); 

            // btnControls

            btnControls = new Button();
            btnControls.Text = "Controls";
            btnControls.Size = new Size(100, 50);
            btnControls.Location = new Point(400, 250); 
            btnControls.Click += new EventHandler(this.btnControls_Click); 
            btnControls.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnControls.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnControls.BackColor = Color.DarkOrchid;
            Controls.Add(btnControls);

            // btnExit

            btnExit = new Button();
            btnExit.Text = "Exit";
            btnExit.Size = new Size(100, 50);
            btnExit.Location = new Point(400, 325); 
            btnExit.Click += new EventHandler(this.btnExit_Click);
            btnExit.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnExit.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnExit.BackColor = Color.DarkOrchid;
            Controls.Add(btnExit); 

            // call the method to create buttons
            CreateButtons();

        }
        
        // creates the grid for user to place ships in
        private void CreateButtons()
        {
            // create ships
            ships = new ShipControl[5, 1];
            for (int i = 0; i < 4; i++)
            {
                ships[i, 0] = new ShipControl();
                ships[i, 0].Location = new Point(10, 100 + i * 60); // display ships on the left hand side of the page one on top of another
                ships[i, 0].Size = new Size((i + 2) * 50, 50); // each ship increases in unit size from 2-5
                ships[i, 0].Text = shipNames[i]; // set the text to the matching ship name

                for (int u = 0; u < 7; u++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        ships[i, 0].boardLocation[u, j] = 0; // set the ships board location to 0
                    }
                }
                Controls.Add(ships[i, 0]); 
            }

            // create the grid of buttons
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    btn[x, y] = new Button();
                    btn[x, y].SetBounds(25 + (50 * x), 25 + (50 * y), 50, 50); // each button is 50 x 50
                    btn[x, y].BackColor = Color.PowderBlue;
                    btn[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    btn[x, y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(btn[x, y]);
                }
            }

            
        }

        // when button is clicked
        private void btnEvent_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text);
        }

        // when mosue enters change colour
        void Btn_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = System.Drawing.Color.Blue;
        }

        // when mouse leaves change colour to original
        void Btn_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = System.Drawing.Color.DarkOrchid;
        }

        // when the check button is clicked let the user know if their ships are in valid locations
        private void btnCheck_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Check button clicked!");
            check();

            if (ValidShips)
            {
                btnCheck.BackColor = Color.Green; // set to green if ships are valid
            }
            else
            {
                btnCheck.BackColor = Color.Red; // set to red if ships are not valid
            }

            for (int i = 0; i < 4; i++)
            {
                Debug.Write($"{shipNames[i]} is located at: ");

                for (int x = 0; x < 7; x++)
                {
                    for (int y = 0; y < 7; y++)
                    {
                        if (ships[i, 0].boardLocation[x, y] == 1)
                        {
                            Debug.Write($"({x + 1},{y + 1}) ");
                        }
                    }
                }

                Debug.WriteLine(""); 
            }
        }

        // when the reset button is clicked remove all ships off the board back their original state
        private void btnReset_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Reset button clicked!");
            for (int i = 0; i < 4; i++)
            {
                
                ships[i, 0].Location = new Point(10, 100 + i * 60); 

            }
        }

        // when the controls button is clicked display a message box showing the controls
        private void btnControls_Click(object sender, EventArgs e)
        {
            string message = "LMB - Click and Drag ships\n" +
                             "R - Rotate Selected ships\n" +
                             "All of the ship must be on the grids\n" +
                             "Ships cannot overlap\n\n" +
                             "Press CHECK to see if ships are in a valid positon\n" +
                             "Press SUBMIT to start playing the game";
            MessageBox.Show(message, "Controls", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // when the exit button is clicked go back to menu
        void btnExit_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text);
            // Update the game state
            Manager.Instance.GameState = 0;
            // Close the menu form
            this.Close();
        }

        // check to see if ships are valid
        private void check()
        {      
            ValidShips = true;

            // reset the occupied board
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    occupied[i, j] = 0;
                }
            }
            
            // loop through all ships
            for (int i = 0; i < 4; i++)
            {
                Button btnClosest = new Button();
                int distance = 0;
                int closest = 1000000;
                int closestX = 0; int closestY = 0;

                // reset the ships board location
                for (int u = 0; u < 7; u++)
                {
                    for (int v = 0; v < 7; v++)
                    {
                        ships[i, 0].boardLocation[u, v] = 0;
                    }
                }

                // find the button closest to the ships position
                for (int x = 0; x < 7; x++)
                {
                    for (int y = 0; y < 7; y++)
                    {
                        Point shipPos = new Point(ships[i, 0].Left, ships[i, 0].Top);
                        Point btnPos = new Point(btn[x, y].Left, btn[x, y].Top);
                        distance = ShipControl.CalculateDistance(shipPos, btnPos);

                        if (distance < closest) // if new closest distance found
                        {
                            closest = distance;
                            btnClosest = btn[x, y];
                            closestX = x; closestY = y;
                        }
                    }


                }

                // set the ships location to the closest button
                ships[i, 0].Location = btnClosest.Location;
                ShipControl checkShip = ships[i, 0];
                if (checkShip.Height > checkShip.Width) // if ships is vertical
                {
                    // check to see if running off the grid
                    int HeightValue = checkShip.Height / 50;
                    if (closestY + HeightValue > 7)
                    {
                        Debug.Print("INVALID SHIP OF LENGTH: {0}", HeightValue);
                        errorMessage = $"The {shipNames[HeightValue-2]} is not fully on the grid";
                        ValidShips = false;
                    }

                    // if occupied is greater than one ships will be overlapping
                    else
                    {
                        for (int j = 0; j < HeightValue; j++)
                        {
                            ships[i,0].boardLocation[closestX, closestY + j] += 1;
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
                        errorMessage = $"The {shipNames[WidthValue - 2]} is not fully on the grid";
                        ValidShips = false;
                    }

                    // if occupied is greater than one ships will be overlapping
                    else
                    {
                        for (int j = 0; j < WidthValue; j++)

                        {
                            ships[i, 0].boardLocation[closestX + j, closestY] += 1;
                            occupied[closestX + j, closestY] += 1;
                        }
                    }
                }

                // loop through occupied array to check if ships are overlapping at any point
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

        // when submit button is clicked go to GAMEPLAY screen if valid ships
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Submit button clicked!");
            check();
            if (ValidShips)
            {
                // go to gameplay against computer
                Manager.Instance.GameState = 3;
                Manager.Instance.playerGrid = occupied;
                Manager.Instance.playerShips = ships;

                this.Close();
            }
            else
            {
                // display a pop-up window saying invalid ships
                MessageBox.Show(errorMessage, "Invalid Ships", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnCheck.BackColor = Color.Red; // set to red if ships are not valid
            }
        }


        private void GAME_Load(object sender, EventArgs e) // REQUIRED
        {
            // Your GAME_Load code goes here
        }
    }
}