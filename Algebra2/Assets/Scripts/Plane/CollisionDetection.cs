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
        private float planeDrawSize = 3;

        private Vec3[,,] theMesh;

        private Vec3[] object1Points;
        private Vec3[] object2Points;

        private void Start()
        {
            theMesh = myMesh.GetMesh();
        }

        List<MyPlane> planes = new List<MyPlane>();


        private void OnDrawGizmos()
        {
            /*
            // Draw a line along the plane's normal
            FindPoints(object1, object1Points);
            var position = object1.transform.position;
            for (int i = 0; i < planes.Count; i++)
            {
                Debug.DrawLine(position, position + planes[i].normal, Color.blue);

                // Calculate two vectors perpendicular to the plane's normal
                Vector3 tangent1 = Vector3.Cross(planes[i].normal, Vector3.up).normalized;
                Vector3 tangent2 = Vector3.Cross(planes[i].normal, tangent1).normalized;

                // Draw a square using the two perpendicular vectors and the plane's distance from the origin
                Vector3 p1 = position + tangent1 * planeDrawSize + tangent2 * planeDrawSize + planes[i].distance * planes[i].normal;
                Vector3 p2 = position - tangent1 * planeDrawSize + tangent2 * planeDrawSize + planes[i].distance * planes[i].normal;
                Vector3 p3 = position - tangent1 * planeDrawSize - tangent2 * planeDrawSize + planes[i].distance * planes[i].normal;
                Vector3 p4 = position + tangent1 * planeDrawSize - tangent2 * planeDrawSize + planes[i].distance * planes[i].normal;

                Debug.DrawLine(p1, p2, Color.green);
                Debug.DrawLine(p2, p3, Color.green);
                Debug.DrawLine(p3, p4, Color.green);
                Debug.DrawLine(p4, p1, Color.green);
            }*/
        }

        private void FindPoints(GameObject theObject, Vec3[] objectPoints)
        {
            Mesh objectMesh = theObject.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = objectMesh.vertices;


            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = i + 1; j < vertices.Length; j++)
                {
                    for (int k = j + 1; k < vertices.Length; k++)
                    {
                        planes.Add(new MyPlane(vertices[i], vertices[j], vertices[k]));
                    }
                }
            }
        }
    }
}