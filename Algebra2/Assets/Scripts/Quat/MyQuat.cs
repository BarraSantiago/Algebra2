using System;
using CustomMath;
using UnityEngine;
using UnityEngine.Internal;

namespace Quat
{
    public struct MyQuat
    {
        #region Variables

        // Un cuaternion es una extension de los numeros complejos que representa rotaciones en el espacio tridimensional.
        // Esta compuesto por una parte escalar (W) y tres partes vectoriales (X Y Z). Se utilizan para evitar  el bloqueo del gimbal.
        // Pueden sobrepasar el gimbal lock, que ocurre cuando los ejes X y Z estan en paralelo, al representar las rotaciones en un espacio de mayor dimensionalidad (4D).
        // Tambien se usan para hacer operaciones que mantienen la continuidad y suavidad en las rotaciones.
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

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

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    case 3:
                        return W;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    case 3:
                        W = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
        }

        public static MyQuat identity = new MyQuat(0f, 0f, 0f, 1f);

        //TODO agregar comentarios
        /// <summary>
        /// Calcula y devuelve los angulos de Euler de un cuaternion en forma de un Vec3
        /// </summary>
        /// <returns> Transformacion a Euler </returns>
        public Vec3 eulerAngles()
        {
            // Proyectas el vector, usando la pryecsion con la profundidad crea un prisma
            // Calculate the yaw (Z rotation).
            float yaw = Mathf.Atan2(2 * X * Y + 2 * W * Z, 1 - 2 * X * X - 2 * Y * Y);
            // Calculate the pitch (Y rotation).
            float pitch = Mathf.Asin(2 * X * Z - 2 * W * Y);
            // Calculate the roll (X rotation).
            float roll = Mathf.Atan2(2 * Y * Z + 2 * W * X, 1 - 2 * Y * Y - 2 * Z * Z);
            return new Vec3(yaw, pitch, roll);
        }

        /// <summary>
        /// Calcula y devuelve un nuevo cuaternion normalizado (magnitud/longitud = 1) o si la magnitud es 0 devuelve el cuaternion identidad.
        /// </summary>
        /// <returns> Nuevo cuaternion normalizado o la identidad </returns>
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
        /// <param name="a"> Primer cuaternion </param>
        /// <param name="b"> Segundo cuaternion </param>
        /// <returns> Angulo entre el primer y segundo cuaternion en radianes </returns>
        public static float Angle(MyQuat a, MyQuat b)
        {
            // Tratamos a los cuaterniones como vectores y les hacemos el producto escalar para medir la alineacion entre 
            // estos y se toma el valor absoluto para, principalmente, devolver un valor positivo
            float absDotProduct = Mathf.Abs(Dot(a, b));
            // Como el producto escalar esta directamente relacionado con el coseno del angulo
            // al aplicar el arcocoseo nos devuelve el angulo en radianes.
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
            return AxisAngle(axis, angle);
        }

        /// <summary>
        /// Crea un cuaternion que representa la rotacion alrededor del eje especificado y con el angulo especificado.
        /// </summary>
        /// <param name="axis"> Eje de rotacion (debe estar normalizado) </param>
        /// <param name="angle"> Angulo que se quiere rotar </param>
        /// <returns> Cuaternion con la rotacion requerida </returns>
        public static MyQuat AxisAngle(Vec3 axis, float angle)
        {
            // Calcula la mitad del angulo
            float halfAngle = angle * 0.5f;
            // Calcula el seno del angulo
            float sinHalfAngle = Mathf.Sin(halfAngle);

            // Se multiplican las componentes por el seno del angulo medio permitiendo que el cuaternion represente
            // una rotacion alrededor del eje especificado.
            MyQuat newQuat = new MyQuat();
            newQuat.X = axis.x * sinHalfAngle;
            newQuat.Y = axis.y * sinHalfAngle;
            newQuat.Z = axis.z * sinHalfAngle;
            newQuat.W = Mathf.Cos(halfAngle);

            return newQuat;
        }

        /// <summary>
        /// Calcula el producto escalar entre dos cuaterniones, lo cual permite medir la similitud o alineacion entre
        /// ellos. Un valor mas alto indica una mayor similitud o alineacion.
        /// </summary>
        /// <param name="a"> Primer cuaternion </param>
        /// <param name="b"> Segundo cuaternion </param>
        /// <returns> Producto escalar entre los dos cuaterniones </returns>
        static float Dot(MyQuat a, MyQuat b)
        {
            // Calcula el producto escalar entre los cuaterniones
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
        }

