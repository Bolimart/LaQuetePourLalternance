namespace LaQuetePourLalternance;

using System;
public class Ia
{
    public Ia()
    {
        var rand = new Random();
        var heroClass = (HeroClassesEnum)(rand.Next() % 3);
        Class = InitiateClass(heroClass);
    }
    
    private Hero Class;

    public int DamageTaken; // Pour l'IA Ameliorer
    private int CurrentHealth => Class.CurrentHp; // Pour l'IA Ameliorer
    private int MaxHealth => Class.MaxHp; // pour l'IA Ameliorer

    public void SetAdversary(Hero adv) => Class.SetAdversary(adv);
    
    public void Attack()
    {
        var rand = new Random();

        int rdmNumber = rand.Next() % 3;
        if (rdmNumber == 0)
        {
            Class.Defend();
        }else if (rdmNumber == 1)
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
                return new Damager(this);
                break;
            case HeroClassesEnum.Healer :
                return new Healer(this);
                break;
            case HeroClassesEnum.Tank :
                return new Tank(this);
                break;
            default:
                return new Damager(this);
        } 
    }
}