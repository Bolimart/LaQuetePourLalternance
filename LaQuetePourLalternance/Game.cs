using System.ComponentModel;
using System.ComponentModel.Design;
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
            string choixAction = ("");

            while (!(choixAction == "a") || !choixAction == "d" || !choixAction == "s")
            {
                Console.WriteLine("Au tour du joueur");
                Console.WriteLine("- Attaquer -a");
                Console.WriteLine("- Défendre -d");
                Console.WriteLine("- Attaque spéciale -s");

                choixAction = Console.ReadLine();

                if (!choixAction == "a" || !choixAction == "d" || !choixAction == "s")
                    Console.WriteLine("Choix invalide");

                else
                {
                    
                    
                    break;
                }  


            }  




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
