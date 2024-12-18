﻿using UnityEngine;
using System;

namespace CustomMath
{
    public struct Vec3 : IEquatable<Vec3>
    {
        #region Variables

        public float x;
        public float y;
        public float z;

        public float sqrMagnitude
        {
            get { return x * x + y * y + z * z; }
        }

        public Vec3 Normalized
        {
            get
            {
                float mag = magnitude;


                if (mag > 0)
                {
                    return new Vec3(x / mag, y / mag, z / mag);
                }

                return new Vec3(0, 0, 0);
            }
        }

        public float magnitude
        {
            get { return Mathf.Sqrt(x * x + y * y + z * z); }
        }

        #endregion

        #region constants

        public const float epsilon = 1e-05f;

        #endregion

        #region Default Values

        public static Vec3 Zero
        {
            get { return new Vec3(0.0f, 0.0f, 0.0f); }
        }

        public static Vec3 One
        {
            get { return new Vec3(1.0f, 1.0f, 1.0f); }
        }

        public static Vec3 Forward
        {
            get { return new Vec3(0.0f, 0.0f, 1.0f); }
        }

        public static Vec3 Back
        {
            get { return new Vec3(0.0f, 0.0f, -1.0f); }
        }

        public static Vec3 Right
        {
            get { return new Vec3(1.0f, 0.0f, 0.0f); }
        }

        public static Vec3 Left
        {
            get { return new Vec3(-1.0f, 0.0f, 0.0f); }
        }

        public static Vec3 Up
        {
            get { return new Vec3(0.0f, 1.0f, 0.0f); }
        }

        public static Vec3 Down
        {
            get { return new Vec3(0.0f, -1.0f, 0.0f); }
        }

        public static Vec3 PositiveInfinity
        {
            get { return new Vec3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity); }
        }

