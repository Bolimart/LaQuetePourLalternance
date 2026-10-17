using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace LaQuetePourLalternance.Graphics2dApi.BasicObjects;

public class CharacterObject : Square
{
    public char Char = 'a';

    public CharacterObject(char character, Vector3 position, Vector3 scale, float rotation) : base(
        new SquareData(position, scale, rotation, LetterData.Texture, LetterData.Shader))
    {
        Char = character;
        Data.Scale = new Vector3(scale.X * 12,scale.Y * 21, 1);
    }

    public override void Use(FrameEventArgs args)
    {
        Data.Shader.SetFloat("index", Char - 32);
        base.Use(args);
    }
}


public class StringObject : GameObject
{
    public Vector3 Position = Vector3.Zero;
    public float Rotation = 0f;
    public Vector3 Scale = Vector3.One;

    public string Text = "";
    public int DrawnIndex = 0;
    public CharacterObject[] CharacterObjects;

    private float _timeToNextChar = 0f;
    private float _timeBetweenChar = 0.15f;

    public StringObject(string text, Vector3 position, Vector3 scale, float rotation, float timeBetweenChar = 0.15f)
    {
        Text = text;
        _timeBetweenChar = timeBetweenChar;
        Position = position;
        Rotation = rotation;
        Scale = new Vector3(12 * scale.X, 21 * scale.Y, 1);
        CharacterObjects = new CharacterObject[text.Length];
        float advance = 12f * scale.X; // largeur d'un glyphe à l'échelle demandée
        var dir = new Vector3(MathF.Cos(rotation), MathF.Sin(rotation), 0);
        for (int i = 0; i < text.Length; i++)
        {
            var pos = position + dir * (i * advance);
            CharacterObjects[i] = new CharacterObject(text[i], pos, scale, rotation);
        }
    }

    public override void Use(FrameEventArgs args)
    {
        base.Use(args);
        for (int i = 0; i <= DrawnIndex - 1; i++)
        {
            CharacterObjects[i].Use(args);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
    }

    public override void Update(FrameEventArgs args)
    {
        base.Update(args);

        _timeToNextChar += (float)args.Time;
        if (_timeToNextChar >= _timeBetweenChar && DrawnIndex <= Text.Length - 1)
        {
            DrawnIndex++;
            _timeToNextChar = 0f;
        }
        
        for (int i = 0; i <= DrawnIndex - 1; i++)
        {
            CharacterObjects[i].Update(args);
        }
    }
}

public static class LetterData
{
    public static Shader Shader = new Shader("character.vert", "character.frag");
    public static Texture Texture = new Texture("CharSprites.png");

    public static void Unload()
    {
        Shader.Dispose();
        Texture.Dispose();
    }
}