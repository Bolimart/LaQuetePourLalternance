using System.Diagnostics;
using OpenTK.Mathematics;
using Photon;

namespace Photon;

public struct SquareData
{
    public Vector3 Position;
    public Vector3 Scale;
    public float Rotation;
    public Texture Texture;
    public Shader Shader;

    public SquareData(Vector3 position, Vector3 scale, float rotation, Texture texture, Shader shader)
    {
        Position = position;
        Scale = scale;
        Rotation = rotation;
        Texture = texture;
        Shader = shader;
    }
    
    public void Use()
    {
        Texture.Use();
        Shader.Use();
    }
    
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
}

public class Square
{
    public SquareData Data;
    public Stopwatch Time = new();
    
    public Square(SquareData data)
    {
        Data = data;
    }
    
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
        Data.Use();

        Data.Rotation += 1f * dtime;
    }
    
    // Scale -> Rotation -> Translation
    public Matrix4 GetModelMatrix() => Matrix4.CreateScale(Data.Scale)
                                       * Matrix4.CreateFromAxisAngle(Vector3.UnitZ, Data.Rotation)
                                       * Matrix4.CreateTranslation(Data.Position);
}