        /// <summary>
        /// Toma un Vec3 que contiene angulos de Euler en grados y devuelve un cuaternion que representa la misma rotacion.
        /// </summary>
        /// <param name="euler"> Vec3 en euler para ser pasado a cuaternion </param>
        /// <returns> Cuaternion basado en el Vec3 </returns>
        public static MyQuat Euler(Vec3 euler)
        {
            return Euler(euler.x, euler.y, euler.z);
        }

        /// <summary>
        /// Toma 3 floats que representan un Vec3 de Euler en grados y devuelve un cuaternion que representa la misma rotacion.
        /// </summary>
        /// <param name="x"> Componente X del vector </param>
        /// <param name="y"> Componente Y del vector </param>
        /// <param name="z"> Componente Z del vector </param>
        /// <returns> Retorna el vec3 convertido a cuaternion </returns>
        public static MyQuat Euler(float x, float y, float z)
        {
            // Convertir los angulos de Euler de grados a radianes
            float yaw = y * Mathf.Deg2Rad * 0.5f; // Yaw (rotacion vertical)
            float pitch = x * Mathf.Deg2Rad * 0.5f; // Pitch (rotacion horizontal)
            float roll = z * Mathf.Deg2Rad * 0.5f; // Roll (rotacion de profundidad)

            // Calcular los valores trigonometricos de los angulos de Euler
            float cosYaw = Mathf.Cos(yaw);
            float sinYaw = Mathf.Sin(yaw);
            float cosPitch = Mathf.Cos(pitch);
            float sinPitch = Mathf.Sin(pitch);
            float cosRoll = Mathf.Cos(roll);
            float sinRoll = Mathf.Sin(roll);

            MyQuat newQuat = new MyQuat();
            
            // Construir el cuaternion utilizando los valores calculados
            // Calculos necesarios para convertir de Euler a cuaternion
            // TODO agregar mas comentarios
            newQuat.W = cosYaw * cosPitch * cosRoll + sinYaw * sinPitch * sinRoll;
            newQuat.X = cosYaw * cosPitch * sinRoll - sinYaw * sinPitch * cosRoll;
            newQuat.Y = sinYaw * cosPitch * sinRoll + cosYaw * sinPitch * cosRoll;
            newQuat.Z = sinYaw * cosPitch * cosRoll - cosYaw * sinPitch * sinRoll;

            return newQuat;
        }

        /// <summary>
        /// Crea un cuaternion que representa la rotacion necesaria para ir desde una direccion "fromDirection" a la otra "toDirection".
        /// </summary>
        /// <param name="fromDirection"> Direccion inicial </param>
        /// <param name="toDirection"> Direccion final </param>
        /// <returns> Cuaternion que representa la rotacion requerida </returns>
        public static MyQuat FromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            // Normalizar los vectores de direccion
            Vec3 fromNormalized = fromDirection.Normalized;
            Vec3 toNormalized = toDirection.Normalized;

            // Calcular el producto cruzado entre los vectores de direccion normalizados
            Vec3 rotationAxis = Vec3.Cross(fromNormalized, toNormalized);

            // Calcular el producto punto entre los vectores de direccion normalizados
            float dotProduct = Vec3.Dot(fromNormalized, toNormalized);

            // Calcular el angulo entre los vectores de direccion
            float angle = Mathf.Acos(dotProduct);

            // Crear el cuaternión a partir del angulo y el eje de rotacion
            MyQuat rotationQuat = AxisAngle(rotationAxis, angle);

