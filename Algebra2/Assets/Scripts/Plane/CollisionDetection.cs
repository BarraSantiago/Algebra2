using System;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

namespace Plane
{
    public class CollisionDetection : MonoBehaviour
    {
        [SerializeField] private GameObject object1;
        [SerializeField] private GameObject object2;

        private MyMesh myMesh;
        private float planeDrawSize; 

        private Vec3[,,] meshVectors;

        private Vec3[] object1Points;
        private Vec3[] object2Points;

        private void Start()
        {
            planeDrawSize = 0.5f;
            meshVectors = myMesh.GetMesh();
        }

        List<MyPlane> planes = new List<MyPlane>();

        private void FindPoints(GameObject theObject, Vec3[] objectPoints)
        {
            Mesh objectMesh = theObject.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = objectMesh.vertices;
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
    }
}