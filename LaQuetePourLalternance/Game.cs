using System.ComponentModel;
using System.Reflection.Emit;

namespace LaQuetePourLalternance;

public class Game
{
    #region - Fields & Properties

    Hero player;

    

    #endregion
    #region - Methods
    public Game()
    {
        while (!endGame())
        {
            playerTurn();

            if (endGame()) break;

            botTurn();
        }

        displayResult();

    



    }

    public void playerTurn()
        {
            
        }  
    
    public void botTurn()
        {

        }

    public void displayResult()
    {
        Console.WriteLine($"")

    }

    public bool endGame()
    {
        
        
        return false;
    }
    #endregion
}
