using System;
using CustomMath;
using UnityEngine;
using UnityEngine.Internal;

namespace Quat
{
    public struct MyQuat
    {
        #region Variables

        private float X { get; set; }
        private float Y { get; set; }
        private float Z { get; set; }
        private float W { get; set; }

        #endregion

        #region Constructor

        public MyQuat(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        #endregion

        //this[int index]

        public static MyQuat identity = new MyQuat(0f, 0f, 0f, 1f);


        public Vec3 eulerAngles()
        {
            // Calculate the yaw (Z rotation).
            float yaw = Mathf.Atan2(2 * X * Y + 2 * W * Z, 1 - 2 * X * X - 2 * Y * Y);
            // Calculate the pitch (Y rotation).
            float pitch = Mathf.Asin(2 * X * Z - 2 * W * Y);
            // Calculate the roll (X rotation).
            float roll = Mathf.Atan2(2 * Y * Z + 2 * W * X, 1 - 2 * Y * Y - 2 * Z * Z);
            // Return the Euler angles.
            return new Vec3(yaw, pitch, roll);
        }


        public MyQuat normalized()
        {
            float magnitude = Mathf.Sqrt(X * X + Y * Y + Z * Z + W * W);

            if (magnitude > 0)
            {
                return new MyQuat(X / magnitude, Y / magnitude, Z / magnitude, W / magnitude);
            }

            // If the magnitude is zero, return the original quaternion
            return identity;
        }

        public static float Angle(MyQuat a, MyQuat b)
        {
            //tratamos a los cuaterniones como vectores y les hacemos el producto escalar para medir la alineacion entre estos
            //y se toma el valor absoluto para, principalmente, devolver un valor positivo
            float absDotProduct = Math.Abs(Dot(a, b));
            //como el producto escalar esta directamente relacionado con el coseno del angulo
            //al aplicar el arcocoseo nos devuelve el angulo en sí en radianes.
            return Mathf.Acos(absDotProduct);
        }

        public static MyQuat AngleAxis(float angle, Vec3 axis)
        {
            return identity;
        }

        public static MyQuat AxisAngle(Vec3 axis, float angle)
        {
            return identity;
        }

        static float Dot(MyQuat a, MyQuat b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static MyQuat Euler(Vec3 euler)
        {
            float yaw = euler.y * (float)Math.PI / 180f; // Yaw (rotacion vertical)
            float pitch = euler.x * (float)Math.PI / 180f; // Pitch (rotacion horizontal)
            float roll = euler.z * (float)Math.PI / 180f; // Roll (rotacion profundidad )

            float cy = (float)Math.Cos(yaw * 0.5f);
            float sy = (float)Math.Sin(yaw * 0.5f);
            float cp = (float)Math.Cos(pitch * 0.5f);
            float sp = (float)Math.Sin(pitch * 0.5f);
            float cr = (float)Math.Cos(roll * 0.5f);
            float sr = (float)Math.Sin(roll * 0.5f);

            MyQuat quaternion = new MyQuat();
            quaternion.W = cy * cp * cr + sy * sp * sr;
            quaternion.X = cy * cp * sr - sy * sp * cr;
            quaternion.Y = sy * cp * sr + cy * sp * cr;
            quaternion.Z = sy * cp * cr - cy * sp * sr;

            return quaternion;
        }

        public static MyQuat Euler(float x, float y, float z)
        {
            return identity;
        }

        //que tan desplazado esta del origen
        public static MyQuat FromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            return identity;
        }

        public static MyQuat Inverse(MyQuat rotation)
        {
            //Es necesario calcular la magnitud del cuaternion para despues 
            //no se aplica la raiz cuadrada a la magnitud debido a que esta es suficiente para obtener la normalizacion del cuaternion
            //y realizar la raiz cuadrada puede tener un costo de eficiencia alto
            float magnitudeSquared = rotation.X * rotation.X + rotation.Y * rotation.Y + rotation.Z * rotation.Z +
                                     rotation.W * rotation.W;

            return new MyQuat(-rotation.X / magnitudeSquared, -rotation.Y / magnitudeSquared,
                -rotation.Z / magnitudeSquared, rotation.W / magnitudeSquared);
        }

        public static MyQuat Lerp(MyQuat a, MyQuat b, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t)); // Clamp t between 0 and 1

            // Calculate the interpolated quaternion components
            float lerpX = a.X + (b.X - a.X) * t;
            float lerpY = a.Y + (b.Y - a.Y) * t;
            float lerpZ = a.Z + (b.Z - a.Z) * t;
            float lerpW = a.W + (b.W - a.W) * t;

            // Create and return the interpolated quaternion
            return new MyQuat { X = lerpX, Y = lerpY, Z = lerpZ, W = lerpW };
        }

