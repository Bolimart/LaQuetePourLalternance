using System.Diagnostics;
using OpenTK.Mathematics;
using LaQuetePourLalternance.Graphics2dApi;
using OpenTK.Graphics.ES11;
using OpenTK.Windowing.Common;

namespace LaQuetePourLalternance.Graphics2dApi;

public class SquareData
{
    public Vector3 Position;
    public Vector3 Scale;
    public float Rotation;
    public Texture Texture;
    public Shader Shader;
    
    public static float[] Verticies = {
        // Triangle 1
        0.5f,  0.5f, 0.0f, 1.0f, 1.0f,
        0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
        -0.5f,  0.5f, 0.0f, 0.0f, 1.0f,
        // Triangle 2
        0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
        -0.5f,  0.5f, 0.0f, 0.0f, 1.0f,
    };

    public SquareData(Vector3 position, Vector3 scale, float rotation, Texture? texture = null, Shader? shader = null)
    {
        Position = position;
        Scale = scale;
        Rotation = MathHelper.DegreesToRadians(rotation);
        Texture = texture;
        Shader = shader;
        shader.AddReference();
        texture.AddReference();
    }

    public virtual void Use()
    {
        Shader.Use();
        Texture.Use();
    }
    
    public void ScaleAccordingToTex(float scale = 1)
    {
        Scale.X = Texture.Dimension.X * scale;
        Scale.Y = Texture.Dimension.Y * scale;
    }
}

public class Square : GameObject, IDrawable
{
    public SquareData Data { get; set; }
    public Stopwatch Time = new();

    public float DepthZ
    {
        get => Data.Position.Z;
        set => Data.Position.Z = value;
    }
    
    public Square(SquareData data, float depthZ = 0f)
    {
        Data = data;
        DepthZ = depthZ;
    }
    
    /// <summary>
    /// Called on every logic update.
    /// </summary>
    public override void Update(FrameEventArgs args)
    {
        
    }
    
    /// <summary>
    /// Called on every render call.
    /// </summary>
    public override void Use(FrameEventArgs args)
    {
        Data.Use();
        
        Matrix4 model = GetModelMatrix();
            
        // adding a projection matrix allow for a better world placement not in the clip space
        Data.Shader.SetMatrix4("projection", GameEnvironement.Window.Projection); // Now with proper projection !
        Data.Shader.SetMatrix4("model", model);
    }

    public virtual void Unload()
    {
        Data.Shader.Dispose();
        Data.Texture.Dispose();
    }

    public void Flip() => Data.Scale.X *= 1;
    
    // Scale -> Rotation -> Translation
    public Matrix4 GetModelMatrix() => Matrix4.CreateScale(Data.Scale)
                                       * Matrix4.CreateFromAxisAngle(Vector3.UnitZ, Data.Rotation)
                                       * Matrix4.CreateTranslation(Data.Position);
    
    // Unload the texture and the shader when a square is'nt used anymore.
    ~Square() => Unload();
}