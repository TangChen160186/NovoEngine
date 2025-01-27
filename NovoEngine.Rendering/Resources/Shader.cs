using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace NovoEngine.Rendering.Resources;

/// <summary>
/// OpenGL shader program wrapper
/// </summary>
public class Shader : IDisposable
{
    private readonly int _handle;
    private readonly Dictionary<string, UniformInfo> _uniforms;
    private bool _isLinked;
    private bool _isDisposed;

    /// <summary>
    /// Creates a new shader program
    /// </summary>
    public Shader()
    {
        _handle = GL.CreateProgram();
        _uniforms = new Dictionary<string, UniformInfo>();
        _isLinked = false;
    }

    /// <summary>
    /// Attach a shader stage to the program
    /// </summary>
    public void AttachShader(string source, ShaderType type)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        // Check for compilation errors
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(shader);
            GL.DeleteShader(shader);
            throw new Exception($"Error compiling {type} shader: {infoLog}");
        }

        GL.AttachShader(_handle, shader);
        GL.DeleteShader(shader);
    }

    /// <summary>
    /// Link the shader program
    /// </summary>
    public void Link()
    {
        GL.LinkProgram(_handle);

        // Check for linking errors
        GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(_handle);
            throw new Exception($"Error linking shader program: {infoLog}");
        }

        _isLinked = true;
        QueryUniforms();
    }

    /// <summary>
    /// Bind the shader program
    /// </summary>
    public void Use()
    {
        if (!_isLinked)
            throw new InvalidOperationException("Cannot use shader program: not linked");

        GL.UseProgram(_handle);
    }

    /// <summary>
    /// Query all uniforms in the shader program
    /// </summary>
    private void QueryUniforms()
    {
        GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out int uniformCount);

        for (int i = 0; i < uniformCount; i++)
        {
            string name = GL.GetActiveUniform(_handle, i, out int size, out ActiveUniformType type);
            int location = GL.GetUniformLocation(_handle, name);

            if (location != -1)
            {
                _uniforms[name] = new UniformInfo(location, ConvertUniformType(type), name);
            }
        }
    }

    private UniformType ConvertUniformType(ActiveUniformType type)
    {
        return type switch
        {
            ActiveUniformType.Bool => UniformType.Bool,
            ActiveUniformType.Int => UniformType.Int,
            ActiveUniformType.Float => UniformType.Float,
            ActiveUniformType.FloatVec2 => UniformType.Vector2,
            ActiveUniformType.FloatVec3 => UniformType.Vector3,
            ActiveUniformType.FloatVec4 => UniformType.Vector4,
            ActiveUniformType.FloatMat3 => UniformType.Matrix3,
            ActiveUniformType.FloatMat4 => UniformType.Matrix4,
            ActiveUniformType.Sampler2D => UniformType.Sampler2D,
            ActiveUniformType.SamplerCube => UniformType.SamplerCube,
            _ => throw new ArgumentException($"Unsupported uniform type: {type}")
        };
    }

    #region Set Uniforms

    public void SetBool(string name, bool value)
    {
        if (_uniforms.TryGetValue(name, out var info))
            GL.Uniform1(info.Location, value ? 1 : 0);
    }

    public void SetInt(string name, int value)
    {
        if (_uniforms.TryGetValue(name, out var info))
            GL.Uniform1(info.Location, value);
    }

    public void SetFloat(string name, float value)
    {
        if (_uniforms.TryGetValue(name, out var info))
            GL.Uniform1(info.Location, value);
    }

    public void SetVector2(string name, Vector2 value)
    {
        if (_uniforms.TryGetValue(name, out var info))
            GL.Uniform2(info.Location, value);
    }

    public void SetVector3(string name, Vector3 value)
    {
        if (_uniforms.TryGetValue(name, out var info))
            GL.Uniform3(info.Location, value);
    }

    public void SetVector4(string name, Vector4 value)
    {
        if (_uniforms.TryGetValue(name, out var info))
            GL.Uniform4(info.Location, value);
    }

    public void SetMatrix3(string name, Matrix3 value)
    {
        if (_uniforms.TryGetValue(name, out var info))
            GL.UniformMatrix3(info.Location, false, ref value);
    }

    public void SetMatrix4(string name, Matrix4 value)
    {
        if (_uniforms.TryGetValue(name, out var info))
            GL.UniformMatrix4(info.Location, false, ref value);
    }

    public void SetTexture(string name, int textureUnit)
    {
        if (_uniforms.TryGetValue(name, out var info))
            GL.Uniform1(info.Location, textureUnit);
    }

    #endregion

    /// <summary>
    /// Gets the OpenGL handle of the shader program
    /// </summary>
    public int Handle => _handle;

    /// <summary>
    /// Gets whether the shader program is linked
    /// </summary>
    public bool IsLinked => _isLinked;

    /// <summary>
    /// Gets information about all uniforms in the shader program
    /// </summary>
    public IReadOnlyDictionary<string, UniformInfo> Uniforms => _uniforms;

    /// <summary>
    /// Dispose the shader program
    /// </summary>
    public void Dispose()
    {
        if (!_isDisposed)
        {
            GL.DeleteProgram(_handle);
            _isDisposed = true;
        }
    }
}

public enum UniformType
{
    Bool,
    Int,
    Float,
    Vector2,
    Vector3,
    Vector4,
    Matrix3,
    Matrix4,
    Sampler2D,
    SamplerCube
}

public class UniformInfo
{
    public int Location { get; }
    public UniformType Type { get; }
    public string Name { get; }

    public UniformInfo(int location, UniformType type, string name)
    {
        Location = location;
        Type = type;
        Name = name;
    }
}