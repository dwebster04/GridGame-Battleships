using System.Drawing;
using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace GridGame_Battleships
{
    partial class Menu
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
            this.btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.DarkOrchid;
            this.btnStart.Location = new System.Drawing.Point(351, 25);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(150, 45);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "START GAME";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStartEvent_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 800);
            this.Controls.Add(this.btnStart);
            this.Name = "Menu";
            this.Text = "Menu";
            this.ResumeLayout(false);

        }
           
        void btnStartEvent_Click(object sender, EventArgs e)
        {
        Debug.WriteLine(((Button)sender).Text); // SAME handler as before
        // Update the game state
        Manager.Instance.GameState = 1; // open game
        // Close the menu form
        this.Close();
        }
        void Menu_Load(object sender, EventArgs e) //REQUIRED
        { }

        private Button btnStart;
    }
}
