using LaQuetePourLalternance.Graphics2dApi;
using LaQuetePourLalternance.Graphics2dApi.BasicObjects;
using Window = LaQuetePourLalternance.Graphics2dApi.Window;

namespace LaQuetePourLalternance;

internal class Program
{
    public static void Main(string[] args)
    {
        GameEnvironement.Init(854, 480, "La quête pour l'alternance");
        
        GameEnvironement.Window.Run();
        
        // game.StartGame();
    }
}

static class GameEnvironement
{
    public static Random Rand = new ();
    public static Window Window { get; private set; }
    public static Game Game { get; private set; }

    public static void Init(int width, int height, string title)
    {
        Window = new Window(width, height, title);
        Game = new Game();
    }
}


static class Helper 
{
    public static string Asking(string question, string?[] answers = null)
    {
        string answer = "";
        //
        bool valid = false;
        
        while (!valid){
            Console.WriteLine(question);
            //Si on note rien en readline c'est possible que ça renvoie une erreur donc pour s'assurer que tout se passe bien
            //on dit "si il n'y a rien, tu vas stocker "" (string vide)
            answer = Console.ReadLine() ?? "";

            //Si answers est vide 
            if (answers == null && answer != "") break;

            else{
                foreach (string a in answers){
                    if (a == answer) {
                        valid = true;
                        break; 
                    }
                }
            }
        }
        return answer;
    }
    
}