using System.Security.Cryptography.X509Certificates;
using LaQuetePourLalternance.Graphics2dApi;
using LaQuetePourLalternance.Graphics2dApi.BasicObjects;
using Window = LaQuetePourLalternance.Graphics2dApi.Window;

namespace LaQuetePourLalternance;

internal class Program
{
    public static void Main(string[] args)
    {
        GameEnvironement.Init(854, 480, "La quête pour l'alternance");
        
        var shader = new Shader("shader.vert", "shader.frag");

        var fond = new StaticImage(new SquareData(
            new(0, 0, 0), new(2, 2, 1), 0.0f,
            new Texture("EpicGameIntro.png"), shader));
        fond.Data.Scale *= 1.5f;
        GameEnvironement.Window.AddGameObject(fond);
        
        Texture[] t = new[] { new Texture("Julien/hit1.png") };

        Action<string> a = s => Console.WriteLine(s);
        
        var Nom = new StringObject("Quel est ton nom ?", new(-200, 30, 0), new(1f, 1f, 1), 45.0f);
        var input = new InputObject(a, new(-200, 0, 0), new(1f, 1f, 1), 0.0f);
        
        GameEnvironement.Window.AddGameObjects(new []{Nom, input});
        
        GameEnvironement.Window.Run();
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