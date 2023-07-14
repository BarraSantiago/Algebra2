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

        public static MyMatrix4x4 zero = new MyMatrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

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
        /// <returns> El cuaternion de rotacio </returns>
        public MyQuat rotation
        {
            get
            {
                // Extraer la escala, rotacion y traslacion de la matriz
                Vec3 scale = lossyScale;
                MyMatrix4x4 rotationMatrix = new MyMatrix4x4(this[0, 0] / scale.x, this[0, 1] / scale.y,
                    this[0, 2] / scale.z, 0f,
                    this[1, 0] / scale.x, this[1, 1] / scale.y, this[1, 2] / scale.z, 0f,
                    this[2, 0] / scale.x, this[2, 1] / scale.y, this[2, 2] / scale.z, 0f,
                    0f, 0f, 0f, 1f);

                // Obtener el cuaternion a partir de la matriz de rotacion
                MyQuat quaternion = rotationMatrix.ToMyQuat();

                return quaternion;
            }
        }


        /// <summary>
        /// Obtiene la escala en el espacio mundial (world / escala real) del objeto.
        /// </summary>
        public Vec3 lossyScale
        {
            get { return new Vec3(GetColumn(0).magnitude, GetColumn(1).magnitude, GetColumn(2).magnitude); }
        }
        
        public bool isIdentity
        {
            get
            {
                // Comprueba si la matriz es la matriz identidad
                // La matriz identidad tiene todos los elementos diagonales iguales a 1 y todos los demás elementos iguales a 0.
                // Esta función verifica si todos los elementos de la matriz son iguales a los elementos de la matriz identidad.

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

        //MATRIZ DETERMINANTE
        // Determina si se puede invertir sin perder informacion
        /// <summary>
        /// Devuelve la determinante de la matriz
        /// </summary>
        public float determinant
        {
            get
            {
                // Calcula el determinante de la matriz
                // El determinante de una matriz de 4x4 se calcula utilizando una fórmula específica que implica sumas y restas de productos de elementos de la matriz.
                // Esta función implementa esa fórmula para calcular el determinante de la matriz.

                float det = 0f;

                // Calcula el determinante utilizando la expansión de cofactores
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

        public MyMatrix4x4 transpose
        {
            get
            {
                // Transpone la matriz
                // La transposición de una matriz intercambia las filas por columnas.
                // Esta función crea una nueva matriz donde las filas son las columnas originales y las columnas son las filas originales.

                MyMatrix4x4 transposedMatrix = new MyMatrix4x4();

                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        transposedMatrix[row, col] = this[col, row];
                    }
                }

                return transposedMatrix;
            }
        }

        public MyMatrix4x4 inverse
        {
            get
            {
                // Calcula la inversa de la matriz
                // La inversa de una matriz se calcula utilizando una serie de operaciones matematicas específicas.
                // Esta función implementa esas operaciones para calcular la matriz inversa.

                float det = determinant;

                // Verifica si la matriz es singular (determinante igual a cero)
                if (Mathf.Approximately(det, 0f))
                {
                    throw new Exception("La matriz no tiene inversa. Es singular.");
                }

                MyMatrix4x4 inverseMatrix = new MyMatrix4x4();

                // Calcula la inversa utilizando la formula matematica
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
            rotated[0, 1] = num7 - num12; // Seno de rotacion del eje Y
            rotated[0, 2] = num8 + num11; // Seno de rotacion del eje Z
            rotated[0, 3] = 0.0f; // Traslacion en el eje X (no hay ninguna translacion)

            rotated[1, 0] = num7 + num12; // Seno de la rotacion del eje X
            rotated[1, 1] = 1.0f - num4 + num6; // Factor de escala de la rotacion al rededor del eje Y 
            rotated[1, 2] = num9 - num10; // Seno de rotacion del eje Z
            rotated[1, 3] = 0.0f; // Traslacion en el eje Y (no hay ninguna translacion)

            rotated[2, 0] = num8 - num11; // Seno de rotacion del eje X
            rotated[2, 1] = num9 + num10; // Seno de rotacion del eje Y
            rotated[2, 2] = 1.0f - num4 + num5; // Factor de escala de la rotacion al rededor del eje Z
            rotated[2, 3] = 0.0f; // Traslacion en el eje Z (no hay ninguna translacion)

            rotated[3, 0] = 0.0f; // Traslacion en el eje X (no hay ninguna translacion)
            rotated[3, 1] = 0.0f; // Traslacion en el eje Y (no hay ninguna translacion)
            rotated[3, 2] = 0.0f; // Traslacion en el eje Z (no hay ninguna translacion)
            rotated[3, 3] = 1.0f; // Factor de escala (en 1 para no afectar la translacion)

            return rotated;
        }

        /// <summary>
        /// Crea una matriz de escala basada en el vector de escala proporcionado.
        /// </summary>
        /// <param name="vector">Vector de escala en los ejes x, y, z.</param>
        /// <returns>La matriz de escala resultante.</returns>
        public static MyMatrix4x4 Scale(Vec3 vector)
        {
            MyMatrix4x4 matrix = new MyMatrix4x4();

            // Establecer los elementos de la matriz de escala
            matrix[0, 0] = vector.x;
            matrix[1, 1] = vector.y;
            matrix[2, 2] = vector.z;
            matrix[3, 3] = 1f;

            return matrix;
        }

        /// <summary>
        /// Crea una matriz de traslacion basada en el vector de traslacion proporcionado.
        /// </summary>
        /// <param name="vector">Vector de traslacion en los ejes x, y, z.</param>
        /// <returns>La matriz de traslacion resultante.</returns>
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
        /// <param name="m">La matriz a transponer.</param>
        /// <returns>La matriz transpuesta resultante.</returns>
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
        
        /// <summary>
        /// Crea una matriz de transformacion compuesta por una traslacion, rotacion y escala.
        /// </summary>
        /// <param name="pos"> Vector de traslacion </param>
        /// <param name="q"> Cuaternion de rotacion </param>
        /// <param name="s"> Vector de escala </param>
        /// <returns> La matriz de transformacion TRS resultante </returns>
        public static MyMatrix4x4 TRS(Vec3 pos, MyQuat q, Vec3 s)
        {
            MyMatrix4x4 matrix = identity;

            // Establecer la traslacion
            matrix[0, 3] = pos.x;
            matrix[1, 3] = pos.y;
            matrix[2, 3] = pos.z;

            // Establecer la rotacion
            MyMatrix4x4 rotationMatrix = QuaternionToMatrix(q);
            matrix *= rotationMatrix;

            // Establecer la escala
            matrix[0, 0] *= s.x;
            matrix[1, 1] *= s.y;
            matrix[2, 2] *= s.z;

            return matrix;
        }


        /// <summary>
        /// Obtiene la columna especificada de la matriz de transformacion.
        /// </summary>
        /// <param name="index">El índice de la columna (0-3).</param>
        /// <returns>El vector columna correspondiente.</returns>
        public Vector4 GetColumn(int index)
        {
            // Crea un vector columna a partir de los elementos de la matriz en la columna especificada
            return new Vector4(this[0, index], this[1, index], this[2, index], this[3, index]);
        }

        /// <summary>
        /// Obtiene la posición de traslacion de la matriz de transformacion.
        /// </summary>
        /// <returns>El vector de posición.</returns>
        public Vec3 GetPosition()
        {
            // Obtiene la última columna de la matriz que representa la posición de traslacion
            Vector4 column = GetColumn(3);
            return new Vec3(column.x, column.y, column.z);
        }

        /// <summary>
        /// Obtiene la fila especificada de la matriz de transformacion.
        /// </summary>
        /// <param name="index">El índice de la fila (0-3).</param>
        /// <returns>El vector fila correspondiente.</returns>
        public Vector4 GetRow(int index)
        {
            // Crea un vector fila a partir de los elementos de la matriz en la fila especificada
            return new Vector4(this[index, 0], this[index, 1], this[index, 2], this[index, 3]);
        }

        /// <summary>
        /// Multiplica un punto por la matriz de transformacion, teniendo en cuenta la traslacion.
        /// </summary>
        /// <param name="point">El punto a multiplicar.</param>
        /// <returns>El resultado de la multiplicación.</returns>
        public Vec3 MultiplyPoint(Vec3 point)
        {
            // Crea un vector extendido a partir del punto con un componente de 1 para tener en cuenta la traslacion
            Vector4 extendedPoint = new Vector4(point.x, point.y, point.z, 1f);

            // Multiplica el vector extendido por la matriz de transformacion
            Vector4 transformedPoint = this * extendedPoint;

            // Retorna el resultado como un vector de posición sin el componente de traslacion
            return new Vec3(transformedPoint.x, transformedPoint.y, transformedPoint.z);
        }

        /// <summary>
        /// Multiplica un punto por la matriz de transformacion sin tener en cuenta la traslacion.
        /// </summary>
        /// <param name="point">El punto a multiplicar.</param>
        /// <returns>El resultado de la multiplicación.</returns>
        public Vec3 MultiplyPoint3x4(Vec3 point)
        {
            // Crea un vector extendido a partir del punto con un componente de 1 para tener en cuenta la traslacion
            Vector4 extendedPoint = new Vector4(point.x, point.y, point.z, 1f);

            // Multiplica el vector extendido por la matriz de transformacion sin tener en cuenta la última columna
            Vector4 transformedPoint = this * extendedPoint;

            // Retorna el resultado como un vector de posición sin el componente de traslacion
            return new Vec3(transformedPoint.x, transformedPoint.y, transformedPoint.z);
        }

        /// <summary>
        /// Multiplica un vector por la matriz de transformacion sin tener en cuenta la traslacion ni el escalamiento.
        /// </summary>
        /// <param name="vector">El vector a multiplicar.</param>
        /// <returns>El resultado de la multiplicación.</returns>
        public Vec3 MultiplyVector(Vec3 vector)
        {
            // Crea un vector extendido a partir del vector con un componente de 0 para no tener en cuenta la traslacion
            Vector4 extendedVector = new Vector4(vector.x, vector.y, vector.z, 0f);

            // Multiplica el vector extendido por la matriz de transformacion sin tener en cuenta la última columna ni la última fila
            Vector4 transformedVector = this * extendedVector;

            // Retorna el resultado como un vector de dirección sin el componente de traslacion
            return new Vec3(transformedVector.x, transformedVector.y, transformedVector.z);
        }

        /// <summary>
        /// Establece la columna en el índice especificado con el vector de columna proporcionado.
        /// </summary>
        /// <param name="index">El índice de la columna a establecer.</param>
        /// <param name="column">El vector de columna a asignar.</param>
        public void SetColumn(int index, Vector4 column)
        {
            // Asegura que el índice de la columna esté dentro de los límites del arreglo bidimensional
            if (index < 0 || index >= 4)
                throw new ArgumentOutOfRangeException("index", "El índice de la columna está fuera de los límites.");

            // Actualiza la columna en el índice especificado con el vector de columna proporcionado
            for (int row = 0; row < 4; row++)
            {
                matrix[row, index] = column[row];
            }
        }

        /// <summary>
        /// Establece la fila en el índice especificado con el vector de fila proporcionado.
        /// </summary>
        /// <param name="index">El índice de la fila a establecer.</param>
        /// <param name="row">El vector de fila a asignar.</param>
        public void SetRow(int index, Vector4 row)
        {
            // Asegura que el índice de la fila esté dentro de los límites del arreglo bidimensional
            if (index < 0 || index >= 4)
                throw new ArgumentOutOfRangeException("index", "El índice de la fila está fuera de los límites.");

            // Actualiza la fila en el índice especificado con el vector de fila proporcionado
            for (int col = 0; col < 4; col++)
            {
                matrix[index, col] = row[col];
            }
        }
        
        /// <summary>
        /// Establece la matriz de transformacion utilizando una combinación de posición, rotacion y escala.
        /// </summary>
        /// <param name="pos">La posición de la transformacion.</param>
        /// <param name="q">La rotacion de la transformacion en forma de Cuaternion.</param>
        /// <param name="s">La escala de la transformacion.</param>
        public void SetTRS(Vec3 pos, MyQuat q, Vec3 s)
        {
            // Crea la matriz de transformacion utilizando la posición, rotacion y escala proporcionadas
            MyMatrix4x4 translationMatrix = Translate(pos);
            MyMatrix4x4 rotationMatrix = QuaternionToMatrix(q);
            MyMatrix4x4 scaleMatrix = Scale(s);

            // Realiza la multiplicación en el orden: escala * rotacion * traslacion
            SetMatrix(scaleMatrix * rotationMatrix * translationMatrix);
        }

        /// <summary>
        /// Checks if the matrix represents a valid TRS transformation.
        /// </summary>
        /// <returns>True if the matrix is a valid TRS transformation, false otherwise.</returns>
        public bool ValidTRS()
        {
            // Check if the matrix is orthogonally normalized
            if (!Mathf.Approximately(this.GetColumn(0).magnitude, 1f) ||
                !Mathf.Approximately(this.GetColumn(1).magnitude, 1f) ||
                !Mathf.Approximately(this.GetColumn(2).magnitude, 1f))
            {
                return false;
            }

            // Check if the matrix is uniformly scaled
            Vec3 scale = new Vec3(this[0, 0], this[1, 1], this[2, 2]);
            if (!Mathf.Approximately(scale.x, scale.y) ||
                !Mathf.Approximately(scale.x, scale.z))
            {
                return false;
            }

            // Check if the matrix is rigid-body transformed
            MyMatrix4x4 transposed = transpose;
            MyMatrix4x4 inverse = transposed.inverse;
            MyMatrix4x4 identity = transposed * inverse;
            if (!identity.isIdentity)
            {
                return false;
            }

            return true;
        }


        public static Vector4 operator *(MyMatrix4x4 lhs, Vector4 vector)
        {
            // Realiza la multiplicación de la matriz por el vector
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

        public static MyMatrix4x4 operator *(MyMatrix4x4 lhs, MyMatrix4x4 rhs)
        {
            // Realiza la multiplicación de matrices
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
        /// Convierte la matriz de transformacion en un Cuaternion que representa la rotacion.
        /// </summary>
        /// <returns>El Cuaternion que representa la rotacion.</returns>
        public MyQuat ToMyQuat()
        {
            // Extrae la escala, la rotacion y la posición de la matriz de transformacion
            Vec3 scale = lossyScale;
            MyQuat rotationQuat = rotation;

            // Normaliza la escala
            scale.Normalize();

            // Crea una matriz de transformacion auxiliar para almacenar solo la rotacion y la escala
            MyMatrix4x4 transformMatrix = new MyMatrix4x4();
            transformMatrix.SetTRS(Vec3.Zero, rotationQuat, scale);

            // Convierte la matriz de transformacion auxiliar en un Cuaternion
            MyQuat quaternion = transformMatrix.ToMyQuat();

            return quaternion;
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