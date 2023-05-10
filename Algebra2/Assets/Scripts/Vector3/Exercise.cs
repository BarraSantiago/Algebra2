using CustomMath;
using UnityEngine;

public class Exercise : MonoBehaviour
{
    [SerializeField] private Vector3 aVector3;
    [SerializeField] private Vector3 bVector3;

    private Vec3 aVec3;
    private Vec3 bVec3;
    private Vec3 cVec3;

    private void OnValidate()
    {
        
    }

    private void Update()
    {
        aVec3 = Vector3ToVec3(aVector3);
        bVec3 = Vector3ToVec3(bVector3);
        cVec3 = Exercise10();
        Debug.Log(cVec3);
        //For Exercise 5
        /*
        if (cVec3 == bVec3)
        {
            cVec3 = aVec3;
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Vec3.Zero, aVec3);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(Vec3.Zero, bVec3);
        Gizmos.color = Color.black;
        Gizmos.DrawRay(Vec3.Zero, cVec3);
    }

    private Vec3 Vector3ToVec3(Vector3 vector3)
    {
        return new Vec3(vector3.x, vector3.y, vector3.z);
    }

    private Vec3 Exercise1()
    {
        return new Vec3(aVec3 + bVec3);
    }

    private Vec3 Exercise2()
    {
        return new Vec3(aVec3 - bVec3);
    }

    private Vec3 Exercise3()
    {
        aVec3.Scale(bVec3);

        return new Vec3(aVec3);
    }

    private Vec3 Exercise4()
    {
        return Vec3.Cross(bVec3, aVec3);
    }

    private Vec3 Exercise5()
    {
        float speed = 0.1f;
        Vec3 aToB = bVec3 - cVec3;
        float distance = aToB.magnitude;

        if (distance <= speed || distance == 0f)
        {
            return bVec3;
        }

        return cVec3 + aToB / distance * speed;
    }

    private Vec3 Exercise6()
    {
        float x = aVec3.x > bVec3.x ? aVec3.x : bVec3.x;
        float y = aVec3.y > bVec3.y ? aVec3.y : bVec3.y;
        float z = aVec3.z > bVec3.z ? aVec3.z : bVec3.z;
        return new Vec3(x, y, z);
    }

    private Vec3 Exercise7()
    {
        return bVec3.Normalized * aVec3.magnitude;
    }

    private Vec3 Exercise8() {
        float x = (aVec3.x + bVec3.x) / 2f;
        float y = (aVec3.y + bVec3.y) / 2f;
        float z = (aVec3.z + bVec3.z) / 2f;
        Vec3 c = new Vec3(x,y,z);
        c = c.Normalized * Vec3.Distance(aVec3, bVec3);
        return c;
    }

    private Vec3 Exercise9()
    {
        Vec3 projection = bVec3 * (Vec3.Dot(aVec3, bVec3) / Mathf.Pow(bVec3.magnitude, 2));

        // Get the reflection of vecToReflect about the plane defined by bVec3
        Vec3 reflection = aVec3 - 2 * projection;

        return reflection;
    }

    private Vec3 Exercise10()
    {
        Vec3 cross = Vec3.Cross(aVec3, bVec3);

        // Get the reflection of the cross product in the plane perpendicular to A
        Vec3 reflection = Vec3.Reflect(cross, aVec3.Normalized);


        return -reflection;
        
    }
}