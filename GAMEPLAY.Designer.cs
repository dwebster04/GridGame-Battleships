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



            CreateButtons();

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

                    //playersBoard[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
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

        private void playersBoard_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text);
        }
        private void computersBoard_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Debug.WriteLine(clickedButton.Text);
            clickedButton.BackColor = Color.Red;
        }
    }
}