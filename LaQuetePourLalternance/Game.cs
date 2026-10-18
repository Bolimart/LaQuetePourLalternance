using LaQuetePourLalternance.Graphics2dApi;
using LaQuetePourLalternance.Graphics2dApi.BasicObjects;

namespace LaQuetePourLalternance;

public class Game : GameObject, ISceneInitializer
{
    #region - Fields & Properties

    private Player player;
    private Ia bot;
    private Window _window;
    
    // text
    private StringObject _text;

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
        display();
        player.Class.StartTurn();
        player.Attack();
        Console.ReadLine();
    }  
    
    public void botTurn()
    {
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
            _text.Text = "Bravo ! Vous avez gagné !"; 
        else
            _text.Text = "Boouuuh vous avez perdu !"; 
    }

    public bool endGame()
    {
        return player.Class.CurrentHp <= 0 || bot.Class.CurrentHp <= 0;
    }
    #endregion

    public void LoadScene()
    {
        _window = GameEnvironement.Window;

        _text = new("", new(-200, -80, 0), timeBetweenChar: 0.1f);
        
        // Init Player 
        
        // Init Ennemy
        
    }

    public void UnloadScene()
    {
        throw new NotImplementedException();
    }
}
