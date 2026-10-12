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
        
        _class = InitiateClass(choice);
    }
    
    private Hero _class;

    public Hero Class => _class;

    public int DamageTaken; // Pour l'IA Ameliorer
    private int CurrentHealth => _class.CurrentHp; // Pour l'IA Ameliorer
    private int MaxHealth => _class.MaxHp; // pour l'IA Ameliorer

    public void SetAdversary(Hero adv) => _class.SetAdversary(adv);
    
    public void Attack()
    {
        int choice = 0;
        var s = $"""
                 1 - Attack : {_class.attackDesc}
                 2 - Defend : {_class.deffendDesc}
                 3 - Special : {_class.specialDesc}
                 """;
        choice = int.Parse(Helper.Asking(s, new []{"1", "2", "3"}));
            
        if (choice == 2)
        {
            _class.Defend();
        }else if (choice == 3)
        {
            _class.Special();
        }
        else
        {
            _class.DoDamage(_class.Damage);
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