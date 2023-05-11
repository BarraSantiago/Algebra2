using System.Collections;
using CustomMath;
using UnityEngine;

namespace Plane
{
    public class MyMesh : MonoBehaviour
    {
        private Vec3[,,] theMesh;

        [Header("Mesh Configuration")]
        [SerializeField] private float delta;
        [SerializeField] private float meshSize;
        [SerializeField] private int axisSteps;

        
        [Header("Points Print Configuration")]
        [SerializeField] private int batchSize = 5;
        [SerializeField] private GameObject pointPrefab;

        private void Start()
        {
            axisSteps = Mathf.RoundToInt(meshSize / delta);
            theMesh = new Vec3[axisSteps, axisSteps, axisSteps];
            CreateMesh();
            DrawMesh();
        }

        public Vec3[,,] GetMesh()
        {
            return theMesh;
        }

        public int GetAxisSteps()
        {
            return axisSteps;
        }

        private void CreateMesh()
        {
            for (int x = 0; x < axisSteps; x++)
            {
                for (int y = 0; y < axisSteps; y++)
                {
                    for (int z = 0; z < axisSteps; z++)
                    {
                        float xCoord = x * delta;
                        float yCoord = y * delta;
                        float zCoord = z * delta;
                        Vec3 point = new Vec3(xCoord, yCoord, zCoord);
                        theMesh[x, y, z] = point;
                    }
                }
            }
        }

        private void DrawMesh()
        {
            StartCoroutine(DrawMeshCoroutine());
        }

        private IEnumerator DrawMeshCoroutine()
        {
            for (int x = 0; x < axisSteps; x++)
            {
                for (int y = 0; y < axisSteps; y++)
                {
                    for (int z = 0; z < axisSteps; z++)
                    {
                        if (z % batchSize == 0 && z > 0)
                        {
                            yield return null;
                        }

                        Instantiate(pointPrefab, theMesh[x, y, z], Quaternion.identity);
                    }
                }
            }
        }
    }
}