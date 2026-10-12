namespace LaQuetePourLalternance;

public class Player
{
    public Player()
    {
        var choice = HeroClassesEnum.Custom; //TODO : make choice for player;
        Class = InitiateClass(choice);
    }
    
    private Hero Class;

    public int DamageTaken; // Pour l'IA Ameliorer
    private int CurrentHealth => Class.CurrentHp; // Pour l'IA Ameliorer
    private int MaxHealth => Class.MaxHp; // pour l'IA Ameliorer

    public void SetAdversary(Hero adv) => Class.SetAdversary(adv);
    
    public void Attack()
    {
        
        //Make player choose
        int choice = 0;
        if (choice == 0)
        {
            Class.Defend();
        }else if (choice == 1)
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