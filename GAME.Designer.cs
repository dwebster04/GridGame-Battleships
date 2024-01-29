using System.Drawing;
using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace GridGame_Battleships
{

    partial class GAME
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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 800);
            this.Text = "GAME";

            Button[,] btn = new Button[10, 10];

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    btn[x, y] = new Button();
                    btn[x, y].SetBounds(55 + (55 * x), 55 + (55 * y), 45, 45);
                    btn[x, y].BackColor = Color.PowderBlue; btn[x, y].Text = Convert.ToString((x + 1) + "," + (y + 1));
                    btn[x, y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(btn[x, y]);
                }

            }

        }
        void btnEvent_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text); // SAME handler as before
        }
        void GAME_Load(object sender, EventArgs e) //REQUIRED
        { }
    }
}

         