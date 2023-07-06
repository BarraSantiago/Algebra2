using System;
using CustomMath;
using UnityEngine;
using UnityEngine.Internal;

namespace Quat
{
    public struct MyQuat
    {
        #region Variables

        private float X { get; set; }
        private float Y { get; set; }
        private float Z { get; set; }
        private float W { get; set; }

        #endregion

        #region Constructor

        public MyQuat(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        #endregion

        //this[int index]

        public static MyQuat identity = new MyQuat(0f, 0f, 0f, 1f);

        /// <summary>
        /// Calcula y devuelve los angulos de Euler de un cuaternion en forma de un Vec3
        /// </summary>
        /// <returns></returns>
        public Vec3 eulerAngles()
        {
            // Calculate the yaw (Z rotation).
            float yaw = Mathf.Atan2(2 * X * Y + 2 * W * Z, 1 - 2 * X * X - 2 * Y * Y);
            // Calculate the pitch (Y rotation).
            float pitch = Mathf.Asin(2 * X * Z - 2 * W * Y);
            // Calculate the roll (X rotation).
            float roll = Mathf.Atan2(2 * Y * Z + 2 * W * X, 1 - 2 * Y * Y - 2 * Z * Z);
            // Return the Euler angles.
            return new Vec3(yaw, pitch, roll);
        }

        /// <summary>
        /// Calcula y devuelve un nuevo cuaternion normalizado (magnitud/longitud = 1) o si la magnitud es 0 devuelve el cuaternion identidad 
        /// </summary>
        /// <returns> nuevo cuaternion normalizado o la identidad </returns>
        public MyQuat normalized()
        {
            float magnitude = Mathf.Sqrt(X * X + Y * Y + Z * Z + W * W);

            if (magnitude > 0)
            {
                // Divide cada componente por la magnitud para normalizar el cuaternión
                return new MyQuat(X / magnitude, Y / magnitude, Z / magnitude, W / magnitude);
            }

            // Si la magnitud es zero, retorna el cuaternion identidad
            return identity;
        }

        /// <summary>
        /// Calcula el angulo en radianes entre dos cuaterniones
        /// </summary>
        /// <param name="a"> Cuaternion 1 </param>
        /// <param name="b"> Cuaternion 2 </param>
        /// <returns> angulo entre a y b en radianes </returns>
        public static float Angle(MyQuat a, MyQuat b)
        {
            // Tratamos a los cuaterniones como vectores y les hacemos el producto escalar para medir la alineacion entre 
            // estos y se toma el valor absoluto para, principalmente, devolver un valor positivo
            float absDotProduct = Mathf.Abs(Dot(a, b));
            // Como el producto escalar esta directamente relacionado con el coseno del angulo
            // al aplicar el arcocoseo nos devuelve el angulo en sí en radianes.
            return Mathf.Acos(absDotProduct);
        }

        /// <summary>
        /// Crea un cuaternion que representa la rotacion alrededor del eje especificado y con el angulo especificado.
        /// </summary>
        /// <param name="angle"> Angulo que se quiere rotar </param>
        /// <param name="axis"> Eje de rotacion (debe estar normalizado) </param>
        /// <returns> cuaternion con la rotacion requerida </returns>
        public static MyQuat AngleAxis(float angle, Vec3 axis)
        {
            // Calcula la mitad del angulo
            float halfAngle = angle * 0.5f;

            // Calcula el seno del angulo
            float sinHalfAngle = Mathf.Sin(halfAngle);

            MyQuat newQuat = new MyQuat();
            // Se multiplican las componentes por el seno del ángulo medio permitiendo que el cuaternion represente
            // una rotacion alrededor del eje especificado.
            newQuat.X = axis.x * sinHalfAngle;
            newQuat.Y = axis.y * sinHalfAngle;
            newQuat.Z = axis.z * sinHalfAngle;
            newQuat.W = Mathf.Cos(halfAngle);

            return newQuat;
        }

        /// <summary>
        /// Crea un cuaternion que representa la rotacion alrededor del eje especificado y con el angulo especificado.
        /// </summary>
        /// <param name="axis"> Eje de rotacion (debe estar normalizado) </param>
        /// <param name="angle"> Angulo que se quiere rotar </param>
        /// <returns> cuaternion con la rotacion requerida </returns>
        public static MyQuat AxisAngle(Vec3 axis, float angle)
        {
            // Calcula el ángulo medio y el seno del ángulo medio
            float halfAngle = angle * 0.5f;
            float sinHalfAngle = Mathf.Sin(halfAngle);

            // Crea los componentes del cuaternión
            MyQuat quaternion = new MyQuat();
            quaternion.X = axis.x * sinHalfAngle;
            quaternion.Y = axis.y * sinHalfAngle;
            quaternion.Z = axis.z * sinHalfAngle;
            quaternion.W = Mathf.Cos(halfAngle);

            return quaternion;
        }

        /// <summary>
        /// Calcula el producto escalar entre dos cuaterniones, lo cual permite medir la similitud o alineacion entre
        /// ellos. Un valor mas alto indica una mayor similitud o alineacion.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns> similitud o alineacion entre cuanternion a y b </returns>
        static float Dot(MyQuat a, MyQuat b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        /// <summary>
        /// Toma un Vec3 que contiene angulos de Euler en grados y devuelve un cuaternion que representa la misma rotación.
        /// </summary>
        /// <param name="euler"> Vec3 en euler para ser pasado a cuaternion </param>
        /// <returns> Cuaternion basado en el Vec3 </returns>
        public static MyQuat Euler(Vec3 euler)
        {
            // Convertir los angulos de Euler de grados a radianes
            float yaw = euler.y * Mathf.PI / 180f; // Yaw (rotación vertical)
            float pitch = euler.x * Mathf.PI / 180f; // Pitch (rotación horizontal)
            float roll = euler.z * Mathf.PI / 180f; // Roll (rotación de profundidad)

            // Calcular los valores trigonometricos de los ángulos de Euler
            float cosYaw = Mathf.Cos(yaw * 0.5f);
            float sinYaw = Mathf.Sin(yaw * 0.5f);
            float cosPitch = Mathf.Cos(pitch * 0.5f);
            float sinPitch = Mathf.Sin(pitch * 0.5f);
            float cosRoll = Mathf.Cos(roll * 0.5f);
            float sinRoll = Mathf.Sin(roll * 0.5f);

            // Construir el cuaternión utilizando los valores calculados
            MyQuat newQuat = new MyQuat();
            // Calculos necesarios para convertir de euler a cuaternion
            newQuat.W = cosYaw * cosPitch * cosRoll + sinYaw * sinPitch * sinRoll;
            newQuat.X = cosYaw * cosPitch * sinRoll - sinYaw * sinPitch * cosRoll;
            newQuat.Y = sinYaw * cosPitch * sinRoll + cosYaw * sinPitch * cosRoll;
            newQuat.Z = sinYaw * cosPitch * cosRoll - cosYaw * sinPitch * sinRoll;

            return newQuat;
        }
        
        /// <summary>
        /// Toma 3 floats que representan un Vec3 de Euler en grados y devuelve un cuaternion que representa la misma rotación.
        /// </summary>
        /// <param name="x"> Componente X del vector </param>
        /// <param name="y"> Componente Y del vector </param>
        /// <param name="z"> Componente Z del vector </param>
        /// <returns> Retorna el vec3 convertido a cuaternion </returns>
        public static MyQuat Euler(float x, float y, float z)
        {
            // Convertir los angulos de Euler de grados a radianes
            float yaw = y * Mathf.PI / 180f; // Yaw (rotación vertical)
            float pitch = x * Mathf.PI / 180f; // Pitch (rotación horizontal)
            float roll = z * Mathf.PI / 180f; // Roll (rotación de profundidad)

            // Calcular los valores trigonometricos de los ángulos de Euler
            float cosYaw = Mathf.Cos(yaw * 0.5f);
            float sinYaw = Mathf.Sin(yaw * 0.5f);
            float cosPitch = Mathf.Cos(pitch * 0.5f);
            float sinPitch = Mathf.Sin(pitch * 0.5f);
            float cosRoll = Mathf.Cos(roll * 0.5f);
            float sinRoll = Mathf.Sin(roll * 0.5f);

            // Construir el cuaternión utilizando los valores calculados
            MyQuat newQuat = new MyQuat();
            // Calculos necesarios para convertir de euler a cuaternion
            newQuat.W = cosYaw * cosPitch * cosRoll + sinYaw * sinPitch * sinRoll;
            newQuat.X = cosYaw * cosPitch * sinRoll - sinYaw * sinPitch * cosRoll;
            newQuat.Y = sinYaw * cosPitch * sinRoll + cosYaw * sinPitch * cosRoll;
            newQuat.Z = sinYaw * cosPitch * cosRoll - cosYaw * sinPitch * sinRoll;

            return newQuat;
        }

        //que tan desplazado esta del origen
        public static MyQuat FromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            return identity;
        }

        public static MyQuat Inverse(MyQuat rotation)
        {
            //Es necesario calcular la magnitud del cuaternion para despues 
            //no se aplica la raiz cuadrada a la magnitud debido a que esta es suficiente para obtener la normalizacion del cuaternion
            //y realizar la raiz cuadrada puede tener un costo de eficiencia alto
            float magnitudeSquared = rotation.X * rotation.X + rotation.Y * rotation.Y + rotation.Z * rotation.Z +
                                     rotation.W * rotation.W;

            return new MyQuat(-rotation.X / magnitudeSquared, -rotation.Y / magnitudeSquared,
                -rotation.Z / magnitudeSquared, rotation.W / magnitudeSquared);
        }

        public static MyQuat Lerp(MyQuat a, MyQuat b, float t)
        {
            t = Mathf.Max(0f, Mathf.Min(1f, t)); // Clamp t between 0 and 1

            // Calculate the interpolated MyQuat components
            float lerpX = a.X + (b.X - a.X) * t;
            float lerpY = a.Y + (b.Y - a.Y) * t;
            float lerpZ = a.Z + (b.Z - a.Z) * t;
            float lerpW = a.W + (b.W - a.W) * t;

            return new MyQuat(lerpX, lerpY, lerpZ, lerpW);
        }

        public static MyQuat LerpUnclamped(MyQuat a, MyQuat b, float t)
        {
            return identity;
        }

        public static MyQuat LookRotation(Vec3 forward)
        {
            // Crea un vector que representa la nueva dirección hacia adelante
            Vec3 newForward = forward.Normalized;

            // Crea un vector que representa la direccion hacia adelante base
            Vec3 defaultForward = Vec3.Forward;

            // Se calcula el producto cruz para determinar la perpendicular a ambos, dando el eje de rotacion necesario
            // para alinear los vectores hacia adelante predeterminado y nuevo en la función 
            Vec3 rotationAxis = Vec3.Cross(defaultForward, newForward);

            // Calcula el arco coseno para sacar el angulo entre los dos vectores
            // El producto punto entre dos vectores normalizados resulta en el coseno del angulo entre ellos (Acos del cos = angulo)
            float rotationAngle = Mathf.Acos(Vec3.Dot(defaultForward, newForward));

            // Crea un cuaternión que representa la rotación
            MyQuat rotationQuat = AxisAngle(rotationAxis, rotationAngle);

            return rotationQuat;
        }


        public static MyQuat LookRotation(Vec3 forward, [DefaultValue("Vec3.up")] Vec3 upwards)
        {
        }

        public void Normalize()
        {
            float magnitude = Mathf.Sqrt(X * X + Y * Y + Z * Z + W * W);

            X /= magnitude;
            Y /= magnitude;
            Z /= magnitude;
            W /= magnitude;
        }

        public static MyQuat Normalize(MyQuat q)
        {
            float magnitude = Mathf.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W);

            if (magnitude > 0)
            {
                return new MyQuat(q.X / magnitude, q.Y / magnitude, q.Z / magnitude, q.W / magnitude);
            }

            return identity;
        }

