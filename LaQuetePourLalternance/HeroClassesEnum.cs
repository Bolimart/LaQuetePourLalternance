namespace LaQuetePourLalternance;

public enum HeroClassesEnum
{
    Healer, Tank, Damager, Custom
}

public abstract class Hero
{
    public HeroClassesEnum classe;
    
    public int CurrentHp;
    public int MaxHp;
    public int Damage;
    
    
    public void TakeDamage()
    {
        
    }

    public virtual void Special()
    {
        
    }
}

public class Healer : Hero
{
    
    
    public override void Special()
    {
        
    }
}

public class Tank : Hero
{
    
}

public class Damager : Hero
{
    
}