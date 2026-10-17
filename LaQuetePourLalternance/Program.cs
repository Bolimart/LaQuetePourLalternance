namespace LaQuetePourLalternance;

internal class Program
{
    public static void Main(string[] args)
    {
        Helper.Audio.Init();
        Helper.Audio.StartEvent("event:/Music/mx_fight");

        Game game = new();
        game.StartGame();
        
        Helper.Audio.StopEvent("event:/Music/mx_fight");
        Helper.Audio.Shutdown();
    }
}


static class Helper 
{
    public static Random Rand = new ();
    public static readonly AudioManager Audio = new();
    
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