using System.ComponentModel;
using System.ComponentModel.Design;
using System.Formats.Asn1;
using System.Reflection.Emit;
using System.Security.Cryptography;

namespace LaQuetePourLalternance;

public class Game
{
    #region - Fields & Properties

    Player player;
    Ia bot;

    

    #endregion
    #region - Methods
    public Game()
    {
        while(true)
        {
            player = new Player();
            bot = new(); 
            
            while (!endGame())
            {
                playerTurn();

                if (endGame()) break;

                botTurn();
            }

            displayResult();

            string restartAnswer = Helper.Asking("Voulez-vous recommencer la partie ?", new[]{"o", "n"});

            if (restartAnswer == "n")
                break;
        }

    }

    public void playerTurn()
    {
        display();
        player.Attack();
    }  
    
    public void botTurn()
    {
        bot.Attack();
        display();
    }

    public void displayResult()
    {
        

    }

    public bool endGame()
    {
        
        
        return false;
    }
    #endregion
}
