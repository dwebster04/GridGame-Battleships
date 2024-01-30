using System.Windows.Forms;
using System.Drawing;
using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace GridGame_Battleships
{
    partial class PLAY
    {
        public Button[,] btnGrid = new Button[7, 7];

        

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "PLAY";

            // Call the method to create grid of buttons
            createGridButton();
        }

        private void createGridButton()
        {
            // this grid will be used by the user to choose which position on the grid 

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    btnGrid[x, y] = new Button();
                    btnGrid[x, y].SetBounds(25 + (50 * x), 25 + (50 * y), 50, 50);
                    btnGrid[x, y].BackColor = Color.PowderBlue;
                    btnGrid[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    btnGrid[x, y].Click += new EventHandler(this.btnGridEvent_Click);
                    Controls.Add(btnGrid[x, y]);
                }
            }
        }

        private void btnGridEvent_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text);
        }

        #endregion
    }

    
}
