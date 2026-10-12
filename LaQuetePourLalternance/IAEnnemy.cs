namespace LaQuetePourLalternance;

using System;
public class IAEnnemy
{
    public IAEnnemy()
    {
        var rand = new Random();
        AiClass = (HeroClassesEnum)(rand.Next() % 3);
        Class = InitiateClass(AiClass);
    }
    
    public HeroClassesEnum AiClass;
    private Hero Class;

    private int damageTaken; // Pour l'IA Ameliorer
    private int _currentHealth => Class.CurrentHp; // Pour l'IA Ameliorer
    private int _maxHealth => Class.MaxHp; // pour l'IA Ameliorer

    public void TakeDamage(int dmg, bool isSpecial)
    {
        damageTaken = dmg;
        Class.TakeDamage(dmg, isSpecial);
    }

    private void Special()
    {
        // TODO : Class.Special()
    }

    private void Defend()
    {
        // TODO : Class.Defend()
    }

    private void DoDamage()
    {
        // TODO : Class.DoDamage()
    }

    public void Attack()
    {
        var rand = new Random();

        int rdmNumber = rand.Next() % 3;
        if (rdmNumber == 0)
        {
            Defend();
        }else if (rdmNumber == 1)
        {
            Special();
        }
        else
        {
            DoDamage();
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