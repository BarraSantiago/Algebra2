using System;

namespace Matrix
{
    public class MyMatrix4x4
    {
        #region Variables

        public float[,] matrix;
        public float a1;
        public float a2;
        public float a3;
        public float a4;
        public float b1;
        public float b2;
        public float b3;
        public float b4;
        public float c1;
        public float c2;
        public float c3;
        public float c4;
        public float d1;
        public float d2;
        public float d3;
        public float d4;

        #endregion

        public MyMatrix4x4()
        {
            matrix = new float[4, 4];
        }

        public MyMatrix4x4(float[,] data)
        {
            if (data.GetLength(0) != 4 || data.GetLength(1) != 4)
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
            this.a1 = a1;
            this.a2 = a2;
            this.a3 = a3;
            this.a4 = a4;
            this.b1 = b1;
            this.b2 = b2;
            this.b3 = b3;
            this.b4 = b4;
            this.c1 = c1;
            this.c2 = c2;
            this.c3 = c3;
            this.c4 = c4;
            this.d1 = d1;
            this.d2 = d2;
            this.d3 = d3;
            this.d4 = d4;
        }

        public MyMatrix4x4()
        {
            this.a1 = new float();
            this.a2 = new float();
            this.a3 = new float();
            this.a4 = new float();
            this.b1 = new float();
            this.b2 = new float();
            this.b3 = new float();
            this.b4 = new float();
            this.c1 = new float();
            this.c2 = new float();
            this.c3 = new float();
            this.c4 = new float();
            this.d1 = new float();
            this.d2 = new float();
            this.d3 = new float();
            this.d4 = new float();
        }

        public static MyMatrix4x4 zero = new MyMatrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        public static MyMatrix4x4 identity = new MyMatrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        /*
        this[int index]
        this[int row, int column]
        rotation
        lossyScale
        isIdentity
        determinant
        transpose
        inverse
        Determinant(MyMatrix4x4 m);
        Inverse(MyMatrix4x4 m);
        Rotate(MyQuat.MyQuat q);
        Scale(Vec3 vector);
        Translate(Vec3 vector);
        Transpose(MyMatrix4x4 m);
        TRS(Vec3 pos, MyQuat.MyQuat q, Vec3 s);
        GetColumn(int index);
        GetPosition();
        GetRow(int index);
        MultiplyPoint(Vec3 point);
        MultiplyPoint3x4(Vec3 point);
        MultiplyVector(Vec3 vector);
        SetColumn(int index, Vector4 column);
        SetRow(int index, Vector4 row);
        SetTRS(Vec3 pos, MyQuat.MyQuat q, Vec3 s);
        ValidTRS();
        Vector4 operator *(MyMatrix4x4 lhs, Vector4 vector);
        MyMatrix4x4 operator *(MyMatrix4x4 lhs, MyMatrix4x4 rhs);
        bool operator ==(MyMatrix4x4 lhs, MyMatrix4x4 rhs);
        bool operator !=(MyMatrix4x4 lhs, MyMatrix4x4 rhs);
        */
    }
}