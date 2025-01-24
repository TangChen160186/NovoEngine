using Assimp;
using OpenTK.Mathematics;

namespace OvRenderingCS.Resources.Parsers
{
    /// <summary>
    /// Model parser using Assimp library
    /// </summary>
    public class AssimpParser : IModelParser
    {
        private static readonly string[] SupportedExtensions = {
            ".fbx", ".obj", ".3ds", ".dae", ".blend", ".ply", ".stl", ".gltf", ".glb"
        };

        private readonly AssimpContext _context;

        /// <summary>
        /// Creates a new Assimp parser
        /// </summary>
        public AssimpParser()
        {
            _context = new AssimpContext();
        }

        /// <summary>
        /// Load a model from a file
        /// </summary>
        public Model LoadFromFile(string path, ModelParserFlags flags = ModelParserFlags.Default)
        {
            var importFlags = ConvertFlags(flags);
            var scene = _context.ImportFile(path, importFlags);

            if (scene == null)
                throw new Exception($"Failed to load model: {_context.GetErrorString()}");

            var model = new Model();

            // Process each mesh in the scene
            foreach (var mesh in scene.Meshes)
            {
                var vertices = new List<Geometry.Vertex>();
                var indices = new List<uint>();

                // Get vertices
                for (int i = 0; i < mesh.VertexCount; i++)
                {
                    var position = mesh.HasVertices ? ToVector3(mesh.Vertices[i]) : Vector3.Zero;
                    var texCoords = mesh.HasTextureCoords(0) ? ToVector2(mesh.TextureCoordinateChannels[0][i]) : Vector2.Zero;
                    var normal = mesh.HasNormals ? ToVector3(mesh.Normals[i]) : Vector3.UnitY;
                    var tangent = mesh.HasTangentBasis ? ToVector3(mesh.Tangents[i]) : Vector3.UnitX;
                    var bitangent = mesh.HasTangentBasis ? ToVector3(mesh.BiTangents[i]) : Vector3.UnitZ;

                    vertices.Add(new Geometry.Vertex(position, texCoords, normal, tangent, bitangent));
                }

                // Get indices
                for (int i = 0; i < mesh.FaceCount; i++)
                {
                    var face = mesh.Faces[i];
                    foreach (var index in face.Indices)
                    {
                        indices.Add((uint)index);
                    }
                }

                // Create mesh
                var ovMesh = new Mesh(vertices.ToArray(), indices.ToArray());
                model.AddMesh(ovMesh, mesh.MaterialIndex >= 0 ? scene.Materials[mesh.MaterialIndex].Name : null);
            }

            return model;
        }

        /// <summary>
        /// Gets whether the parser supports the given file format
        /// </summary>
        public bool SupportsExtension(string extension)
        {
            return SupportedExtensions.Contains(extension.ToLowerInvariant());
        }

        private static PostProcessSteps ConvertFlags(ModelParserFlags flags)
        {
            var steps = PostProcessSteps.None;

            if (flags.HasFlag(ModelParserFlags.CalculateTangentSpace))
                steps |= PostProcessSteps.CalculateTangentSpace;
            if (flags.HasFlag(ModelParserFlags.JoinIdenticalVertices))
                steps |= PostProcessSteps.JoinIdenticalVertices;
            if (flags.HasFlag(ModelParserFlags.MakeLeftHanded))
                steps |= PostProcessSteps.MakeLeftHanded;
            if (flags.HasFlag(ModelParserFlags.Triangulate))
                steps |= PostProcessSteps.Triangulate;
            if (flags.HasFlag(ModelParserFlags.RemoveRedundantComponents))
                steps |= PostProcessSteps.RemoveRedundantMaterials;
            if (flags.HasFlag(ModelParserFlags.GenerateSmoothNormals))
                steps |= PostProcessSteps.GenerateSmoothNormals;
            if (flags.HasFlag(ModelParserFlags.GenerateNormals))
                steps |= PostProcessSteps.GenerateNormals;
            if (flags.HasFlag(ModelParserFlags.FixInvalidNormals))
                steps |= PostProcessSteps.FixInfacingNormals;
            if (flags.HasFlag(ModelParserFlags.OptimizeMeshes))
                steps |= PostProcessSteps.OptimizeMeshes;
            if (flags.HasFlag(ModelParserFlags.OptimizeGraph))
                steps |= PostProcessSteps.OptimizeGraph;
            if (flags.HasFlag(ModelParserFlags.FlipUVs))
                steps |= PostProcessSteps.FlipUVs;

            return steps;
        }

        private static Vector3 ToVector3(Vector3D vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        private static Vector2 ToVector2(Vector3D vector)
        {
            return new Vector2(vector.X, vector.Y);
        }
    }
}
