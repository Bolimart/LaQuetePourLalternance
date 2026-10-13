using OpenTK.Mathematics;

namespace LaQuetePourLalternance.Graphics2dApi;
using System;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

public class Texture : IDisposable
{
    #region - Fields & Propreties
    
    public int Handle = 0;
    private bool _disposedValue;
    private int _references; // Count the number of squares that reference this texture
    public Vector2i Dimension;
    
    #endregion
    #region - Constructors
    
    public Texture(string fileName)
    {
        Handle = GL.GenTexture();
        
        // stb_image loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
        // This will correct that, making the texture display properly.
        StbImage.stbi_set_flip_vertically_on_load(1);
        
        // Load the image
        string path = Path.Combine(AppContext.BaseDirectory, "Asset", "Texture", fileName);
        ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
        Dimension = new Vector2i(image.Width, image.Height);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
        
        // Upload the texture to the GPU
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        
        // Generate the mipmap
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    }
    
    #endregion
    #region - Methods
    
    /// <summary>
    /// Activate the texture at the given texture unit and bind it.
    /// </summary>
    /// <param name="unit">The texture unit to activate.</param>
    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }

    #endregion
    #region - IDisposable Handling
    
    public void AddReference() => _references++;
    
    /// <summary>
    /// The function that allows the IDisposable interface to work.
    /// It deletes the texture from the VRAM.
    /// </summary>
    /// <param name="disposing">If the object is already being disposed</param>
    protected virtual void Dispose(bool disposing) // protected function that delete the shader from VRAM to prevent leaking
    {
        if (!_disposedValue)
        {
            GL.DeleteTexture(Handle);
            _disposedValue = true;
        }
    }
    
    /// <summary>
    /// At the end of the program, if the shader haven't been disposed, inform the developper.
    /// </summary>
    ~Texture() // Destructor to prevent GPU resource leak
    {
        if (_disposedValue == false)
        {
            Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }
    }
    
    /// <summary>
    /// The function to call to dispose of the texture. It will get disposed only if every reference of this
    /// shader is being disposed.
    /// Is mandatory to call when you end the program to not have any GPU resource leak.
    /// </summary>
    public void Dispose()
    {
        _references--;
        if (_references <= 0)
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    #endregion
}