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

                    //computersBoard[x, y].Click += new EventHandler(this.computersBoard_Click);
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

            // assign the computer's ships randomly
            // to determine if a ship is vertical (0) or horizontal (1)
            Random direction = new Random();
         
            Random rd = new Random();
            int rand_num = rd.Next();
            

            // ship1 --> covers 2 squares
            int s1x1 = rd.Next(1, 7);
            int s1y1 = rd.Next(1, 7);
            int s1x2;
            int s1y2;
            // ship1's first coordinate is (s1x1, s1y1)
            
            int ship1d = direction.Next(0, 1);
            // find out ship1's other coordinate
            if (ship1d == 0)
            {
                s1x2 = s1x1;
                // vertical - change y
                if (s1x1 == 1)
                {
                    s1y2 = 2;
                } else if (s1x1 == 7)
                {
                    s1y2 = 6;
                } else
                {
                    s1y2 = s1y1 + 1;
                }

            } else
            {
                // horizontal - change x
                s1y2 = s1y1;
                if (s1x1 == 1)
                {
                    s1x2 = 2;
                }
                else if (s1x1 == 7)
                {
                    s1x2 = 6;
                }
                else
                {
                    s1x2 = s1x1 + 1;
                }
            }
            // add the coordinates to computerOccupied
            computerOccupied[s1x1, s1y1] = 1;
            computerOccupied[s1x2, s1y2] = 1;
            


            // ship2 --> covers 3 squares
            int s2x1 = rd.Next(1, 7);
            int s2y1 = rd.Next(1, 7);
            // check that this point isn't already occupied
            while (computerOccupied[s2x1, s2y1] != 0)
            {
                // guess again until it is a position that isn't occupied
                s2x1 = rd.Next(1, 7);
                s2y1 = rd.Next(1, 7);
            }
            // ship2's first coordinate is (s2x1, s2y1)
            int s2x2, s2x3;
            int s2y2, s2y3;
            int ship2d = direction.Next(0, 1);
            if (ship2d == 0)
            {
                // vertical
                
                
            } else
            {
                // horizontal
            }


            // ship3 --> covers 4 squares
            

            // ship4 --> covers 5 squares
            
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
    }
}