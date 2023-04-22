using System;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

namespace Plane
{
    public class MyMesh : MonoBehaviour
    {
        private List<Vec3> theMesh = new List<Vec3>();
        private Vec3[,,] theMesh2;

        public float delta;
        public float meshSize;
        public int axisSteps;

        public GameObject pointPrefab;

        public ParticleSystem particleSystemPrefab;
        
        private void Start()
        {
            axisSteps = Mathf.RoundToInt(meshSize / delta);
            theMesh2 = new Vec3[axisSteps, axisSteps, axisSteps];
            CreateMesh();
            DrawMesh();
            DrawMeshParticles();
        }

        private void OnDrawGizmos()
        {
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
                        theMesh2[x, y, z] = point;
                        //theMesh.Add(point);
                    }
                }
            }
        }


        private void DrawMesh()
        {
            foreach (Vec3 vector in theMesh2)
            {
                Instantiate(pointPrefab, vector, Quaternion.identity);
            }
        }

        private void DrawMeshParticles()
        {
            // Instantiate the particle system prefab
            ParticleSystem ps = Instantiate(particleSystemPrefab);

            // Set the number of particles to the number of theMesh
            var main = ps.main;
            main.maxParticles = theMesh2.Length;

            // Create an array to hold the particle data
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[theMesh2.Length];

            // Set the position of each particle to a point in the list
            int i = 0;
            foreach (Vec3 vector in theMesh2)
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