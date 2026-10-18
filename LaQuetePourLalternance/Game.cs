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

    /// <summary>
    /// Commence une partie, une fois la partie terminée, recomence une partie si le joueur le souhaite.
    /// </summary>
    public void StartGame()
    {
        while(true) // Boucle étant casser plus tard
        {
            player = new Player(); // instantie la classe joueur et le bot
            bot = new(); 
            
            player.SetAdversary(bot.Class); // asigne dans la classe Hero (contenu dans la classe Player et Ia) l'adversaire qu'il va affronter.
            bot.SetAdversary(player.Class);
            
            HeroAction actionJoueur; // struct stockant une Action ainsi que son ordre.
            HeroAction actionBot;
            // Permet d'éviter des problèmes comme l'IA qui se défend après que le joueur attaque
            
            while (!EndGame()) // Tant que le Joueur ou l'Ia n'est pas mort(e) 
            {
                actionJoueur = PlayerTurn(); // Affiche demande le choix d'action au Joueur/Ia, affiche l'UI, puis reinitialise les flags du tour précédent.
                actionBot = BotTurn();
                
                Console.WriteLine("-----------------------------------------------------");
                if (actionJoueur.order <= actionBot.order) // Permet de faire en sorte que les deux actions ce passe en "meme temps", afin d'éviter certains bug.
                {
                    actionJoueur.action();
                    Console.WriteLine();
                    actionBot.action();
                }
                else
                {
                    actionBot.action();
                    Console.WriteLine();
                    actionJoueur.action();
                }
                Console.WriteLine("-----------------------------------------------------");
                
            }

            DisplayResult(); // Affiche qui a gagner.

            string restartAnswer = Helper.Asking("Voulez-vous recommencer la partie ? \n-o OUI\n-n NON", new[]{"o", "n"}); // Demande au joueur si il veux refaire une partie.

            if (restartAnswer == "n") // Si le joueur ne veux pas refaire une partie alors sortir de la boucle
                break;
        }
    }

    /// <summary>
    /// Joue le tour du Joueur
    /// </summary>
    /// <returns>Retourne L'action ainsi que sont ordre dans la pile d'action</returns>
    public HeroAction PlayerTurn()
    {
        Console.WriteLine("[ TOUR DU JOUEUR ]");
        DisplayUi();
        player.Class.StartTurn();
        return player.Attack();
        //Console.ReadLine();
    }  
    
    /// <summary>
    /// Joue le tour de l'IA
    /// </summary>
    /// <returns>Retourne L'action ainsi que sont ordre dans la pile d'action</returns>
    public HeroAction BotTurn()
    {
        Console.WriteLine("[ TOUR DE L'IA ]");
        DisplayUi();
        bot.Class.StartTurn();
        return bot.Attack();
        //Console.ReadLine();
    }
    
    /// <summary>
    /// Big one liner d'affichage UI
    /// </summary>
    public void DisplayUi()
    {
        Console.WriteLine($"Joueur [ {player.Class.Name} ] <<PV>> {player.Class.CurrentHp}/{player.Class.MaxHp} | <<AP>> {player.Class.Damage}        -        IA [ {bot.Class.Name} ] <<PV>> {bot.Class.CurrentHp}/{bot.Class.MaxHp} | <<AP>> {bot.Class.Damage}");
    }
    
    /// <summary>
    /// Affiche si le joueur a gagner, perdu, ou a fais la troisième option secrète
    /// </summary>
    public void DisplayResult()
    {
        if(player.Class.CurrentHp == bot.Class.CurrentHp)
            Console.WriteLine("Bravo ! Vous avez gagné... et perdu en même temps...");
        else if (player.Class.CurrentHp > bot.Class.CurrentHp)
            Console.WriteLine("Bravo ! Vous avez gagné !"); 
        else
            Console.WriteLine("Boouuuh vous avez perdu !"); 
    }

    /// <summary>
    /// Vérifie si l'Ia ou le Joueur est mort
    /// </summary>
    /// <returns>La vérité du la mort</returns>
    public bool EndGame()
    {
        return player.Class.CurrentHp <= 0 || bot.Class.CurrentHp <= 0;
    }
    #endregion
}
