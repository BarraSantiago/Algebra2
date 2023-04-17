using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;
using UnityEngine.Serialization;

public class Exercise : MonoBehaviour
{
    [SerializeField] private Vector3 aVector3;
    [SerializeField] private Vector3 bVector3;

    private Vec3 aVec3;
    private Vec3 bVec3;
    private Vec3 cVec3;

    private void OnValidate()
    {
        Debug.Log(Exercise4());
    }

    private void Update()
    {
        aVec3 = Vector3ToVec3(aVector3);
        bVec3 = Vector3ToVec3(bVector3);
        cVec3 = Exercise5();
        
        //For Exercise 5
        if (cVec3 == bVec3)
        {
            cVec3 = aVec3;
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

    public Vec3 Exercise1()
    {
        return new Vec3(aVec3 + bVec3);
    }

    public Vec3 Exercise2()
    {
        return new Vec3(aVec3 - bVec3);
    }

    public Vec3 Exercise3()
    {
        aVec3.Scale(bVec3);
        
        return new Vec3(aVec3);
    }

    public Vec3 Exercise4()
    {
        return Vec3.Cross(bVec3, aVec3);
    }

    public Vec3 Exercise5()
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
    
}