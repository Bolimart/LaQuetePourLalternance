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
    
    
    public void display()
    {
        Console.WriteLine($"IA [ {player.Class.Name} ] <<PV>> {player.Class.CurrentHp}/{player.Class.MaxHp} | <<AP>> {player.Class.Damage}        -        IA [ {bot.Class.Name} ] <<PV>> {bot.Class.CurrentHp}/{bot.Class.MaxHp} | <<AP>> {bot.Class.Damage}");
    }
    
    
    public void displayResult()
    {
        if (player.Class.CurrentHp > bot.Class.CurrentHp)
            Console.WriteLine("Bravo ! Vous avez gagné !"); 
        else
            Console.WriteLine("Boouuuh vous avez perdu !"); 
    }

    public bool endGame()
    {
        return player.Class.CurrentHp <= 0 || bot.Class.CurrentHp <= 0;
    }
    #endregion
}
