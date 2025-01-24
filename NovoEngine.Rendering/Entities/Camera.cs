using OpenTK.Mathematics;

namespace OvRenderingCS.Entities
{
    /// <summary>
    /// Camera entity for viewing the scene
    /// </summary>
    public class Camera : Entity
    {
        private float _fov;
        private float _near;
        private float _far;
        private float _aspectRatio;
        private bool _projectionDirty;
        private Matrix4 _projection;
        private Matrix4 _view;

        /// <summary>
        /// Creates a new camera with default parameters
        /// </summary>
        public Camera()
        {
            _fov = MathHelper.DegreesToRadians(45.0f);
            _near = 0.1f;
            _far = 1000.0f;
            _aspectRatio = 16.0f / 9.0f;
            _projectionDirty = true;
            _projection = Matrix4.Identity;
            _view = Matrix4.Identity;
        }

        /// <summary>
        /// Gets or sets the field of view (in radians)
        /// </summary>
        public float FieldOfView
        {
            get => _fov;
            set
            {
                _fov = value;
                _projectionDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the near clipping plane distance
        /// </summary>
        public float NearPlane
        {
            get => _near;
            set
            {
                _near = value;
                _projectionDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the far clipping plane distance
        /// </summary>
        public float FarPlane
        {
            get => _far;
            set
            {
                _far = value;
                _projectionDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the aspect ratio (width / height)
        /// </summary>
        public float AspectRatio
        {
            get => _aspectRatio;
            set
            {
                _aspectRatio = value;
                _projectionDirty = true;
            }
        }

        /// <summary>
        /// Gets the camera's projection matrix
        /// </summary>
        public Matrix4 ProjectionMatrix
        {
            get
            {
                if (_projectionDirty)
                {
                    _projection = Matrix4.CreatePerspectiveFieldOfView(_fov, _aspectRatio, _near, _far);
                    _projectionDirty = false;
                }
                return _projection;
            }
        }

        /// <summary>
        /// Gets the camera's view matrix
        /// </summary>
        public Matrix4 ViewMatrix
        {
            get
            {
                _view = Matrix4.Invert(Transform);
                return _view;
            }
        }

        /// <summary>
        /// Gets the camera's forward direction
        /// </summary>
        public Vector3 Forward => -Vector3.Transform(Vector3.UnitZ, Transform.ExtractRotation());

        /// <summary>
        /// Gets the camera's right direction
        /// </summary>
        public Vector3 Right => Vector3.Transform(Vector3.UnitX, Transform.ExtractRotation());

        /// <summary>
        /// Gets the camera's up direction
        /// </summary>
        public Vector3 Up => Vector3.Transform(Vector3.UnitY, Transform.ExtractRotation());

        /// <summary>
        /// Gets the camera's position
        /// </summary>
        public Vector3 Position => Transform.ExtractTranslation();

        /// <summary>
        /// Gets the camera's view-projection matrix
        /// </summary>
        public Matrix4 ViewProjectionMatrix => ViewMatrix * ProjectionMatrix;

        /// <summary>
        /// Look at a target position
        /// </summary>
        public void LookAt(Vector3 target, Vector3 up)
        {
            Transform = Matrix4.LookAt(Position, target, up);
        }

        /// <summary>
        /// Set the camera's orientation using Euler angles
        /// </summary>
        public void SetOrientation(float pitch, float yaw, float roll)
        {
            var rotation = Matrix4.CreateFromQuaternion(
                Quaternion.FromEulerAngles(
                    MathHelper.DegreesToRadians(pitch),
                    MathHelper.DegreesToRadians(yaw),
                    MathHelper.DegreesToRadians(roll)
                )
            );

            Transform = rotation * Matrix4.CreateTranslation(Position);
        }

        /// <summary>
        /// Get frustum planes in world space
        /// </summary>
        public Plane[] GetFrustumPlanes()
        {
            var viewProj = ViewProjectionMatrix;
            var planes = new Plane[6];

            // Left plane
            planes[0] = new Plane(
                viewProj.M14 + viewProj.M11,
                viewProj.M24 + viewProj.M21,
                viewProj.M34 + viewProj.M31,
                viewProj.M44 + viewProj.M41
            ).Normalized();

            // Right plane
            planes[1] = new Plane(
                viewProj.M14 - viewProj.M11,
                viewProj.M24 - viewProj.M21,
                viewProj.M34 - viewProj.M31,
                viewProj.M44 - viewProj.M41
            ).Normalized();

            // Bottom plane
            planes[2] = new Plane(
                viewProj.M14 + viewProj.M12,
                viewProj.M24 + viewProj.M22,
                viewProj.M34 + viewProj.M32,
                viewProj.M44 + viewProj.M42
            ).Normalized();

            // Top plane
            planes[3] = new Plane(
                viewProj.M14 - viewProj.M12,
                viewProj.M24 - viewProj.M22,
                viewProj.M34 - viewProj.M32,
                viewProj.M44 - viewProj.M42
            ).Normalized();

            // Near plane
            planes[4] = new Plane(
                viewProj.M14 + viewProj.M13,
                viewProj.M24 + viewProj.M23,
                viewProj.M34 + viewProj.M33,
                viewProj.M44 + viewProj.M43
            ).Normalized();

            // Far plane
            planes[5] = new Plane(
                viewProj.M14 - viewProj.M13,
                viewProj.M24 - viewProj.M23,
                viewProj.M34 - viewProj.M33,
                viewProj.M44 - viewProj.M43
            ).Normalized();

            return planes;
        }

        /// <summary>
        /// Check if a point is inside the frustum
        /// </summary>
        public bool IsPointInFrustum(Vector3 point)
        {
            var planes = GetFrustumPlanes();

            foreach (var plane in planes)
            {
                if (plane.Normal.X * point.X + plane.Normal.Y * point.Y + plane.Normal.Z * point.Z + plane.D <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if a sphere is inside or intersects the frustum
        /// </summary>
        public bool IsSphereInFrustum(Vector3 center, float radius)
        {
            var planes = GetFrustumPlanes();

            foreach (var plane in planes)
            {
                if (plane.Normal.X * center.X + plane.Normal.Y * center.Y + plane.Normal.Z * center.Z + plane.D <= -radius)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
