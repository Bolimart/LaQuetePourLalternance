namespace LaQuetePourLalternance;

public enum HeroClassesEnum
{
    Healer, Tank, Damager, Custom
}

public abstract class Hero
{
    protected Hero(Ia? ia = null)
    {
        IA = ia;
    }
    
    public HeroClassesEnum Classe;

    public Ia? IA;

    public Hero? Adversary;
    
    public int CurrentHp;
    public int MaxHp;
    public int Damage;

    public bool IsDefending;
    
    public bool IsSpecialActive;

    public string Name = "";
    
    public virtual void StartTurn()
    {
        IsSpecialActive = false;
        IsDefending = false;
    }
    
    public virtual void TakeDamage(int damage, bool isSpecial)
    {
        int dmgTaken = 0;
        if (IsDefending)
        {
            if (isSpecial)
            {
                dmgTaken = 1;
            }
            else
            {
                dmgTaken =  0;
            }
        }
        else
        {
            dmgTaken = damage;
        }

        if (IA != null) IA.DamageTaken = dmgTaken;

        CurrentHp -= dmgTaken;

        if (CurrentHp < 0)
        {
            CurrentHp = 0;
            Death();
        }
    }

    public virtual void DoDamage(int damage)
    {
        Adversary?.TakeDamage(damage, false);
    }
    
    
    public virtual void Special()
    {}

    public void Defend()
    {
        IsDefending = true;
    }
    
    public void Death()
    {
        // à voir avec le game manager
    }

    public void SetAdversary(Hero adv)
    {
        Adversary ??= adv;
    }
    
}

public class Healer : Hero
{
    public Healer(Ia? ia = null) : base(ia)
    {
        Classe = HeroClassesEnum.Healer;
        Damage = 1;
        MaxHp = 4;
        CurrentHp = MaxHp;
        Name = "Nathan";
    }

    public override void Special()
    {
        CurrentHp += 2;
        if (CurrentHp > MaxHp)
        {
            CurrentHp = MaxHp;
        }
    }
    
}

public class Tank : Hero
{
    public Tank(Ia? ia = null) : base(ia)
    {
        Classe = HeroClassesEnum.Tank;
        Damage = 1;
        MaxHp = 5;
        CurrentHp = MaxHp;
        Name = "Younes";
    }
    public override void Special()
    {
        IsSpecialActive = true;
    }
    public override void DoDamage(int damage)
    {
        if (IsSpecialActive)
        {
            // faire le calcul de dmg et rajouter le bool isSpecial
        }
        else
        {
            base.DoDamage(damage);
        }
    }
}

public class Damager : Hero
{
    public Damager(Ia? ia = null) : base(ia)
    {
        Classe = HeroClassesEnum.Damager;
        Damage = 2;
        MaxHp = 3;
        CurrentHp = MaxHp;
        Name = "Vincent";
    }
    
    public override void Special()
    {
        IsSpecialActive = true;
    }

    public override void TakeDamage(int damage, bool isSpecial)
    {
        base.TakeDamage(damage, isSpecial);
        if (IsSpecialActive)
        {
            DoDamage(damage);
        }
    }
}