        public static Vec3 NegativeInfinity
        {
            get { return new Vec3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity); }
        }

        #endregion

        #region Constructors

        public Vec3(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0.0f;
        }

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3(Vec3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector2 v2)
        {
            this.x = v2.x;
            this.y = v2.y;
            this.z = 0.0f;
        }

        #endregion

        #region Operators

        public static bool operator ==(Vec3 left, Vec3 right)
        {
            float diff_x = left.x - right.x;
            float diff_y = left.y - right.y;
            float diff_z = left.z - right.z;
            float sqrmag = diff_x * diff_x + diff_y * diff_y + diff_z * diff_z;
            return sqrmag < epsilon * epsilon;
        }

        public static bool operator !=(Vec3 left, Vec3 right)
        {
            return !(left == right);
        }

        public static Vec3 operator +(Vec3 leftV3, Vec3 rightV3)
        {
            return new Vec3(leftV3.x + rightV3.x, leftV3.y + rightV3.y, leftV3.z + rightV3.z);
        }

        public static Vec3 operator -(Vec3 leftV3, Vec3 rightV3)
        {
            return new Vec3(leftV3.x - rightV3.x, leftV3.y - rightV3.y, leftV3.z - rightV3.z);
        }

        public static Vec3 operator -(Vec3 v3)
        {
            return new Vec3(-v3.x, -v3.y, -v3.z);
        }

        public static Vec3 operator *(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x * scalar, v3.y * scalar, v3.z * scalar);
        }

        public static Vec3 operator *(float scalar, Vec3 v3)
        {
            return new Vec3(v3.x * scalar, v3.y * scalar, v3.z * scalar);
        }

        public static Vec3 operator /(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x / scalar, v3.y / scalar, v3.z / scalar);
        }

        public static implicit operator Vector3(Vec3 v3)
        {
            return new Vector3(v3.x, v3.y, v3.z);
        }

        public static implicit operator Vector2(Vec3 v2)
        {
            return new Vector2(v2.x, v2.y);
        }

        #endregion

        #region Functions

        public override string ToString()
        {
            return "X = " + x + "   Y = " + y + "   Z = " + z;
        }

        public static float Angle(Vec3 from, Vec3 to)
        {
            float dot = Dot(from, to);
            float magProduct = from.magnitude * to.magnitude;

            if (magProduct == 0) return 0;

            float angle = Mathf.Acos(dot / magProduct) * Mathf.Rad2Deg;
            return angle;
        }


        public static Vec3 ClampMagnitude(Vec3 vector, float maxLength)
        {
            if (vector.sqrMagnitude > maxLength * maxLength)
            {
                return new Vec3(vector.Normalized * maxLength);
            }

            return vector;
        }

        public static float Magnitude(Vec3 vector)
        {
            return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        }

        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
            );
        }

        public static float Distance(Vec3 a, Vec3 b)
        {
            return Mathf.Sqrt(Mathf.Pow(b.x - a.x, 2) + Mathf.Pow(b.y - a.y, 2) + Mathf.Pow(b.z - a.z, 2));
        }

        public static float Dot(Vec3 a, Vec3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vec3 Lerp(Vec3 a, Vec3 b, float t)
        {
            return a + (b - a) * Mathf.Clamp01(t);
        }

        public static Vec3 LerpUnclamped(Vec3 a, Vec3 b, float t)
        {
            return a + (b - a) * t;
        }

        public static Vec3 Max(Vec3 a, Vec3 b)
        {
            return new Vec3(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
        }

        public static Vec3 Min(Vec3 a, Vec3 b)
        {
            return new Vec3(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z));
        }

        /// <summary>
        /// Calculates the squared magnitude (length) of a vector.
        /// </summary>
        /// <param name="vector"> The vector whose squared magnitude is to be calculated.</param>
        /// <returns> The squared magnitude of the vector.</returns>
        /// <remarks>
        /// The squared magnitude is useful in performance-critical code where the actual magnitude is not needed,
        /// as it avoids the computational cost of a square root operation.
        /// </remarks>
        public static float SqrMagnitude(Vec3 vector)
        {
            return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
        }

        /// <summary>
        /// Projects a vector onto another vector (normal).
        /// </summary>
        /// <param name="vector"> The vector to be projected.</param>
        /// <param name="onNormal"> The vector onto which the first vector is projected.</param>
        /// <returns> The projection of the vector onto the normal.</returns>
        /// <remarks>
        /// This is useful in physics and graphics to find the component of a vector in the direction of another vector.
        /// </remarks>
        public static Vec3 Project(Vec3 vector, Vec3 onNormal)
        {
            float dot = Dot(vector, onNormal);
            return onNormal * dot / onNormal.sqrMagnitude;
        }

        /// <summary>
        /// Reflects a vector off a surface defined by a normal.
        /// </summary>
        /// <param name="inDirection">The direction vector to be reflected.</param>
        /// <param name="inNormal">The normal vector of the surface.</param>
        /// <returns> The reflected vector.</returns>
        /// <remarks>
        /// This is commonly used in physics simulations and graphics to calculate the direction of a reflected ray or object.
        /// </remarks>
        public static Vec3 Reflect(Vec3 inDirection, Vec3 inNormal)
        {
            return inDirection - 2f * Dot(inDirection, inNormal) * inNormal;
        }

        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }

        public void Scale(Vec3 scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        public void Normalize()
        {
            float mag = magnitude;
            if (mag > 0)
            {
                x /= mag;
                y /= mag;
                z /= mag;
            }
        }

        #endregion

        #region Internals

        public override bool Equals(object other)
        {
            if (other is not Vec3 vec3)
            {
                return false;
            }

            return Equals(vec3);
        }

        public bool Equals(Vec3 other)
        {
            return Mathf.Approximately(x, other.x) &&
                   Mathf.Approximately(y, other.y) &&
                   Mathf.Approximately(z, other.z);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }

        #endregion
    }
}