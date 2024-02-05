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
        public int[,] userGuesses = new int[7, 7]; // used to represent where the user has guessed (0 not guessed/gray, 1 is selected before submit/red, 2 is miss/black, 3 is hit/yellow

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
            /* 
             * computerPlayer_ships();  set the position of the computers ships */

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

            computersBoard[1, 3].BackColor = System.Drawing.Color.Yellow;
            computersBoard[6, 5].BackColor = System.Drawing.Color.Black;
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
                    // Set the Text property to the location string
                    computersBoard[x, y].Text = $"{x + 1},{y + 1}";

                    computersBoard[x, y].Click += new EventHandler(this.computersBoard_Click);
                    Controls.Add(computersBoard[x, y]);

                    userGuesses[x, y] = 0;
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

            // if the computer or player has won
            if ((playerWon() == true) || (computerWon() == true))
            {
                Debug.WriteLine(((Button)sender).Text);
                Manager.Instance.GameState = 4;
                this.Close();
            }

            
            
            
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
            int buttonX = 0; int buttonY = 0;
            string[] coordinates = clickedButton.Text.Split(',');
            if (coordinates.Length == 2 && int.TryParse(coordinates[0], out buttonX) && int.TryParse(coordinates[1], out buttonY))
            {
                // Now, x and y contain the button's position on the grid
                Debug.WriteLine($"Button Clicked - X: {buttonX+1}, Y: {buttonY+1}");
            }

                if (clickedButton.BackColor == System.Drawing.Color.Black || clickedButton.BackColor == System.Drawing.Color.Yellow)
            {
                MessageBox.Show("You've already guessed in this square.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Do not proceed
            }

            if (clickedButton.BackColor == System.Drawing.Color.Gray)
            {
                

                for(int x = 0; x < 7; x++)
                {
                    for(int y =0; y < 7; y++)
                    {
                        if (computersBoard[x, y].BackColor != System.Drawing.Color.Black && computersBoard[x, y].BackColor != System.Drawing.Color.Yellow)
                        {
                            computersBoard[x, y].BackColor = System.Drawing.Color.Gray;
                            userGuesses[x, y] = 0;
                        }

                        if(x+1 == buttonX && y+1 == buttonY)
                        {
                            Debug.WriteLine(clickedButton.Text);
                            clickedButton.BackColor = Color.Red;
                            userGuesses[x, y] = 1;
                        }

                                             
                           
                    }
                }
            }

        }

        private void shipPosition(int length)
        {
            Random p = new Random(); // used for randomly determining a point, used for calculating coordinate
            Random d = new Random(); // used for randomly determing the ship's direction
            bool check = true;
            while (check == true)
            {
                // randomly generate a starting point 
                int x1 = p.Next(1, 7);
                int y1 = p.Next(1, 7);

                // check that this point isn't occupied already
                while (computerOccupied[x1, y1] == 1)
                {
                    // while this point is occupied on the board, choose another
                    x1 = p.Next(1, 7);
                    y1 = p.Next(1, 7);
                }

                // choose direction randomly:
                int direction = d.Next(0, 3);
                bool valid = false;
                int x2, y2, count;

                switch (direction)
                {
                    case 0:
                        // up
                        // check if all coordinates in this direction are valid 
                        count = 0;
                        for (int k = 0; k < length + 1; k++)
                        {
                            y2 = y1 - k;
                            if (y2 < 0)
                            {
                                // if y2 is less than 0 than it's out of bounds
                                break;
                            }

                            // check that (x1, y2) isn't occupied
                            if (computerOccupied[x1, y2] == 1)
                            {
                                // coordinate is already occupied
                                break;
                            }

                            count++;

                            if (count == length)
                            {
                                // all coordinates are valid, add to the board
                                for (int m = 0; m < length; m++)
                                {
                                    y2 = y1 - k;
                                    computerOccupied[x1, y2] = 1;
                                }
                                valid = true;
                            }
                        }

                        break;

                    case 1:
                        // right
                        count = 0;
                        for (int k = 0; k < length + 1; k++)
                        {
                            x2 = x1 + k;
                            if (x2 > 7)
                            {
                                // if x2 is more than 7 than it's out of bounds
                                break;
                            }

                            // check that (x2, y1) isn't occupied
                            if (computerOccupied[x2, y1] == 1)
                            {
                                // coordinate is already occupied
                                break;
                            }

                            count++;

                            if (count == length)
                            {
                                // all coordinates are valid, add to the board
                                for (int m = 0; m < length; m++)
                                {
                                    x2 = x1 + k;
                                    computerOccupied[x2, y1] = 1;
                                }
                                valid = true;
                            }
                        }

                        break;

                    case 2:
                        // down
                        count = 0;
                        for (int k = 0; k < length + 1; k++)
                        {
                            y2 = y1 + k;
                            if (y2 > 7)
                            {
                                // if y2 is more than 7 than it's out of bounds
                                break;
                            }

                            // check that (x1, y2) isn't occupied
                            if (computerOccupied[x1, y2] == 1)
                            {
                                // coordinate is already occupied
                                break;
                            }

                            count++;

                            if (count == length)
                            {
                                // all coordinates are valid, add to the board
                                for (int m = 0; m < length; m++)
                                {
                                    y2 = y1 + k;
                                    computerOccupied[x1, y2] = 1;
                                }
                                valid = true;
                            }
                        }

                        break;

                    case 3:
                        // left
                        count = 0;
                        for (int k = 0; k < length + 1; k++)
                        {
                            x2 = x1 - k;
                            if (x2 < 0)
                            {
                                // if x2 is less than 0 than it's out of bounds
                                break;
                            }

                            // check that (x2, y1) isn't occupied
                            if (computerOccupied[x2, y1] == 1)
                            {
                                // coordinate is already occupied
                                break;
                            }

                            count++;

                            if (count == length)
                            {
                                // all coordinates are valid, add to the board
                                for (int m = 0; m < length; m++)
                                {
                                    x2 = x1 - k;
                                    computerOccupied[x2, y1] = 1;
                                }
                            }
                            valid = true;
                        }

                        break;

                }

                if (valid == false)
                {
                    // if the orignal direction didn't work, try a different direction until you find one that works
                    for (int j = 1; j < 4; j++)
                    {
                        int dir = ((direction + j) % 4);
                        switch (dir)
                        {
                            case 0:
                                // up
                                // check if all coordinates in this direction are valid 
                                count = 0;
                                for (int k = 0; k < length + 1; k++)
                                {
                                    y2 = y1 - j;
                                    if (y2 < 0)
                                    {
                                        // if y2 is less than 0 than it's out of bounds
                                        break;
                                    }

                                    // check that (x1, y2) isn't occupied
                                    if (computerOccupied[x1, y2] == 1)
                                    {
                                        // coordinate is already occupied
                                        break;
                                    }

                                    count++;

                                    if (count == length)
                                    {
                                        // all coordinates are valid, add to the board
                                        for (int m = 0; m < length; m++)
                                        {
                                            y2 = y1 - k;
                                            computerOccupied[x1, y2] = 1;
                                        }
                                        valid = true;
                                    }
                                }

                                break;

                            case 1:
                                // right
                                count = 0;
                                for (int k = 0; k < length + 1; k++)
                                {
                                    x2 = x1 + k;
                                    if (x2 > 7)
                                    {
                                        // if x2 is more than 7 than it's out of bounds
                                        break;
                                    }

                                    // check that (x2, y1) isn't occupied
                                    if (computerOccupied[x2, y1] == 1)
                                    {
                                        // coordinate is already occupied
                                        break;
                                    }

                                    count++;

                                    if (count == length)
                                    {
                                        // all coordinates are valid, add to the board
                                        for (int m = 0; m < length; m++)
                                        {
                                            x2 = x1 + k;
                                            computerOccupied[x2, y1] = 1;
                                        }
                                        valid = true;
                                    }
                                }

                                break;

                            case 2:
                                // down
                                count = 0;
                                for (int k = 0; k < length + 1; k++)
                                {
                                    y2 = y1 + k;
                                    if (y2 > 7)
                                    {
                                        // if y2 is more than 7 than it's out of bounds
                                        break;
                                    }

                                    // check that (x1, y2) isn't occupied
                                    if (computerOccupied[x1, y2] == 1)
                                    {
                                        // coordinate is already occupied
                                        break;
                                    }

                                    count++;

                                    if (count == length)
                                    {
                                        // all coordinates are valid, add to the board
                                        for (int m = 0; m < length; m++)
                                        {
                                            y2 = y1 + k;
                                            computerOccupied[x1, y2] = 1;
                                        }
                                        valid = true;
                                    }
                                }

                                break;

                            case 3:
                                // left
                                count = 0;
                                for (int k = 0; k < length + 1; k++)
                                {
                                    x2 = x1 - k;
                                    if (x2 < 0)
                                    {
                                        // if x2 is less than 0 than it's out of bounds
                                        break;
                                    }

                                    // check that (x2, y1) isn't occupied
                                    if (computerOccupied[x2, y1] == 1)
                                    {
                                        // coordinate is already occupied
                                        break;
                                    }

                                    count++;

                                    if (count == length)
                                    {
                                        // all coordinates are valid, add to the board
                                        for (int m = 0; m < length; m++)
                                        {
                                            x2 = x1 - k;
                                            computerOccupied[x2, y1] = 1;
                                        }
                                    }
                                    valid = true;
                                }

                                break;
                        }
                    }
                }
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
            // ship4 - covers 5 squares
            shipPosition(5);

            // ship3 - covers 4 squares
            shipPosition(4);

            // ship2 - covers 3 squares
            shipPosition(3);

            // ship1 - covers 2 squares
            shipPosition(2);

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