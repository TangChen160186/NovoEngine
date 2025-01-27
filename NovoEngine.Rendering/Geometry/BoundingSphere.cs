using OpenTK.Mathematics;

namespace NovoEngine.Rendering.Geometry
{
    /// <summary>
    /// Data structure that defines a bounding sphere (Position + radius)
    /// </summary>
    public struct BoundingSphere
    {
        /// <summary>
        /// Center position of the sphere
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Radius of the sphere
        /// </summary>
        public float Radius;

        /// <summary>
        /// Creates a new bounding sphere
        /// </summary>
        public BoundingSphere(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        /// <summary>
        /// Creates a bounding sphere that contains all the given points
        /// </summary>
        public static BoundingSphere FromPoints(IEnumerable<Vector3> points)
        {
            if (!points.Any())
                return new BoundingSphere(Vector3.Zero, 0);

            // Calculate center (average of all points)
            var center = Vector3.Zero;
            int count = 0;
            foreach (var point in points)
            {
                center += point;
                count++;
            }
            center /= count;

            // Find radius (maximum distance from center to any point)
            float maxDistanceSquared = 0;
            foreach (var point in points)
            {
                float distanceSquared = (point - center).LengthSquared;
                if (distanceSquared > maxDistanceSquared)
                    maxDistanceSquared = distanceSquared;
            }

            return new BoundingSphere(center, MathF.Sqrt(maxDistanceSquared));
        }

        /// <summary>
        /// Check if a point is inside the sphere
        /// </summary>
        public bool Contains(Vector3 point)
        {
            return (point - Position).LengthSquared <= Radius * Radius;
        }

        /// <summary>
        /// Check if another sphere intersects with this one
        /// </summary>
        public bool Intersects(BoundingSphere other)
        {
            float radiusSum = Radius + other.Radius;
            return (Position - other.Position).LengthSquared <= radiusSum * radiusSum;
        }
    }
}
