using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// Program.cs
// Handles what form 

namespace GridGame_Battleships
{
    internal static class Program
    {
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

                if(Manager.Instance.GameState == 3) // if its 3 it means GAMEPLAY screen should be displayed
                {
                    Application.Run(new GAMEPLAY());
                }

                if (Manager.Instance.GameState == 4) // if its 4 it means a GAME has finished via a win
                {
                    Application.Run(new FINISHED());
                }
            }

        }
    }
}

