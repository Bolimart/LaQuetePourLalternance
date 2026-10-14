using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
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

    public void StartSimulation()
    {
        float c = 0;
        var iaHealer = new Ia(HeroClassesEnum.Healer, true);
        var iaDamager = new Ia(HeroClassesEnum.Damager, true);
        var iaTank = new Ia(HeroClassesEnum.Tank, true);
        var iaCustom = new Ia(HeroClassesEnum.Custom, true);

        float[,] numberOfTimeWon = new float[4,4];

        List<Ia> list = new List<Ia>() { iaDamager, iaHealer, iaCustom, iaTank };
        
        for(int k = 0; k < list.Count; k++)
        for (int w = (k + 1); w < list.Count; w++)
        {
            var currentAi1 = list[k];
            var currentAi2 = list[w];
            
            currentAi1.SetAdversary(currentAi2.Class);
            currentAi2.SetAdversary(currentAi1.Class);
            
            for(int i = 0; i < 100; i++)
            {
            
                HeroAction actionJoueur;
                HeroAction actionBot;

                int j = 0;
                while (!(currentAi1.Class.CurrentHp <= 0 || currentAi2.Class.CurrentHp <= 0) && j < 10000)
                {
                    actionJoueur = AITurn(currentAi1);
                    actionBot = AITurn(currentAi2);
                
                    if (actionJoueur.order <= actionBot.order)
                    {
                        actionJoueur.action.Invoke();
                        actionBot.action.Invoke();
                    }
                    else
                    {
                        actionBot.action.Invoke();
                        actionJoueur.action.Invoke();
                    }
                    
                    j++;

                } // Boucle Gameplay limité a 20 turn;

                if (currentAi1.Class.CurrentHp <= 0)
                {
                    Console.WriteLine($"{currentAi1.Class.Name} has won");
                    numberOfTimeWon[k, w] += 1;
                }
                else if(currentAi2.Class.CurrentHp <= 0)
                {
                    Console.WriteLine($"{currentAi2.Class.Name} has won.");
                    numberOfTimeWon[w, k] += 1;
                }
                else
                {
                    numberOfTimeWon[k, w] += 1;
                    numberOfTimeWon[w, k] += 1;

                }

                c += 1;
                currentAi1.Reset();
                currentAi2.Reset();

                Console.WriteLine($"Simulation is {(c / 600) * 100} Done.");
            }
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Console.Write($"|{numberOfTimeWon[i,j]}|");
            }
            Console.WriteLine();
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

    public HeroAction AITurn(Ia ai)
    {
        ai.Class.StartTurn();
        return ai.Attack();
    }
    
    public void Display()
    {
        Console.WriteLine($"Joueur [ {player.Class.Name} ] <<PV>> {player.Class.CurrentHp}/{player.Class.MaxHp} | <<AP>> {player.Class.Damage}        -        IA [ {bot.Class.Name} ] <<PV>> {bot.Class.CurrentHp}/{bot.Class.MaxHp} | <<AP>> {bot.Class.Damage}");
    }
    
    public void DisplayResult()
    {
        if (player.Class.CurrentHp > bot.Class.CurrentHp)
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
