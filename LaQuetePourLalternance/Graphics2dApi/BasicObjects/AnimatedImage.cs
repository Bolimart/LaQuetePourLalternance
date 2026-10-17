using System.Security.Cryptography;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace LaQuetePourLalternance.Graphics2dApi.BasicObjects;

public class AnimatedImage : Square
{
    private Texture[] _textures;
    private int _currentIndex = 0;
    private float _timer = 0f;
    private float _fps = 0f;
    public bool Shake;
    public Vector3 BasePos;
    
    public AnimatedImage(SquareData data, Texture[] textures, float fps) : base(data)
    {
        Data.ScaleAccordingToTex(0.3f);
        _textures = textures;
        _timer = 1 / fps;
        _fps = fps;
        Data.Scale *= 0.3f;
        BasePos = Data.Position;
        Time.Start();
    }

    public override void Update(FrameEventArgs args)
    {
        base.Update(args);
        _timer -= (float)args.Time;
        if (_timer <= 0)
        {
            _timer = 1 / _fps;
            _currentIndex = _currentIndex >= _textures?.Length - 1 ? 0 : _currentIndex + 1;
            Data.Texture = _textures[_currentIndex];
        }
        
        if (Shake) Data.Position.X = BasePos.X + MathF.Cos((float)Time.Elapsed.TotalSeconds * 50) * 2;
    }
    
    public override void Unload()
    {
        Data.Shader.Dispose();
        foreach (Texture texture in _textures)
        {
            texture.Dispose(); 
        }
    }
}