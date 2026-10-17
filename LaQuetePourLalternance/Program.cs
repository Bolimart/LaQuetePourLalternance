using System.Diagnostics;
using System.Globalization;

namespace LaQuetePourLalternance;

internal class Program
{
    public static void Main(string[] args)
    {
        Game game = new();

        if (args.Length == 2)
        {
            if (!ulong.TryParse(args[1], null, out var result))
            {
                Console.WriteLine("Usage: LaQuetePourLalternance [0 < difficultyOfTheAIs < 3] [numberOfSimulationPerCombat]");
                return ;
            }
            
            if (!int.TryParse(args[0], null, out var result2))
            {
                Console.WriteLine("Usage: LaQuetePourLalternance [0 < difficultyOfTheAIs < 3] [numberOfSimulationPerCombat]");
                return ;
            }

            if (result2 < 0 || result2 > 2)
            {
                Console.WriteLine("Usage: LaQuetePourLalternance [0 < difficultyOfTheAIs < 3] [numberOfSimulationPerCombat]");
                return ;
            }
            
            game.StartSimulation(result, result2);
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