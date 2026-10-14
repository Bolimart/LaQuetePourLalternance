using OpenTK.Mathematics;

namespace LaQuetePourLalternance.Graphics2dApi;

public abstract class GameObject
{
    /// <summary>
    /// Called on every logic update.
    /// </summary>
    public virtual void Update(float dtime)
    {
        
    }
    
    /// <summary>
    /// Called on every render call.
    /// </summary>
    public virtual void Use(float dtime)
    {
        
    }
}

interface IDrawable
{
    public SquareData Data { get; set; }
    public float DepthZ { get; set; }

    public void Unload();
}