        public static MyQuat RotateTowards(MyQuat from, MyQuat to, float maxDegreesDelta)
        {
            return identity;
        }

        public static MyQuat Slerp(MyQuat a, MyQuat b, float t)
        {
            return identity;
        }

        public static MyQuat SlerpUnclamped(MyQuat a, MyQuat b, float t)
        {
            return identity;
        }


        void Set(float newX, float newY, float newZ, float newW)
        {
            X = newX;
            Y = newY;
            Z = newZ;
            W = newW;
        }

        public void SetFromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
        }

        public void SetLookRotation(Vec3 view)
        {
        }

        public void SetLookRotation(Vec3 view, [DefaultValue("Vec3.up")] Vec3 up)
        {
        }

        public void ToAngleAxis(out float angle, out Vec3 axis)
        {
            angle = 0f;
            axis = Vec3.Zero;
        }

        public static Vec3 operator *(MyQuat rotation, Vec3 point)
        {
            // Perform MyQuat multiplication with the vector
            float num1 = rotation.Y * point.z - rotation.Z * point.y;
            float num2 = rotation.Z * point.x - rotation.X * point.z;
            float num3 = rotation.X * point.y - rotation.Y * point.x;
            float num4 = rotation.X * point.x + rotation.Y * point.y + rotation.Z * point.z;

            // Calculate the resulting vector
            float resultX = (num4 * rotation.X + num1 * rotation.W) + (num3 * rotation.Z - num2 * rotation.Y);
            float resultY = (num4 * rotation.Y + num2 * rotation.W) + (num1 * rotation.X - num3 * rotation.Z);
            float resultZ = (num4 * rotation.Z + num3 * rotation.W) + (num2 * rotation.Y - num1 * rotation.X);
            return new Vec3(resultX, resultY, resultZ);
        }

