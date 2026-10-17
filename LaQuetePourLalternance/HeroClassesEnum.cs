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
    
    public int SpecialOrder { get; protected set; }
    public HeroClassesEnum Classe;

    public Ia? IA;
    public Hero? Adversary;
    
    public int CurrentHp;
    public int MaxHp;
    public int StartDamage;
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
            if (isSpecial) { dmgTaken = 1; }
            else { dmgTaken =  0; }
        }
        else { dmgTaken = damage; }

        if (IA != null) IA.DamageTaken = dmgTaken;

        CurrentHp -= dmgTaken;
        if (CurrentHp < 0) { CurrentHp = 0; }
    }

    public virtual void DoDamage(int damage)
    {
        if (IA == null || !IA.Debug)
        {
            Console.WriteLine(Name + " fait " + damage + " dégâts à son adversaire.");
            if (Adversary?.IsDefending ?? false) Console.WriteLine("Mais l'adversaire ce défend et ne prend pas de dégâts.");
        }
        
        Adversary?.TakeDamage(damage, false);
    }

    public virtual void Special()
    {
        // normal si vide, c'est changé + tard
    }

    public void Defend()
    {
        if (IA == null || !IA.Debug)
        {
            Console.WriteLine(Name + " se défend!");
        }

        IsDefending = true;
    }
    
    public virtual void SetAdversary(Hero adv) { Adversary = adv; }
}

public class Healer : Hero
{
    // Variables de base pour le Healer
    public Healer(Ia? ia = null) : base(ia)
    {
        Classe = HeroClassesEnum.Healer;
        StartDamage = 1;
        Damage = 1;
        MaxHp = 4;
        CurrentHp = MaxHp;
        Name = (IA != null ? "Dark " : "") + "Yithash";
        SpecialOrder = 6;
        attackDesc = "Bitch slap.";
        deffendDesc = "Il prend sa pause matcha, ne le dérangez pas";
        specialDesc = "Miku miku beam (nom très à côté de la plaque).";
    }
    
    public override void Special()
    {
        base.Special();
        if (IA == null || !IA.Debug)
        {
            Console.WriteLine($"{Name} se régénère! Il gagne 2 PV !");
        }

        CurrentHp += 2;
        if (CurrentHp > MaxHp) { CurrentHp = MaxHp; }
    }
}

public class Tank : Hero
{
    // Variables de base pour le Tank
    public Tank(Ia? ia = null) : base(ia)
    {
        Classe = HeroClassesEnum.Tank;
        StartDamage = 1;
        Damage = 1;
        MaxHp = 5;
        SpecialOrder = 2;
        CurrentHp = MaxHp;
        Name =  (IA != null ? "Dark " : "")  +"Azdelux";
        attackDesc = "*Donne un coup avec son gourdin* TIEN";
        deffendDesc = "*Se bave dessus en se protegeant avec ses bras* gnehhhhh";
        specialDesc = "*S'acharne en donnant un coup le blessant légèrement* YAAAAAAAAAHHHHH";
    }
    public override void Special()
    {
        if (IA == null || !IA.Debug)
            Console.WriteLine($"{Name} lance une attaque imparable! Il pert 1PV mais fait 2 dégâts.");
        base.Special();
        TakeDamage(1, false);
        Adversary?.TakeDamage(2, true);
    }
}

public class Damager : Hero
{
    // Variables de base pour le Damager
    public Damager(Ia? ia = null) : base(ia)
    {
        Classe = HeroClassesEnum.Damager;
        StartDamage = 2;
        Damage = 2;
        MaxHp = 3;
        SpecialOrder = 1;
        CurrentHp = MaxHp;
        Name =  (IA != null ? "Dark " : "") + "Folet";
        attackDesc = "Donne un coup d'épée, c'est plutôt OK.";
        deffendDesc = "Il se défend, euh, c'est pas mal.";
        specialDesc = "Réfléchie l'attaque ennemie mais prend quand même les dégâts.";
    }
    
    public override void Special()
    {
        base.Special();
        if (IA == null || !IA.Debug)
            Console.WriteLine($"{Name} renverra les prochains coups! (Mais les recevra quand même parce-qu'il est pas super doué).");
        IsSpecialActive = true;
    }
    public override void TakeDamage(int damage, bool isSpecial)
    {
        /*if(IsSpecialActive)
            base.TakeDamage(1, isSpecial);
        else*/ // Amelioration pour le gameplay, mais ça aide pas tant que ça

        /*if (IsSpecialActive)
        {
            int rdm = Helper.Rand.Next() % 10;
            if(rdm < 5)
                base.TakeDamage(damage, isSpecial);
        }
        else*/
        {
            base.TakeDamage(damage, isSpecial);
        }
        
        
        if (IA == null || !IA.Debug) 
            Console.WriteLine($"{Name} renvoie les dégâts. Mais ce fait quand même bléssé.");
        if (IsSpecialActive) { DoDamage(damage); }
    }

    public override void SetAdversary(Hero adv)
    {
        base.SetAdversary(adv);
        if (adv.Classe == HeroClassesEnum.Tank ||adv.Classe == HeroClassesEnum.Custom) //Amelioration faite pour le fair play, mais c'est pas dans les consignes
        {
            MaxHp = 4;
            CurrentHp = 4;
        }
    }
}

public class Custom : Hero
{
    // Variables de base pour la classe custom
    public Custom(Ia? ia = null) : base(ia)
    {
        Classe = HeroClassesEnum.Custom;
        Damage = 0;
        StartDamage = 0;
        MaxHp = 5;
        SpecialOrder = 0;
        CurrentHp = MaxHp;
        Name = (IA != null ? "Dark " : "") + "Bolimart";
        attackDesc = "Il attaque là ou sa fait mal, dans le porte monnaie. Son attaque retombe à 0.";
        deffendDesc = "Il se défend, il protège surtout son contrat.";
        specialDesc = "Il se tue à la tache pour obtenir son salaire. Il gagne un point d'attaque et est vulnérable pour un tour.";
    }
    
    public override void Special()
    {
        if (IA == null || !IA.Debug)
            Console.WriteLine($"{Name} augmente ses dégâts pour le tour mais est vulnérable!");
        IsSpecialActive = true;
        Damage++;
    }

    public override void TakeDamage(int damage, bool isSpecial)
    {
        if (IsSpecialActive)
        {
            if (IA == null || !IA.Debug)
                Console.WriteLine($"{Name} était vulnérable ce tour çi: il prend un dégât en plus");
            
            base.TakeDamage(damage + 1, false);
        }
        else base.TakeDamage(damage, isSpecial);
    }

    public override void DoDamage(int damage)
    {
        base.DoDamage(damage);
        Damage = 0;
    }
}

public struct HeroAction(Action _action, int _order)
{
    public Action action = _action;
    public int order = _order;
}