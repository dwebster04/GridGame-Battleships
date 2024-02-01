using System.Drawing;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Sockets;

namespace GridGame_Battleships
{
    partial class GAMEPLAY
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Label lblYour_Grid;

        private Label lblOpponents_Grid;
        private Button[,] playersBoard = new Button[7, 7];
        private Button[,] computersBoard = new Button[7, 7];
        private int[,] playerBoardData = Manager.Instance.playerGrid;
        private ShipControl[,] playerShipsData = Manager.Instance.playerShips;
        public string[] shipNames = new string[] { "Destroyer", "Submarine", "Battleship", "Carrier" };
        private Color[] shipColors = { Color.DarkMagenta, Color.Orange, Color.Green, Color.Blue };

        private Button btnQuit; // declare the Quit button
        private Button btnSubmit; // declare the submit button
        private Button btnInstruction; // declare the instructions button
        private bool validSelect = false; // used for selecting places on the computer's board

        public int[,] computerOccupied = new int[7, 7]; // used to represent where the computer's ships are 
        public int[,] computerGuesses = new int[7, 7]; // used to represent where the computer has guessed the player's ships are

        // player/computer need 14 hits to win 
        int playerHits = 0;
        int computerHits = 0;
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 450); // 175 + 210 + 25
            this.Text = "GAMEPLAY";


            // 
            // lblYour_Grid
            // 
            lblYour_Grid = new Label();
            lblYour_Grid.Location = new System.Drawing.Point(25, 10);
            lblYour_Grid.Size = new System.Drawing.Size(140, 20);
            lblYour_Grid.BackColor = Color.MediumOrchid;
            lblYour_Grid.Text = "YOUR GRID";
            lblYour_Grid.TextAlign = ContentAlignment.MiddleCenter; // Center the text
            lblYour_Grid.ForeColor = Color.White; // Set font color to white
            Controls.Add(lblYour_Grid); // Add the Label to the form

            // 
            // lblOpponents_Grid
            // 
            lblOpponents_Grid = new Label();
            lblOpponents_Grid.Location = new System.Drawing.Point(175, 10);
            lblOpponents_Grid.Size = new System.Drawing.Size(210, 20);
            lblOpponents_Grid.BackColor = Color.Crimson;
            lblOpponents_Grid.Text = "OPPONENTS GRID";
            lblOpponents_Grid.TextAlign = ContentAlignment.MiddleCenter; // Center the text
            lblOpponents_Grid.ForeColor = Color.White; // Set font color to white
            Controls.Add(lblOpponents_Grid); // Add the Label to the form

            // create and initialise the instruction button
            btnInstruction = new Button();
            btnInstruction.Text = "Instructions";
            btnInstruction.Size = new Size(100, 50);
            btnInstruction.Location = new Point(25, 300);
            btnInstruction.Click += new EventHandler(this.btnInstruction_Click); // associate click event
            btnInstruction.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnInstruction.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnInstruction.BackColor = Color.SkyBlue;
            Controls.Add(btnInstruction);

            // create and initialise the submit button
            btnSubmit = new Button();
            btnSubmit.Text = "Submit";
            btnSubmit.Size = new Size(100, 50);
            btnSubmit.Location = new Point(150, 300);
            btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
            btnSubmit.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnSubmit.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnSubmit.BackColor = Color.SkyBlue;
            Controls.Add(btnSubmit);

            // create and initialise the quit button
            btnQuit = new Button();
            btnQuit.Text = "Quit Game";
            btnQuit.Size = new Size(100, 50);
            btnQuit.Location = new Point(275, 300);
            btnQuit.Click += new EventHandler(this.btnQuit_Click);
            btnQuit.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnQuit.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnQuit.BackColor = Color.SkyBlue;
            Controls.Add(btnQuit);



            CreateButtons(); // creates buttons 
            computerPlayer_ships(); // set the position of the computers ships 

            Debug.WriteLine("GAMEPLAY"); // Add a line break after each ship's coordinates
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

                Debug.WriteLine(""); // Add a line break after each ship's coordinates
            }

            // initialise all values of an array to 0 - will be used to store the coordinates of where the computer has guessed a player's ships are
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

                        // Assign the color of the corresponding ship
                        for (int i = 0; i < 4; i++)
                        {
                            if (playerShipsData[i, 0].boardLocation[x, y] == 1)
                            {
                                playersBoard[x, y].BackColor = shipColors[i];
                                break; // Stop searching once you find the ship
                            }
                        }
                    }

                    playersBoard[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    playersBoard[x, y].Click += new EventHandler(this.playersBoard_Click);

                    // Add the button to the ShipControl
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

                    computersBoard[x, y].Click += new EventHandler(this.computersBoard_Click);
                    Controls.Add(computersBoard[x, y]);
                }
            }
        }

        void Btn_MouseEnter(object sender, EventArgs e)
        {
            // change the button colour when the mouse enters
            ((Button)sender).BackColor = System.Drawing.Color.Lavender;
        }

        void Btn_MouseLeave(object sender, EventArgs e)
        {
            // change the button colour back to the orignal once the mouse has left
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
            // the place on the grid the player chooses is highlighted in red
            Button clickedButton = (Button)sender;
            Debug.WriteLine(clickedButton.Text);
            clickedButton.BackColor = Color.Red;

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