using System.Diagnostics;

namespace LaQuetePourLalternance;

// Permet de savoir facillement de qu'elle classe est un Hero, car c'est pas possible sans variable permettant de le savoir.
public enum HeroClassesEnum
{
    Healer, Tank, Damager, Custom
}

/// <summary>
/// Classe abstraite permettant de crée d'autre classe de Hero, un Hero est une Classe Jouable
/// </summary>
public abstract class Hero 
{
    protected Hero(Ia? ia = null) // Constructeur avec un paramètre IA optionel
    {
        IA = ia;
    }
    
    public int SpecialOrder { get; protected set; } // Permet de davoir lors d'un tour, quand est ce que l'attaque spéciale est sensé être fait 
    public readonly int AttackOrder = 5;
    public readonly int DefenseOrder = 4;
    public HeroClassesEnum Classe; // Permet de savoir la classe d'une Hero

    public Ia? IA; // Permet de savoir si le Hero appartient a une IA et de reférencer l'IA 
    public Hero Adversary; 
    
    public int CurrentHp;
    public int MaxHp;
    public int Damage;

    protected bool IsDefending; // Permet de savoir si 
    protected bool IsSpecialActive;

    public string Name = "";
    public string AttackDesc = "";
    public string DeffendDesc = "";
    public string SpecialDesc = "";
    
    /// <summary>
    /// Reinitialise les variables qui doivent être reinitialiser chaque tour
    /// </summary>
    public void StartTurn()
    {
        IsSpecialActive = false;
        IsDefending = false;
    }
    
    /// <summary>
    /// Methode pour prendre des dégats, virtual pour pouvoir rajouter des fonctionnalité dans d'autre classes
    /// </summary>
    /// <param name="damage">Le nombre de dégats pris</param>
    /// <param name="isSpecial">Si l'attaque viens du spécial du Tank</param>
    public virtual void TakeDamage(int damage, bool isSpecial)
    {
        int dmgTaken = 0;
        if (IsDefending) // Si il est entrain de ce défendre, ne prend pas de dégat, ou 1 si c'est l'attque spéciale du Tank
        {
            if (isSpecial) { dmgTaken = 1; }
            else { dmgTaken =  0; }
        }
        else { dmgTaken = damage; }

        if (IA != null) IA.DamageTaken = dmgTaken; // Permet a l'IA de savoir combien de dégat elle a subi le tour précédent

        CurrentHp -= dmgTaken; // Retire au HP le nombre de dégats pris
        if (CurrentHp < 0) { CurrentHp = 0; } // met les HPs a 0 si on est en dessous.
    }

    /// <summary>
    /// Appèle la fonction TakeDamage de l'adversaire, virtual pour pouvoir rajouter des fonctionnalité dans d'autre classes
    /// </summary>
    /// <param name="damage"> nombre de dégat fait</param>
    public virtual void DoDamage(int damage) 
    {
        Console.WriteLine(Name + " fait " + damage + " dégâts à son adversaire.");
        if (Adversary?.IsDefending ?? false) Console.WriteLine("Mais l'adversaire ce défend et ne prend pas de dégâts.");
        Adversary?.TakeDamage(damage, false);
    }

    public virtual void Special()
    {
        // normal si vide, c'est changé + tard
    }

    public void Defend()
    {
        Console.WriteLine(Name + " se défend!");
        IsDefending = true;
    }
    
    /// <summary>
    /// Change l'adversaire 
    /// </summary>
    /// <param name="adv"> l'enemie</param>
    public void SetAdversary(Hero adv) { Adversary ??= adv; }
}

/// <summary>
/// Healer, son spécial lui permet de heal 2 HP
/// </summary>
public class Healer : Hero
{
    // Variables de base pour le Healer
    public Healer(Ia? ia = null) : base(ia)
    {
        Classe = HeroClassesEnum.Healer;
        Damage = 1;
        MaxHp = 4;
        CurrentHp = MaxHp;
        Name = (IA != null ? "Dark " : "") + "Yithash";
        SpecialOrder = 6;
        AttackDesc = "Bitch slap.";
        DeffendDesc = "Il prend sa pause matcha, ne le dérangez pas";
        SpecialDesc = "Miku miku beam (nom très à côté de la plaque).";
    }
    
