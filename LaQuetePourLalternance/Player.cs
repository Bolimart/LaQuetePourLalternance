namespace LaQuetePourLalternance;

public class Player
{
    public Player()
    {
        var s = """
                   Quelle classe choisissez vous ?
                    1 - Azdelux (Tank) : Il est juste con (c'est lui qui la dit pas moi).
                    2 - Folet (Damager) : Normal man: sa couleur préféré, le bleu; son plat préféré, la pizza; ce crois cool mais personne ne connait son nom.
                    3 - Yithash (Healer) : « Venu soigner votre petit cœur de fragile » -Jacky Prieur.
                    4 - Bolimart (Alternant) : Le vilain de l'histoire, il a juste eu son alternance.
                   """;
        var classId = int.Parse(Helper.Asking(s, new []{"1", "2", "3", "4"}));
        var choice = classId switch
        {
            1 => HeroClassesEnum.Tank,
            2 => HeroClassesEnum.Damager,
            3 => HeroClassesEnum.Healer,
            _ => HeroClassesEnum.Custom
        };
        
        Class = InitiateClass(choice);
    }
    
    private Hero Class;

    public int DamageTaken; // Pour l'IA Ameliorer
    private int CurrentHealth => Class.CurrentHp; // Pour l'IA Ameliorer
    private int MaxHealth => Class.MaxHp; // pour l'IA Ameliorer

    public void SetAdversary(Hero adv) => Class.SetAdversary(adv);
    
    public void Attack()
    {
        int choice = 0;
        var s = $"""
                 1 - Attack : {Class.attackDesc}
                 2 - Defend : {Class.deffendDesc}
                 3 - Special : {Class.specialDesc}
                 """;
        choice = int.Parse(Helper.Asking(s, new []{"1", "2", "3"}));
            
        if (choice == 2)
        {
            Class.Defend();
        }else if (choice == 3)
        {
            Class.Special();
        }
        else
        {
            Class.DoDamage(Class.Damage);
        }
    }

    private Hero InitiateClass(HeroClassesEnum heroChoice)
    {
        switch (heroChoice)
        {
            case HeroClassesEnum.Damager :
                return new Damager();
                break;
            case HeroClassesEnum.Healer :
                return new Healer();
                break;
            case HeroClassesEnum.Tank :
                return new Tank();
                break;
            default:
                return new Damager();
        } 
    }
}