using GridGame_Battleships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Manager
{
    private static Manager instance;

    // Game state property
    public int GameState { get; set; }
    public int[,] playerGrid = new int[7, 7];
    public ShipControl[,] playerShips = new ShipControl[5, 1]; // 5 ships, 1 column;

    // Private constructor to enforce singleton pattern
    private Manager()
    {
        GameState = 0; // Set default initial state, 0 menu, 1 game, 2 exit, 3 gameplay, 4page to ask if they'd like to play again
        
    }

    // Singleton instance
    public static Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Manager();
            }
            return instance;
        }
    }
}