using System.Drawing;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Sockets;

// GAMEPLAY.Designer.cs
// This form is for after the user has chosen where to place there ships and will now take turns guessing where the opponents ships are

namespace GridGame_Battleships
{
    partial class GAMEPLAY
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // labels for player and computer
        private Label lblYour_Grid; private Label lblOpponents_Grid;

        // grid of buttons for player and computer
        private Button[,] playersBoard = new Button[7, 7];  private Button[,] computersBoard = new Button[7, 7];

        // recieve player data of ship locations from Manager
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

        int[] lastHit = new int[2]; // contains the coordinate of the last hit made by the computer
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 450); 
            this.Text = "GAMEPLAY";
             
            // lblYour_Grid
            
            lblYour_Grid = new Label();
            lblYour_Grid.Location = new System.Drawing.Point(25, 10);
            lblYour_Grid.Size = new System.Drawing.Size(140, 20);
            lblYour_Grid.BackColor = Color.MediumOrchid;
            lblYour_Grid.Text = "YOUR GRID";
            lblYour_Grid.TextAlign = ContentAlignment.MiddleCenter; 
            lblYour_Grid.ForeColor = Color.White; 
            Controls.Add(lblYour_Grid); 
            
            // lblOpponents_Grid
             
            lblOpponents_Grid = new Label();
            lblOpponents_Grid.Location = new System.Drawing.Point(175, 10);
            lblOpponents_Grid.Size = new System.Drawing.Size(210, 20);
            lblOpponents_Grid.BackColor = Color.Crimson;
            lblOpponents_Grid.Text = "OPPONENTS GRID";
            lblOpponents_Grid.TextAlign = ContentAlignment.MiddleCenter; 
            lblOpponents_Grid.ForeColor = Color.White; 
            Controls.Add(lblOpponents_Grid); 

            // btnInstruction

            btnInstruction = new Button();
            btnInstruction.Text = "Instructions";
            btnInstruction.Size = new Size(100, 50);
            btnInstruction.Location = new Point(25, 300);
            btnInstruction.Click += new EventHandler(this.btnInstruction_Click); // associate click event
            btnInstruction.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnInstruction.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnInstruction.BackColor = Color.SkyBlue;
            Controls.Add(btnInstruction);

            // btnSubmit

            btnSubmit = new Button();
            btnSubmit.Text = "Submit";
            btnSubmit.Size = new Size(100, 50);
            btnSubmit.Location = new Point(150, 300);
            btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
            btnSubmit.MouseEnter += new EventHandler(this.Btn_MouseEnter);
            btnSubmit.MouseLeave += new EventHandler(this.Btn_MouseLeave);
            btnSubmit.BackColor = Color.SkyBlue;
            Controls.Add(btnSubmit);

            // btnQuit

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

            // initialise all values of an array to 0 - will be used to store the coordinates of where the computer has guessed a player's ships are
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    computerGuesses[i, j] = 0;
                }
            }

           
        }

        // creates the grids
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
                    else if (playerBoardData[x, y] == 1) // if ship is in the location of button
                    {
                        playersBoard[x, y].BackColor = Color.MediumOrchid;

                        // assign the colour of the corresponding ship
                        for (int i = 0; i < 4; i++) // loop through ships to find what ship it is
                        {
                            if (playerShipsData[i, 0].boardLocation[x, y] == 1)
                            {
                                playersBoard[x, y].BackColor = shipColors[i];
                                break; // stop searching once you find the ship
                            }
                        }
                    }

                    playersBoard[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    playersBoard[x, y].ForeColor = playersBoard[x, y].BackColor;
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
                    computersBoard[x, y].Text = $"{x + 1},{y + 1}";
                    computersBoard[x, y].ForeColor = computersBoard[x, y].BackColor;

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
            
            // check if there is a guess
            bool guess = false;
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (userGuesses[x, y] == 1) // if the user has placed a guess
                    {
                        guess = true;
                        break;
                    }
                }
                if (guess) break;
            }

            if (!guess) // display an error message if the user has not selected anything
            {
                MessageBox.Show("Please select a square before submitting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // process the user's guess
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (userGuesses[x, y] == 1) // if the user has placed a guess
                    {
                        // check if the opponent has a ship in that square
                        if (computerOccupied[x, y] == 1)
                        {
                            computersBoard[x, y].BackColor = Color.Yellow; // change color to yellow for a hit
                            computersBoard[x, y].ForeColor = computersBoard[x, y].BackColor;
                            userGuesses[x, y] = 2;
                            playerHits++; // increase the player's hit count
                        }
                        else
                        {
                            computersBoard[x, y].BackColor = Color.Black; // change color to black for a miss
                            computersBoard[x, y].ForeColor = computersBoard[x, y].BackColor;
                            userGuesses[x, y] = 3;
                        }

                        // disable the button after the guess is processed
                        computersBoard[x, y].Enabled = false;
                    }
                }
            }

            // computer's turn to guess
            computerPlayer_turn();

            // check if either the player or the computer has won
            if (playerWon() || computerWon())
            {
                string winner = playerWon() ? "Player" : "Computer";
                MessageBox.Show($"{winner} has won the game!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Transition to the game end screen
                Debug.WriteLine(((Button)sender).Text); 
                // Update the game state
                Manager.Instance.GameState = 4; 
                // Close the GAMEPLAY form
                this.Close();
            }
        }

        // when the quit button is clicked
        private void btnQuit_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text);
            Manager.Instance.GameState = 0;
            this.Close();
        }

        // when clicking a button on the players grid
        private void playersBoard_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text);
        }

        // when clicking the computers grid to select a guess
        private void computersBoard_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int buttonX = 0; int buttonY = 0;
            string[] coordinates = clickedButton.Text.Split(',');
            if (coordinates.Length == 2 && int.TryParse(coordinates[0], out buttonX) && int.TryParse(coordinates[1], out buttonY))
            {
                // x and y contain the button's position on the grid
                Debug.WriteLine($"Button Clicked - X: {buttonX+1}, Y: {buttonY+1}");
            }

            // if the button is black/yellow a guess has already been made in that location and will display this as an error message
            if (clickedButton.BackColor == System.Drawing.Color.Black || clickedButton.BackColor == System.Drawing.Color.Yellow)
            {
                MessageBox.Show("You've already guessed in this square.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // do not proceed
            }

            // if the button is gray this means it has not been guessed in
            if (clickedButton.BackColor == System.Drawing.Color.Gray)
            {
                for(int x = 0; x < 7; x++)
                {
                    for(int y =0; y < 7; y++)
                    {
                        // for every button that is not black or yellow or currently selected set to gray
                        if (computersBoard[x, y].BackColor != System.Drawing.Color.Black && computersBoard[x, y].BackColor != System.Drawing.Color.Yellow)
                        {
                            computersBoard[x, y].BackColor = System.Drawing.Color.Gray;
                            computersBoard[x, y].ForeColor = computersBoard[x, y].BackColor;
                            userGuesses[x, y] = 0;
                        }

                        // set the selected button to red
                        if(x+1 == buttonX && y+1 == buttonY)
                        {
                            Debug.WriteLine(clickedButton.Text);
                            clickedButton.BackColor = Color.Red;
                            computersBoard[x, y].ForeColor = computersBoard[x, y].BackColor;
                            userGuesses[x, y] = 1;
                        }       
                    }
                }
            }
        }

        // assign the computers positions randomly
        private void shipPosition(int length)
        {
            Random random = new Random();
            bool valid = false;

            while (!valid)
            {
                int startX = random.Next(7); // generate random X coordinate
                int startY = random.Next(7); // generate random Y coordinate
                int direction = random.Next(4); // generate random direction (0: up, 1: right, 2: down, 3: left)

                // check if the ship can be placed in the chosen direction
                bool canPlaceShip = true;
                for (int i = 0; i < length; i++)
                {
                    int newX = startX;
                    int newY = startY;

                    // adjust coordinates based on direction
                    switch (direction)
                    {
                        case 0: // up
                            newY -= i;
                            break;
                        case 1: // right
                            newX += i;
                            break;
                        case 2: // down
                            newY += i;
                            break;
                        case 3: // left
                            newX -= i;
                            break;
                    }

                    // check if the new position is out of bounds or overlaps with another ship
                    if (newX < 0 || newX >= 7 || newY < 0 || newY >= 7 || computerOccupied[newX, newY] == 1)
                    {
                        canPlaceShip = false;
                        break;
                    }
                }

                if (canPlaceShip)
                {
                    // place the ship on the board
                    for (int i = 0; i < length; i++)
                    {
                        int newX = startX;
                        int newY = startY;

                        // adjust coordinates based on direction
                        switch (direction)
                        {
                            case 0: //uUp
                                newY -= i;
                                break;
                            case 1: // right
                                newX += i;
                                break;
                            case 2: // down
                                newY += i;
                                break;
                            case 3: // left
                                newX -= i;
                                break;
                        }

                        // mark the position as occupied by the ship
                        computerOccupied[newX, newY] = 1;
                    }

                    valid = true; // exit the loop since a valid position is found
                }
            }
        }

        // set the ships location
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

        // computer guesses where the users ships are
        private void computerPlayer_turn()
        {
            Random rd = new Random();
            int gx, gy;

            // try to guess around the last hit made if available
            if (computerHits != 0)
            {
                // Get the last hit coordinates
                gx = lastHit[0];
                gy = lastHit[1];

                // generate random directions (up, down, left, right)
                int[] directions = { -1, 1 }; // -1 for left/up, 1 for right/down
                int randomDirectionIndex = rd.Next(directions.Length);
                int randomDirection = directions[randomDirectionIndex];

                // randmly select whether to move horizontally or vertically
                if (rd.Next(2) == 0)
                {
                    // try to guess horizontally
                    gx += randomDirection;
                }
                else
                {
                    // try to guess vertically
                    gy += randomDirection;
                }

                // check if the new guess is within the grid bounds
                if (gx >= 0 && gx < 7 && gy >= 0 && gy < 7 && computerGuesses[gx, gy] == 0)
                {
                    // if the new guess is valid, mark it as a guess
                    computerGuesses[gx, gy] = 1;

                    // check if the computer hit a player's ship
                    if (playerBoardData[gx, gy] == 1)
                    {
                        // update UI to indicate a hit
                        playersBoard[gx, gy].BackColor = Color.Yellow;
                        playersBoard[gx, gy].ForeColor = Color.Yellow;
                        computerHits++;
                        lastHit[0] = gx;
                        lastHit[1] = gy;
                    }
                }
                else
                {
                    // if the new guess is not valid, generate a random guess
                    computerPlayer_random_guess();
                }
            }
            else
            {
                // if the computer has not made any previous hits, generate a random guess
                computerPlayer_random_guess();
            }
        }

        private void computerPlayer_random_guess()
        {
            Random rd = new Random();
            int gx, gy;

            do
            {
                gx = rd.Next(7);
                gy = rd.Next(7);
            } while (computerGuesses[gx, gy] != 0);

            // mark the guess
            computerGuesses[gx, gy] = 1;

            // check if the computer hit a player's ship
            if (playerBoardData[gx, gy] == 1)
            {
                // update UI to indicate a hit
                playersBoard[gx, gy].BackColor = Color.Yellow;
                playersBoard[gx, gy].ForeColor = Color.Yellow;
                computerHits++;
                lastHit[0] = gx;
                lastHit[1] = gy;
            }
            else
            {
                // Update UI to indicate a miss
                playersBoard[gx, gy].ForeColor = Color.Black;
                playersBoard[gx, gy].BackColor = Color.Black;
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