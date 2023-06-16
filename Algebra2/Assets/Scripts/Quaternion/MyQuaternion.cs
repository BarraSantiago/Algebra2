using CustomMath;
using UnityEngine;
using UnityEngine.Internal;

namespace MyQuat
{
    public struct MyQuaternion
    {
        Matrix4x4
        /*
        variables
            constructor
        this[int index]
        identity
            eulerAngles
        normalized
        */
        Angle(MyQuaternion a, MyQuaternion b);
        AngleAxis(float angle, Vec3 axis);
        AxisAngle(Vec3 axis, float angle);
        Dot(MyQuaternion a, MyQuaternion b);
        Euler(Vec3 euler);
        Euler(float x, float y, float z);
        FromToRotation(Vec3 fromDirection, Vec3 toDirection);
        Inverse(MyQuaternion rotation);
        Lerp(MyQuaternion a, MyQuaternion b, float t);
        LerpUnclamped(MyQuaternion a, MyQuaternion b, float t);
        LookRotation(Vec3 forward);
        LookRotation(Vec3 forward, [DefaultValue("Vec3.up")] Vec3 upwards);
        Normalize(MyQuaternion q);
        RotateTowards(MyQuaternion from, MyQuaternion to, float maxDegreesDelta);
        Slerp(MyQuaternion a, MyQuaternion b, float t);
        SlerpUnclamped(MyQuaternion a, MyQuaternion b, float t);
        Set(float newX, float newY, float newZ, float newW);
        SetFromToRotation(Vec3 fromDirection, Vec3 toDirection);
        SetLookRotation(Vec3 view);
        SetLookRotation(Vec3 view, [DefaultValue("Vec3.up")] Vec3 up);
        ToAngleAxis(out float angle, out Vec3 axis);
        ToString();
        Vec3 operator *(MyQuaternion rotation, Vec3 point);
        MyQuaternion operator *(MyQuaternion lhs, MyQuaternion rhs);
        bool operator ==(MyQuaternion lhs, MyQuaternion rhs);
        bool operator !=(MyQuaternion lhs, MyQuaternion rhs);
    }
}