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

    public void StartSimulation(ulong nbrOfSimulation, int diffictulty) // utiliser pour la simmulation des algorithmes de l'IA
    {
        ulong c = 0;
        
        // On instancie les IA avec leurs classes respective, en mode debug pour pas qu'elle ne fournissent d'output dans la console
        var iaHealer = new Ia(HeroClassesEnum.Healer, diffictulty,true); 
        var iaDamager = new Ia(HeroClassesEnum.Damager,diffictulty, true);
        var iaTank = new Ia(HeroClassesEnum.Tank,diffictulty, true);
        var iaCustom = new Ia(HeroClassesEnum.Custom,diffictulty, true);
        
        // Tableau qui stocke le nombre de fois gagner par une classe en fonction d'une autre classe, les colonnes représentent le nombre de fois gagner par l'ia, et la ligne l'adversaire.
        float[,] numberOfTimeWon = new float[4,4];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                numberOfTimeWon[i, j] = 0;
            }
        }
        
        HeroAction actionIa1;
        HeroAction actionIa2;

        Ia currentAi1;
        Ia currentAi2;
    
        List<Ia> list = new List<Ia>() { iaDamager, iaHealer, iaCustom, iaTank };
        
        for(int ligne = 0; ligne < 4; ligne++) 
        for (int colonne = (ligne + 1); colonne < 4; colonne++) // Nous n'avons que besoin de ressenser la moitier de la liste (sans diagonale) car il n'y a pas de combat entre deux meme classes,
                                                                // et car il est inutile de simuler l'autre parti du tableau, car les entrée son commutative.
        {
            currentAi1 = list[ligne];
            currentAi2 = list[colonne];
            
            currentAi1.SetAdversary(currentAi2.Class); // Change l'adversaire de l'ia 1 et 2, permet de ne pas a avoir recrée une isntance a chaque changement.
            currentAi2.SetAdversary(currentAi1.Class);

            for(uint i = 0; i < nbrOfSimulation; i++)
            {
                

                int j = 0;
                while (!(currentAi1.Class.CurrentHp <= 0 || currentAi2.Class.CurrentHp <= 0) && j < 20) // Après 100 Tour, le combat s'arrete, car jugée infini, cela coupe le temps de simulation au coup de a fiabilité.
                {
                    actionIa1 = AITurn(currentAi1);
                    actionIa2 = AITurn(currentAi2);
                
                    if (actionIa1.order <= actionIa2.order) 
                    {
                        actionIa1.action.Invoke();
                        actionIa2.action.Invoke();
                    }
                    else
                    {
                        actionIa2.action.Invoke();
                        actionIa1.action.Invoke();
                    }
                    
                    j++;

                } // Fin des tours, commancement du comptage

                if (currentAi1.Class.CurrentHp <= 0) // Ajoute un point a une ia si elle est gagnante, ne compte pas de point si le résultat est compté comme infini.
                {
                    numberOfTimeWon[ligne, colonne] += 1;
                }
                else if(currentAi2.Class.CurrentHp <= 0)
                {
                    numberOfTimeWon[colonne, ligne] += 1;
                }

                c += 1; // counter du nombre de combat simuler
                currentAi1.Reset(); // Reset les variables comme la vie etc pour recommancer un tour.
                currentAi2.Reset();

                Console.WriteLine($"Simulation is {(((float)c / (6 * nbrOfSimulation))).ToString("P3")} Done."); // permet de suivre le progrès de la simulation.
            } // Fin de la simulation des deux ia, changement a d'autre ia
        } // Fin de boucle

        Affichage(numberOfTimeWon, nbrOfSimulation);

    }

    public void Affichage(float[,] numberOfTimeWon, ulong nbrOfSimulation)
    {
        string[] names = { "Damager", "Healer", "Custom", "Tank" };

        const int cellWidth = 12;

        // Ligne de séparation
        string separator = "+";
        for (int i = 0; i < names.Length + 1; i++)
        {
            separator += new string('-', cellWidth) + "+";
        }

        // En-tête
        Console.WriteLine(separator);

        Console.Write($"|{"",cellWidth}");
        foreach (var name in names)
        {
            Console.Write($"|{name,cellWidth}");
        }
        Console.WriteLine("|");

        Console.WriteLine(separator);

        for (int i = 0; i < names.Length; i++)
        {
            Console.Write($"|{names[i],cellWidth}");

            for (int j = 0; j < names.Length; j++)
            {
                string value;

                if (i == j)
                {
                    value = "-";
                }
                else
                {
                    value = (numberOfTimeWon[i, j] / nbrOfSimulation).ToString("P2");
                }

                Console.Write($"|{value,cellWidth}");
            }

            Console.WriteLine("|");
        }

        Console.WriteLine(separator);
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
