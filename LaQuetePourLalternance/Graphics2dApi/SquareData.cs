using System.Diagnostics;
using OpenTK.Mathematics;
using LaQuetePourLalternance.Graphics2dApi;

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

    public SquareData(Vector3 position, Vector3 scale, float rotation, Texture texture, Shader shader)
    {
        Position = position;
        Scale = scale;
        Rotation = rotation;
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
        Data.ScaleAccordingToTex(0.3f);
    }
    
    /// <summary>
    /// Called on every logic update.
    /// </summary>
    public override void Update(float dtime)
    {
        
    }
    
    /// <summary>
    /// Called on every render call.
    /// </summary>
    public override void Use(float dtime)
    {
        base.Use(dtime);
        Data.Use();
        
        Matrix4 model = GetModelMatrix();
            
        // adding a projection matrix allow for a better world placement not in the clip space
        Data.Shader.SetMatrix4("projection", GameEnvironement.Window.Projection); // Now with proper projection !
        Data.Shader.SetMatrix4("model", model);
        
        Data.Rotation += 1f * dtime;
    }
    
    public void Unload()
    {
        Data.Shader.Dispose();
        Data.Texture.Dispose();
    }
    
    // Scale -> Rotation -> Translation
    public Matrix4 GetModelMatrix() => Matrix4.CreateScale(Data.Scale)
                                       * Matrix4.CreateFromAxisAngle(Vector3.UnitZ, Data.Rotation)
                                       * Matrix4.CreateTranslation(Data.Position);
    
    // Unload the texture and the shader when a square is'nt used anymore.
    ~Square() => Unload();
}