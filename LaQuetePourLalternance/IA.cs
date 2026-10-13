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
        switch (_class.Classe)
        {
            case HeroClassesEnum.Healer :
                return HealersAI();
            case HeroClassesEnum.Damager :
                return DamagerAI();
            case HeroClassesEnum.Tank :
                return TankAI();
            default:
                return CustomAI();
        }
    }

    private HeroAction HealersAI()
    {
        Action action;
        int order;
        if (_class.CurrentHp <= 2)
        {
            if (_class.Adversary?.CurrentHp <= 2)
            {
                int rdm = Helper.Rand.Next() % 10;
                if (rdm < 3)
                {
                    action = _class.Special;
                    order = _class.SpecialOrder;
                }else
                {
                    action = () => { 
                        _class.DoDamage(_class.Damage);
                    };
                    order = 5;
                }
            }
            else
            {
                action = _class.Special;
                order = _class.SpecialOrder;
            }
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

    private HeroAction DamagerAI()
    {
        Action action;
        int order;
        if (_class.CurrentHp >= _class.Adversary?.Damage)
        {
            action = _class.Special;
            order = _class.SpecialOrder;
        }
        else
        {
            int rdm = Helper.Rand.Next() % 10;
            if (rdm <= 6)
            {
                action = _class.Defend;
                order = 4;
            }
            else
            {
                action = () => { 
                    _class.DoDamage(_class.Damage);
                };
                order = 5;
            }
        }
       
        return new HeroAction(action, order);
    }

    private HeroAction TankAI()
    {
        Action action;
        int order;
        if (_class.Adversary?.CurrentHp == 1 && _class.CurrentHp != 1)
        {
            action = _class.Special;
            order = _class.SpecialOrder;
        }else if (_class.CurrentHp > 2)
        {
            action = _class.Special;
            order = _class.SpecialOrder;
        }
        else
        {
            int rdm = Helper.Rand.Next() % 10;
            if (rdm < 5)
            {
                action = _class.Defend;
                order = 4;
            }
            else
            {
                action = () => { 
                    _class.DoDamage(_class.Damage);
                };
                order = 5;
            }
        }
        return new HeroAction(action, order);
    }

    private HeroAction CustomAI()
    {
        Action action;
        int order;
        if (_class.Damage == 0)
        {
            action = _class.Special;
            order = _class.SpecialOrder;
        }else if (_class.CurrentHp > 2)
        {
            int rdm = Helper.Rand.Next() % 10;
            if (rdm < 7)
            {
                action = _class.Defend;
                order = 4;
            }
            else
            {
                action = _class.Special;
                order = _class.SpecialOrder;
            }
        }
        else
        {
            int rdm = Helper.Rand.Next() % 10;
            if (rdm == 1)
            {
                action = _class.Special;
                order = _class.SpecialOrder;
            }else if (rdm < 4)
            {
                action = _class.Defend;
                order = 4;
            }
            else
            {
                action = () => { 
                    _class.DoDamage(_class.Damage);
                };
                order = 5;
            }
            
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