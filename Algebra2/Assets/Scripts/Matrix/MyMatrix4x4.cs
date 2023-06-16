using CustomMath;
using MyQuat;
using UnityEngine;

namespace Matrix
{
    public class MyMatrix4x4
    {
        /*
        variables
            constructor
        this[int index]
        this[int row, int column]
        zero
            identity
        rotation
            lossyScale
        isIdentity
            determinant
        transpose
            inverse
            */
        Determinant(MyMatrix4x4 m);
        Inverse(MyMatrix4x4 m);
        Rotate(MyQuaternion q);
        Scale(Vec3 vector);
        Translate(Vec3 vector);
        Transpose(MyMatrix4x4 m);
        TRS(Vec3 pos, MyQuaternion q, Vec3 s);
        GetColumn(int index);
        GetPosition();
        GetRow(int index);
        MultiplyPoint(Vec3 point);
        MultiplyPoint3x4(Vec3 point);
        MultiplyVector(Vec3 vector);
        SetColumn(int index, Vector4 column);
        SetRow(int index, Vector4 row);
        SetTRS(Vec3 pos, MyQuaternion q, Vec3 s);
        ValidTRS();
        Vector4 operator *(MyMatrix4x4 lhs, Vector4 vector);
        MyMatrix4x4 operator *(MyMatrix4x4 lhs, MyMatrix4x4 rhs);
        bool operator ==(MyMatrix4x4 lhs, MyMatrix4x4 rhs);
        bool operator !=(MyMatrix4x4 lhs, MyMatrix4x4 rhs);
    }
}