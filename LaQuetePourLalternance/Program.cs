namespace LaQuetePourLalternance;

internal class Program
{
    public static void Main(string[] args)
    {
        Game game = new();

        if (args.Length >= 1)
        {
            game.StartSimulation();
            return ;
        }
        
        game.StartGame();
        
    }
}


static class Helper 
{
    public static Random Rand = new ();
    
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