        public static MyQuat operator *(MyQuat lhs, MyQuat rhs)
        {
            MyQuat quat = new MyQuat();
            quat.W = (lhs.W * rhs.W - lhs.X * rhs.X - lhs.Y * rhs.Y - lhs.Z * rhs.Z);
            quat.X = (lhs.W * rhs.X + lhs.X * rhs.W + lhs.Y * rhs.Z - lhs.Z * rhs.Y);
            quat.X = (lhs.Y * rhs.W + lhs.W * rhs.Y + lhs.Z * rhs.X - lhs.X * rhs.Z);
            quat.X = (lhs.Z * rhs.W + lhs.W * rhs.Z - lhs.Y * rhs.X + lhs.X * rhs.Y);

            return quat;
        }

        public static bool operator ==(MyQuat lhs, MyQuat rhs)
        {
            // Definir un valor epsilon para comparar valores de punto flotante
            float epsilon = 1e-12f;

            // Compare los componentes individuales de los cuaterniones con una tolerancia de epsilon
            return Mathf.Abs(lhs.X - rhs.X) < epsilon &&
                   Mathf.Abs(lhs.Y - rhs.Y) < epsilon &&
                   Mathf.Abs(lhs.Z - rhs.Z) < epsilon &&
                   Mathf.Abs(lhs.W - rhs.W) < epsilon;
        }

        public static bool operator !=(MyQuat lhs, MyQuat rhs)
        {
            // Definir un valor epsilon para comparar valores de punto flotante
            float epsilon = 1e-05f;

            // Compare los componentes individuales de los cuaterniones con una tolerancia de epsilon
            return !(Mathf.Abs(lhs.X - rhs.X) < epsilon &&
                     Mathf.Abs(lhs.Y - rhs.Y) < epsilon &&
                     Mathf.Abs(lhs.Z - rhs.Z) < epsilon &&
                     Mathf.Abs(lhs.W - rhs.W) < epsilon);
        }
    }
}