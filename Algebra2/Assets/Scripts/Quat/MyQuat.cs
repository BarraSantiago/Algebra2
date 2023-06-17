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

        public MyQuat identity = new MyQuat(0, 0, 0, 1);

        #region eulerAngles

        #endregion

        public MyQuat normalized()
        {
            float magnitude = (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);

            return new MyQuat(X / magnitude, Y / magnitude, Z / magnitude, W / magnitude);
        }

        public float Angle(MyQuat a, MyQuat b)
        {
            //tratamos a los Quats como vectores y les hacemos el producto escalar para medir la alineacion entre estos
            //y se toma el valor absoluto para, principalmente, devolver un valor positivo
            float absDotProduct = Math.Abs(Dot(a, b));
            //como el producto escalar esta directamente relacionado con el coseno del angulo
            //al aplicar el arcocoseo nos devuelve el angulo en sí en radianes.
            return (float)Math.Acos(absDotProduct);
        }

        AngleAxis(float angle, Vec3 axis);
        AxisAngle(Vec3 axis, float angle);

        float Dot(MyQuat a, MyQuat b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        Euler(Vec3 euler);
        Euler(float x, float y, float z);
        FromToRotation(Vec3 fromDirection, Vec3 toDirection);
        Inverse(MyQuat rotation);
        Lerp(MyQuat a, MyQuat b, float t);
        LerpUnclamped(MyQuat a, MyQuat b, float t);
        LookRotation(Vec3 forward);
        LookRotation(Vec3 forward, [DefaultValue("Vec3.up")] Vec3 upwards);
        Normalize(MyQuat q);
        RotateTowards(MyQuat from, MyQuat to, float maxDegreesDelta);
        Slerp(MyQuat a, MyQuat b, float t);
        SlerpUnclamped(MyQuat a, MyQuat b, float t);


        void Set(float newX, float newY, float newZ, float newW)
        {
            X = newX;
            Y = newY;
            Z = newZ;
            W = newW;
        }

        SetFromToRotation(Vec3 fromDirection, Vec3 toDirection);
        SetLookRotation(Vec3 view);
        SetLookRotation(Vec3 view, [DefaultValue("Vec3.up")] Vec3 up);
        ToAngleAxis(out float angle, out Vec3 axis);

        Vec3 operator *(MyQuat rotation, Vec3 point)
        {
            
        }
        MyQuat operator *(MyQuat lhs, MyQuat rhs);
        bool operator ==(MyQuat lhs, MyQuat rhs);
        bool operator !=(MyQuat lhs, MyQuat rhs);
    }
}