using NovoEngine.Rendering.Buffers;
using NovoEngine.Rendering.Entities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace NovoEngine.Rendering.Features
{
    /// <summary>
    /// Render feature for debug shapes
    /// </summary>
    public class DebugShapeRenderFeature : ARenderFeature
    {
        private readonly VertexArray _lineVao;
        private readonly VertexBuffer _lineVbo;
        private readonly List<(Vector3 start, Vector3 end, Color4 color)> _lines;
        private readonly List<(Vector3 center, float radius, Color4 color)> _spheres;
        private bool _isDirty;

        /// <summary>
        /// Creates a new debug shape render feature
        /// </summary>
        public DebugShapeRenderFeature()
        {
            _lineVao = new VertexArray();
            _lineVbo = new VertexBuffer();
            _lines = new List<(Vector3, Vector3, Color4)>();
            _spheres = new List<(Vector3, float, Color4)>();
            _isDirty = false;

            // Setup line VAO
            _lineVao.AddVertexBuffer(_lineVbo, new[]
            {
                new VertexBufferElement(0, 3, VertexAttribPointerType.Float, false, 28, 0),  // Position
                new VertexBufferElement(1, 4, VertexAttribPointerType.Float, false, 28, 12), // Color
            });
        }

        /// <summary>
        /// Draw a line
        /// </summary>
        public void DrawLine(Vector3 start, Vector3 end, Color4 color)
        {
            _lines.Add((start, end, color));
            _isDirty = true;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        public void DrawSphere(Vector3 center, float radius, Color4 color)
        {
            _spheres.Add((center, radius, color));
            _isDirty = true;
        }

        /// <summary>
        /// Draw a box
        /// </summary>
        public void DrawBox(Vector3 min, Vector3 max, Color4 color)
        {
            // Bottom face
            DrawLine(new Vector3(min.X, min.Y, min.Z), new Vector3(max.X, min.Y, min.Z), color);
            DrawLine(new Vector3(max.X, min.Y, min.Z), new Vector3(max.X, min.Y, max.Z), color);
            DrawLine(new Vector3(max.X, min.Y, max.Z), new Vector3(min.X, min.Y, max.Z), color);
            DrawLine(new Vector3(min.X, min.Y, max.Z), new Vector3(min.X, min.Y, min.Z), color);

            // Top face
            DrawLine(new Vector3(min.X, max.Y, min.Z), new Vector3(max.X, max.Y, min.Z), color);
            DrawLine(new Vector3(max.X, max.Y, min.Z), new Vector3(max.X, max.Y, max.Z), color);
            DrawLine(new Vector3(max.X, max.Y, max.Z), new Vector3(min.X, max.Y, max.Z), color);
            DrawLine(new Vector3(min.X, max.Y, max.Z), new Vector3(min.X, max.Y, min.Z), color);

            // Vertical edges
            DrawLine(new Vector3(min.X, min.Y, min.Z), new Vector3(min.X, max.Y, min.Z), color);
            DrawLine(new Vector3(max.X, min.Y, min.Z), new Vector3(max.X, max.Y, min.Z), color);
            DrawLine(new Vector3(max.X, min.Y, max.Z), new Vector3(max.X, max.Y, max.Z), color);
            DrawLine(new Vector3(min.X, min.Y, max.Z), new Vector3(min.X, max.Y, max.Z), color);
        }

        /// <summary>
        /// Clear all debug shapes
        /// </summary>
        public void Clear()
        {
            _lines.Clear();
            _spheres.Clear();
            _isDirty = true;
        }

        /// <summary>
        /// Setup the feature
        /// </summary>
        public override void Setup()
        {
            // No setup needed
        }

        /// <summary>
        /// Destroy the feature
        /// </summary>
        public override void Destroy()
        {
            _lineVao.Dispose();
            _lineVbo.Dispose();
            Shader?.Dispose();
        }

        /// <summary>
        /// Render debug shapes
        /// </summary>
        public override void Render(Camera camera)
        {
            if (!Enabled || Shader == null)
                return;

            if (_isDirty)
            {
                UpdateBuffers();
                _isDirty = false;
            }

            if (_lines.Count == 0 && _spheres.Count == 0)
                return;

            Shader.Use();
            Shader.SetMatrix4("u_ViewProjection", camera.ViewProjectionMatrix);

            // Draw lines
            if (_lines.Count > 0)
            {
                _lineVao.Bind();
                GL.DrawArrays(PrimitiveType.Lines, 0, _lines.Count * 2);
            }

            // Draw spheres
            foreach (var (center, radius, color) in _spheres)
            {
                var transform = Matrix4.CreateScale(radius) * Matrix4.CreateTranslation(center);
                Shader.SetMatrix4("u_Model", transform);
                Shader.SetVector4("u_Color", color);

                // Draw sphere wireframe using lines
                // In a real implementation, you might want to use a pre-generated sphere mesh
                const int segments = 16;
                const int rings = 8;

                for (int i = 0; i < segments; i++)
                {
                    float angle1 = i * MathHelper.TwoPi / segments;
                    float angle2 = (i + 1) * MathHelper.TwoPi / segments;

                    for (int j = 0; j < rings; j++)
                    {
                        float ringAngle1 = j * MathHelper.Pi / rings;
                        float ringAngle2 = (j + 1) * MathHelper.Pi / rings;

                        Vector3 p1 = new(
                            MathF.Sin(ringAngle1) * MathF.Cos(angle1),
                            MathF.Cos(ringAngle1),
                            MathF.Sin(ringAngle1) * MathF.Sin(angle1)
                        );

                        Vector3 p2 = new(
                            MathF.Sin(ringAngle1) * MathF.Cos(angle2),
                            MathF.Cos(ringAngle1),
                            MathF.Sin(ringAngle1) * MathF.Sin(angle2)
                        );

                        Vector3 p3 = new(
                            MathF.Sin(ringAngle2) * MathF.Cos(angle1),
                            MathF.Cos(ringAngle2),
                            MathF.Sin(ringAngle2) * MathF.Sin(angle1)
                        );

                        DrawLine(p1 * radius + center, p2 * radius + center, color);
                        DrawLine(p1 * radius + center, p3 * radius + center, color);
                    }
                }
            }
        }

        private void UpdateBuffers()
        {
            if (_lines.Count == 0)
                return;

            // Convert lines to vertex data
            var vertices = new List<float>();
            foreach (var (start, end, color) in _lines)
            {
                // Start vertex
                vertices.Add(start.X);
                vertices.Add(start.Y);
                vertices.Add(start.Z);
                vertices.Add(color.R);
                vertices.Add(color.G);
                vertices.Add(color.B);
                vertices.Add(color.A);

                // End vertex
                vertices.Add(end.X);
                vertices.Add(end.Y);
                vertices.Add(end.Z);
                vertices.Add(color.R);
                vertices.Add(color.G);
                vertices.Add(color.B);
                vertices.Add(color.A);
            }

            // Update buffer data
            _lineVbo.SetData(vertices.ToArray());
        }
    }
}