            return rotationQuat;
        }

        /// <summary>
        /// Calcula el cuaternion inverso (misma direccion, pero magnitud invertida) de la rotacion dada.
        /// </summary>
        /// <param name="rotation"> Cuaternion de rotacion </param>
        /// <returns> Cuaternion inverso de la rotacion </returns>
        public static MyQuat Inverse(MyQuat rotation)
        {
            // Calcula el cuadrado de la magnitud del cuaternion. Se usa la magnitud al cuadrado debido a que salteas el
            // paso de hacer la raiz cuadrada (que es una operacion costosa.
            float magnitudeSquared = rotation.X * rotation.X + rotation.Y * rotation.Y + rotation.Z * rotation.Z +
                                     rotation.W * rotation.W;

            if (magnitudeSquared == 0)
            {
                return identity;
            }

            // Crea un nuevo cuaternion inverso a partir de las componentes negativas y la magnitud al cuadrado
            return new MyQuat(-rotation.X / magnitudeSquared, -rotation.Y / magnitudeSquared,
                -rotation.Z / magnitudeSquared, rotation.W / magnitudeSquared);
        }

        /// <summary>
        /// Interpola linealmente entre dos cuaterniones con restricciones de limite. Considerando una linea recta entre
        /// el primer y el segundo cuaternion, te devuelve un valor en la linea relativo a t.
        /// </summary>
        /// <param name="a"> Primer cuaternion </param>
        /// <param name="b"> Segundo cuaternion </param>
        /// <param name="t"> Valor a interpolar (entre 0 y 1) </param>
        /// <returns> Cuaternion interpolado normalizado </returns>
        public static MyQuat Lerp(MyQuat a, MyQuat b, float t)
        {
            // Restringe el valor de interpolacion dentro del rango de 0 a 1
            float tClamped = Mathf.Max(0f, Mathf.Min(1f, t));

            // Realiza la interpolacion lineal utilizando LerpUnclamped
            return LerpUnclamped(a, b, tClamped);
        }

        /// <summary>
        /// Interpola linealmente entre dos cuaterniones sin restricciones de limite.
        /// </summary>
        /// <param name="a"> Primer cuaternion </param>
        /// <param name="b"> Segundo cuaternion </param>
        /// <param name="t"> Valor de interpolacion </param>
        /// <returns> Cuaternion interpolado normalizado </returns>
        public static MyQuat LerpUnclamped(MyQuat a, MyQuat b, float t)
        {
            // Interpola cada componente del cuaternion por separado
            float lerpX = a.X + (b.X - a.X) * t;
            float lerpY = a.Y + (b.Y - a.Y) * t;
            float lerpZ = a.Z + (b.Z - a.Z) * t;
            float lerpW = a.W + (b.W - a.W) * t;

            return new MyQuat(lerpX, lerpY, lerpZ, lerpW).normalized();
        }

        /// <summary>
        /// Transforma un vector director en una rotación que tenga su eje z alineado con “forward”.
        /// </summary>
        /// <param name="forward"> Direccion hacia la cual se desea mirar </param>
        /// <returns> Cuaternion de rotacion para mirar hacia la direccion especificada </returns>
        public static MyQuat LookRotation(Vec3 forward)
        {
            return LookRotation(forward, Vec3.Up);
        }

        /// <summary>
        /// Crea un cuaternion que representa una rotacion para mirar hacia la direccion especificada, con un vector de direccion upwards opcional.
        /// </summary>
        /// <param name="forward"> Direccion hacia la cual se desea mirar </param>
        /// <param name="upwards"> Direccion upwards relativa </param>
        /// <returns> Cuaternion de rotacion para mirar hacia la direccion especificada con la direccion upwards relativa </returns>
        public static MyQuat LookRotation(Vec3 forward, [DefaultValue("Vec3.Up")] Vec3 upwards)
        {
            // Normaliza el vector de direccion hacia adelante
            Vec3 newForward = forward.Normalized;

            // Normaliza el vector de direccion upwards relativa
            Vec3 newUpwards = upwards.Normalized;

            // Calcula el producto cruz entre el vector de direccion hacia adelante predeterminado y el vector de direccion hacia adelante nuevo
            // Esto nos da el eje de rotacion necesario para alinear los vectores hacia adelante predeterminado y nuevo en la funcion
            Vec3 right = Vec3.Cross(newForward, newUpwards);

            // Recalcula el vector upwards utilizando el producto cruz entre el vector de direccion hacia adelante y el vector hacia la derecha
            Vec3 recalculatedUpwards = Vec3.Cross(right, newForward);

            // Normaliza el vector upwards recalculado para asegurarse de que tenga una longitud/magnitud de 1
            Vec3 normalizedUpwards = recalculatedUpwards.Normalized;

            // Calcula el angulo de rotacion entre el vector upwards recalculado y el vector upwards original
            float rotationAngle = Vec3.Angle(newUpwards, normalizedUpwards);

            // Calcula el eje de rotacion utilizando el producto cruz entre el vector upwards recalculado y el vector upwards original
            Vec3 rotationAxis = Vec3.Cross(newUpwards, normalizedUpwards);

            // Crea un cuaternion que representa la rotacion utilizando el eje de rotacion y el angulo calculados
            MyQuat rotationQuat = AxisAngle(rotationAxis, rotationAngle);

            return rotationQuat;
        }

        /// <summary>
        /// Modifica al cuaternion, normalizandolo (haciendo que tenga una longitud/magnitud de 1).
        /// </summary>
        public void Normalize()
        {
            Set(normalized());
        }

        /// <summary>
        /// Normaliza el cuaternion especificado, asegurando que tenga una longitud/magnitud de 1.
        /// </summary>
        /// <param name="quat"> El cuaternion a normalizar </param>
        /// <returns> El cuaternion normalizado </returns>
        public static MyQuat Normalize(MyQuat quat)
        {
            return quat.normalized();
        }

        /// <summary>
        /// Rota gradualmente desde un cuaternion inicial hacia un cuaternion objetivo, limitando el angulo maximo de rotacion.
        /// </summary>
        /// <param name="from"> Cuaternion inicial </param>
        /// <param name="to"> Cuaternion final </param>
        /// <param name="maxDegreesDelta"> El angulo maximo de rotacion en grados </param>
        /// <returns> El cuaternion resultante despues de la rotacion </returns>
        public static MyQuat RotateTowards(MyQuat from, MyQuat to, float maxDegreesDelta)
        {
            // Convierte el angulo maximo de rotacion de grados a radianes
            float maxRadiansDelta = maxDegreesDelta * Mathf.Deg2Rad;

            // Calcula el angulo entre los cuaterniones de inicio y fin
            float angle = Angle(from, to);

            // Limita el angulo a rotar segun el angulo maximo de rotacion
            float t = Mathf.Min(1f, maxRadiansDelta / angle);

            // Interpola linealmente entre los cuaterniones de inicio y objetivo segun el factor de interpolacion t
            MyQuat result = SlerpUnclamped(from, to, t);

            // Normaliza el cuaternion resultante para asegurar que tenga una longitud/magnitud de 1
            return result.normalized();
        }

        /// <summary>
        /// Realiza una interpolacion esferica (Hiperbola) entre dos cuaterniones con un factor de interpolacion restringido.
        /// </summary>
        /// <param name="a"> Cuaternion inicial </param>
        /// <param name="b"> Cuaternion final </param>
        /// <param name="t"> El factor de interpolacion restringido </param>
        /// <returns> El cuaternion resultante despues de la interpolacion </returns>
        public static MyQuat Slerp(MyQuat a, MyQuat b, float t)
        {
            // Restringe el factor de interpolacion entre 0 y 1 para asegurarse de que este dentro del rango valido
            float tClamped = Mathf.Max(0f, Mathf.Min(1f, t));

            // Realiza la interpolacion esferica utilizando el factor de interpolacion clamped
            return SlerpUnclamped(a, b, tClamped);
        }


        /// <summary>
        /// Realiza una interpolacion esferica sin restricciones entre dos cuaterniones.
        /// </summary>
        /// <param name="a"> El cuaternion de inicio </param>
        /// <param name="b"> El cuaternion final </param>
        /// <param name="t"> El factor de interpolacion </param>
        /// <returns> El cuaternion resultante despues de la interpolacion </returns>
        public static MyQuat SlerpUnclamped(MyQuat a, MyQuat b, float t)
        {
            // Calcula el producto punto entre los cuaterniones de inicio y objetivo
            float dotProduct = Dot(a, b);

            // Si el producto punto es negativo, invierte uno de los cuaterniones
            if (dotProduct < 0f)
            {
                b = Inverse(b);
                dotProduct = -dotProduct;
            }

            // Define el umbral de diferencia angular para utilizar la interpolacion lineal en lugar de la interpolacion esferica
            const float threshold = 0.99995f;

            // Si los cuaterniones son casi colineales, utiliza la interpolacion lineal
            MyQuat result;
            if (dotProduct > threshold)
            {
                // Esto es mas eficiente que "return LerpUnclamped(a, b, t).normalized();". Debido a que en esta manera se
                // crean 2 cuanternione diferentes, uno en LerpUnclamped y otro en normalized.
                result = LerpUnclamped(a, b, t);
                result.Normalize();
                return result;
            }

            // Calcula el angulo entre los cuaterniones inicial y final
            float angle = Angle(a, b);

            // Calcula los factores de interpolacion para los cuaterniones
            // A medida que t aumenta, factorA disminuye gradualmente, lo que permite que el cuaternion b tenga mas influencia en la interpolacion final.
            float factorA = Mathf.Sin((1f - t) * angle);
            float factorB = Mathf.Sin(t * angle);

            // Realiza la interpolacion esferica utilizando los factores de interpolacion
            result = new MyQuat(
                factorA * a.X + factorB * b.X,
                factorA * a.Y + factorB * b.Y,
                factorA * a.Z + factorB * b.Z,
                factorA * a.W + factorB * b.W
            );

            // Normaliza el cuaternion resultante para asegurar que tenga una longitud/magnitud de 1
            result.Normalize();

            return result;
        }


        /// <summary>
        /// Modifica los componentes del cuaternion con los nuevos valores especificados.
        /// </summary>
        /// <param name="newX"> El nuevo valor de X </param>
        /// <param name="newY"> El nuevo valor de Y </param>
        /// <param name="newZ"> El nuevo valor de Z </param>
        /// <param name="newW"> El nuevo valor de W </param>
        void Set(float newX, float newY, float newZ, float newW)
        {
            X = newX;
            Y = newY;
            Z = newZ;
            W = newW;
        }

        /// <summary>
        /// Modifica los componentes del cuaternion con los nuevos valores especificados.
        /// </summary>
        /// <param name="a"> Valores a copiar </param>
        void Set(MyQuat a)
        {
            Set(a.X, a.Y, a.Z, a.W);
        }

        /// <summary>
        /// Establece el cuaternion para representar una rotacion desde una direccion inicial a una direccion final.
        /// </summary>
        /// <param name="fromDirection"> La direccion inicial </param>
        /// <param name="toDirection"> La direccion final </param>
        public void SetFromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            Set(FromToRotation(fromDirection, toDirection));
        }


        /// <summary>
        /// Establece el cuaternion para que apunte en la direccion especificada por el vector view.
        /// </summary>
        /// <param name="view"> Vector de direccion hacia el cual se quiere apuntar </param>
        public void SetLookRotation(Vec3 view)
        {
            Set(LookRotation(view));
        }

        /// <summary>
        /// Crea un cuaternion que representa una rotacion para mirar hacia la direccion especificada, con un vector de direccion upwards opcional.
        /// </summary>
        /// <param name="view"> Direccion hacia la cual se desea mirar </param>
        /// <param name="up"> Direccion upwards relativa </param>
        public void SetLookRotation(Vec3 view, [DefaultValue("Vec3.up")] Vec3 up)
        {
            Set(LookRotation(view, up));
        }

        /// <summary>
        /// Obtiene el angulo y el eje de rotacion representados por el cuaternion.
        /// </summary>
        /// <param name="angle"> Variable de salida para almacenar el angulo de rotacion en radianes </param>
        /// <param name="axis"> Variable de salida para almacenar el eje de rotacion como un vector de direccion </param>
        public void ToAngleAxis(out float angle, out Vec3 axis)
        {
            // Calcula el angulo de rotacion
            angle = 2 * Mathf.Acos(W);

            // Calcula el factor de escala para obtener los componentes del eje de rotacion
            float scale = Mathf.Sqrt(1 - W * W);

            // Si el cuaternion es casi un cuaternion de identidad, establece el eje de rotacion como el vector "up" predeterminado
            if (scale < 0.001f)
            {
                axis = Vec3.Up;
            }
            else
            {
                // Calcula los componentes del eje de rotación dividiendo los componentes X, Y y Z del cuaternión por el factor de escala
                axis = new Vec3(X / scale, Y / scale, Z / scale);
            }
        }


        /// <summary>
        /// Realiza la multiplicacion de un cuaternion por un vector.
        /// </summary>
        /// <param name="rotation"> El cuaternion a multiplicar </param>
        /// <param name="point"> El vector a multiplicar </param>
        /// <returns> El resultado de la multiplicacion del cuaternion por el vector </returns>
        public static Vec3 operator *(MyQuat rotation, Vec3 point)
        {
            // Realiza la multiplicación del cuaternión con el vector
            float num1 = rotation.Y * point.z - rotation.Z * point.y;
            float num2 = rotation.Z * point.x - rotation.X * point.z;
            float num3 = rotation.X * point.y - rotation.Y * point.x;
            float num4 = rotation.X * point.x + rotation.Y * point.y + rotation.Z * point.z;

            // Calcula el vector resultante
            float resultX = (num4 * rotation.X + num1 * rotation.W) + (num3 * rotation.Z - num2 * rotation.Y);
            float resultY = (num4 * rotation.Y + num2 * rotation.W) + (num1 * rotation.X - num3 * rotation.Z);
            float resultZ = (num4 * rotation.Z + num3 * rotation.W) + (num2 * rotation.Y - num1 * rotation.X);

            // Crea y devuelve un nuevo vector con los componentes resultantes
            return new Vec3(resultX, resultY, resultZ);
        }

        // Multiplicacion matricial no conmutativa. 
        // La multiplicacion de cuaterniones se utiliza para representar rotaciones en el espacio tridimensional.
        // La combinacion de dos rotaciones mediante la multiplicación de cuaterniones es equivalente a aplicar ambas rotaciones en secuencia.
        /// <summary>
        /// Realiza la multiplicacion de dos cuaterniones.
        /// </summary>
        /// <param name="lhs"> Primer cuaternion </param>
        /// <param name="rhs"> Segundo cuaternion </param>
        /// <returns> El resultado de la multiplicacion de los dos cuaterniones </returns>
        public static MyQuat operator *(MyQuat lhs, MyQuat rhs)
        {
            MyQuat quat = new MyQuat();
            quat.W = (lhs.W * rhs.W - lhs.X * rhs.X - lhs.Y * rhs.Y - lhs.Z * rhs.Z);
            quat.X = (lhs.W * rhs.X + lhs.X * rhs.W + lhs.Y * rhs.Z - lhs.Z * rhs.Y);
            quat.Y = (lhs.Y * rhs.W + lhs.W * rhs.Y + lhs.Z * rhs.X - lhs.X * rhs.Z);
            quat.Z = (lhs.Z * rhs.W + lhs.W * rhs.Z - lhs.Y * rhs.X + lhs.X * rhs.Y);

            return quat;
        }

        /// <summary>
        /// Compara dos cuaterniones para determinar si son iguales.
        /// </summary>
        /// <param name="lhs"> El primer cuaternion </param>
        /// <param name="rhs"> El segundo cuaternion </param>
        /// <returns> True si los cuaterniones son iguales, False de lo contrario </returns>
        public static bool operator ==(MyQuat lhs, MyQuat rhs)
        {
            // Define un valor epsilon para comparar valores de punto flotante
            float epsilon = 1e-12f;

            // Compara los componentes individuales de los cuaterniones con una tolerancia de epsilon
            return Mathf.Abs(lhs.X - rhs.X) < epsilon &&
                   Mathf.Abs(lhs.Y - rhs.Y) < epsilon &&
                   Mathf.Abs(lhs.Z - rhs.Z) < epsilon &&
                   Mathf.Abs(lhs.W - rhs.W) < epsilon;
        }

        /// <summary>
        /// Compara dos cuaterniones para determinar si son diferentes.
        /// </summary>
        /// <param name="lhs"> Primer cuaternion </param>
        /// <param name="rhs"> Segundo cuaternion </param>
        /// <returns> True si los cuaterniones son diferentes, False de lo contrario </returns>
        public static bool operator !=(MyQuat lhs, MyQuat rhs)
        {
            // Define un valor epsilon para comparar valores de punto flotante
            float epsilon = 1e-05f;

            // Compara los componentes individuales de los cuaterniones con una tolerancia de epsilon
            return !(Mathf.Abs(lhs.X - rhs.X) < epsilon &&
                     Mathf.Abs(lhs.Y - rhs.Y) < epsilon &&
                     Mathf.Abs(lhs.Z - rhs.Z) < epsilon &&
                     Mathf.Abs(lhs.W - rhs.W) < epsilon);
        }
    }
}