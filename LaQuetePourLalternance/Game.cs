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

    public void StartGame()
    {
        while(true)
        {
            player = new Player();
            bot = new(); 
            
            player.SetAdversary(bot.Class);
            bot.SetAdversary(player.Class);
            
            while (!endGame())
            {
                playerTurn();

                if (endGame()) break;

                botTurn();
            }

            displayResult();

            string restartAnswer = Helper.Asking("Voulez-vous recommencer la partie ? \n-o OUI\n-n NON", new[]{"o", "n"});

            if (restartAnswer == "n")
                break;
        }
    }

    public void playerTurn()
    {
        Console.WriteLine("[ TOUR DU JOUEUR ]");
        display();
        player.Class.StartTurn();
        player.Attack();
        Console.ReadLine();
    }  
    
    public void botTurn()
    {
        Console.WriteLine("[ TOUR DE L'IA ]");
        display();
        bot.Class.StartTurn();
        bot.Attack();
        Console.ReadLine();
    }
    
    
    public void display()
    {
        Console.WriteLine($"Joueur [ {player.Class.Name} ] <<PV>> {player.Class.CurrentHp}/{player.Class.MaxHp} | <<AP>> {player.Class.Damage}        -        IA [ {bot.Class.Name} ] <<PV>> {bot.Class.CurrentHp}/{bot.Class.MaxHp} | <<AP>> {bot.Class.Damage}");
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
