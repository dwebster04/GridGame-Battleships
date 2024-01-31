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
        private Button[,] playersBoard = new Button[7, 7];
        private int[,] playerBoardData = Manager.Instance.playerGrid;


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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


            CreateButtons();
        }

        private void CreateButtons()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    playersBoard[x, y] = new Button();
                    playersBoard[x, y].SetBounds(25 + (20 * x), 35 + (20 * y), 20, 20);
                    if (playerBoardData[x,y] == 0)
                    {
                        playersBoard[x, y].BackColor = Color.PowderBlue;
                    }
                    if (playerBoardData[x, y] == 1)
                    {
                        playersBoard[x, y].BackColor = Color.MediumOrchid;
                    }
                    
                    //playersBoard[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    playersBoard[x, y].Click += new EventHandler(this.playersBoard_Click);
                    Controls.Add(playersBoard[x, y]);
                }
            }


        }

        private void playersBoard_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text);
        }
    }
}