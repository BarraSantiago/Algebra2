using System;
using CustomMath;

namespace Plane
{
    // Objeto matematico que esta compuesto por una susecion infinita de rectas que divide el espacio en 2
    // Esta definido por un punto y una normal o 3 puntos no colineales
    public class MyPlane : IEquatable<MyPlane>
    {
        // Normal del plano, es un vector perpendicular al plano
        // La normal se usa para determinar la orientación del plano y calcular distancias
        private Vec3 mNormal;

        // La distancia en un plano representa la distancia perpendicular más corta desde el origen (0,0,0) hasta el plano
        // Osea, es la distancia que hay que recorrer a lo largo de la normal del plano para llegar al plano desde el origen
        private float mDistance;

        public Vec3 Normal
        {
            get => mNormal;
            set => mNormal = value;
        }


        public float Distance
        {
            get => mDistance;
            set => mDistance = value;
        }


        public MyPlane(Vec3 inNormal, Vec3 inPoint)
        {
            mNormal = inNormal.Normalized;
            // Calcula la distancia del plano al punto dado usando el producto escalar
            // La distancia es la proyección del punto sobre la normal del plano
            // La normal del plano es un vector unitario, por lo que la distancia se calcula
            // como el producto escalar de la normal y el punto, más la distancia del plano
            // La distancia se guarda como un valor negativo porque la ecuación del plano es Normal · Point + Distance = 0
            // Por lo tanto, para que el punto esté en el plano, la normal del plano debe ser perpendicular al vector que va desde el origen al punto
            mDistance = -Vec3.Dot(mNormal, inPoint);
        }


        public MyPlane(Vec3 inNormal, float d)
        {
            mNormal = inNormal.Normalized;
            mDistance = d;
        }

        public MyPlane(Vec3 a, Vec3 b, Vec3 c)
        {
            // Calcula la normal del plano usando el producto cruzado de dos vectores formados por los puntos a, b y c
            // El producto cruzado de dos vectores da un vector perpendicular a ambos, que es la normal del plano definido por esos puntos
            mNormal = Vec3.Cross(b - a, c - a).Normalized;

            // calcula la proyección del punto a sobre la normal
            // Esta proyección representa qué tan "lejos" está el punto del origen en la dirección de la normal
            // Esta negativo porque esta es la ecuacion del plano, teniendo la normal y un punto se consigue la distancia Normal · Point + Distance = 0
            mDistance = -Vec3.Dot(mNormal, a);
        }

        public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
        {
            mNormal = inNormal.Normalized;
            mDistance = -Vec3.Dot(inNormal, inPoint);
        }

        public void Set3Points(Vec3 a, Vec3 b, Vec3 c)
        {
            mNormal = Vec3.Cross(b - a, c - a);
            mNormal = mNormal.Normalized;
            mDistance = -Vec3.Dot(mNormal, a);
        }

        public void Flip()
        {
            mNormal = -mNormal;
            mDistance = -mDistance;
        }

        public MyPlane Flipped()
        {
            return new MyPlane(-Normal, -Distance);
        }

        public void Translate(Vec3 translation)
        {
            mDistance += Vec3.Dot(mNormal, translation);
        }

        public static MyPlane Translate(MyPlane plane, Vec3 translation)
        {
            plane.Translate(translation);
            return plane;
        }

        public Vec3 ClosestPointOnPlane(Vec3 point)
        {
            // Calcula la distancia del punto al plano con el producto escalar
            // La distancia es la proyección del punto sobre la normal del plano
            // Si la normal es (nx, ny, nz) y el punto es (px, py, pz), la distancia se calcula como:
            // dist = nx * px + ny * py + nz * pz + d
            // La proyección del punto sobre la normal del plano se obtiene restando la normal escalada por la distancia del punto al plano
            // La normal del plano es un vector unitario
            // Por lo tanto, la distancia al plano es el producto escalar de la normal
            float dist = GetDistanceToPoint(point);

            // Resta la normal del plano escalada por la distancia al punto
            // Esto da como resultado el punto más cercano en el plano al punto dado
            // Si la distancia es positiva, el punto está por encima del plano, si es negativa, está por debajo del plano
            // Si la distancia es cero, el punto está en el plano
            // Calcula un vector que apunta desde el punto al plano dando el punto más cercano en el plano
            return point - mNormal * dist;
        }

        public float GetDistanceToPoint(Vec3 point)
        {
            // Calcula la distancia del punto al plano usando el producto escalar
            // La distancia es la proyección del punto sobre la normal del plano
            return Vec3.Dot(mNormal, point) + mDistance;
        }

        public bool GetSide(Vec3 point)
        {
            // Determina en qué lado del plano se encuentra el punto
            // Si el producto escalar del punto con la normal del plano más la distancia es mayor
            // que cero, el punto está en el lado positivo del plano, de lo contrario,
            // está en el lado negativo o sobre el plano
            return GetDistanceToPoint(point) > 0;
        }

        public bool SameSide(Vec3 pointA, Vec3 pointB)
        {
            // Determina si dos puntos están en el mismo lado del plano
            bool aSide = GetSide(pointA);
            bool bSide = GetSide(pointB);
            return (aSide && bSide) || (!aSide && !bSide);
        }

        public bool Equals(MyPlane other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return mNormal.Equals(other.mNormal) && mDistance.Equals(other.mDistance);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MyPlane)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(mNormal, mDistance);
        }
    }
}