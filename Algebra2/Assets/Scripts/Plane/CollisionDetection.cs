using System;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Plane
{
    public class CollisionDetection : MonoBehaviour
    {
        [Header("Objects Mesh")]
        [SerializeField] private MeshFilter object1Mesh;
        [SerializeField] private MeshFilter object2Mesh;

        private Vec3[,,] theMesh;
        [SerializeField] private MyMesh myMesh;

        private void Start()
        {
            theMesh = myMesh.GetMesh();

            if (object1Mesh != null && object2Mesh != null)
            {
                CreatePlanes(object1Mesh, out List<MyPlane> planesObj1, out List<Vec3> trianglesObj1);
                CreatePlanes(object2Mesh, out List<MyPlane> planesObj2, out List<Vec3> trianglesObj2);

                CheckObjectPoints(planesObj1, out List<Vec3> pointsInsideObj1, trianglesObj1);
                CheckObjectPoints(planesObj2, out List<Vec3> pointsInsideObj2, trianglesObj2);

                if (CheckRepetiton(pointsInsideObj1, pointsInsideObj2))
                {
                    Debug.Log("HOUSTON, TENEMOS CONTACTO.");
                }
            }
        }

        private void CreatePlanes(MeshFilter objectMesh, out List<MyPlane> planes, out List<Vec3> trianglesObj)
        {
            planes = new List<MyPlane>();
            trianglesObj = new List<Vec3>();
    
            Vector3[] vertices = objectMesh.mesh.vertices;
            Vec3[] verticesVec3 = new Vec3[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                verticesVec3[i] = new Vec3(objectMesh.transform.TransformPoint(vertices[i]));
            }

            // Create two triangles for each face of the cube
            for (int i = 0; i < vertices.Length; i += 4)
            {
                trianglesObj.Add(verticesVec3[i]);
                trianglesObj.Add(verticesVec3[i + 1]);
                trianglesObj.Add(verticesVec3[i + 2]);
                trianglesObj.Add(verticesVec3[i + 2]);
                trianglesObj.Add(verticesVec3[i + 1]);
                trianglesObj.Add(verticesVec3[i + 3]);
                planes.Add(new MyPlane(verticesVec3[i], verticesVec3[i + 1], verticesVec3[i + 2]));
                planes.Add(new MyPlane(verticesVec3[i + 2], verticesVec3[i + 1], verticesVec3[i + 3]));
            }
        }


        private void CheckObjectPoints(List<MyPlane> planes, out List<Vec3> objectPoints, List<Vec3> trianglesObj)
        {
            objectPoints = new List<Vec3>();
            //Traverses through myMesh
            for (int x = 0; x < myMesh.GetAxisSteps(); x++)
            {
                for (int y = x + 1; y < myMesh.GetAxisSteps(); y++)
                {
                    for (int z = y + 1; z < myMesh.GetAxisSteps(); z++)
                    {
                        int collisions = 0;
                        for (var i = 0; i < planes.Count; i++)
                        {
                            //checks intersection between line and planes
                            if (LinePlaneIntersection(theMesh[x, y, z], planes[i], out Vec3 intersectionPoint))
                            {
                                if (intersectionPoint == Vec3.Back) continue; //no intersection case
                                
                                //checks if point is inside triangle
                                if (IsPointInsideTriangle(intersectionPoint, trianglesObj[i * 3],
                                        trianglesObj[i * 3 + 1], trianglesObj[i * 3 + 2]))
                                {
                                    collisions++;
                                    break;
                                }
                            }
                        }
                        //if odd ammount of collisions, adds point to points inside the object
                        if (collisions % 2 == 1) objectPoints.Add(theMesh[x, y, z]);
                    }
                }
            }
        }

        private bool LinePlaneIntersection(Vec3 lineStart, MyPlane plane, out Vec3 intersectionPoint)
        {
            Vec3 planeNormal = plane.Normal;
            Vec3 planePoint = planeNormal * -plane.Distance;
            
            Vec3 lineEnd = Vec3.Down; //uses this vec3 as reference because its outside of the mesh

            //calculates distance between points and start
            float distance1 = Vec3.Dot(planePoint - lineStart, planeNormal);
            //calculates direction of line compared to plane
            float distance2 = Vec3.Dot(planeNormal, lineEnd - lineStart);

            if (distance2 == 0) // line is parallel to plane
            {
                intersectionPoint = Vec3.Back;
                return false;
            }

            float distance3 = distance1 / distance2;
            //distance between start point and intersection point
            intersectionPoint = lineStart + (lineEnd - lineStart) * distance3;
            if (distance3 < 0 || distance3 > 1) // intersection point is not on the line segment
            {
                return false;
            }

            return true;
        }

        //barycentric coordinates method to calculate if point is inside triangle
        private bool IsPointInsideTriangle(Vec3 point, Vec3 v1, Vec3 v2, Vec3 v3)
        {
            Vec3 v1V2 = v2 - v1;
            Vec3 v1V3 = v3 - v1;
            Vec3 vp = point - v1;

            // Compute dot products
            float dot11 = Vec3.Dot(v1V2, v1V2);
            float dot12 = Vec3.Dot(v1V2, v1V3);
            float dot22 = Vec3.Dot(v1V3, v1V3);
            float dotp1 = Vec3.Dot(vp, v1V2);
            float dotp2 = Vec3.Dot(vp, v1V3);

            // Compute barycentric coordinates
            float invDenom = 1 / (dot11 * dot22 - dot12 * dot12);
            float u = (dot22 * dotp1 - dot12 * dotp2) * invDenom;
            float v = (dot11 * dotp2 - dot12 * dotp1) * invDenom;

            // Check if point is inside triangle
            return (u >= 0) && (v >= 0) && (u + v <= 1);
        }

        private bool CheckRepetiton(List<Vec3> points1, List<Vec3> points2)
        {
            foreach (Vec3 t in points1)
            {
                foreach (Vec3 t1 in points2)
                {
                    if (t == t1) return true;
                }
            }

            return false;
        }
    }
}