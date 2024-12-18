using System;
using CustomMath;
using UnityEngine;
using UnityEngine.Internal;

namespace Quat
{
    public struct MyQuat
    {
        #region Variables

        
        // Un cuaternion es una extension matematica de numeros complejos que se utiliza para representar rotaciones 3D.
        // En graficos 3D, los cuaterniones son muy utiles porque evitan problemas como el bloqueo de gimbal (gimbal lock).
        // Esta compuesto por una parte escalar (W) y tres partes vectoriales (X Y Z). Se utilizan para evitar el bloqueo de cardan o gimbal.
        // Pueden sobrepasar el gimbal lock, que ocurre cuando los ejes X y Z estan en paralelo.
        // Tambien se usa para hacer operaciones que mantienen la continuidad y suavidad en las rotaciones.
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

        // Matematicamente, cuando hablamos de cuaterniones y transformaciones, el espacio tridimensional se mapea a
        // traves de estas divisiones triangulares. Esto puede ser util al visualizar rotaciones o calcular proyecciones.


        /// <summary>
        /// Calcula y devuelve los angulos de Euler a partir de un cuaternion, retornandolos como un vector (Vec3).
        /// Los angulos de Euler son una representacion alternativa de rotaciones en el espacio tridimensional, usando
        /// rotaciones consecutivas alrededor de los ejes X, Y y Z.
        /// </summary>
        /// <returns> Transformacion a angulos de Euler en un objeto Vec3</returns>
        public Vec3 eulerAngles()
        {
            // Proyectamos el cuaternion sobre los ejes del espacio tridimensional. 
            // Esto puede interpretarse como "descomponer" la orientacion en componentes angulares especificas para cada eje.
            // La idea de proyeccion puede asociarse con la division espacial en triangulos, donde cada cara tiene su influencia en los calculos.

            // Calcular la rotacion en Z (yaw), que corresponde al giro alrededor del eje Z.
            // Matematicamente, se utiliza la funcion atan2 para manejar el angulo correctamente en todos los cuadrantes.
            float yaw = Mathf.Atan2(2 * X * Y + 2 * W * Z, 1 - 2 * X * X - 2 * Y * Y);

            // Calcular la rotacion en Y (pitch), que es el giro alrededor del eje Y.
            // Para este calculo se utiliza la funcion arcsen (Asin), que toma en cuenta la componente perpendicular al plano XZ.
            float pitch = Mathf.Asin(2 * X * Z - 2 * W * Y);

            // Calcular la rotacion en X (roll), que corresponde al giro alrededor del eje X.
            // Nuevamente, usamos atan2 para manejar correctamente el angulo en el espacio tridimensional.
            float roll = Mathf.Atan2(2 * Y * Z + 2 * W * X, 1 - 2 * Y * Y - 2 * Z * Z);

            // Retornamos los angulos calculados como un vector. Este vector representa las rotaciones equivalentes en terminos
            // de angulos de Euler, utiles para interpretaciones como animaciones, fisica 3D o transformaciones visuales.
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
                // Divide cada componente por la magnitud para normalizar el cuaternion
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
        /// Las rotaciones de Euler se especifican usualmente en el orden (pitch, yaw, roll) o (x, y, z) dependiendo del contexto.
        /// </summary>
        /// <param name="x"> Componente X del vector (pitch) </param>
        /// <param name="y"> Componente Y del vector (yaw) </param>
        /// <param name="z"> Componente Z del vector (roll) </param>
        /// <returns> Retorna un cuaternion que representa la rotacion especificada por los angulos Euler</returns>
        public static MyQuat Euler(float x, float y, float z)
        {
            // Primero se convierten los angulos de Euler (en grados) a radianes.
            // Los cuaterniones se basan en rotaciones continuas, y las funciones trigonometricas en la mayoria de motores esperan radianes.
            float yaw = y * Mathf.Deg2Rad * 0.5f;   // Yaw: rotacion en torno al eje vertical (ej: mirando hacia la izquierda/derecha)
            float pitch = x * Mathf.Deg2Rad * 0.5f; // Pitch: rotacion en torno al eje lateral (ej: mirando hacia arriba/abajo)
            float roll = z * Mathf.Deg2Rad * 0.5f;  // Roll: rotacion en torno al eje longitudinal (ej: inclinacion lateral)

            // Calcular los valores trigonometricos de la mitad de los angulos. Esto se hace porque la farmula para pasar de Euler a cuaternion
            // utiliza angulos medios. La representacion de cuaterniones se basa en la mitad del angulo de rotacion en cada eje.
            float cosYaw = Mathf.Cos(yaw);
            float sinYaw = Mathf.Sin(yaw);
            float cosPitch = Mathf.Cos(pitch);
            float sinPitch = Mathf.Sin(pitch);
            float cosRoll = Mathf.Cos(roll);
            float sinRoll = Mathf.Sin(roll);

            MyQuat newQuat = new MyQuat();
            // El cuaternion se compone de cuatro componentes: W, X, Y, Z.

            // Estas formulas provienen de la composicion de rotaciones individuales y la definicion del cuaternion a partir de Euler.
            // Basicamente, se toma cada rotacion parcial, se la convierte en un cuaternion y se multiplican entre si.
            // El resultado final es un cuaternion que representa la rotacion total sin los problemas de gimbal lock de las Euler.
            
            // Lo que estas lineas hacen es sintetizar esa combinacion trigonometrica en un solo paso.
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

            // Crear el cuaternion a partir del angulo y el eje de rotacion
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
        /// Transforma un vector director en una rotacion que tenga su eje z alineado con “forward”.
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
            const float epsilon = 0.99995f;

            // Si los cuaterniones son casi colineales, utiliza la interpolacion lineal
            MyQuat result;
            if (dotProduct > epsilon)
            {
                // Esto es mas eficiente que "return LerpUnclamped(a, b, t).normalized();" debido a que en esa manera se
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
                // Calcula los componentes del eje de rotacion dividiendo los componentes X, Y y Z del cuaternion por el factor de escala
                axis = new Vec3(X / scale, Y / scale, Z / scale);
            }
        }

        
        // La multiplicacion con un vector permite aplicar una rotacion en 3D sin sufrir los problemas de interpolacion
        // o gimbal lock que ocurren con los ángulos de Euler.
        
        // Esto puede usarse para rotar un punto en el espacio 3D utilizando un cuaternion que representa
        // una orientacion o transformacion rotacional.

        /// <summary>
        /// Realiza la multiplicacion de un cuaternion por un vector, aplicando una rotacion al vector original.
        /// </summary>
        /// <param name="rotation"> El cuaternion que representa la rotacion</param>
        /// <param name="point"> El vector a rotar</param>
        /// <returns> El resultado de la rotacion aplicada al vector</returns>
        public static Vec3 operator *(MyQuat rotation, Vec3 point)
        {
            // Paso 1: Calcular los productos cruzados parciales entre las componentes del cuaternion y el vector.
            // Estas operaciones corresponden a la aplicacion de las reglas de multiplicacion de cuaterniones,
            // adaptadas para trabajar con un vector (asumiendo W = 0 para el vector extendido).
            float num1 = rotation.Y * point.z - rotation.Z * point.y; // Producto cruzado parcial en X
            float num2 = rotation.Z * point.x - rotation.X * point.z; // Producto cruzado parcial en Y
            float num3 = rotation.X * point.y - rotation.Y * point.x; // Producto cruzado parcial en Z

            // Paso 2: Calcular el producto escalar entre el vector y el cuaternion.
            // Este termino asegura que la rotacion sea correctamente aplicada considerando la "magnitud" del cuaternion.
            float num4 = rotation.X * point.x + rotation.Y * point.y + rotation.Z * point.z; // Producto escalar

            // Paso 3: Calcular los componentes del vector resultante tras la rotacion.
            // Aqui aplicamos las reglas completas de multiplicacion de cuaterniones, considerando la orientacion
            // (almacenada en W del cuaternion) y las interacciones cruzadas.
            float resultX = (num4 * rotation.X + num1 * rotation.W) + (num3 * rotation.Z - num2 * rotation.Y);
            float resultY = (num4 * rotation.Y + num2 * rotation.W) + (num1 * rotation.X - num3 * rotation.Z);
            float resultZ = (num4 * rotation.Z + num3 * rotation.W) + (num2 * rotation.Y - num1 * rotation.X);

            // Paso 4: Crear y devolver un nuevo vector que contiene las coordenadas rotadas.
            // Este vector representa la posicion del punto original tras ser rotado por el cuaternion.
            return new Vec3(resultX, resultY, resultZ);
        }


        // Multiplicacion matricial no conmutativa. 
        // La multiplicacion de cuaterniones se utiliza para representar rotaciones en el espacio tridimensional.
        // La combinacion de dos rotaciones mediante la multiplicacion de cuaterniones es equivalente a aplicar ambas rotaciones en secuencia.
        /// <summary>
        /// Realiza la multiplicacion de dos cuaterniones.
        /// </summary>
        /// <param name="lhs"> Primer cuaternion </param>
        /// <param name="rhs"> Segundo cuaternion </param>
        /// <returns> El resultado de la multiplicacion de los dos cuaterniones </returns>
        public static MyQuat operator *(MyQuat lhs, MyQuat rhs)
        {
            MyQuat quat = new MyQuat
            {
                W = (lhs.W * rhs.W - lhs.X * rhs.X - lhs.Y * rhs.Y - lhs.Z * rhs.Z),
                X = (lhs.W * rhs.X + lhs.X * rhs.W + lhs.Y * rhs.Z - lhs.Z * rhs.Y),
                Y = (lhs.Y * rhs.W + lhs.W * rhs.Y + lhs.Z * rhs.X - lhs.X * rhs.Z),
                Z = (lhs.Z * rhs.W + lhs.W * rhs.Z - lhs.Y * rhs.X + lhs.X * rhs.Y)
            };

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