namespace LaQuetePourLalternance;

using System;
public class Ia
{
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
    
    public HeroAction Attack()
    {
        var rand = Helper.Rand;
        //TODO : Faire soit en sorte que le Bot commence en premier, soit que le text ce montre apres l'action
        // Car actuellement on peut voir ce que le bot fait a l'avance

        Action action;
        int order = 0;

        int rdmNumber = rand.Next() % 3;
        if (rdmNumber == 0)
        {
            //Console.WriteLine($"{Class.Name} décide de se défendre face au danger !");
            action = _class.Defend;
            order = 4;
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
            order = 5;
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