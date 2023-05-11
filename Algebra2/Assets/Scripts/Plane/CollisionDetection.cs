using System;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Plane
{
    public class CollisionDetection : MonoBehaviour
    {
        [SerializeField] private MeshFilter object1Mesh;
        [SerializeField] private MeshFilter object2Mesh;

        private List<MyPlane> planesObj1;
        private List<MyPlane> planesObj2;

        private List<Vec3> pointsInsideObj1;
        private List<Vec3> pointsInsideObj2;

        private Vec3[,,] theMesh;
        [SerializeField] private MyMesh myMesh;

        private void Start()
        {
            theMesh = myMesh.GetMesh();
            planesObj1 = new List<MyPlane>();
            planesObj2 = new List<MyPlane>();
            pointsInsideObj1 = new List<Vec3>();
            pointsInsideObj2 = new List<Vec3>();
            if (object1Mesh != null && object2Mesh != null)
            {
                CreatePlanes(object1Mesh, planesObj1);
                CreatePlanes(object2Mesh, planesObj2);

                CheckPointsInsideObject(planesObj1, pointsInsideObj1);
                CheckPointsInsideObject(planesObj2, pointsInsideObj2);

                if (CheckRepetiton(pointsInsideObj1, pointsInsideObj2))
                {
                    Debug.Log("HOUSTON, TENEMOS CONTACTO.");
                }
            }
        }
        
        private void CreatePlanes(MeshFilter objectMesh, List<MyPlane> planes)
        {
            Vector3[] vertices = objectMesh.mesh.vertices;
            Vec3[] verticesVec3 = new Vec3[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                verticesVec3[i] = new Vec3(vertices[i]);
            }
            // Create two triangles for each face of the cube
            for (int i = 0; i < vertices.Length; i += 4)
            {
                planes.Add(new MyPlane(verticesVec3[i], verticesVec3[i+1], verticesVec3[i+2]));
                planes.Add(new MyPlane(verticesVec3[i+2], verticesVec3[i+1], verticesVec3[i+3]));
            }
        }

        public bool LinePlaneIntersection(Vec3 lineStart, MyPlane plane)
        {
            Vec3 planeNormal = plane.Normal;
            Vec3 planePoint = planeNormal * -plane.Distance;
            Vector3 lineEndVector = Random.onUnitSphere;
            Vec3 lineEnd;
            lineEnd.x = lineEndVector.x;
            lineEnd.y = lineEndVector.y;
            lineEnd.z = lineEndVector.z;

            float distance1 = Vec3.Dot(planePoint - lineStart, planeNormal);
            float distance2 = Vec3.Dot(planeNormal, lineEnd - lineStart);

            if (distance2 == 0) // line is parallel to plane
            {
                return false;
            }

            float distance3 = distance1 / distance2;

            if (distance3 < 0 || distance3 > 1) // intersection point is not on the line segment
            {
                return false;
            }

            return true;
        }

        private void CheckPointsInsideObject(List<MyPlane> planes, List<Vec3> pointsInsideObject)
        {
            for (int x = 0; x < myMesh.GetAxisSteps(); x++)
            {
                for (int y = x + 1; y < myMesh.GetAxisSteps(); y++)
                {
                    for (int z = y + 1; z < myMesh.GetAxisSteps(); z++)
                    {
                        int collisions = 0;
                        foreach (MyPlane plane in planes)
                        {
                            if (LinePlaneIntersection(theMesh[x, y, z], plane)) collisions++;
                        }

                        if (collisions % 2 != 0) pointsInsideObject.Add(new Vec3(x, y, z));
                    }
                }
            }
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