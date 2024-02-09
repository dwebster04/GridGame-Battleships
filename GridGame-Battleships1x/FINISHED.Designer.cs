using System;
using System.Diagnostics;
using System.Windows.Forms;

// FINISHED.Designer.cs
// This form is for when the game ends and gives the user the option to quit or play again

namespace GridGame_Battleships
{
    partial class FINISHED
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

        private Button btnAgain;
        private Button btnExit;
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 70);
            this.Text = "FINISHED";

            // btnAgain

            this.btnAgain = new Button();
            this.btnAgain.BackColor = System.Drawing.Color.SkyBlue;
            this.btnAgain.Location = new System.Drawing.Point(10, 10);
            this.btnAgain.Name = "btnAgain";
            this.btnAgain.Size = new System.Drawing.Size(100, 50);
            this.btnAgain.TabIndex = 0;
            this.btnAgain.Text = "Play again";
            this.btnAgain.UseVisualStyleBackColor = false;
            this.btnAgain.Click += new System.EventHandler(this.btnAgain_Click);
            btnAgain.MouseEnter += new EventHandler(Btn_MouseEnter);
            btnAgain.MouseLeave += new EventHandler(Btn_MouseLeave);

            // btnExit

            this.btnExit = new Button();
            this.btnExit.BackColor = System.Drawing.Color.SkyBlue;
            this.btnExit.Location = new System.Drawing.Point(120, 10);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 50);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExitEvent_Click);
            btnExit.MouseEnter += new EventHandler(Btn_MouseEnter);
            btnExit.MouseLeave += new EventHandler(Btn_MouseLeave);

            this.Controls.Add(btnAgain);
            this.Controls.Add(btnExit);
        }

        // change the button color when the mouse enters
        void Btn_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = System.Drawing.Color.Blue;
        }

        // change the button color when the mouse enters
        void Btn_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = System.Drawing.Color.DarkOrchid;
        }

        // when play again is clicked display the GAME screen where the user chooses their ship locations
        void btnAgain_Click(object sender, EventArgs e)
        {
            Manager.Instance.GameState = 1;
            this.Close();
        }

        // when the exit button is clicked display the menu
        void btnExitEvent_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text);
            Manager.Instance.GameState = 0;
            this.Close();
        }
    }
}