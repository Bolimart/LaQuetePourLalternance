namespace LaQuetePourLalternance;

public enum HeroClassesEnum
{
    Healer, Tank, Damager, Custom
}

public abstract class Hero
{
    public HeroClassesEnum classe;
    
    public void TakeDamage()
    {
        
    }

    public virtual void Special()
    {
        
    }
}

public class Healer : Hero
{
    private int Health = 5;
    public Healer(int _health = 5)
    {
        Health = _health;
    }
    public override void Special()
    {
        base.Special();
    }
}