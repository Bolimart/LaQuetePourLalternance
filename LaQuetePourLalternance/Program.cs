using LaQuetePourLalternance.Graphics2dApi;
using Window = LaQuetePourLalternance.Graphics2dApi.Window;

namespace LaQuetePourLalternance;

internal class Program
{
    public static void Main(string[] args)
    {
        GameEnvironement.Init(800, 600, "La quête pour l'alternance");
        
        var shader = new Shader("shader.vert", "shader.frag");
        
        GameEnvironement.Window.AddSquare(new Square(new SquareData(
            new(0, 0, 0), new(2, 2, 1), 0.0f, 
            new Texture("EpicGameIntro.png"), shader)));
        
        GameEnvironement.Window.AddSquare(new Square(new SquareData(
            new(0, 0, 0), new(1, 1, 1), 0.0f, 
            new Texture("Vincent_Dead.png"), shader)));
        
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