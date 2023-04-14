using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlane : MonoBehaviour
{
    internal const int size = 16;
    private Vector3 m_Normal;
    private float m_Distance;
    
    /// <summary>
    ///   <para>Normal vector of the plane.</para>
    /// </summary>
    //public Vector3 normal { }

    
    /// <summary>
    ///   <para>The distance measured from the Plane to the origin, along the Plane's normal.</para>
    /// </summary>
    //public float distance { }

    /// <summary>
    ///   <para>Creates a plane.</para>
    /// </summary>
    /// <param name="inNormal"></param>
    /// <param name="inPoint"></param>
    public MyPlane(Vector3 inNormal, Vector3 inPoint)
    {
    }

    /// <summary>
    ///   <para>Creates a plane.</para>
    /// </summary>
    /// <param name="inNormal"></param>
    /// <param name="d"></param>
    public MyPlane(Vector3 inNormal, float d)
    {
    }

    /// <summary>
    ///   <para>Creates a plane.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    public MyPlane(Vector3 a, Vector3 b, Vector3 c)
    {
    }

    /// <summary>
    ///   <para>Sets a plane using a point that lies within it along with a normal to orient it.</para>
    /// </summary>
    /// <param name="inNormal">The plane's normal vector.</param>
    /// <param name="inPoint">A point that lies on the plane.</param>
    public void SetNormalAndPosition(Vector3 inNormal, Vector3 inPoint)
    {
    }

    /// <summary>
    ///   <para>Sets a plane using three points that lie within it.  The points go around clockwise as you look down on the top surface of the plane.</para>
    /// </summary>
    /// <param name="a">First point in clockwise order.</param>
    /// <param name="b">Second point in clockwise order.</param>
    /// <param name="c">Third point in clockwise order.</param>
    public void Set3Points(Vector3 a, Vector3 b, Vector3 c)
    {
    }

    /// <summary>
    ///   <para>Makes the plane face in the opposite direction.</para>
    /// </summary>
    public void Flip()
    {
    }

    /// <summary>
    ///   <para>Returns a copy of the plane that faces in the opposite direction.</para>
    /// </summary>
    public Plane flipped;

    /// <summary>
    ///   <para>Moves the plane in space by the translation vector.</para>
    /// </summary>
    /// <param name="translation">The offset in space to move the plane with.</param>
    public void Translate(Vector3 translation)
    {
        m_Distance += Vector3.Dot(m_Normal, translation);
    }

    /// <summary>
    ///   <para>Returns a copy of the given plane that is moved in space by the given translation.</para>
    /// </summary>
    /// <param name="plane">The plane to move in space.</param>
    /// <param name="translation">The offset in space to move the plane with.</param>
    /// <returns>
    ///   <para>The translated plane.</para>
    /// </returns>
    /*
    public static Plane Translate(Plane plane, Vector3 translation)
    {
        return new Plane(, plane.m_Distance + Vector3.Dot(m_Normal, translation));
    }
    */
    /// <summary>
    ///   <para>For a given point returns the closest point on the plane.</para>
    /// </summary>
    /// <param name="point">The point to project onto the plane.</param>
    /// <returns>
    ///   <para>A point on the plane that is closest to point.</para>
    /// </returns>
   /*
    public Vector3 ClosestPointOnPlane(Vector3 point)
    {
    }

    /// <summary>
    ///   <para>Returns a signed distance from plane to point.</para>
    /// </summary>
    /// <param name="point"></param>
    public float GetDistanceToPoint(Vector3 point);

    /// <summary>
    ///   <para>Is a point on the positive side of the plane?</para>
    /// </summary>
    /// <param name="point"></param>
    public bool GetSide(Vector3 point);

    /// <summary>
    ///   <para>Are two points on the same side of the plane?</para>
    /// </summary>
    /// <param name="inPt0"></param>
    /// <param name="inPt1"></param>
    public bool SameSide(Vector3 inPt0, Vector3 inPt1)
    {
    }

    public bool Raycast(Ray ray, out float enter)
    {
    }
    */
}