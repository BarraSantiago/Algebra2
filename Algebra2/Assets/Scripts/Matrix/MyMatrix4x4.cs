using System;
using CustomMath;
using Quat;
using UnityEngine;

namespace Matrix
{
    public struct MyMatrix4x4
    {
        private float[,] matrix;
        private const int MaxRows = 4;
        private const int MaxColumns = 4;
        private const int TotalSize = 16;

        #region Constructors

        public MyMatrix4x4(float[,] data)
        {
            if (data.GetLength(0) != MaxRows || data.GetLength(1) != MaxColumns)
            {
                throw new ArgumentException("Matrix dimensions are invalid. Must be 4x4.");
            }

            matrix = data;
        }

        public MyMatrix4x4(float a1, float a2, float a3, float a4,
            float b1, float b2, float b3, float b4,
            float c1, float c2, float c3, float c4,
            float d1, float d2, float d3, float d4)
        {
            matrix = new float[MaxRows, MaxColumns];
            matrix[0, 0] = a1;
            matrix[0, 1] = a2;
            matrix[0, 2] = a3;
            matrix[0, 3] = a4;
            matrix[1, 0] = b1;
            matrix[1, 1] = b2;
            matrix[1, 2] = b3;
            matrix[1, 3] = b4;
            matrix[2, 0] = c1;
            matrix[2, 1] = c2;
            matrix[2, 2] = c3;
            matrix[2, 3] = c4;
            matrix[3, 0] = d1;
            matrix[3, 1] = d2;
            matrix[3, 2] = d3;
            matrix[3, 3] = d4;
        }

        public MyMatrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
        {
            matrix = new float[MaxRows, MaxColumns];
            matrix[0, 0] = column0.x;
            matrix[0, 1] = column0.y;
            matrix[0, 2] = column0.z;
            matrix[0, 3] = column0.w;
            matrix[1, 0] = column1.x;
            matrix[1, 1] = column1.y;
            matrix[1, 2] = column1.z;
            matrix[1, 3] = column1.w;
            matrix[2, 0] = column2.x;
            matrix[2, 1] = column2.y;
            matrix[2, 2] = column2.z;
            matrix[2, 3] = column2.w;
            matrix[3, 0] = column3.x;
            matrix[3, 1] = column3.y;
            matrix[3, 2] = column3.z;
            matrix[3, 3] = column3.w;
        }

        /// <summary>
        /// Obtiene o crea la matriz completa.
        /// </summary>
        public MyMatrix4x4 Matrix => new(matrix);

        #endregion

        /// <summary>
        /// Matriz zero, todos los componentes son 0.
        /// </summary>
        public static MyMatrix4x4 zero = new MyMatrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        /// <summary>
        /// Matrtriz identidad. Todos los elementos de la diagonal son 1. Cualquier matriz multiplicada por esta da como resultado la misma matriz.
        /// </summary>
        public static MyMatrix4x4 identity = new MyMatrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

        /// <summary>
        /// Obtiene o reescribe el valor de un elemento en la matriz usando un indice.
        /// </summary>
        /// <param name="index"> El indice del elemento </param>
        /// <returns> El valor del elemento en la posicion especificada </returns>
        /// <exception cref="IndexOutOfRangeException"> Se produce si el indice esta fuera del rango esperado </exception>
        public float this[int index]
        {
            get
            {
                if (index < 0 || index >= TotalSize)
                    throw new IndexOutOfRangeException("Index out of range. The valid range is 0 to 15.");

                int row = index / MaxRows;
                int column = index % MaxColumns;
                return matrix[row, column];
            }
            set
            {
                if (index < 0 || index >= TotalSize)
                    throw new IndexOutOfRangeException("Index out of range. The valid range is 0 to 15.");

                int row = index / MaxRows;
                int column = index % MaxColumns;
                matrix[row, column] = value;
            }
        }

        /// <summary>
        /// Obtiene o reescribe el valor de un elemento en la matriz usando un indice de fila y otro de columna.
        /// </summary>
        /// <param name="row"> El indice de la fila </param>
        /// <param name="column"> El indice de la columna </param>
        /// <exception cref="IndexOutOfRangeException"> Se produce si el indice esta fuera del rango esperado </exception>
        public float this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= MaxRows || column < 0 || column >= MaxColumns)
                    throw new IndexOutOfRangeException(
                        "Indices out of range. The valid range is 0 to 3 for both row and column.");

