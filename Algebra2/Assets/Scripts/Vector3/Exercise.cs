using System;
using CustomMath;
using UnityEngine;

public class Exercise : MonoBehaviour
{
    enum ExerciseEnum
    {
        Uno = 1,
        Dos,
        Tres,
        Cuatro,
        Cinco,
        Seis,
        Siete,
        Ocho,
        Nueve,
        Diez
    }

    [Header("Exercise")]
    [SerializeField]
    private ExerciseEnum exercise = ExerciseEnum.Uno;

    [SerializeField] private bool printVec3 = false;

    [Header("Vectors")] [SerializeField] private Vector3 aVector3;
    [SerializeField] private Vector3 bVector3;


    private Vec3 aVec3;
    private Vec3 bVec3;
    private Vec3 cVec3;


    private void Update()
    {
        aVec3 = Vector3ToVec3(aVector3);
        bVec3 = Vector3ToVec3(bVector3);

        switch (exercise)
        {
            case ExerciseEnum.Uno:
                cVec3 = Exercise1();
                break;
            case ExerciseEnum.Dos:
                cVec3 = Exercise2();
                break;
            case ExerciseEnum.Tres:
                cVec3 = Exercise3();
                break;
            case ExerciseEnum.Cuatro:
                cVec3 = Exercise4();
                break;
            case ExerciseEnum.Cinco:
                cVec3 = Exercise5();
                break;
            case ExerciseEnum.Seis:
                cVec3 = Exercise6();
                break;
            case ExerciseEnum.Siete:
                cVec3 = Exercise7();
                break;
            case ExerciseEnum.Ocho:
                cVec3 = Exercise8();
                break;
            case ExerciseEnum.Nueve:
                cVec3 = Exercise9();
                break;
            case ExerciseEnum.Diez:
                cVec3 = Exercise10();
                break;
        }
    }

    private void OnValidate()
    {
        if (printVec3)
        {
            Debug.Log(cVec3);
        }
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
        //Sum of A and B
        return new Vec3(aVec3 + bVec3);
    }

    private Vec3 Exercise2()
    {
        //C is the difference between A and B
        return new Vec3(aVec3 - bVec3);
    }

    private Vec3 Exercise3()
    {
        //C is multiplication of A and B
        aVec3.Scale(bVec3);

        return new Vec3(aVec3);
    }

    private Vec3 Exercise4()
    {
        //Vec C perpendicular || to  A and B 
        return Vec3.Cross(bVec3, aVec3);
    }

    private Vec3 Exercise5()
    {
        float speed = 0.1f;
        Vec3 aToB = bVec3 - cVec3;
        float distance = aToB.magnitude;

        //Moves Vec C from A to B in a constant speed
        if (distance <= speed || distance == 0f)
        {
            return bVec3;
        }

        return cVec3 + aToB / distance * speed;
    }

    private Vec3 Exercise6()
    {
        //Vec C has the greatest value of each vec
        float x = aVec3.x > bVec3.x ? aVec3.x : bVec3.x;
        float y = aVec3.y > bVec3.y ? aVec3.y : bVec3.y;
        float z = aVec3.z > bVec3.z ? aVec3.z : bVec3.z;
        return new Vec3(x, y, z);
    }

    private Vec3 Exercise7()
    {
        //Vec C  perpendicular ⟂ to B with magnitude of A
        return bVec3.Normalized * aVec3.magnitude;
    }

    private Vec3 Exercise8()
    {
        float x = (aVec3.x + bVec3.x) / 2f;
        float y = (aVec3.y + bVec3.y) / 2f;
        float z = (aVec3.z + bVec3.z) / 2f;
        Vec3 c = new Vec3(x, y, z);
        
        //Creates vec C that targets the middle point of vec A and B with the magnitude of the distance of A to B
        c = c.Normalized * Vec3.Distance(aVec3, bVec3);
        return c;
    }

    private Vec3 Exercise9()
    {
        //Vec3 projection = bVec3 * (Vec3.Dot(aVec3, bVec3) / Mathf.Pow(bVec3.magnitude, 2));
        Vec3 projection = Vec3.Project(aVec3, bVec3);
        
        //Calculates the reflection of vec A over B
        Vec3 reflection = aVec3 - 2 * projection;

        return reflection;
    }

    private Vec3 Exercise10()
    {
        Vec3 cross = Vec3.Cross(aVec3, bVec3);

        // reflection of the cross product in the plane perpendicular to A
        Vec3 reflection = Vec3.Reflect(cross, aVec3.Normalized);


        return -reflection;
    }
}