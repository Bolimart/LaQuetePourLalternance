using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace LaQuetePourLalternance.Graphics2dApi;

public abstract class GameObject
{
    /// <summary>
    /// Called on every logic update.
    /// </summary>
    public virtual void Update(FrameEventArgs args)
    {
        
    }
    
    /// <summary>
    /// Called on every render call.
    /// </summary>
    public virtual void Use(FrameEventArgs args)
    {
        
    }
}

interface IDrawable
{
    public SquareData Data { get; set; }
    public float DepthZ { get; set; }

    public void Unload();
}