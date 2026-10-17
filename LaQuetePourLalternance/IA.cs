namespace LaQuetePourLalternance;

using System;
public class Ia
{
    public Ia(HeroClassesEnum? classe = null,int difficulty = 1 ,bool debug = false)
    {
        var rand = Helper.Rand;
        var heroClass = classe == null? (HeroClassesEnum)(rand.Next() % 3) : classe.Value;
        _class = InitiateClass(heroClass);
        Difficulty = difficulty;
        Debug = debug;
    }

    private int Difficulty;

    public bool Debug;
    
    private Hero _class;
    
    public Hero Class => _class;

    public int DamageTaken; // Pour l'IA Ameliorer
    private int CurrentHealth => _class.CurrentHp; // Pour l'IA Ameliorer
    private int MaxHealth => _class.MaxHp; // pour l'IA Ameliorer

    public void SetAdversary(Hero adv) => _class.SetAdversary(adv);

    public HeroAction Attack()
    {
        if (Difficulty == 1)
        {
            return AttackDifficulty1();
        }
        else
        {
            return AttackDifficulty2();
        }
    }
    public HeroAction AttackDifficulty2()
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
    
    public HeroAction AttackDifficulty1()
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

    public void Reset()
    {
        Class.CurrentHp = MaxHealth;
        Class.IsDefending = false;
        Class.IsSpecialActive = false;
        Class.Damage = Class.StartDamage;
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
                if (rdm <= 6)
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
                int rdm = Helper.Rand.Next() % 10;
                if (rdm < 5)
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
        if (_class.Adversary?.CurrentHp <=2)
        {
            int rdm = Helper.Rand.Next() % 10;
            if (rdm < 2)
            {
                action = _class.Defend;
                order = 4;
            }
            else
            {
                action = () => { _class.DoDamage(_class.Damage); };
                order = 5;
            }
            
            
        }else if (_class.CurrentHp >= _class.Adversary?.Damage)
        {
            int rdm = Helper.Rand.Next() % 10;
            if (rdm < 3)
            {
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
        
        if (_class.Adversary?.CurrentHp <= 2)
        {
            int rdm = Helper.Rand.Next() % 10;
            if (rdm < 3)
            {
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
        }else if (_class.CurrentHp > (3 + _class.Adversary?.StartDamage - 1))
        {
            int rdm = Helper.Rand.Next() % 10;
            if (rdm < 2)
            {
                rdm = Helper.Rand.Next() % 10;
                if(rdm > 5)
                {
                    action = _class.Special;
                    order = _class.SpecialOrder;
                }
                else
                {
                    action = _class.Defend;
                    order = 4;
                }
            }
            else
            {
                action = () => { 
                    _class.DoDamage(_class.Damage);
                };
                order = 5;
            }
            
        }
        else
        {
            int rdm = Helper.Rand.Next() % 10;
            if (rdm < 3)
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
            int rdm = Helper.Rand.Next() % 10;
            if (rdm < 7)
            {
                action = _class.Special;
                order = _class.SpecialOrder;
            }
            else
            {
                action = _class.Defend;
                order = 4;
            }
            
        }else if (_class.CurrentHp > 2)
        {
            int rdm = Helper.Rand.Next() % 10;
            if (rdm < 6)
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
            }else if (rdm < 5)
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