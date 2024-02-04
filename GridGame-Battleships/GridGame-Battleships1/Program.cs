using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// menu will have a start button to start the game, rules button that will display info on how to play
//*leaderboard button, log in button (if not logged in player will be 'Guest')

// will be player vs computer
// on game start user decides where to place their ships on the grid
// computers ships are randomly selected on to the computers grid
// user selects a square, if a square selected is the a sqaure that the computer has a ship on turn green, if not turn red
// computer is then to select a square to attempt a hit on a user ship
// game ends when either the player or computers ships have all been sunk
// display winner to the user

//*have saved users that would have saved wins and losses, leaderboard with highest win/loss ratio at the top

namespace GridGame_Battleships
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (Manager.Instance.GameState != 2) // if its 2 it means EXIT has been pressed
            {
                if (Manager.Instance.GameState == 0)
                {
                    Application.Run(new Menu());
                }

                if (Manager.Instance.GameState == 1) // if its 1 it means START has been pressed
                {
                    Application.Run(new GAME());
                }

                if(Manager.Instance.GameState == 3)
                {
                    Application.Run(new GAMEPLAY());
                }

                if (Manager.Instance.GameState == 4)
                {
                    Application.Run(new FINISHED());
                }
            }

        }
    }
}