    public override void Special()
    {
        base.Special();
        Console.WriteLine($"{Name} se régénère! Il gagne 2 PV !");
        CurrentHp += 2;
        if (CurrentHp > MaxHp) { CurrentHp = MaxHp; }
    }
}

/// <summary>
/// Tank, son spécial est une attaque qui cause 2PV, et traverse la defense
/// </summary>
public class Tank : Hero
{
    // Variables de base pour le Tank
    public Tank(Ia? ia = null) : base(ia)
    {
        Classe = HeroClassesEnum.Tank;
        Damage = 1;
        MaxHp = 5;
        SpecialOrder = 2;
        CurrentHp = MaxHp;
        Name =  (IA != null ? "Dark " : "")  +"Azdelux";
        AttackDesc = "*Donne un coup avec son gourdin* TIEN";
        DeffendDesc = "*Se bave dessus en se protegeant avec ses bras* gnehhhhh";
        SpecialDesc = "*S'acharne en donnant un coup le blessant légèrement* YAAAAAAAAAHHHHH";
    }
    public override void Special()
    {
        Console.WriteLine($"{Name} lance une attaque imparable! Il pert 1PV mais fait 2 dégâts.");
        base.Special();
        TakeDamage(1, false);
        Adversary?.TakeDamage(2, true);
    }
}

/// <summary>
/// Damager, son spécial fait qu'il retourne les dégat pris
/// </summary>
public class Damager : Hero
{
    // Variables de base pour le Damager
    public Damager(Ia? ia = null) : base(ia)
    {
        Classe = HeroClassesEnum.Damager;
        Damage = 2;
        MaxHp = 3;
        SpecialOrder = 1;
        CurrentHp = MaxHp;
        Name =  (IA != null ? "Dark " : "") + "Folet";
        AttackDesc = "Donne un coup d'épée, c'est plutôt OK.";
        DeffendDesc = "Il se défend, euh, c'est pas mal.";
        SpecialDesc = "Réfléchie l'attaque ennemie mais prend quand même les dégâts.";
    }
    
    public override void Special()
    {
        base.Special();
        Console.WriteLine($"{Name} renverra les prochains coups! (Mais les recevra quand même parce-qu'il est pas super doué).");
        IsSpecialActive = true;
    }
    public override void TakeDamage(int damage, bool isSpecial)
    {
        base.TakeDamage(damage, isSpecial);

        if (IsSpecialActive)
        {
            Console.WriteLine($"{Name} renvoie les dégâts. Mais ce fait quand même bléssé.");
            DoDamage(damage);
        }
    }
}

/// <summary>
/// Classe Custom, commence avec 0 dégat, permet d'augmenter son nombre de dégat avec son spécial, mais est plus vulnérable. Retour a 0 si il ce fait toucher.
/// </summary>
public class Custom : Hero
{
    // Variables de base pour la classe custom
    public Custom(Ia? ia = null) : base(ia)
    {
        Classe = HeroClassesEnum.Custom;
        Damage = 0;
        MaxHp = 5;
        SpecialOrder = 0;
        CurrentHp = MaxHp;
        Name = (IA != null ? "Dark " : "") + "Bolimart";
        AttackDesc = "Il attaque là ou sa fait mal, dans le porte monnaie. Son attaque retombe à 0.";
        DeffendDesc = "Il se défend, il protège surtout son contrat.";
        SpecialDesc = "Il se tue à la tache pour obtenir son salaire. Il gagne un point d'attaque et est vulnérable pour un tour.";
    }
    
    public override void Special()
    {
        Console.WriteLine($"{Name} augmente ses dégâts pour le tour mais est vulnérable!");
        IsSpecialActive = true;
        Damage++;
    }

    public override void TakeDamage(int damage, bool isSpecial)
    {
        if (IsSpecialActive)
        {
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

/// <summary>
/// Struct stockant une action d'un Hero ainsi que dans quel ordre elle ce passe
/// </summary>
/// <param name="_action"></param>
/// <param name="_order"></param>
public struct HeroAction(Action _action, int _order)
{
    public Action action = _action;
    public int order = _order;
}