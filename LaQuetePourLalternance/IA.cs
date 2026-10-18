namespace LaQuetePourLalternance;

using System;

/// <summary>
/// Classe s'occupant du fonctionnement de l'IA
/// </summary>
public class Ia
{
    /// <summary>
    /// Selectionne l'ia de façons aléatoire
    /// </summary>
    public Ia()
    {
        var rand = Helper.Rand;
        var heroClass = (HeroClassesEnum)(rand.Next() % 3); 
        _class = InitiateClass(heroClass);
    }
    
    private Hero _class;
    
    public Hero Class => _class;

    public int DamageTaken; // Pour l'IA Ameliorer
    private int CurrentHealth => _class.CurrentHp; // Pour l'IA Ameliorer
    private int MaxHealth => _class.MaxHp; // pour l'IA Ameliorer

    public void SetAdversary(Hero adv) => _class.SetAdversary(adv);
    
    /// <summary>
    /// Fait choisir l'action que l'IA va faire aléatoirement
    /// </summary>
    /// <returns>l'action faite et son ordre</returns>
    public HeroAction Attack()
    {
        var rand = Helper.Rand;

        Action action;
        int order = 0;

        int rdmNumber = rand.Next() % 3;
        if (rdmNumber == 0)
        {
            //Console.WriteLine($"{Class.Name} décide de se défendre face au danger !");
            action = _class.Defend;
            order = _class.DefenseOrder;
        }else if (rdmNumber == 1)
        {
            //Console.WriteLine($"{Class.Name} utilise son abilité spéciale !");
            action = _class.Special;
            order = _class.SpecialOrder;
        }
        else
        {
            action = () => { 
                _class.DoDamage(_class.Damage);
            };
            order = _class.AttackOrder;
        }

        return new HeroAction(action, order);
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