        public static MyQuat LerpUnclamped(MyQuat a, MyQuat b, float t)
        {
            return identity;
        }

        public static MyQuat LookRotation(Vec3 forward)
        {
            return identity;
        }

        public static MyQuat LookRotation(Vec3 forward, [DefaultValue("Vec3.up")] Vec3 upwards)
        {
            return identity;
        }

        public void Normalize()
        {
            float magnitude = Mathf.Sqrt(X * X + Y * Y + Z * Z + W * W);

            X /= magnitude;
            Y /= magnitude;
            Z /= magnitude;
            W /= magnitude;
        }

        public static MyQuat Normalize(MyQuat q)
        {
            float magnitude = Mathf.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W);

            if (magnitude > 0)
            {
                return new MyQuat(q.X / magnitude, q.Y / magnitude, q.Z / magnitude, q.W / magnitude);
            }

            return identity;
        }

        public static MyQuat RotateTowards(MyQuat from, MyQuat to, float maxDegreesDelta)
        {
            return identity;
        }

        public static MyQuat Slerp(MyQuat a, MyQuat b, float t)
        {
            return identity;
        }

        public static MyQuat SlerpUnclamped(MyQuat a, MyQuat b, float t)
        {
            return identity;
        }


        void Set(float newX, float newY, float newZ, float newW)
        {
            X = newX;
            Y = newY;
            Z = newZ;
            W = newW;
        }

        public void SetFromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
        }

        public void SetLookRotation(Vec3 view)
        {
        }

        public void SetLookRotation(Vec3 view, [DefaultValue("Vec3.up")] Vec3 up)
        {
        }

        public void ToAngleAxis(out float angle, out Vec3 axis)
        {
            angle = 0f;
            axis = Vec3.Zero;
        }

        public static Vec3 operator *(MyQuat rotation, Vec3 point)
        {
            // Perform quaternion multiplication with the vector
            float num1 = rotation.Y * point.z - rotation.Z * point.y;
            float num2 = rotation.Z * point.x - rotation.X * point.z;
            float num3 = rotation.X * point.y - rotation.Y * point.x;
            float num4 = rotation.X * point.x + rotation.Y * point.y + rotation.Z * point.z;

            // Calculate the resulting vector
            float resultX = (num4 * rotation.X + num1 * rotation.W) + (num3 * rotation.Z - num2 * rotation.Y);
            float resultY = (num4 * rotation.Y + num2 * rotation.W) + (num1 * rotation.X - num3 * rotation.Z);
            float resultZ = (num4 * rotation.Z + num3 * rotation.W) + (num2 * rotation.Y - num1 * rotation.X);
            return new Vec3(resultX, resultY, resultZ);
        }

        public static MyQuat operator *(MyQuat lhs, MyQuat rhs)
        {
            MyQuat quat = new MyQuat();
            quat.W = (lhs.W * rhs.W - lhs.X * rhs.X - lhs.Y * rhs.Y - lhs.Z * rhs.Z);
            quat.X = (lhs.W * rhs.X + lhs.X * rhs.W + lhs.Y * rhs.Z - lhs.Z * rhs.Y);
            quat.X = (lhs.Y * rhs.W + lhs.W * rhs.Y + lhs.Z * rhs.X - lhs.X * rhs.Z);
            quat.X = (lhs.Z * rhs.W + lhs.W * rhs.Z - lhs.Y * rhs.X + lhs.X * rhs.Y);

            return quat;
        }

        public static bool operator ==(MyQuat lhs, MyQuat rhs)
        {
            // Definir un valor epsilon para comparar valores de punto flotante
            float epsilon = 1e-12f;

            // Compare los componentes individuales de los cuaterniones con una tolerancia de epsilon
            return Math.Abs(lhs.X - rhs.X) < epsilon &&
                   Math.Abs(lhs.Y - rhs.Y) < epsilon &&
                   Math.Abs(lhs.Z - rhs.Z) < epsilon &&
                   Math.Abs(lhs.W - rhs.W) < epsilon;
        }

        public static bool operator !=(MyQuat lhs, MyQuat rhs)
        {
            // Definir un valor epsilon para comparar valores de punto flotante
            float epsilon = 1e-05f;

            // Compare los componentes individuales de los cuaterniones con una tolerancia de epsilon
            return !(Math.Abs(lhs.X - rhs.X) < epsilon &&
                     Math.Abs(lhs.Y - rhs.Y) < epsilon &&
                     Math.Abs(lhs.Z - rhs.Z) < epsilon &&
                     Math.Abs(lhs.W - rhs.W) < epsilon);
        }
    }
}