namespace LaQuetePourLalternance;

/// <summary>
/// Classe s'occupant du fonctionnement du Joueur
/// </summary>
public class Player
{
    /// <summary>
    /// Demande au joueur qu'elle classe il veux jouer
    /// </summary>
    public Player()
    {
        var s = """
                   Quelle classe choisissez vous ?
                    1 - Azdelux (Tank) : Il est juste con (c'est lui qui la dit pas moi).
                    2 - Folet (Damager) : Normal man: sa couleur préféré, le bleu; son plat préféré, la pizza; ce crois cool mais personne ne connait son nom.
                    3 - Yithash (Healer) : « Venu soigner votre petit cœur de fragile » -Jacky Prieur.
                    4 - Bolimart (Alternant) : Le vilain de l'histoire, il a juste eu son alternance.
                   """;
        var classId = Helper.Asking(s, new []{"1", "2", "3", "4"});
        var choice = classId switch
        {
            "1" => HeroClassesEnum.Tank,
            "2" => HeroClassesEnum.Damager,
            "3" => HeroClassesEnum.Healer,
            _ => HeroClassesEnum.Custom,
        };
        
        _class = InitiateClass(choice);
    }
    
    private Hero _class;

    public Hero Class => _class;

    public void SetAdversary(Hero adv) => _class.SetAdversary(adv);
    
    /// <summary>
    /// Fait choisir l'action faite au joueur
    /// </summary>
    /// <returns>Action faite et son ordre</returns>
    public HeroAction Attack()
    {
        int choice = 0;
        var s = $"""
                 1 - Attack : {_class.AttackDesc}
                 2 - Defend : {_class.DeffendDesc}
                 3 - Special : {_class.SpecialDesc}
                 """;
        choice = int.Parse(Helper.Asking(s, new []{"1", "2", "3"}));

        Action action;
        int order = 0;
        if (choice == 2)
        {
            action =_class.Defend;
            order = 4;
        }else if (choice == 3)
        {
            action =_class.Special;
            order = _class.SpecialOrder;
        }
        else
        {
            action = () => _class.DoDamage(_class.Damage);
            order = 5;
        }

        return new HeroAction(action, order);
    }

    private Hero InitiateClass(HeroClassesEnum heroChoice)
    {
        switch (heroChoice)
        {
            case HeroClassesEnum.Damager :
                return new Damager();
            case HeroClassesEnum.Healer :
                return new Healer();
            case HeroClassesEnum.Tank :
                return new Tank();
            case HeroClassesEnum.Custom:
                return new Custom();
            default:
                return new Custom();
        } 
    }
}