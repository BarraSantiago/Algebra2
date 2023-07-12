using System;
using Quat;
using UnityEngine;

namespace Matrix
{
    public class MyMatrix4x4
    {
        public float[,] matrix;
        private const int MaxRows = 4;
        private const int MaxColumns = 4;
        private const int TotalSize = 16;

        #region Constructors
        
        public MyMatrix4x4()
        {
            matrix = new float[MaxRows, MaxColumns];
        }

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
            matrix = new float[4, 4];
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
            matrix = new float[4, 4];
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
        
        #endregion
        
        public static MyMatrix4x4 zero = new MyMatrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        public static MyMatrix4x4 identity = new MyMatrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

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

        public float this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= MaxRows || column < 0 || column >= MaxColumns)
                    throw new IndexOutOfRangeException("Indices out of range. The valid range is 0 to 3 for both row and column.");

                return matrix[row, column];
            }
            set
            {
                if (row < 0 || row >= MaxRows || column < 0 || column >= MaxColumns)
                    throw new IndexOutOfRangeException("Indices out of range. The valid range is 0 to 3 for both row and column.");

                matrix[row, column] = value;
            }
        }

        /*
        MyQuat rotation(){}
        public Vector3 lossyScale
        public bool isIdentity;
        public float determinant;
        public Matrix4x4 transpose;
        public Matrix4x4 inverse;
        
        float Determinant(MyMatrix4x4 m);
        MyMatrix4x4 Inverse(MyMatrix4x4 m);
        MyMatrix4x4 Rotate(MyQuat.MyQuat q);
        static Matrix4x4 Scale(Vec3 vector);
        public static Matrix4x4 Translate(Vec3 vector);
        public static Matrix4x4 Transpose(MyMatrix4x4 m);
        public static Matrix4x4 TRS(Vec3 pos, MyQuat.MyQuat q, Vec3 s);
        public Vector4 GetColumn(int index);
        public Vector3 GetPosition()();
        public Vector4 GetRow(int index);
        public Vector3 MultiplyPoint(Vec3 point);
        public Vector3 MultiplyPoint3x4(Vec3 point);
        public Vector3 MultiplyVector(Vec3 vector);
        public void SetColumn(int index, Vector4 column);
        public void SetRow(int index, Vector4 row);
        public void SetTRS(Vec3 pos, MyQuat.MyQuat q, Vec3 s);
        public bool ValidTRS();
        public static Vector4 operator *(MyMatrix4x4 lhs, Vector4 vector);
        public static Matrix4x4 operator *(MyMatrix4x4 lhs, MyMatrix4x4 rhs);
        public static bool operator ==(MyMatrix4x4 lhs, MyMatrix4x4 rhs);
        public static bool operator !=(MyMatrix4x4 lhs, MyMatrix4x4 rhs);
        */
    }
}