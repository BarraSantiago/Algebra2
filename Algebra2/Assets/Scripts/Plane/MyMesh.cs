using System.Collections;
using CustomMath;
using UnityEngine;

namespace Plane
{
    public class MyMesh : MonoBehaviour
    {
        private Vec3[,,] theMesh;

        [SerializeField] private float delta;
        [SerializeField] private float meshSize;
        [SerializeField] private int axisSteps;

        [SerializeField] private GameObject pointPrefab;

        [SerializeField] private ParticleSystem particleSystemPrefab;
        [SerializeField] private int batchSize = 5;

        private void Start()
        {
            axisSteps = Mathf.RoundToInt(meshSize / delta);
            theMesh = new Vec3[axisSteps, axisSteps, axisSteps];
            CreateMesh();
            //DrawMesh();
            //DrawMeshParticles();
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

        private void OnDrawGizmos()
        {
            for (int x = 0; x < axisSteps; x++)
            {
                for (int y = 0; y < axisSteps; y++)
                {
                    for (int z = 0; z < axisSteps; z++)
                    {
                        // Gizmos.DrawSphere(theMesh[x, y, z], 0.01f);
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

        private void DrawMeshParticles()
        {
            // Instantiate the particle system prefab
            ParticleSystem ps = Instantiate(particleSystemPrefab);

            // Set the number of particles to the number of theMesh
            var main = ps.main;
            main.maxParticles = theMesh.Length;

            // Create an array to hold the particle data
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[theMesh.Length];

            // Set the position of each particle to a point in the list
            int i = 0;
            foreach (Vec3 vector in theMesh)
            {
                particles[i].position = vector;
                particles[i].startSize = 0.1f; // Set the size of the particle
                particles[i].startColor = Color.white; // Set the color of the particle
                i++;
            }


            // Set the particles for the particle system
            ps.SetParticles(particles, particles.Length);
        }
    }
}