using System.Diagnostics;
using LaQuetePourLalternance.Graphics2dApi;
using OpenTK.Mathematics;

namespace LaQuetePourLalternance;

public enum HeroClassesEnum
{
    Healer, Tank, Damager, Custom
}

public abstract class Hero : Square
{
    protected Hero(Ia? ia = null) : base(new SquareData(new Vector3(0,0,0), new(1,1,1), 0, null, new Shader("shader.vert", "shader.frag")))
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

    public List<Texture> idleTextures = new List<Texture>();
    public Texture hitTexture;
    public List<Texture> attackTextures = new List<Texture>();
    public List<Texture> specialTextures = new List<Texture>();
    public Texture deathTexture;
    public Texture defendTexture;

    public bool isPlayingAnimation;
    
    public virtual void StartTurn()
    {
        PlayAnimation(idleTextures, 1f);
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
                ChangeTexture(hitTexture, true);
                dmgTaken = 1; 
            }
            else
            {
                dmgTaken =  0;
            }
        }
        else
        {
            ChangeTexture(hitTexture, true);
            dmgTaken = damage;
        }

        if (IA != null) IA.DamageTaken = dmgTaken;

        CurrentHp -= dmgTaken;

        if (CurrentHp < 0)
        {
            CurrentHp = 0;
        }
    }

    public virtual void DoDamage(int damage)
    {
        Console.WriteLine(Name + " fait " + damage + " dégâts à son adversaire.");
        if (Adversary?.IsDefending ?? false) Console.WriteLine("Mais l'adversaire ce défend et ne prend pas de dégâts.");
        Adversary?.TakeDamage(damage, false);
    }


    public virtual void Special()
    {
        
    }

    public void Defend()
    {
        Console.WriteLine(Name + " se défend!");
        IsDefending = true;
    }
    
    public void SetAdversary(Hero adv)
    {
        Adversary ??= adv;
    }

    public void PlayAnimation(List<Texture> anim, float cdBetweenFrames)
    {
        while (isPlayingAnimation)
        {
            foreach (var sprite in anim)
            {
                ChangeTexture(sprite);
                // wait cdBetweenFrames
            }
        }
    }
    
    public void ChangeTexture(Texture texture, bool isShaking = false)
    {
        // faire le tremblement
        Data.Texture = texture;
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
        Name = "Yithash";
        attackDesc = "Bitch slap.";
        deffendDesc = "Il prend sa pause matcha, ne le dérangez pas";
        specialDesc = "Miku miku beam (nom très à côté de la plaque).";

        idleTextures = new List<Texture> { new Texture("Nathan/idle1.png"), new Texture("Nathan/idle2.png"), new Texture("Nathan/idle3.png") };
        hitTexture = new Texture("Nathan/hit1.png");
        attackTextures = new List<Texture> { new Texture("Nathan/attack1.png"), new Texture("Nathan/attack2.png")};
        specialTextures = new List<Texture> { new Texture("Nathan/special.png")};
        deathTexture = new Texture("Nathan/death.png");
        defendTexture = new Texture("Nathan/defend.png");
    }

    public override void Special()
    {
        base.Special();
        Console.WriteLine("Yithash se régénère! Il gagne 2 PV !");
        CurrentHp += 2;
        if (CurrentHp > MaxHp)
            CurrentHp = MaxHp;
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
        Name = "Azdelux";
        attackDesc = "*Donne un coup avec son gourdin* TIEN";
        deffendDesc = "*Se bave dessus en se protegeant avec ses bras* gnehhhhh";
        specialDesc = "*S'acharne en donnant un coup le blessant légèrement* YAAAAAAAAAHHHHH";
        
        idleTextures = new List<Texture> { new Texture("Younes/idle1.png"), new Texture("Younes/idle2.png")};
        hitTexture = new Texture("Younes/hit1.png");
        attackTextures = new List<Texture> { new Texture("Younes/attack1.png"), new Texture("Younes/attack2.png")};
        specialTextures = new List<Texture> { new Texture("Younes/special1.png"), new Texture("Younes/special2.png")};
        deathTexture = new Texture("Younes/death.png");
        defendTexture = new Texture("Younes/defend.png");
    }
    public override void Special()
    {
        Console.WriteLine("Azdelux lance une attaque imparable! Il pert 1PV mais fait 2 dégâts.");
        base.Special();
        TakeDamage(1, false);
        Adversary?.TakeDamage(2, true);
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
        Name = "Folet";
        attackDesc = "Donne un coup d'épée, c'est plutôt OK.";
        deffendDesc = "Il se défend, euh, c'est pas mal.";
        specialDesc = "Réfléchie l'attaque ennemie mais prend quand même les dégâts.";
        
        
        idleTextures = new List<Texture> { new Texture("Vincent/idle1.png"), new Texture("Vincent/idle2.png")};
        hitTexture = new Texture("Vincent/hit1.png");
        attackTextures = new List<Texture> { new Texture("Vincent/attack1.png"), new Texture("Vincent/attack2.png")};
        specialTextures = new List<Texture> { new Texture("Vincent/special1.png"), new Texture("Vincent/special2.png")};
        deathTexture = new Texture("Vincent/death.png");
        defendTexture = new Texture("Vincent/defend1.png");
    }
    
    public override void Special()
    {
        base.Special();
        Console.WriteLine("Folet renverra les prochains coups! (Mais les recevra quand même parce-qu'il est pas super doué).");
        IsSpecialActive = true;
    }

    public override void TakeDamage(int damage, bool isSpecial)
    {
        base.TakeDamage(damage, isSpecial);
        Console.WriteLine("Folet renvoie les dégâts. Mais ce fait quand même bléssé.");
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
        Name = "Bolimart";
        attackDesc = "Il attaque là ou sa fait mal, dans le porte monnaie. Son attaque retombe à 0.";
        deffendDesc = "Il se défend, il protège surtout son contrat.";
        specialDesc = "Il se tue à la tache pour obtenir son salaire. Il gagne un point d'attaque et est vulnérable pour un tour.";
        
        
        idleTextures = new List<Texture> { new Texture("Julien/idle1.png"), new Texture("Julien/idle2.png")};
        hitTexture = new Texture("Julien/hit1.png");
        attackTextures = new List<Texture> { new Texture("Julien/attack1.png"), new Texture("Julien/attack2.png")};
        specialTextures = new List<Texture> { new Texture("Julien/special1.png")};
        deathTexture = new Texture("Julien/dead1.png");
        defendTexture = new Texture("Julien/defend1.png");
    }
    
    public override void Special()
    {
        Console.WriteLine("Bolimart augmente ses dégâts pour le tour mais est vulnérable!");
        IsSpecialActive = true;
        Damage++;
    }

    public override void TakeDamage(int damage, bool isSpecial)
    {
        if (IsSpecialActive)
        {
            Console.WriteLine("Bolimart était vulnérable ce tour çi: il prend un dégât en plus");
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