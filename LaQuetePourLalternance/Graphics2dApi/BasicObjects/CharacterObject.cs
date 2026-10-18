using System.Runtime.InteropServices;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LaQuetePourLalternance.Graphics2dApi.BasicObjects;

public class CharacterObject : Square
{
    public char Char = 'a';
    public Vector3 Color = Vector3.One;

    public CharacterObject(char character, Vector3 position, Vector3 scale, float rotation, Vector3? color = null) : base(
        new SquareData(position, scale, rotation, LetterData.Texture, LetterData.Shader))
    {
        Char = character;
        Color = color ?? Vector3.One;
        Data.Scale = new Vector3(scale.X * 12,scale.Y * 21, 1);
    }

    public override void Use(FrameEventArgs args)
    {
        Data.Shader.SetFloat("index", Char - 32);
        Data.Shader.SetVector3("color", Color);
        base.Use(args);
    }
}


public class StringObject : GameObject
{
    public Vector3 Position = Vector3.Zero;
    public float Rotation = 0f;
    public Vector3 Scale = Vector3.One;

    private string _text = "";

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            CharacterObjects = new CharacterObject[Text.Length];
            float advance = 12f * Scale.X; // largeur d'un glyphe à l'échelle demandée
            var dir = new Vector3(MathF.Cos(Rotation), MathF.Sin(Rotation), 0);
            for (int i = 0; i < Text.Length; i++)
            {
                var pos = Position + dir * (i * advance);
                CharacterObjects[i] = new CharacterObject(Text[i], pos, Scale, MathHelper.RadiansToDegrees(Rotation));
            }

            DrawnIndex = 0;
        }
    }

    public int DrawnIndex = 0;
    public CharacterObject[] CharacterObjects;

    private float _timeToNextChar = 0f;
    private float _timeBetweenChar = 0.15f;

    public StringObject(string text, Vector3 position, Vector3? scale = null, float rotation = 0f, float timeBetweenChar = 0.15f)
    {
        _timeBetweenChar = timeBetweenChar;
        Position = position;
        Rotation = MathHelper.DegreesToRadians(rotation);
        Scale = scale ?? new(1, 1, 1);
        Text = text;
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


public class InputObject : StringObject
{
    public InputObject(Action<string> inputGet, Vector3 position, Vector3 scale, float rotation) : base("", position, scale, rotation, 0f)
    {
        OnInputEnd += inputGet;
    }

    public override void Update(FrameEventArgs args)
    {
        var kbState = GameEnvironement.Window.KeyboardState;
        bool isMaj = false;

        if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyPressed(Keys.LeftShift))
            isMaj = true;
        
        foreach (Keys key in Enum.GetValues(typeof(Keys)))
        {
            if (key == Keys.Unknown) continue;
            if (kbState.IsKeyPressed(key))
            {
                // Special case for enter and backspace
                if ((key == Keys.Backspace || key == Keys.Delete) && Text.Length > 0) Text = Text[..^1];

                if (key == Keys.Space) Text += ' ';

                else if (key == Keys.Enter) OnInputEnd.Invoke(Text);

                else if ((int)key >= 32 && (int)key < 123)
                {
                    // If is in maj
                    if (!isMaj)
                    {
                        if ((int)key >= 65 && (int)key < 91) Text += (char)(key + 32);
                    }
                    else Text += (char)key;
                }
            }
        }
        base.Update(args);
    }

    public event Action<string> OnInputEnd;
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