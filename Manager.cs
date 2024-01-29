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

    // Private constructor to enforce singleton pattern
    private Manager()
    {
        GameState = 0; // Set default initial state, 0 menu, 1 game, 2 exit
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