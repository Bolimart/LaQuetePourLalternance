using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Photon;

namespace LaQuetePourLalternance;

internal class Program
{
    public static void Main(string[] args)
    {
        GameEnvironement.Init(854, 480, "La quête pour l'alternance");
        GameEnvironement.Window.Run();
        // game.StartGame();
    }
}

public class Window(int width, int height, string title) : GameWindow(GameWindowSettings.Default,
    new NativeWindowSettings() { ClientSize = (width, height), Title = title })
{
    #region - Fields & Propreties

    private List<Square> squares = new List<Square>();
    
    // Handles :
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    
    private int _width = width;
    private int _height = height;
    
    #endregion
    #region - Methods override

    /// <summary>
    /// Called at the initialization of the window
    /// </summary>
    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.2f, 1.0f);
        
        // Load Objects
        _vertexBufferObject = GL.GenBuffer();
        _vertexArrayObject = GL.GenVertexArray();
        
        // Bind and configure Buffers
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, SquareData.Verticies.Length * sizeof(float), SquareData.Verticies, BufferUsageHint.StaticDraw);
        GL.BindVertexArray(_vertexArrayObject);
        
        // Configure VAO
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0); // Vertex coord
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float)); // Vertex UV
        GL.EnableVertexAttribArray(1);

        var shader = new Shader("shader.vert", "shader.frag");
        
        squares.Add(new Square(new SquareData(
            new(0, 0, 0), new(2, 2, 1), 0.0f, 
            new Texture("EpicGameIntro.png"), shader)));
        
        squares.Add(new Square(new SquareData(
            new(0, 0, 0), new(1, 1, 1), 0.0f, 
            new Texture("Vincent_Dead.png"), shader)));
    }

    /// <summary>
    /// Called when a new frame is being rendered.
    /// Where the render code goes.
    /// </summary>
    /// <param name="args"></param>
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        GL.BindVertexArray(_vertexArrayObject);
        
        foreach (var square in squares)
        {
            square.Use((float)args.Time);
            
            Matrix4 model = square.GetModelMatrix();
            
            square.Data.Shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
        
        // Swap the old frame with the newly rendered frame.
        SwapBuffers();
    }

    /// <summary>
    /// Called when the render has ended and is ready to update the screen.
    /// Where the logic code goes.
    /// </summary>
    /// <param name="args"></param>
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        foreach (var square in squares)
        {
            square.Update((float)args.Time);
        }

        if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
    }

    /// <summary>
    /// called when the window (and therefore the buffer) is resized.
    /// </summary>
    /// <param name="args"></param>
    protected override void OnFramebufferResize(FramebufferResizeEventArgs args)
    {
        base.OnFramebufferResize(args);

        _width = args.Width;
        _height = args.Height;
            
        GL.Viewport(0, 0, args.Width, args.Height);
    }

    /// <summary>
    /// Called at the closing of the window
    /// </summary>
    protected override void OnUnload()
    {
        base.OnUnload();
    }
    
    #endregion
    #region Methods
        
    /// <summary>
    /// Generate a transform matrix.
    /// </summary>
    /// <param name="trans">The transform vector.</param>
    /// <param name="scale">The scale vector.</param>
    /// <param name="rot">Currently the rot is only on the Z axis.</param>
    /// <returns>The transform matrix.</returns>
    private Matrix4 GetTransformMatrix(Vector3 trans, Vector3 scale, Vector3 rot)
    {
        Matrix4 mRot = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rot.Z));
        mRot *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rot.X));
        mRot *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rot.Y));
        Matrix4 mScale = Matrix4.CreateScale(scale);
        Matrix4 mTrans = Matrix4.CreateTranslation(trans);
        return mRot * mScale * mTrans;
    }

    #endregion
}


// Singleton that countains important variable
static class GameEnvironement
{
    public static Random Rand = new ();
    public static Window Window { get; private set; }
    public static Game Game { get; private set; }

    public static void Init(int width, int height, string title)
    {
        Window = new Window(width, height, title);
        Game = new Game();
    }
}


static class Helper 
{
    public static string Asking(string question, string?[] answers = null)
    {
        string answer = "";
        //
        bool valid = false;
        
        while (!valid){
            Console.WriteLine(question);
            //Si on note rien en readline c'est possible que ça renvoie une erreur donc pour s'assurer que tout se passe bien
            //on dit "si il n'y a rien, tu vas stocker "" (string vide)
            answer = Console.ReadLine() ?? "";

            //Si answers est vide 
            if (answers == null && answer != "") break;

            else{
                foreach (string a in answers){
                    if (a == answer) {
                        valid = true;
                        break; 
                    }
                }
            }
        }
        return answer;
    }
    
}