                return matrix[row, column];
            }
            set
            {
                if (row < 0 || row >= MaxRows || column < 0 || column >= MaxColumns)
                    throw new IndexOutOfRangeException(
                        "Indices out of range. The valid range is 0 to 3 for both row and column.");

                matrix[row, column] = value;
            }
        }

        /// <summary>
        /// Obtiene el cuaternion de rotacion de la matriz.
        /// </summary>
        /// <returns> El cuaternion de rotacion </returns>
        public MyQuat rotation
        {
            get
            {
                // Extraer la escala, rotacion y traslacion de la matriz
                Vec3 scale = lossyScale;

                MyMatrix4x4 rotationMatrix = new MyMatrix4x4(this[0, 0] / scale.x, this[0, 1] / scale.y,
                    this[0, 2] / scale.z, 0f, this[1, 0] / scale.x, this[1, 1] / scale.y,
                    this[1, 2] / scale.z, 0f, this[2, 0] / scale.x, this[2, 1] / scale.y,
                    this[2, 2] / scale.z, 0f, 0f, 0f, 0f, 1f);

                // Obtener el cuaternion a partir de la matriz de rotacion
                MyQuat quaternion = rotationMatrix.rotation;

                return quaternion;
            }
        }

        /// <summary>
        /// Obtiene la escala resultante de la matriz de transformacion.
        /// </summary>
        /// <returns> El vector que representa la escala resultante </returns>
        /// <remarks>
        /// Esta funcion calcula la escala resultante de la matriz de transformacion. La matriz de transformacion puede
        /// incluir una combinacion de rotacion, escala y traslacion. La escala resultante es el factor de cambio en el
        /// tamaño de un objeto despues de aplicar la matriz de transformacion. Esta funcion devuelve la escala
        /// "aparente" del objeto en el espacio del mundo, que puede verse afectada por la jerarquia de transformacion.
        /// </remarks>
        public Vec3 lossyScale
        {
            get { return new Vec3(GetColumn(0).magnitude, GetColumn(1).magnitude, GetColumn(2).magnitude); }
        }

        /// <summary>
        /// Indica si la matriz es una matriz identidad.
        /// </summary>
        /// <returns> True si la matriz es una matriz identidad, False de lo contrario </returns>
        /// <remarks>
        /// Una matriz identidad es una matriz cuadrada en la que todos los elementos fuera de la diagonal principal son cero y todos los elementos en la diagonal principal son uno.
        /// Esta funcion verifica si la matriz cumple con esta condicion para determinar si es una matriz identidad.
        /// </remarks>
        public bool isIdentity
        {
            get
            {
                // Comprueba si la matriz es la matriz identidad
                // La matriz identidad tiene todos los elementos diagonales iguales a 1 y todos los demas elementos iguales a 0.

                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        if (row == col)
                        {
                            if (this[row, col] != 1f)
                                return false;
                        }
                        else
                        {
                            if (this[row, col] != 0f)
                                return false;
                        }
                    }
                }

                return true;
            }
        }

        // MATRIZ DETERMINANTE
        // Determina si se puede invertir sin perder informacion
        // El resultado de la determinante muestra como la matriz afecta la escala de los objetos en el espacio.
        // Un determinante mayor que 1 indica una ampliacion, un determinante menor que 1 indica una compresion
        // y un determinante igual a 1 indica que no hay cambios en la escala.
        /// <summary>
        /// Calcula el determinante de la matriz.
        /// </summary>
        /// <returns> El valor del determinante de la matriz </returns>
        /// <remarks>
        /// El determinante de una matriz es un valor numerico que se calcula a partir de los elementos de la matriz.
        /// Representa una medida de la escala del espacio transformado por la matriz.
        /// Un determinante igual a cero indica que la matriz es singular y no tiene inversa.
        /// </remarks>
        public float determinant
        {
            get
            {
                // Calcula el determinante de la matriz
                // El determinante de una matriz de 4x4 se calcula utilizando una formula especifica que implica sumas y
                // restas de productos de elementos de la matriz.
                // Esta funcion implementa esa formula para calcular el determinante de la matriz.

                float det = 0f;

                // Calcula el determinante utilizando la expansion de cofactores
                det += this[0, 0] * (this[1, 1] * (this[2, 2] * this[3, 3] - this[2, 3] * this[3, 2]) -
                                     this[1, 2] * (this[2, 1] * this[3, 3] - this[2, 3] * this[3, 1]) +
                                     this[1, 3] * (this[2, 1] * this[3, 2] - this[2, 2] * this[3, 1]));
                det -= this[0, 1] * (this[1, 0] * (this[2, 2] * this[3, 3] - this[2, 3] * this[3, 2]) -
                                     this[1, 2] * (this[2, 0] * this[3, 3] - this[2, 3] * this[3, 0]) +
                                     this[1, 3] * (this[2, 0] * this[3, 2] - this[2, 2] * this[3, 0]));
                det += this[0, 2] * (this[1, 0] * (this[2, 1] * this[3, 3] - this[2, 3] * this[3, 1]) -
                                     this[1, 1] * (this[2, 0] * this[3, 3] - this[2, 3] * this[3, 0]) +
                                     this[1, 3] * (this[2, 0] * this[3, 1] - this[2, 1] * this[3, 0]));
                det -= this[0, 3] * (this[1, 0] * (this[2, 1] * this[3, 2] - this[2, 2] * this[3, 1]) -
                                     this[1, 1] * (this[2, 0] * this[3, 2] - this[2, 2] * this[3, 0]) +
                                     this[1, 2] * (this[2, 0] * this[3, 1] - this[2, 1] * this[3, 0]));

                return det;
            }
        }

        /// <summary>
        /// Devuelve la matriz transpuesta de la matriz actual.
        /// La matriz transpuesta se obtiene intercambiando filas por columnas.
        /// </summary>
        /// <returns> La matriz transpuesta </returns>
        public MyMatrix4x4 transpose
        {
            get
            {
                // Transpone la matriz
                // La transposicion de una matriz intercambia las filas por columnas.
                // Esta funcion crea una nueva matriz donde las filas son las columnas originales y las columnas son las filas originales.

                MyMatrix4x4 transposedMatrix = new MyMatrix4x4();

                for (int row = 0; row < MaxRows; row++)
                {
                    for (int col = 0; col < MaxColumns; col++)
                    {
                        transposedMatrix[row, col] = this[col, row];
                    }
                }

                return transposedMatrix;
            }
        }

        /// <summary>
        /// Calcula la matriz inversa de la matriz actual.
        /// La matriz inversa se utiliza para deshacer la transformacion aplicada por la matriz original.
        /// Si la matriz no es invertible, se devuelve una matriz identidad.
        /// </summary>
        /// <returns> La matriz inversa </returns>
        public MyMatrix4x4 inverse
        {
            get
            {
                // Calcula la inversa de la matriz
                float det = determinant;

                // Verifica si la matriz es singular (determinante igual a cero)
                if (Mathf.Approximately(det, 0f))
                {
                    throw new Exception("La matriz no tiene inversa. Es singular.");
                }

                MyMatrix4x4 inverseMatrix = new MyMatrix4x4();

                inverseMatrix[0, 0] = (this[1, 1] * (this[2, 2] * this[3, 3] - this[2, 3] * this[3, 2]) -
                                       this[1, 2] * (this[2, 1] * this[3, 3] - this[2, 3] * this[3, 1]) +
                                       this[1, 3] * (this[2, 1] * this[3, 2] - this[2, 2] * this[3, 1])) / det;
                inverseMatrix[0, 1] = -(this[0, 1] * (this[2, 2] * this[3, 3] - this[2, 3] * this[3, 2]) -
                                        this[0, 2] * (this[2, 1] * this[3, 3] - this[2, 3] * this[3, 1]) +
                                        this[0, 3] * (this[2, 1] * this[3, 2] - this[2, 2] * this[3, 1])) / det;
                inverseMatrix[0, 2] = (this[0, 1] * (this[1, 2] * this[3, 3] - this[1, 3] * this[3, 2]) -
                                       this[0, 2] * (this[1, 1] * this[3, 3] - this[1, 3] * this[3, 1]) +
                                       this[0, 3] * (this[1, 1] * this[3, 2] - this[1, 2] * this[3, 1])) / det;
                inverseMatrix[0, 3] = -(this[0, 1] * (this[1, 2] * this[2, 3] - this[1, 3] * this[2, 2]) -
                                        this[0, 2] * (this[1, 1] * this[2, 3] - this[1, 3] * this[2, 1]) +
                                        this[0, 3] * (this[1, 1] * this[2, 2] - this[1, 2] * this[2, 1])) / det;

                inverseMatrix[1, 0] = -(this[1, 0] * (this[2, 2] * this[3, 3] - this[2, 3] * this[3, 2]) -
                                        this[1, 2] * (this[2, 0] * this[3, 3] - this[2, 3] * this[3, 0]) +
                                        this[1, 3] * (this[2, 0] * this[3, 2] - this[2, 2] * this[3, 0])) / det;
                inverseMatrix[1, 1] = (this[0, 0] * (this[2, 2] * this[3, 3] - this[2, 3] * this[3, 2]) -
                                       this[0, 2] * (this[2, 0] * this[3, 3] - this[2, 3] * this[3, 0]) +
                                       this[0, 3] * (this[2, 0] * this[3, 2] - this[2, 2] * this[3, 0])) / det;
                inverseMatrix[1, 2] = -(this[0, 0] * (this[1, 2] * this[3, 3] - this[1, 3] * this[3, 2]) -
                                        this[0, 2] * (this[1, 0] * this[3, 3] - this[1, 3] * this[3, 0]) +
                                        this[0, 3] * (this[1, 0] * this[3, 2] - this[1, 2] * this[3, 0])) / det;
                inverseMatrix[1, 3] = (this[0, 0] * (this[1, 2] * this[2, 3] - this[1, 3] * this[2, 2]) -
                                       this[0, 2] * (this[1, 0] * this[2, 3] - this[1, 3] * this[2, 0]) +
                                       this[0, 3] * (this[1, 0] * this[2, 2] - this[1, 2] * this[2, 0])) / det;

                inverseMatrix[2, 0] = (this[1, 0] * (this[2, 1] * this[3, 3] - this[2, 3] * this[3, 1]) -
                                       this[1, 1] * (this[2, 0] * this[3, 3] - this[2, 3] * this[3, 0]) +
                                       this[1, 3] * (this[2, 0] * this[3, 1] - this[2, 1] * this[3, 0])) / det;
                inverseMatrix[2, 1] = -(this[0, 0] * (this[2, 1] * this[3, 3] - this[2, 3] * this[3, 1]) -
                                        this[0, 1] * (this[2, 0] * this[3, 3] - this[2, 3] * this[3, 0]) +
                                        this[0, 3] * (this[2, 0] * this[3, 1] - this[2, 1] * this[3, 0])) / det;
                inverseMatrix[2, 2] = (this[0, 0] * (this[1, 1] * this[3, 3] - this[1, 3] * this[3, 1]) -
                                       this[0, 1] * (this[1, 0] * this[3, 3] - this[1, 3] * this[3, 0]) +
                                       this[0, 3] * (this[1, 0] * this[3, 1] - this[1, 1] * this[3, 0])) / det;
                inverseMatrix[2, 3] = -(this[0, 0] * (this[1, 1] * this[2, 3] - this[1, 3] * this[2, 1]) -
                                        this[0, 1] * (this[1, 0] * this[2, 3] - this[1, 3] * this[2, 0]) +
                                        this[0, 3] * (this[1, 0] * this[2, 1] - this[1, 1] * this[2, 0])) / det;

                inverseMatrix[3, 0] = -(this[1, 0] * (this[2, 1] * this[3, 2] - this[2, 2] * this[3, 1]) -
                                        this[1, 1] * (this[2, 0] * this[3, 2] - this[2, 2] * this[3, 0]) +
                                        this[1, 2] * (this[2, 0] * this[3, 1] - this[2, 1] * this[3, 0])) / det;
                inverseMatrix[3, 1] = (this[0, 0] * (this[2, 1] * this[3, 2] - this[2, 2] * this[3, 1]) -
                                       this[0, 1] * (this[2, 0] * this[3, 2] - this[2, 2] * this[3, 0]) +
                                       this[0, 2] * (this[2, 0] * this[3, 1] - this[2, 1] * this[3, 0])) / det;
                inverseMatrix[3, 2] = -(this[0, 0] * (this[1, 1] * this[3, 2] - this[1, 2] * this[3, 1]) -
                                        this[0, 1] * (this[1, 0] * this[3, 2] - this[1, 2] * this[3, 0]) +
                                        this[0, 2] * (this[1, 0] * this[3, 1] - this[1, 1] * this[3, 0])) / det;
                inverseMatrix[3, 3] = (this[0, 0] * (this[1, 1] * this[2, 2] - this[1, 2] * this[2, 1]) -
                                       this[0, 1] * (this[1, 0] * this[2, 2] - this[1, 2] * this[2, 0]) +
                                       this[0, 2] * (this[1, 0] * this[2, 1] - this[1, 1] * this[2, 0])) / det;

                return inverseMatrix;
            }
        }

        /// <summary>
        /// Crea una matriz de rotacion a partir del cuaternion especificado.
        /// La matriz de rotacion se utiliza para aplicar una rotacion tridimensional a puntos o vectores.
        /// </summary>
        /// <param name="q"> El cuaternion que representa la rotacion </param>
        /// <returns> La matriz de rotacion resultante </returns>
        public static MyMatrix4x4 Rotate(Quaternion q)
        {
            // Se duplica los componentes complejos para simplificar las siguientes cuentas. 
            float num1 = q.x * 2f;
            float num2 = q.y * 2f;
            float num3 = q.z * 2f;

            // Es requerido cuadrar los componentes para conseguir los elementos diagonales de la matriz de rotacion.
            float num4 = q.x * num1;
            float num5 = q.y * num2;
            float num6 = q.z * num3;

            // Se hacen estos calculos para conseguir los elementos de la diagonal opuesta de la matriz.
            float num7 = q.x * num2;
            float num8 = q.x * num3;
            float num9 = q.y * num3;
            float num10 = q.w * num1;
            float num11 = q.w * num2;
            float num12 = q.w * num3;

            MyMatrix4x4 rotated = new MyMatrix4x4();

            // Se asignan los valores calculados de la matriz de rotacion, la matriz resultante representa la transformacion de rotacion.
            
            rotated[0, 0] = 1.0f - num5 + num6; // Factor de escala de la rotacion al rededor del eje X
            rotated[0, 1] = num7 - num12;       // Seno de rotacion del eje Y
            rotated[0, 2] = num8 + num11;       // Seno de rotacion del eje Z
            rotated[0, 3] = 0.0f;               // Traslacion en el eje X (no hay ninguna translacion)

            rotated[1, 0] = num7 + num12;       // Seno de la rotacion del eje X
            rotated[1, 1] = 1.0f - num4 + num6; // Factor de escala de la rotacion al rededor del eje Y 
            rotated[1, 2] = num9 - num10;       // Seno de rotacion del eje Z
            rotated[1, 3] = 0.0f;               // Traslacion en el eje Y (no hay ninguna translacion)

            rotated[2, 0] = num8 - num11;       // Seno de rotacion del eje X
            rotated[2, 1] = num9 + num10;       // Seno de rotacion del eje Y
            rotated[2, 2] = 1.0f - num4 + num5; // Factor de escala de la rotacion al rededor del eje Z
            rotated[2, 3] = 0.0f;               // Traslacion en el eje Z (no hay ninguna translacion)

            rotated[3, 0] = 0.0f;               // Traslacion en el eje X (no hay ninguna translacion)
            rotated[3, 1] = 0.0f;               // Traslacion en el eje Y (no hay ninguna translacion)
            rotated[3, 2] = 0.0f;               // Traslacion en el eje Z (no hay ninguna translacion)
            rotated[3, 3] = 1.0f;               // Factor de escala (en 1 para no afectar la translacion)

            return rotated;
        }

        /// <summary>
        /// Crea una matriz de escala a partir del vector especificado.
        /// La matriz de escala se utiliza para aplicar una escala uniforme o no uniforme a puntos o vectores en el espacio tridimensional.
        /// </summary>
        /// <param name="vector"> El vector que especifica la escala en cada dimension (x, y, z) </param>
        /// <returns> La matriz de escala resultante </returns>
        public static MyMatrix4x4 Scale(Vec3 vector)
        {
            MyMatrix4x4 matrix = new MyMatrix4x4();

            matrix[0, 0] = vector.x;
            matrix[1, 1] = vector.y;
            matrix[2, 2] = vector.z;
            matrix[3, 3] = 1f;

            return matrix;
        }

        /// <summary>
        /// Crea una matriz de translacion a partir del vector especificado.
        /// La matriz de translacion se utiliza para desplazar puntos o vectores en el espacio tridimensional.
        /// </summary>
        /// <param name="vector"> El vector que especifica las coordenadas de translacion en cada dimension (x, y, z) </param>
        /// <returns> La matriz de translacion resultante </returns>
        public static MyMatrix4x4 Translate(Vec3 vector)
        {
            MyMatrix4x4 matrix = new MyMatrix4x4();

            // Establecer los elementos de la matriz de traslacion
            matrix[0, 3] = vector.x;
            matrix[1, 3] = vector.y;
            matrix[2, 3] = vector.z;
            matrix[3, 3] = 1f;

            return matrix;
        }

        /// <summary>
        /// Transpone la matriz dada intercambiando filas por columnas.
        /// </summary>
        /// <param name="m"> La matriz a transponer </param>
        /// <returns> La matriz transpuesta resultante </returns>
        public static MyMatrix4x4 Transpose(MyMatrix4x4 m)
        {
            MyMatrix4x4 result = new MyMatrix4x4();

            // Transponer la matriz intercambiando filas por columnas
            for (int row = 0; row < MaxRows; row++)
            {
                for (int col = 0; col < MaxColumns; col++)
                {
                    result[row, col] = m[col, row];
                }
            }

            return result;
        }


        // TRS
        // TRANSFORMACION ROTACION ESCALA
        // MATRIZ QUE SE GENERA MULTIPLICA LAS MATRICES EN EL ORDEN ESPECIFICADO
        // MULTIPLICACION DE TRS REPRESENTA LA ROTACION DE UN OBJETO PADRE SOBRE UN OBJETO HIJO
        // LA MULTIPLICACION DE TRS A SOBRE TRS B (AxB) SE APLICAN LAS MODIFICACIONES DE B EN A

        // Columna 1, 2 y 3: representan la direccion y magnitud de los 3 ejes ortogonales (en orden x, y, z)
        // Columna 4: translation
        // Diagonal principal: Escala
        // Los 4 del centro represental la rotacion de X.
        // 0,0 0,2 2,0 2,2 (es como una X) Rotacion de Y
        // 0,0 0,1 1,0 1,1 (es un cuadrado) Rotacion de Z
        
        /// <summary>
        /// Crea una matriz de transformacion compuesta por una translacion, rotacion y escala.
        /// La matriz de transformacion combina los efectos de desplazamiento, rotacion y escala en un solo objeto.
        /// </summary>
        /// <param name="pos"> El vector que especifica la posicion de translacion </param>
        /// <param name="rotation"> El cuaternion que representa la rotacion </param>
        /// <param name="scale"> El vector que especifica la escala en cada dimension (x, y, z) </param>
        /// <returns> La matriz de transformacion resultante </returns>
        public static MyMatrix4x4 TRS(Vec3 pos, MyQuat rotation, Vec3 scale)
        {
            MyMatrix4x4 matrix = identity;

            // Establecer la traslacion
            matrix[0, 3] = pos.x;
            matrix[1, 3] = pos.y;
            matrix[2, 3] = pos.z;

            // Establecer la rotacion
            MyMatrix4x4 rotationMatrix = QuaternionToMatrix(rotation);
            matrix *= rotationMatrix;

            // Establecer la escala
            matrix[0, 0] *= scale.x;
            matrix[1, 1] *= scale.y;
            matrix[2, 2] *= scale.z;

            return matrix;
        }


        /// <summary>
        /// Obtiene la columna especificada de la matriz de transformacion.
        /// </summary>
        /// <param name="index"> El indice de la columna (0-3) </param>
        /// <returns> El vector columna correspondiente </returns>
        public Vector4 GetColumn(int index)
        {
            // Crea un vector columna a partir de los elementos de la matriz en la columna especificada
            return new Vector4(this[0, index], this[1, index], this[2, index], this[3, index]);
        }

        /// <summary>
        /// Obtiene la posicion de traslacion de la matriz de transformacion.
        /// </summary>
        /// <returns>El vector de posicion.</returns>
        public Vec3 GetPosition()
        {
            // Obtiene la ultima columna de la matriz que representa la posicion de traslacion
            Vector4 column = GetColumn(3);
            return new Vec3(column.x, column.y, column.z);
        }

        /// <summary>
        /// Obtiene la fila especificada de la matriz de transformacion.
        /// </summary>
        /// <param name="index"> El indice de la fila (0-3) </param>
        /// <returns> El vector fila correspondiente </returns>
        public Vector4 GetRow(int index)
        {
            // Crea un vector fila a partir de los elementos de la matriz en la fila especificada
            return new Vector4(this[index, 0], this[index, 1], this[index, 2], this[index, 3]);
        }

        /// <summary>
        /// Multiplica un vector de punto por la matriz de transformacion, teniendo en cuenta la translacion.
        /// </summary>
        /// <param name="point"> El vector de punto a multiplicar </param>
        /// <returns> El vector resultante que representa la posicion transformada del punto </returns>
        /// <remarks>
        /// Esta funcion se suele utilizar para transformar la posicion de un punto en el espacio, teniendo en cuenta la translacion,
        /// la rotacion y la escala definida por la matriz de transformacion.
        /// </remarks>
        public Vec3 MultiplyPoint(Vec3 point)
        {
            // Crea un vector extendido a partir del punto con un componente de 1 para tener en cuenta la traslacion
            Vector4 extendedPoint = new Vector4(point.x, point.y, point.z, 1f);

            // Multiplica el vector extendido por la matriz de transformacion
            Vector4 transformedPoint = this * extendedPoint;

            // Retorna el resultado como un vector de posicion sin el componente de traslacion
            return new Vec3(transformedPoint.x, transformedPoint.y, transformedPoint.z);
        }

        /// <summary>
        /// Multiplica un vector de punto por los primeros tres elementos de la matriz de transformacion (ignorando la
        /// ultima fila), teniendo en cuenta la translacion pero sin considerar la perspectiva.
        /// </summary>
        /// <param name="point"> El vector de punto a multiplicar </param>
        /// <returns> El vector resultante que representa la posicion transformada del punto sin considerar la perspectiva </returns>
        /// <remarks>
        /// Se suele utilizar para transformar la posicion de un punto en el espacio, teniendo en cuenta la translacion,
        /// pero sin considerar la perspectiva. Es util en situaciones donde se necesita transformar puntos 3D en un espacio proyectado
        /// en 2D, como en graficos por computadora o renderizacion.
        /// </remarks>
        public Vec3 MultiplyPoint3x4(Vec3 point)
        {
            // Crea un vector extendido a partir del punto con un componente de 1 para tener en cuenta la traslacion
            Vector4 extendedPoint = new Vector4(point.x, point.y, point.z, 1f);

            // Multiplica el vector extendido por la matriz de transformacion sin tener en cuenta la ultima columna
            Vector4 transformedPoint = this * extendedPoint;

            // Retorna el resultado como un vector de posicion sin el componente de traslacion
            return new Vec3(transformedPoint.x, transformedPoint.y, transformedPoint.z);
        }

        /// <summary>
        /// Multiplica un vector por la matriz de transformacion sin tener en cuenta la traslacion ni el escalado.
        /// </summary>
        /// <param name="vector"> El vector a multiplicar </param>
        /// <returns> El vector resultante que representa la direccion transformada del vector sin considerar la traslacion ni el escalado </returns>
        /// <remarks>
        /// Se suele utilizar para transformar la direccion de un vector en el espacio, sin tener en cuenta la traslacion
        /// ni el escalado. Es util en situaciones donde se necesita transformar direcciones de vectores, como en calculos de normales
        /// o en sistemas de particulas.
        /// </remarks>
        public Vec3 MultiplyVector(Vec3 vector)
        {
            // Crea un vector extendido a partir del vector con un componente de 0 para no tener en cuenta la traslacion
            Vector4 extendedVector = new Vector4(vector.x, vector.y, vector.z, 0f);

            // Multiplica el vector extendido por la matriz de transformacion sin tener en cuenta la ultima columna ni la ultima fila
            Vector4 transformedVector = this * extendedVector;

            // Retorna el resultado como un vector de direccion sin el componente de traslacion
            return new Vec3(transformedVector.x, transformedVector.y, transformedVector.z);
        }

        /// <summary>
        /// Establece la columna en el indice especificado con el vector de columna proporcionado.
        /// </summary>
        /// <param name="index"> El indice de la columna a establecer </param>
        /// <param name="column"> El vector de columna a asignar </param>
        public void SetColumn(int index, Vector4 column)
        {
            // Asegura que el indice de la columna este dentro de los limites del arreglo bidimensional
            if (index < 0 || index >= MaxColumns)
            {
                throw new ArgumentOutOfRangeException("index", "El indice de la columna esta fuera de los limites.");
            }

            // Actualiza la columna en el indice especificado con el vector de columna proporcionado
            for (int row = 0; row < MaxColumns; row++)
            {
                matrix[row, index] = column[row];
            }
        }

        /// <summary>
        /// Establece la fila en el indice especificado con el vector de fila proporcionado.
        /// </summary>
        /// <param name="index"> El indice de la fila a establecer </param>
        /// <param name="row"> El vector de fila a asignar </param>
        public void SetRow(int index, Vector4 row)
        {
            // Asegura que el indice de la fila este dentro de los limites del arreglo bidimensional
            if (index < 0 || index >= MaxRows)
            {
                throw new ArgumentOutOfRangeException("index", "El indice de la fila esta fuera de los limites.");
            }

            // Actualiza la fila en el indice especificado con el vector de fila proporcionado
            for (int col = 0; col < MaxRows; col++)
            {
                matrix[index, col] = row[col];
            }
        }

        /// <summary>
        /// Establece la matriz de transformacion utilizando una combinacion de posicion, rotacion y escala.
        /// </summary>
        /// <param name="pos">La posicion de la transformacion.</param>
        /// <param name="q">La rotacion de la transformacion en forma de Cuaternion.</param>
        /// <param name="s">La escala de la transformacion.</param>
        public void SetTRS(Vec3 pos, MyQuat q, Vec3 s)
        {
            // Crea la matriz de transformacion utilizando la posicion, rotacion y escala proporcionadas
            MyMatrix4x4 translationMatrix = Translate(pos);
            MyMatrix4x4 rotationMatrix = QuaternionToMatrix(q);
            MyMatrix4x4 scaleMatrix = Scale(s);

            // Realiza la multiplicacion en el orden: escala * rotacion * traslacion
            SetMatrix(scaleMatrix * rotationMatrix * translationMatrix);
        }

        /// <summary>
        /// Verifica si la matriz de transformacion cumple con las condiciones de una transformacion valida (TRS: traslacion, rotacion, escala).
        /// </summary>
        /// <returns> True si la matriz de transformacion cumple con las condiciones de una transformacion valida </returns>
        /// <remarks>
        /// Esta funcion comprueba si la matriz de transformacion cumple con las condiciones de una transformacion valida.
        /// Si representa una combinacion valida de traslacion, rotacion y escala. Para ser considerada valida,
        /// la matriz debe cumplir con tener una matriz de rotacion ortogonal y una escala positiva. Es util para
        /// verificar si una matriz de transformacion se ha construido correctamente y puede ser utilizada para
        /// transformar objetos de manera adecuada.
        /// </remarks>
        public bool ValidTRS()
        {
            // Verificar si la matriz esta normalizada ortogonalmente
            // Si alguna de las magnitudes no es aproximadamente igual a 1, se considera que la matriz no esta normalizada ortogonalmente.
            if (!Mathf.Approximately(this.GetColumn(0).magnitude, 1f) ||
                !Mathf.Approximately(this.GetColumn(1).magnitude, 1f) ||
                !Mathf.Approximately(this.GetColumn(2).magnitude, 1f))
            {
                return false;
            }

            // Verificar si la matriz tiene una escala uniforme
            // Si alguno de los componentes de la diagonal principal difiere de los otros, se considera que la matriz no tiene una escala uniforme.
            Vec3 scale = new Vec3(this[0, 0], this[1, 1], this[2, 2]);
            if (!Mathf.Approximately(scale.x, scale.y) ||
                !Mathf.Approximately(scale.x, scale.z))
            {
                return false;
            }

            // Verificar si la matriz tiene una transformación rígida
            MyMatrix4x4 identityMatrix = transpose * transpose.inverse;

            // Si el resultado no es una matriz de identidad, significa que la matriz tiene algun tipo de deformaciozn o no representa una transformacion rigida.
            if (!identityMatrix.isIdentity)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Multiplica un vector de 4 dimensiones por una matriz de 4x4.
        /// </summary>
        /// <param name="lhs">La matriz de 4x4.</param>
        /// <param name="vector">El vector de 4 dimensiones.</param>
        /// <returns>El vector resultante de la multiplicacion.</returns>
        /// <remarks>
        /// Esta operacion realiza la multiplicacion entre un vector de 4 dimensiones y una matriz de 4x4. 
        /// El vector se trata como una columna y se multiplica por cada fila de la matriz. El resultado 
        /// es un nuevo vector de 4 dimensiones.
        /// </remarks>
        public static Vector4 operator *(MyMatrix4x4 lhs, Vector4 vector)
        {
            // Realiza la multiplicacion de la matriz por el vector
            Vector4 result = new Vector4();

            for (int row = 0; row < MaxRows; row++)
            {
                for (int col = 0; col < MaxColumns; col++)
                {
                    result[row] += lhs[row, col] * vector[col];
                }
            }

            return result;
        }

        /// <summary>
        /// Multiplica dos matrices de 4x4.
        /// </summary>
        /// <param name="lhs"> La primera matriz de 4x4 </param>
        /// <param name="rhs"> La segunda matriz de 4x4 </param>
        /// <returns> La matriz resultante de la multiplicacion </returns>
        /// <remarks>
        /// Esta operacion realiza la multiplicacion entre dos matrices de 4x4. 
        /// Cada elemento de la matriz resultante se calcula multiplicando cada elemento correspondiente de las matrices de entrada y sumando los resultados.
        /// </remarks>
        public static MyMatrix4x4 operator *(MyMatrix4x4 lhs, MyMatrix4x4 rhs)
        {
            // Realiza la multiplicacion de matrices
            MyMatrix4x4 result = new MyMatrix4x4();

            for (int row = 0; row < MaxRows; row++)
            {
                for (int col = 0; col < MaxColumns; col++)
                {
                    float value = 0;

                    // Multiplica las filas por las columnas para sacar el valor del casillero que ambas pisan 
                    for (int k = 0; k < MaxRows; k++)
                    {
                        value += lhs[row, k] * rhs[k, col];
                    }

                    result[row, col] = value;
                }
            }

            return result;
        }

        /// <summary>
        /// Compara si dos matrices de 4x4 son iguales.
        /// </summary>
        /// <param name="lhs"> La primera matriz de 4x4 </param>
        /// <param name="rhs"> La segunda matriz de 4x4 </param>
        /// <returns> true si las matrices son iguales </returns>
        public static bool operator ==(MyMatrix4x4 lhs, MyMatrix4x4 rhs)
        {
            const float epsilon = 1e-05f;

            // Compara cada elemento de las matrices y verifica si la diferencia entre si es menor a epsilon
            for (int row = 0; row < MaxRows; row++)
            {
                for (int col = 0; col < MaxColumns; col++)
                {
                    if (lhs[row, col] - rhs[row, col] > epsilon)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compara si dos matrices de 4x4 son diferentes.
        /// </summary>
        /// <param name="lhs"> La primera matriz de 4x4 </param>
        /// <param name="rhs"> La segunda matriz de 4x4 </param>
        /// <returns> true si las matrices son diferentes </returns>
        /// <remarks>
        /// Esta operacion compara cada elemento correspondiente de las matrices de entrada para verificar si son diferentes.
        /// Si al menos uno de los elementos es diferente, se considera que las matrices son diferentes.
        /// </remarks>
        public static bool operator !=(MyMatrix4x4 lhs, MyMatrix4x4 rhs)
        {
            const float epsilon = 1e-05f;

            // Compara cada elemento de las matrices y verifica si la diferencia entre si es menor a epsilon
            for (int row = 0; row < MaxRows; row++)
            {
                for (int col = 0; col < MaxColumns; col++)
                {
                    if (lhs[row, col] - rhs[row, col] > epsilon)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Convierte un cuaternion en una matriz de 4x4.
        /// </summary>
        /// <param name="q"> El cuaternion a convertir </param>
        /// <returns> La matriz de 4x4 resultante </returns>
        /// <remarks>
        /// Esta funcion realiza la conversion de un cuaternion a una matriz de transformacion de 4x4.
        /// La matriz resultante representa la rotacion y la escala del cuaternion, pero no incluye la traslacion.
        /// Para obtener una matriz completa de TRS (traslacion, rotacion y escala), se debe combinar esta matriz
        /// con una matriz de traslacion y una matriz de escala.
        /// </remarks>
        public static MyMatrix4x4 QuaternionToMatrix(MyQuat q)
        {
            MyMatrix4x4 matrix = identity;

            // Elementos de la matriz de rotacion
            float xx = q.X * q.X;
            float xy = q.X * q.Y;
            float xz = q.X * q.Z;
            float xw = q.X * q.W;

            float yy = q.Y * q.Y;
            float yz = q.Y * q.Z;
            float yw = q.Y * q.W;

            float zz = q.Z * q.Z;
            float zw = q.Z * q.W;

            // Llena la matriz de rotacion con los elementos calculados
            matrix[0, 0] = 1 - 2 * (yy + zz);
            matrix[0, 1] = 2 * (xy - zw);
            matrix[0, 2] = 2 * (xz + yw);

            matrix[0, 0] = 2 * (xy + zw);
            matrix[0, 1] = 1 - 2 * (xx + zz);
            matrix[0, 2] = 2 * (yz - xw);

            matrix[2, 0] = 2 * (xz - yw);
            matrix[2, 1] = 2 * (yz + xw);
            matrix[2, 2] = 1 - 2 * (xx + yy);

            return matrix;
        }

        /// <summary>
        /// Reemplaza los valores de la matriz actual con los valores de una nueva matriz.
        /// </summary>
        /// <param name="newMatrix">La nueva matriz de reemplazo.</param>
        public void SetMatrix(MyMatrix4x4 newMatrix)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    // Asignar el valor del elemento correspondiente en la matriz
                    matrix[row, col] = newMatrix[row, col];
                }
            }
        }
    }
}