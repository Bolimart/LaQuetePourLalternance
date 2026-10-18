using OpenTK.Mathematics;

namespace LaQuetePourLalternance.Graphics2dApi;

static class GameEnvironement
{
    public static Random Rand = new ();
    public static Window Window { get; private set; }
    public static Game Game { get; private set; }

    public static SceneManager SceneManager;

    public static void Init(SceneManager sceneManager,int width, int height, string title)
    {
        Window = new Window(width, height, title);
        Game = new Game();
        SceneManager = sceneManager;
    }
}


public static class GameHelper
{
    public static Vector2 WorldToScreen(Vector2 worldPos)
    {
        var screenPos = Vector2.Zero;
        
        // Get the world coordinate in 2D to 3D
        Vector4 clip = new Vector4(worldPos.X, worldPos.Y, 0f, 1f);
        
        // Project the point
        clip = Vector4.TransformRow(clip, GameEnvironement.Window.Projection);

        // Set the projected point -> NDC (Normalized Device Coordinates)
        float ndcX = clip.X / clip.W;
        float ndcY = clip.Y / clip.W;

        // Move the pixel from NDC [-1, 1] -> screenspace [0, 1]
        Vector2i size = GameEnvironement.Window.ClientSize;
        screenPos.X = (ndcX * 0.5f + 0.5f) * size.X;
        screenPos.Y = (1f - (ndcY * 0.5f + 0.5f)) * size.Y;

        return screenPos;
    }
    
    public static Vector2 ScreenToWorld(Vector2 screen)
    {
        Vector2i size = GameEnvironement.Window.ClientSize;
        
        // Move the pixel from screenspace [0, 1] -> NDC [-1, 1]
        float ndcX = screen.X / size.X * 2f - 1f;
        float ndcY = 1f - screen.Y / size.Y * 2f;
        
        // Un-project the point (only work with ortho projection)
        Matrix4 inv = Matrix4.Invert(GameEnvironement.Window.Projection);
        Vector4 w = Vector4.TransformRow(new Vector4(ndcX, ndcY, 0f, 1f), inv);

        return new Vector2(w.X, w.Y);
    }
}