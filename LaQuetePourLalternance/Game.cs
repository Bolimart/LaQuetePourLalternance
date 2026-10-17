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
            HeroAction actionJoueur;
            HeroAction actionBot;

            
            while (!EndGame())
            {
                actionJoueur = PlayerTurn();
                actionBot = BotTurn();
                
                Console.WriteLine("-----------------------------------------------------");
                if (actionJoueur.order <= actionBot.order)
                {
                    actionJoueur.action.Invoke();
                    Console.WriteLine();
                    actionBot.action.Invoke();
                }
                else
                {
                    actionBot.action.Invoke();
                    Console.WriteLine();
                    actionJoueur.action.Invoke();
                }
                Console.WriteLine("-----------------------------------------------------");
                
            }

            DisplayResult();

            string restartAnswer = Helper.Asking("Voulez-vous recommencer la partie ? \n-o OUI\n-n NON", new[]{"o", "n"});

            if (restartAnswer == "n")
                break;
        }
    }

    public HeroAction PlayerTurn()
    {
        Console.WriteLine("[ TOUR DU JOUEUR ]");
        Display();
        player.Class.StartTurn();
        return player.Attack();
        //Console.ReadLine();
    }  
    
    public HeroAction BotTurn()
    {
        Console.WriteLine("[ TOUR DE L'IA ]");
        Display();
        bot.Class.StartTurn();
        return bot.Attack();
        //Console.ReadLine();
    }
    
    
    public void Display()
    {
        Console.WriteLine($"Joueur [ {player.Class.Name} ] <<PV>> {player.Class.CurrentHp}/{player.Class.MaxHp} | <<AP>> {player.Class.Damage}        -        IA [ {bot.Class.Name} ] <<PV>> {bot.Class.CurrentHp}/{bot.Class.MaxHp} | <<AP>> {bot.Class.Damage}");
    }
    
    public void DisplayResult()
    {
        if(player.Class.CurrentHp == bot.Class.CurrentHp)
            Console.WriteLine("Bravo ! Vous avez gagné... et perdu en même temps...");
        else if (player.Class.CurrentHp > bot.Class.CurrentHp)
            Console.WriteLine("Bravo ! Vous avez gagné !"); 
        else
            Console.WriteLine("Boouuuh vous avez perdu !"); 
    }

    public bool EndGame()
    {
        return player.Class.CurrentHp <= 0 || bot.Class.CurrentHp <= 0;
    }
    #endregion
}
