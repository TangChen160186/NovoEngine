using OpenTK.Mathematics;

namespace OvRenderingCS.Resources
{
    /// <summary>
    /// Model resource that contains multiple meshes
    /// </summary>
    public class Model : IDisposable
    {
        private readonly List<Mesh> _meshes;
        private readonly Dictionary<string, int> _materialIndices;
        private Matrix4 _transform;

        /// <summary>
        /// Creates a new empty model
        /// </summary>
        public Model()
        {
            _meshes = new List<Mesh>();
            _materialIndices = new Dictionary<string, int>();
            _transform = Matrix4.Identity;
        }

        /// <summary>
        /// Add a mesh to the model
        /// </summary>
        public void AddMesh(Mesh mesh, string? materialName = null)
        {
            int index = _meshes.Count;
            _meshes.Add(mesh);

            if (materialName != null)
            {
                _materialIndices[materialName] = index;
            }
        }

        /// <summary>
        /// Get a mesh by index
        /// </summary>
        public Mesh GetMesh(int index)
        {
            return _meshes[index];
        }

        /// <summary>
        /// Get a mesh by material name
        /// </summary>
        public Mesh? GetMeshByMaterial(string materialName)
        {
            return _materialIndices.TryGetValue(materialName, out int index) ? _meshes[index] : null;
        }

        /// <summary>
        /// Gets or sets the model's transform matrix
        /// </summary>
        public Matrix4 Transform
        {
            get => _transform;
            set => _transform = value;
        }

        /// <summary>
        /// Gets the number of meshes in the model
        /// </summary>
        public int MeshCount => _meshes.Count;

        /// <summary>
        /// Gets all meshes in the model
        /// </summary>
        public IReadOnlyList<Mesh> Meshes => _meshes;

        /// <summary>
        /// Gets the material indices
        /// </summary>
        public IReadOnlyDictionary<string, int> MaterialIndices => _materialIndices;

        /// <summary>
        /// Dispose the model and all its meshes
        /// </summary>
        public void Dispose()
        {
            foreach (var mesh in _meshes)
            {
                mesh.Dispose();
            }
            _meshes.Clear();
            _materialIndices.Clear();
        }
    }
}
