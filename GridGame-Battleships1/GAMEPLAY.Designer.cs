using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GridGame_Battleships
{
    partial class GAMEPLAY
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblYour_Grid;
        private Label lblOpponents_Grid;
        private Button[,] playersBoard = new Button[7, 7];
        private Button[,] computersBoard = new Button[7, 7];
        private int[,] playerBoardData = Manager.Instance.playerGrid;
        private ShipControl[,] playerShipsData = Manager.Instance.playerShips;
        private string[] shipNames = new string[] { "Destroyer", "Submarine", "Battleship", "Carrier" };
        private Color[] shipColors = { Color.DarkMagenta, Color.Orange, Color.Green, Color.Blue };
        private int[,] buttonSelected = new int[7, 7];
        private Button btnQuit;
        private Button btnSubmit;
        private Button btnInstruction;
        private bool validSelect = false;
        public int[,] computerOccupied = new int[7, 7];
        public int[,] computerGuesses = new int[7, 7];
        private int playerHits = 0;
        private int computerHits = 0;

       

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 450);
            this.Text = "GAMEPLAY";

            // Set selected to none on start
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    buttonSelected[x, y] = 0;
                }
            }

            lblYour_Grid = new Label();
            lblYour_Grid.Location = new System.Drawing.Point(25, 10);
            lblYour_Grid.Size = new System.Drawing.Size(140, 20);
            lblYour_Grid.BackColor = Color.MediumOrchid;
            lblYour_Grid.Text = "YOUR GRID";
            lblYour_Grid.TextAlign = ContentAlignment.MiddleCenter;
            lblYour_Grid.ForeColor = Color.White;
            Controls.Add(lblYour_Grid);

            lblOpponents_Grid = new Label();
            lblOpponents_Grid.Location = new System.Drawing.Point(175, 10);
            lblOpponents_Grid.Size = new System.Drawing.Size(210, 20);
            lblOpponents_Grid.BackColor = Color.Crimson;
            lblOpponents_Grid.Text = "OPPONENTS GRID";
            lblOpponents_Grid.TextAlign = ContentAlignment.MiddleCenter;
            lblOpponents_Grid.ForeColor = Color.White;
            Controls.Add(lblOpponents_Grid);

            btnInstruction = new Button();
            btnInstruction.Text = "Instructions";
            btnInstruction.Size = new Size(100, 50);
            btnInstruction.Location = new Point(25, 300);
            btnInstruction.Click += new EventHandler(this.btnInstruction_Click);
            btnInstruction.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnInstruction.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnInstruction.BackColor = Color.SkyBlue;
            Controls.Add(btnInstruction);

            btnSubmit = new Button();
            btnSubmit.Text = "Submit";
            btnSubmit.Size = new Size(100, 50);
            btnSubmit.Location = new Point(150, 300);
            btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
            btnSubmit.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnSubmit.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnSubmit.BackColor = Color.SkyBlue;
            Controls.Add(btnSubmit);

            btnQuit = new Button();
            btnQuit.Text = "Quit Game";
            btnQuit.Size = new Size(100, 50);
            btnQuit.Location = new Point(275, 300);
            btnQuit.Click += new EventHandler(this.btnQuit_Click);
            btnQuit.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnQuit.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnQuit.BackColor = Color.SkyBlue;
            Controls.Add(btnQuit);

            CreateButtons();
            computerPlayer_ships();

            Debug.WriteLine("GAMEPLAY");

            for (int i = 0; i < 4; i++)
            {
                Debug.Write($"{shipNames[i]} is located at: ");

                for (int x = 0; x < 7; x++)
                {
                    for (int y = 0; y < 7; y++)
                    {
                        if (playerShipsData[i, 0].boardLocation[x, y] == 1)
                        {
                            Debug.Write($"({x + 1},{y + 1}) ");
                        }
                    }
                }

                Debug.WriteLine("");
            }

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    computerGuesses[i, j] = 0;
                }
            }
        }

        private void CreateButtons()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    playersBoard[x, y] = new Button();
                    playersBoard[x, y].SetBounds(25 + (20 * x), 35 + (20 * y), 20, 20);

                    if (playerBoardData[x, y] == 0)
                    {
                        playersBoard[x, y].BackColor = Color.PowderBlue;
                    }
                    else if (playerBoardData[x, y] == 1)
                    {
                        playersBoard[x, y].BackColor = Color.MediumOrchid;

                        for (int i = 0; i < 4; i++)
                        {
                            if (playerShipsData[i, 0].boardLocation[x, y] == 1)
                            {
                                playersBoard[x, y].BackColor = shipColors[i];
                                break;
                            }
                        }
                    }

                    playersBoard[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    playersBoard[x, y].Click += new EventHandler(this.playersBoard_Click);
                    Controls.Add(playersBoard[x, y]);
                }
            }

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    computersBoard[x, y] = new Button();
                    computersBoard[x, y].SetBounds(175 + (30 * x), 35 + (30 * y), 30, 30);
                    computersBoard[x, y].BackColor = Color.Gray;
                    computersBoard[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    computersBoard[x, y].Click += new EventHandler(this.computersBoard_Click);
                    Controls.Add(computersBoard[x, y]);
                }
            }
        }

        void Btn_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = System.Drawing.Color.Lavender;
        }

        void Btn_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = System.Drawing.Color.SkyBlue;
        }

        private void btnInstruction_Click(object sender, EventArgs e)
        {
            // display a textbox explaining the game's instructions
            string message = "Guess where your opponent's ship is by choosing a coordinate on the opponen's grid \n" + "Then press 'submit' to confirm";
            MessageBox.Show(message, "Instructions", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // process users guess

            /*
             * here write code so when submit is pressed it checks theres a button selected
             * then changes colour to according to if it hit a ship or not, for example black if it is a miss, orange for a hit
             *  could add an explosion sound to meet further extension requirement
             */

            // determine whether the user hit a ship

            

            // computer's turn to guess
            computerPlayer_turn();
            
            
            
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text);
            Manager.Instance.GameState = 0;
            this.Close();
        }

        private void playersBoard_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text);
        }

        private void computersBoard_Click(object sender, EventArgs e)
        {
            // Reset all buttons to grey
            ResetComputerBoardButtons();

            // Highlight the selected button in red
            Button clickedButton = (Button)sender;
            clickedButton.BackColor = Color.Red;

            // Update the buttonSelected array
            UpdateButtonSelectedArray(clickedButton);
        }

        private void ResetComputerBoardButtons()
        {
            // Reset all computer board buttons to grey
            foreach (Button button in computersBoard)
            {
                button.BackColor = Color.Gray;
            }
        }

        private void UpdateButtonSelectedArray(Button clickedButton)
        {
            // Extract coordinates from the button's text
            string[] coordinates = clickedButton.Text.Split(',');

            // Check if parsing is successful
            if (coordinates.Length == 2 && int.TryParse(coordinates[0].Trim(), out int xCoord) && int.TryParse(coordinates[1].Trim(), out int yCoord))
            {
                int x = xCoord - 1;
                int y = yCoord - 1;

                // Check if the coordinates are within the valid range
                if (x >= 0 && x < 7 && y >= 0 && y < 7)
                {
                    // Update buttonSelected array
                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (i == x && j == y)
                            {
                                buttonSelected[i, j] = 1; // Set the selected button to 1
                            }
                            else
                            {
                                buttonSelected[i, j] = 0; // Reset other buttons to 0
                            }
                        }
                    }
                }
                else
                {
                    // Handle out-of-range coordinates, for example, by displaying an error message
                    MessageBox.Show("Invalid button coordinates. Coordinates should be within the range (1,1) to (7,7).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Handle parsing failure, for example, by displaying an error message
                MessageBox.Show("Invalid button format. Cannot parse coordinates.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    
                     Debug.Write($"({buttonSelected[y, x]}) ");
                    
                }

                Debug.WriteLine(""); // Add a line break after each ship's coordinates
            }
        }

        private void computerPlayer_ships()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    computerOccupied[i, j] = 0;
                }
            }

            // assign the computer's ships randomly and records in computerOccupied
            /*
            // ship4 - covers 5 squares
            Random direction = new Random(); // randomly determines position --> vertical 0, horizontal 1
            Random p = new Random(); // randomly determines a point, used for calculating coordinate
            //randomly generate first coordinate of ship4;
            int s4x1 = p.Next(1, 7);
            int s4y1 = p.Next(1, 7);
            // calculate all other coordinates of the boat;
            int s4d = direction.Next(0, 1);

            int s4x2, s4x3, s4x4, s4x5; // initialise ship4's x coordinates
            s4x2 = s4x1;
            s4x3 = s4x1;
            s4x4 = s4x1;
            s4x5 = s4x1;
            int s4y2, s4y3, s4y4, s4y5; // initialise ship4's y coordinates
            s4y2 = s4y1;
            s4y3 = s4y1;
            s4y4 = s4y1;
            s4y5 = s4y1;
            if (s4d == 0)
            {
                // vertical - only y coordinate changes

                switch (s4y1)
                {
                    case 1:
                        // s4y1 = 1, increase all 
                        s4y2 = s4y1 + 1;
                        s4y3 = s4y1 + 2;
                        s4y4 = s4y1 + 3;
                        s4y5 = s4y1 + 4;
                        break;
                    case 2:
                        // s4y1 = 2, increase all 
                        s4y2 = s4y1 + 1;
                        s4y3 = s4y1 + 2;
                        s4y4 = s4y1 + 3;
                        s4y5 = s4y1 + 4;
                        break;
                    case 3:
                        // s4y1 = 3, increase all
                        s4y2 = s4y1 + 1;
                        s4y3 = s4y1 + 2;
                        s4y4 = s4y1 + 3;
                        s4y5 = s4y1 + 4;
                        break;
                    case 4:
                        // s4y1 = 4, 2 above 2 below
                        s4y2 = s4y1 + 1;
                        s4y3 = s4y1 + 2;
                        s4y4 = s4y1 - 2;
                        s4y5 = s4y1 - 1;
                        break;
                    case 5:
                        // s4y1 = 5
                        s4y2 = s4y1 - 1;
                        s4y3 = s4y1 - 2;
                        s4y4 = s4y1 - 3;
                        s4y5 = s4y1 - 4;
                        break;
                    case 6:
                        // s4y1 = 6
                        s4y2 = s4y1 - 1;
                        s4y3 = s4y1 - 2;
                        s4y4 = s4y1 - 3;
                        s4y5 = s4y1 - 4;
                        break;
                    case 7:
                        // s4y1 = 7, decrease all
                        s4y2 = s4y1 - 1;
                        s4y3 = s4y1 - 2;
                        s4y4 = s4y1 - 3;
                        s4y5 = s4y1 - 4;
                        break;

                }
            } else
            {
                // horizontal - only x coordinate changes

                switch (s4x1)
                {
                    case 1:
                        // s4x1 = 1, increase all 
                        s4x2 = s4x1 + 1;
                        s4x3 = s4x1 + 2;
                        s4x4 = s4x1 + 3;
                        s4x5 = s4x1 + 4;
                        break;
                    case 2:
                        // s4x1 = 2, increase all 
                        s4x2 = s4x1 + 1;
                        s4x3 = s4x1 + 2;
                        s4x4 = s4x1 + 3;
                        s4x5 = s4x1 + 4;
                        break;
                    case 3:
                        // s4x1 = 3, increase all
                        s4x2 = s4x1 + 1;
                        s4x3 = s4x1 + 2;
                        s4x4 = s4x1 + 3;
                        s4x5 = s4x1 + 4;
                        break;
                    case 4:
                        // s4x1 = 4, 2 left 2 right
                        s4x2 = s4x1 + 1;
                        s4x3 = s4x1 + 2;
                        s4x4 = s4x1 - 2;
                        s4x5 = s4x1 - 1;
                        break;
                    case 5:
                        // s4x1 = 5
                        s4x2 = s4x1 - 1;
                        s4x3 = s4x1 - 2;
                        s4x4 = s4x1 - 3;
                        s4x5 = s4x1 - 4;
                        break;
                    case 6:
                        // s4x1 = 6
                        s4x2 = s4x1 - 1;
                        s4x3 = s4x1 - 2;
                        s4x4 = s4x1 - 3;
                        s4x5 = s4x1 - 4;
                        break;
                    case 7:
                        // s4x1 = 7
                        s4x2 = s4x1 - 1;
                        s4x3 = s4x1 - 2;
                        s4x4 = s4x1 - 3;
                        s4x5 = s4x1 - 4;
                        break;
                }
            }
            // add the ships coordinates to computerOccupied --> no need to check occupation as no ship has been placed on the board yet
            computerOccupied[s4x1, s4y1] = 1;
            computerOccupied[s4x2, s4y2] = 1;
            computerOccupied[s4x3, s4y3] = 1;
            computerOccupied[s4x4, s4y4] = 1;
            computerOccupied[s4x5, s4y5] = 1;


            // ship3 - covers 4 squares
            int s3x1 = p.Next(1, 7);
            int s3y1 = p.Next(1, 7);
            // check that this point isn't already occupied
            while (computerOccupied[s3x1, s3y1] == 1)
            {
                // only enters while loop if the coordinate is already occupied
                s3x1 = p.Next(1, 7);
                s3y1 = p.Next(1, 7);
            }
            int s3x2, s3x3, s3x4; // initialise ship4's x coordinates
            s3x2 = s4x1;
            s3x3 = s4x1;
            s3x4 = s4x1;
            int s3y2, s3y3, s3y4; // initialise ship4's y coordinates
            s3y2 = s4y1;
            s3y3 = s4y1;
            s3y4 = s4y1;

            switch (s3x1)
            {
                case 1:
                    // if s3x1=1
                    
                    
                    break;
                case 2:
                    // if s3x1=2
                    break;
                case 3:
                    // if s3x1=3
                    break;
                case 4:
                    // if s3x1=4
                    break;
                case 5:
                    // if s3x1=5
                    break;
                case 6:
                    // if s3x1=6
                    break;
                case 7:
                    // if s3x1=7
                    break;
            }

            



            // ship2 - covers 3 squares
            int s2x1 = p.Next(1, 7);
            int s2y1 = p.Next(1, 7);
            // check that this point isn't already occupied
            while (computerOccupied[s2x1, s2y1] == 1)
            {
                // only enters while loop if the coordinate is already occupied
                s2x1 = p.Next(1, 7);
                s2y1 = p.Next(1, 7);
            }

            int s2x2, s2x3; // initialise ship4's x coordinates
            s2x2 = s4x1;
            s2x3 = s4x1;
            int s2y2, s2y3; // initialise ship4's y coordinates
            s2y2 = s4y1;
            s2y3 = s4y1;

            // ship1 - covers 2 squares
            int s1x1 = p.Next(1, 7);
            int s1y1 = p.Next(1, 7);
            // check that this point isn't already occupied
            while (computerOccupied[s2x1, s2y1] == 1)
            {
                // only enters while loop if the coordinate is already occupied
                s1x1 = p.Next(1, 7);
                s1y1 = p.Next(1, 7);
            }

            int s1x2; // initialise ship4's x coordinates
            s1x2 = s4x1;
            int s1y2; // initialise ship4's y coordinates
            s1y2 = s4y1;
            */

        }

        private void computerPlayer_turn()
        {
            // check if the computer has made a hit guess already
            

            // the computer guesses a random coordinate on the player's board
            Random rd = new Random();
            int rand_num = rd.Next();

            // guessed coordinate
            int gx = rd.Next(1, 7);
            int gy = rd.Next(1, 7);

            // check that this coorindate isn't in computer guesses array
            while (computerGuesses[gx, gy] != 1)
            {
                // this place has already been guessed if the corresponding array value is not 0
                // another random guess is done, while loop continues until a guess that hasn't already been made is made
                gx = rd.Next(1, 7);
                gy = rd.Next(1, 7);
            }

            // add the guess to the computer guesses array
            computerGuesses[gx, gy] = 1;

            // determine whether the computer hit a player's ship
            if (playerBoardData[gx, gy] == 1)
            {
                // the computer hit the player's ship
                // increase computer hits by 1
                computerHits += 1;
                
            }

        }

        private bool playerWon()
        {
            // function to check if a player has won yet
            if (playerHits == 14)
            {
                // return true if player has won
                return true; ;
            } else
            {
                return false;
            }
        }

        private bool computerWon()
        {
            // function to check if the computer has won yet
            if (computerHits == 14)
            {
                // return true if the computer has won
                return true; ;
            }
            else
            {
                return false;
            }
        }
    }
}