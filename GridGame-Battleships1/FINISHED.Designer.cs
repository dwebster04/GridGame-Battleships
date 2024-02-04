using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GridGame_Battleships
{
    partial class FINISHED
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

        private Button btnAgain;
        private Button btnExit;
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 450); // 175 + 210 + 25
            this.Text = "FINISHED";

            // create and initialise the play again button
            this.btnAgain.BackColor = System.Drawing.Color.SkyBlue;
            this.btnAgain.Location = new System.Drawing.Point(280, 175);
            this.btnAgain.Name = "btnAgain";
            this.btnAgain.Size = new System.Drawing.Size(400, 75);
            this.btnAgain.TabIndex = 0;
            this.btnAgain.Text = "Play again";
            this.btnAgain.UseVisualStyleBackColor = false;
            this.btnAgain.Click += new System.EventHandler(this.btnAgain_Click);
            btnAgain.MouseEnter += new EventHandler(Btn_MouseEnter);
            btnAgain.MouseLeave += new EventHandler(Btn_MouseLeave);

            // create and initialise the exit button
            this.btnExit.BackColor = System.Drawing.Color.SkyBlue;
            this.btnExit.Location = new System.Drawing.Point(280, 275);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(400, 75);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExitEvent_Click);
            btnExit.MouseEnter += new EventHandler(Btn_MouseEnter);
            btnExit.MouseLeave += new EventHandler(Btn_MouseLeave);

        }

        void Btn_MouseEnter(object sender, EventArgs e)
        {
            // Change the button color when the mouse enters
            ((Button)sender).BackColor = System.Drawing.Color.Blue;
        }

        void Btn_MouseLeave(object sender, EventArgs e)
        {
            // Change the button color back to its original color when the mouse leaves
            ((Button)sender).BackColor = System.Drawing.Color.DarkOrchid;
        }

        void btnAgain_Click(object sender, EventArgs e)
        {
            Manager.Instance.GameState = 1;
            this.Close();
        }

        void btnExitEvent_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text);
            Manager.Instance.GameState = 0;
            this.Close();
        }
    }
}