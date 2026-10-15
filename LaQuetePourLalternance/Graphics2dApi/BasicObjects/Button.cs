using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LaQuetePourLalternance.Graphics2dApi.BasicObjects;

public class Button : Square
{
    public Vector4 HoveredColor
    {
        set => Data.Shader.SetVector4("hoveredColor", value);
    }
    public Vector4 ClickedColor
    {
        set => Data.Shader.SetVector4("clickedColor", value);
    }

    private event Action _action;
    
    public Button(Vector3 position, Vector3 scale, float rotation, Texture texture, Action action) : base(new SquareData(position, scale, rotation, texture, new Shader("button.vert", "button.frag")))
    {
        Data.ScaleAccordingToTex();
        _action = action;
        HoveredColor = new Vector4(1f, 1f, 1f, 1f);
        ClickedColor = new Vector4(0f, 0f, 0f, 1f);
    }

    public override void Update(FrameEventArgs args)
    {
        base.Update(args);

        var mouse = GameEnvironement.Window.MouseState;
        var isMouseOn = IsInBoudingBox(mouse.Position);
        
        if (isMouseOn)
        {
            if (mouse.IsButtonPressed(MouseButton.Left))
            {
                _action.Invoke();
                Console.WriteLine("Button pressed");
            }
            else if (mouse.IsButtonDown(MouseButton.Left))
            {
                Data.Shader.SetBool("isHovered", false);
                Data.Shader.SetBool("isClicked", true);
            }
            else if (mouse.IsButtonReleased(MouseButton.Left))
                Data.Shader.SetBool("isClicked", false);
            else Data.Shader.SetBool("isHovered", true);
        }
        else Data.Shader.SetBool("isHovered", false);
    }
    
    /// <summary>
    /// Check if a pos is in a bounding box.
    /// </summary>
    /// <param name="pos">The pos to check</param>
    /// <returns></returns>
    public bool IsInBoudingBox(Vector2 pos)
    {
        // Convert the mouse coordinate into world space.
        Vector2 mouseCoord = GameHelper.ScreenToWorld(pos);

        Vector2 half = Data.Scale.Xy / 2f;
        
        // Inversed rotation and delta to go into the button local coordinate
        Vector2 delta = mouseCoord - Data.Position.Xy;
        float cos = MathF.Cos(-Data.Rotation);
        float sin = MathF.Sin(-Data.Rotation);

        Vector2 local = new Vector2(
            delta.X * cos - delta.Y * sin,
            delta.X * sin + delta.Y * cos);
        Console.WriteLine($"écran={pos}  world={mouseCoord}  bouton={Data.Position.Xy}");
        
        return MathF.Abs(local.X) < half.X && MathF.Abs(local.Y) < half.Y;
    }
}