using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LaQuetePourLalternance.Graphics2dApi;

public class Window(int width, int height, string title) : GameWindow(GameWindowSettings.Default,
    new NativeWindowSettings() { ClientSize = (width, height), Title = title })
{
    #region - Fields & Propreties

    public List<GameObject> Scene = new List<GameObject>();

    private Matrix4 _projection = Matrix4.CreateOrthographic(width, height, -1f, 100f);

    public Matrix4 Projection {
        get => _projection;
        private set => _projection = value;
    }

    // Handles :
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    
    private int VIRT_W = width;   // largeur logique
    private int VIRT_H = height;   // hauteur logique
    private float VIRT_RATIO = (float)width / height;
    
    #endregion
    #region - Methods override

    /// <summary>
    /// Called at the initialization of the window
    /// </summary>
    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        
        // Configure OpenGL
        GL.Enable(EnableCap.Blend); // mix the sourced frag (already on the buffer) with the new one (allow transparency)
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        
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
    }

    /// <summary>
    /// Called when a new frame is being rendered.
    /// Where the render code goes.
    /// </summary>
    /// <param name="args"></param>
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        Console.WriteLine(GameEnvironement.Window.Scene.Count);
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        GL.BindVertexArray(_vertexArrayObject);
        
        foreach (var gameObject in Scene)
        {
            gameObject.Use((float)args.Time);

            if (gameObject is IDrawable)
            {
                try
                {
                    IDrawable drawable = gameObject as IDrawable;
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                }
                catch (NullReferenceException e) {}
            }
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

        foreach (var gameObject in Scene)
        {
            gameObject.Update((float)args.Time);
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

        int winW = args.Width;
        int winH = args.Height;
        float winRatio = (float)winW / winH;

        // If the window is larger than the ratio
        if (winRatio > VIRT_RATIO)
        {
            int viewportW = (int)(winH * VIRT_RATIO);
            int offsetX    = (winW - viewportW) / 2;
            GL.Viewport(offsetX, 0, viewportW, winH);
            _projection = Matrix4.CreateOrthographic(
                VIRT_W, VIRT_H, -1f, 1f);
        }         
        // If the window is bigger than the ratio
        else
        {
            int viewportH = (int)(winW / VIRT_RATIO);
            int offsetY   = (winH - viewportH) / 2;
            GL.Viewport(0, offsetY, winW, viewportH);
            _projection = Matrix4.CreateOrthographic(VIRT_W, VIRT_H, -1f, 1f);
        }
    }

    /// <summary>
    /// Called at the closing of the window
    /// </summary>
    protected override void OnUnload()
    {
        base.OnUnload();

        foreach (var gameObject in Scene)
            if (gameObject is IDrawable)
            {
                try
                {
                    IDrawable drawable = gameObject as IDrawable;
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                }
                catch (NullReferenceException e) {}
            }
    }
    
    #endregion
    #region Methods

    public void AddGameObject(GameObject gameObject) => Scene.Add(gameObject);

    public void AddGameObjects(GameObject[] gameObjects) => Scene.AddRange(gameObjects);
    
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

