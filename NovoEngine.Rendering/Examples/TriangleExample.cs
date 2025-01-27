using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;
using NovoEngine.Rendering.Buffers;
using NovoEngine.Rendering.Settings;
using NovoEngine.Rendering.Resources;

namespace NovoEngine.Rendering.Examples;

public class TriangleExample : IDisposable
{
    private VertexArray _vao;
    private VertexBuffer _vbo;
    private IndexBuffer _ibo;
    private UniformBuffer _ubo;
    private Shader _shader;

    // 顶点结构
    private struct Vertex
    {
        public Vector3 Position;
        public Vector4 Color;

        public Vertex(Vector3 position, Vector4 color)
        {
            Position = position;
            Color = color;
        }
    }

    // uniform数据结构
    private struct UniformData
    {
        public Matrix4 Transform;
    }

    private const string VertexShaderSource = @"
        #version 450 core
        
        layout(location = 0) in vec3 aPosition;
        layout(location = 1) in vec4 aColor;
        
        layout(std140, binding = 0) uniform TransformUBO
        {
            mat4 transform;
        };
        
        out vec4 vColor;
        
        void main()
        {
            gl_Position = transform * vec4(aPosition, 1.0);
            vColor = aColor;
        }
    ";

    private const string FragmentShaderSource = @"
        #version 450 core
        
        in vec4 vColor;
        out vec4 FragColor;
        
        void main()
        {
            FragColor = vColor;
        }
    ";

    public void Initialize()
    {
        // 创建着色器
        _shader = new Shader();
        _shader.CompileFromString(VertexShaderSource, FragmentShaderSource);

        // 创建顶点数据
        var vertices = new Vertex[]
        {
            new(new Vector3(-0.5f, -0.5f, 0.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f)),
            new(new Vector3( 0.5f, -0.5f, 0.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f)),
            new(new Vector3( 0.0f,  0.5f, 0.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f))
        };

        // 创建索引数据
        var indices = new uint[] { 0, 1, 2 };

        // 创建并设置顶点缓冲区
        _vbo = new VertexBuffer();
        _vbo.SetData(vertices);

        // 创建并设置索引缓冲区
        _ibo = new IndexBuffer();
        _ibo.SetData(indices);

        // 创建并设置顶点数组对象
        _vao = new VertexArray();
        
        // 设置顶点属性
        _vao.AddVertexBuffer(_vbo, 0, 3, EDataType.Float, false, 
            Marshal.SizeOf<Vertex>(), IntPtr.Zero); // position
        _vao.AddVertexBuffer(_vbo, 1, 4, EDataType.Float, false, 
            Marshal.SizeOf<Vertex>(), Marshal.OffsetOf<Vertex>("Color")); // color
        
        // 设置索引缓冲区
        _vao.SetIndexBuffer(_ibo);

        // 创建uniform缓冲区（用于变换矩阵）
        _ubo = new UniformBuffer(0);
        var transform = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(45.0f));
        _ubo.SetData(new UniformData[] { new() { Transform = transform } });

        // 设置OpenGL状态
        GL.Enable(EnableCap.DepthTest);
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    }

    public void Render()
    {
        // 清除缓冲区
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // 使用着色器程序
        _shader.Use();

        // 绑定VAO和相关缓冲区
        _vao.Bind();
        _ubo.Bind();

        // 绘制三角形
        GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, 0);

        // 解绑
        _vao.Unbind();
        _ubo.Unbind();
        _shader.Unbind();
    }

    public void Update(float deltaTime)
    {
        // 更新变换矩阵（旋转）
        var angle = deltaTime * 90.0f; // 每秒旋转90度
        var transform = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
        _ubo.SetData(new UniformData[] { new() { Transform = transform } });
    }

    public void Dispose()
    {
        // 释放资源
        _vao.Dispose();
        _vbo.Dispose();
        _ibo.Dispose();
        _ubo.Dispose();
        _shader.Dispose();
    }
} 