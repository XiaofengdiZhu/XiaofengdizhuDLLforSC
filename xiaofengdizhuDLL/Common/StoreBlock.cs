using Engine;

namespace Game
{
    public struct StoreBlock
    {
        public int X;
        public int Y;
        public int Z;
        public int Value;

        public StoreBlock(int x, int y, int z, int value)
        {
            X = x;
            Y = y;
            Z = z;
            Value = value;
        }

        public StoreBlock(int n)
        {
            X = n;
            Y = n;
            Z = n;
            Value = 0;
        }

        public StoreBlock(Point3 point3, int value)
        {
            X = point3.X;
            Y = point3.Y;
            Z = point3.Z;
            Value = value;
        }

        public StoreBlock(Vector3 vector3, int value)
        {
            X = (int)vector3.X;
            Y = (int)vector3.Y;
            Z = (int)vector3.Z;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", new object[]
            {
                X,Y,Z,Value
            });
        }

        public Point3 ToPoint3()
        {
            return new Point3(X, Y, Z);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public StoreBlock Translate3D(int dx, int dy, int dz)
        {
            return new StoreBlock(X + dx, Y + dy, Z + dz, Value);
        }

        public StoreBlock TranslateX(int dx)
        {
            return new StoreBlock(X + dx, Y, Z, Value);
        }

        public StoreBlock TranslateY(int dy)
        {
            return new StoreBlock(X, Y + dy, Z, Value);
        }

        public StoreBlock TranslateZ(int dz)
        {
            return new StoreBlock(X, Y, Z + dz, Value);
        }

        public StoreBlock Transform(Quaternion q)
        {
            return new StoreBlock(Vector3.Transform(ToVector3(), q), Value);
        }

        public static StoreBlock Transform(StoreBlock storeBlock, Quaternion q)
        {
            return new StoreBlock(Vector3.Transform(storeBlock.ToVector3(), q), storeBlock.Value);
        }

        public StoreBlock Transform(Matrix m)
        {
            return new StoreBlock(Vector3.Transform(ToVector3(), m), Value);
        }

        public static StoreBlock Transform(StoreBlock storeBlock, Matrix m)
        {
            return new StoreBlock(Vector3.Transform(storeBlock.ToVector3(), m), storeBlock.Value);
        }
    }
}