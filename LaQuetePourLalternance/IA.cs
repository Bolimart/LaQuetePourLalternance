namespace LaQuetePourLalternance;

using System;
public class Ia
{
    public Ia()
    {
        var rand = new Random();
        var heroClass = (HeroClassesEnum)(rand.Next() % 3);
        _class = InitiateClass(heroClass);
    }
    
    private Hero _class;
    
    public Hero Class => _class;

    public int DamageTaken; // Pour l'IA Ameliorer
    private int CurrentHealth => _class.CurrentHp; // Pour l'IA Ameliorer
    private int MaxHealth => _class.MaxHp; // pour l'IA Ameliorer

    public void SetAdversary(Hero adv) => _class.SetAdversary(adv);
    
    public void Attack()
    {
        var rand = new Random();

        int rdmNumber = rand.Next() % 3;
        if (rdmNumber == 0)
        {
            _class.Defend();
        }else if (rdmNumber == 1)
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