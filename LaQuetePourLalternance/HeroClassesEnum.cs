namespace LaQuetePourLalternance;

public enum HeroClassesEnum
{
    Healer, Tank, Damager, Custom
}

public abstract class Hero
{
    public HeroClassesEnum Classe;

    public IAEnnemy IA;
    
    public int CurrentHp;
    public int MaxHp;
    public int Damage;

    public bool IsDefending;
    
    public bool IsSpecialActive;

    public string Name;

    public virtual void StartOfTurn()
    {
        IsSpecialActive = false;
        IsDefending = false;
    }
    
    public virtual void TakeDamage(int damage, bool isSpecial)
    {
        if (IsDefending)
        {
            if (isSpecial == true)
            {
                CurrentHp -= 1;
            }
            else
            {
                return;
            }
        }
        else
        {
            CurrentHp -= damage;
        }
        
        if (CurrentHp < 0)
        {
            CurrentHp = 0;
            Death();
        }
    }

    public virtual void DoDamage(int damage)
    {
        // à faire aussi
    }
    
    
    public virtual void Special()
    {
        // normal que ça soit vide
    }

    public void Defend()
    {
        IsDefending = true;
    }
    
    public void Death()
    {
        // à voir avec le game manager
    }
    
}


public class Healer : Hero
{
    public Healer()
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
    public Tank()
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
    public Damager()
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