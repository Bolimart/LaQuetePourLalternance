using System.Diagnostics;

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
    public string attackDesc = "";
    public string deffendDesc = "";
    public string specialDesc = "";
    
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
        attackDesc = "Bitch slap.";
        deffendDesc = "Il prend sa pause matcha, ne le dérangez pas";
        specialDesc = "Miku miku beam (nom très à côté de la plaque).";
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
        attackDesc = "*Donne un coup avec son glaive* Prend ça !";
        deffendDesc = "*Se protège* Je ne faiblirais pas !";
        specialDesc = "*S'acharne en donnant un coup le blessant légèrement* YAAAAAAAAAHHHHH";
    }
    public override void Special()
    {
        IsSpecialActive = true;
    }
    public override void DoDamage(int damage)
    {
        if (IsSpecialActive)
        {
            // TODO faire le calcul de dmg et rajouter le bool isSpecial
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
        attackDesc = "Donne un coup d'épée, c'est plutôt OK.";
        deffendDesc = "Il se défend, euh, c'est pas mal.";
        specialDesc = "Réfléchie l'attaque ennemie mais prend quand même les dégâts.";
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

public class Custom : Hero
{
    public Custom(Ia? ia = null) : base(ia)
    {
        Classe = HeroClassesEnum.Custom;
        Damage = 0;
        MaxHp = 5;
        CurrentHp = MaxHp;
        Name = "Julien";
        attackDesc = $"Il attaque là ou sa fait mal, dans le porte monnaie. Son attaque retombe à 0.";
        deffendDesc = "Il se défend, il protège surtout son contrat.";
        specialDesc = "Il se tue à la tache pour obtenir son salaire. Il gagne un point d'attaque et est vulnérable pour un tour.";
    }
    
    public override void Special()
    {
        IsSpecialActive = true;
        Damage++;
    }

    public override void TakeDamage(int damage, bool isSpecial)
    {
        if (IsSpecialActive) base.TakeDamage(damage + 1, false);
        else base.TakeDamage(damage, isSpecial);
    }

    public override void DoDamage(int damage)
    {
        base.DoDamage(damage);
        Damage = 0;
    }
}