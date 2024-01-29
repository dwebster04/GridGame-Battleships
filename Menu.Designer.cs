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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.DarkOrchid;
            this.btnStart.Location = new System.Drawing.Point(280, 175);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(400, 75);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "START GAME";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStartEvent_Click);
            btnStart.MouseEnter += new EventHandler(Btn_MouseEnter);
            btnStart.MouseLeave += new EventHandler(Btn_MouseLeave);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.DarkOrchid;
            this.btnExit.Location = new System.Drawing.Point(280, 275);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(400, 75);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "EXIT";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExitEvent_Click);
            btnExit.MouseEnter += new EventHandler(Btn_MouseEnter);
            btnExit.MouseLeave += new EventHandler(Btn_MouseLeave);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(980, 590);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnExit);
            this.Name = "Menu";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.Menu_Load_1);
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

        void btnExitEvent_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((Button)sender).Text); // SAME handler as before
                                                    // Update the game state
            Manager.Instance.GameState = 2; // open game
                                            // Close the menu form
            this.Close();
        }

        void Btn_MouseEnter(object sender, EventArgs e)
        {
            // Change the button color when the mouse enters
            ((Button)sender).BackColor = System.Drawing.Color.Red;
        }

        void Btn_MouseLeave(object sender, EventArgs e)
        {
            // Change the button color back to its original color when the mouse leaves
            ((Button)sender).BackColor = System.Drawing.Color.DarkOrchid;
        }
        void Menu_Load(object sender, EventArgs e) //REQUIRED
        { }

        private Button btnStart;
        private Button btnExit;
    }
}
