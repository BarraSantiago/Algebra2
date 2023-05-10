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

            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = i + 1; j < vertices.Length; j++)
                {
                    for (int k = j + 1; k < vertices.Length; k++)
                    {
                        planes.Add(new MyPlane(verticesVec3[i], verticesVec3[j], verticesVec3[k]));
                    }
                }
            }
        }

        private bool LinePlaneIntersection(Vector3 origin, MyPlane plane)
        {
            Vector3 direction = Random.onUnitSphere;
            float distance;

            // Check if the line and plane are not parallel
            if (Vector3.Dot(direction, plane.Normal) != 0)
            {
                distance = (plane.Distance - Vector3.Dot(plane.Normal, origin)) / Vector3.Dot(direction, plane.Normal);

                // Check if the intersection is in front of the line's origin
                if (distance > 0)
                {
                    return true;
                }
            }

            return false;
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
            for (int i = 0; i < points1.Count; i++)
            {
                for (int j = 0; j < points2.Count; j++)
                {
                    if (points1[i] == points2[j]) return true;
                }
            }

            return false;
        }
    }
}