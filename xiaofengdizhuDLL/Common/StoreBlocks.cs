using Engine;
using System.Collections.Generic;

namespace Game
{
    public class StoreBlocks : List<StoreBlock>
    {
        public void Add(int x, int y, int z, int value)
        {
            Add(new StoreBlock(x, y, z, value));
        }

        public bool IsAbsolute;

        public StoreBlock GetOriginFast()
        {
            return this[0];
        }

        public Point3 GetOriginPointFast()
        {
            return new Point3(this[0].X, this[0].Y, this[0].Z);
        }

        public StoreBlock GetOrigin()
        {
            var origin = new StoreBlock(1 << 31 - 1);
            foreach (StoreBlock storeBlock in this)
            {
                if (storeBlock.X < origin.X) origin.X = storeBlock.X;
                if (storeBlock.Y < origin.Y) origin.Y = storeBlock.Y;
                if (storeBlock.Z < origin.Z) origin.Z = storeBlock.Z;
            }
            return origin;
        }

        public Point3 GetOriginPoint()
        {
            return GetOrigin().ToPoint3();
        }

        public StoreBlocks ResetOrigin()
        {
            StoreBlock newOrigin = GetOrigin();
            int originIndex = IndexOf(newOrigin);
            if (originIndex == -1)
            {
                Insert(0, newOrigin);
            }
            else
            {
                this[originIndex] = this[0];
                this[0] = newOrigin;
            }
            return this;
        }

        public StoreBlocks ToRelative()
        {
            if (IsAbsolute)
            {
                Point3 originPoint = -1 * GetOriginPointFast();
                StoreBlocks storeBlocks = Translate3D(originPoint);
                storeBlocks.IsAbsolute = false;
                return storeBlocks;
            }
            return this;
        }

        public StoreBlocks ClearDuplicatePosition()
        {
            var storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = IsAbsolute;
            var dictionary = new Dictionary<Point3, int>();
            foreach (StoreBlock block in this)
            {
                var position = block.ToPoint3();
                if (dictionary.ContainsKey(position))
                {
                    dictionary[position] = block.Value;
                }
                else
                {
                    dictionary.Add(position, block.Value);
                }
            }
            foreach (KeyValuePair<Point3, int> block in dictionary)
            {
                storeBlocks.Add(new StoreBlock(block.Key, block.Value));
            }
            return storeBlocks;
        }

        //平移
        public StoreBlocks Translate3D(Point3 point3)
        {
            return Translate3D(point3.X, point3.Y, point3.Z);
        }

        public StoreBlocks Translate3D(int dx, int dy, int dz)
        {
            var storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = IsAbsolute;
            foreach (StoreBlock storeBlock in this)
            {
                storeBlocks.Add(storeBlock.Translate3D(dx, dy, dz));
            }
            return storeBlocks;
        }

        public StoreBlocks TranslateX(int dx)
        {
            var storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = IsAbsolute;
            foreach (StoreBlock storeBlock in this)
            {
                storeBlocks.Add(storeBlock.TranslateX(dx));
            }
            return storeBlocks;
        }

        public StoreBlocks TranslateY(int dy)
        {
            var storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = IsAbsolute;
            foreach (StoreBlock storeBlock in this)
            {
                storeBlocks.Add(storeBlock.TranslateY(dy));
            }
            return storeBlocks;
        }

        public StoreBlocks TranslateZ(int dz)
        {
            var storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = IsAbsolute;
            foreach (StoreBlock storeBlock in this)
            {
                storeBlocks.Add(storeBlock.TranslateZ(dz));
            }
            return storeBlocks;
        }

        public StoreBlocks Scale3D(float mx, float my, float mz)
        {
            if (mx < 1 || my < 1 || mz < 1) return this;
            var storeBlocks = new StoreBlocks();
            foreach (StoreBlock storeBlock in ToRelative())
            {
                for (int x = 0; x < mx; x++)
                {
                    int x1 = (int)(storeBlock.X * mx) + x;
                    for (int y = 0; y < my; y++)
                    {
                        int y1 = (int)(storeBlock.Y * my) + y;
                        for (int z = 0; z < mz; z++)
                        {
                            storeBlocks.Add(x1, y1, (int)(storeBlock.Z * mz) + z, storeBlock.Value);
                        }
                    }
                }
            }
            if (IsAbsolute)
            {
                storeBlocks.IsAbsolute = true;
                return storeBlocks.Translate3D(GetOriginPointFast());
            }
            return storeBlocks;
        }

        public StoreBlocks ScaleX(float mx)
        {
            if (mx <= 1) return this;
            var storeBlocks = new StoreBlocks();
            foreach (StoreBlock storeBlock in ToRelative())
            {
                for (int x = 0; x < mx; x++)
                {
                    storeBlocks.Add((int)(storeBlock.X * mx) + x, storeBlock.Y, storeBlock.Z, storeBlock.Value);
                }
            }
            return IsAbsolute ? storeBlocks.Translate3D(GetOriginPointFast()) : storeBlocks;
        }

        public StoreBlocks ScaleY(float my)
        {
            if (my <= 1) return this;
            var storeBlocks = new StoreBlocks();
            foreach (StoreBlock storeBlock in ToRelative())
            {
                for (int y = 0; y < my; y++)
                {
                    storeBlocks.Add(storeBlock.X, (int)(storeBlock.Y * my) + y, storeBlock.Z, storeBlock.Value);
                }
            }
            return IsAbsolute ? storeBlocks.Translate3D(GetOriginPointFast()) : storeBlocks;
        }

        public StoreBlocks ScaleZ(float mz)
        {
            if (mz <= 1)
                return this;
            var storeBlocks = new StoreBlocks();
            foreach (StoreBlock storeBlock in ToRelative())
            {
                for (int z = 0; z < mz; z++)
                {
                    storeBlocks.Add(storeBlock.X, storeBlock.Y, (int)(storeBlock.Z * mz) + z, storeBlock.Value);
                }
            }
            return IsAbsolute ? storeBlocks.Translate3D(GetOriginPointFast()) : storeBlocks;
        }

        public StoreBlocks RotateFrowAxisAngle(Vector3 axis, float angle)
        {
            return Transform(Quaternion.CreateFromAxisAngle(axis, angle));
        }

        public StoreBlocks RotateFrowYawPitchRoll(float yaw, float pitch, float roll)
        {
            return Transform(Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll));
        }

        public StoreBlocks Transform(Quaternion q)
        {
            var storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = true;
            foreach (StoreBlock storeBlock in ToRelative())
            {
                storeBlocks.Add(storeBlock.Transform(q));
            }
            return storeBlocks.ResetOrigin().ToRelative();
        }

        public StoreBlocks Transform(Matrix m)
        {
            var storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = true;
            foreach (StoreBlock storeBlock in ToRelative())
            {
                storeBlocks.Add(storeBlock.Transform(m));
            }
            return storeBlocks.ResetOrigin().ToRelative();
        }
    }
}