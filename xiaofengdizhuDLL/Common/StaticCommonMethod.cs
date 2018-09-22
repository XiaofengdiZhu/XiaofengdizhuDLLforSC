using Engine;
using System;
using System.Collections.Generic;
using Engine.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static class StaticCommonMethod
    {
        public static StoreBlocks getTriangle(Vector3 v1, Vector3 v2, Vector3 v3, int value)
        {
            Point3 A = RoundV3toP3(v1);
            Point3 B = RoundV3toP3(v2);
            Point3 C = RoundV3toP3(v3);
            StoreBlocks BC = getLine3D(B.X, B.Y, B.Z, C.X, C.Y, C.Z, value);
            StoreBlocks ABC = new StoreBlocks();
            ABC.IsAbsolute = true;
            ABC.AddRange(BC);
            foreach (StoreBlock D in BC)
            {
                ABC.AddRange(getLine3D(A.X, A.Y, A.Z, D.X, D.Y, D.Z, value));
            }
            return ABC;
        }
        public static Point3 RoundV3toP3(Vector3 v)
        {
            return new Point3((int)MathUtils.Round(v.X), (int)MathUtils.Round(v.Y), (int)MathUtils.Round(v.Z));
        }
        public static StoreBlocks getLine3D(int dx, int dy, int dz, int value)
        {
            return getLine3D(0, 0, 0, dx, dy, dz, value);
        }
        public static StoreBlocks getLine3D(int x1, int y1, int z1, int x2, int y2, int z2, int value)
        {
            StoreBlocks storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = true;
            Bresenham3D.DoLine(x1, y1, z1, x2, y2, z2, (int x3, int y3, int z3) => storeBlocks.Add(x3, y3, z3, value));
            return storeBlocks;
        }
        public static StoreBlocks getCircle(int centerX, int centerY, int diameter, int value)
        {
            StoreBlocks storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = true;
            int radius = diameter >> 1;
            bool isOdd = ((diameter & 1) == 1);
            int x = 0, y = radius, d = 3 - (radius << 1);
            while (x < y)
            {
                storeBlocks.AddRange(CirclePlot(centerX, centerY, x, y, value, isOdd));
                if (d < 0)
                {
                    d = d + (x << 2) + 6;
                }
                else
                {
                    d = d + ((x - y) << 2) + 10;
                    y--;
                }
                x++;
            }
            return storeBlocks;
        }
        public static StoreBlocks CirclePlot(int centerX, int centerY, int x, int y, int value, bool isOdd)
        {
            StoreBlocks storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = true;
            if (isOdd)
            {
                storeBlocks.Add(centerX + x, 0, centerY + y, value);
                storeBlocks.Add(centerX - x, 0, centerY + y, value);
                storeBlocks.Add(centerX + x, 0, centerY - y, value);
                storeBlocks.Add(centerX - x, 0, centerY - y, value);
                storeBlocks.Add(centerX + y, 0, centerY + x, value);
                storeBlocks.Add(centerX - y, 0, centerY + x, value);
                storeBlocks.Add(centerX + y, 0, centerY - x, value);
                storeBlocks.Add(centerX - y, 0, centerY - x, value);
            }
            else
            {
                storeBlocks.Add(centerX + y - 1, 0, centerY + x - 1, value);
                storeBlocks.Add(centerX + y - 1, 0, centerY - x, value);
                storeBlocks.Add(centerX - y, 0, centerY + x - 1, value);
                storeBlocks.Add(centerX - y, 0, centerY - x, value);
                storeBlocks.Add(centerX + x - 1, 0, centerY + y - 1, value);
                storeBlocks.Add(centerX + x - 1, 0, centerY - y, value);
                storeBlocks.Add(centerX - x, 0, centerY + y - 1, value);
                storeBlocks.Add(centerX - x, 0, centerY - y, value);
            }
            return storeBlocks;
        }
        public static Dictionary<Color, int> color2colorInt = new Dictionary<Color, int>(){
            { WorldPalette.DefaultColors[0],0},
            { WorldPalette.DefaultColors[1],1},
            { WorldPalette.DefaultColors[2],2},
            { WorldPalette.DefaultColors[3],3},
            { WorldPalette.DefaultColors[4],4},
            { WorldPalette.DefaultColors[5],5},
            { WorldPalette.DefaultColors[6],6},
            { WorldPalette.DefaultColors[7],7},
            { WorldPalette.DefaultColors[8],8},
            { WorldPalette.DefaultColors[9],9},
            { WorldPalette.DefaultColors[10],10},
            { WorldPalette.DefaultColors[11],11},
            { WorldPalette.DefaultColors[12],12},
            { WorldPalette.DefaultColors[13],13},
            { WorldPalette.DefaultColors[14],14},
            { WorldPalette.DefaultColors[15],15}
        };
        //生成xz平面像素画，默认使用彩色粘土Clay,defaultColorIndex默认颜色序号（色表中不存在该颜色时的颜色）
        public static StoreBlocks getImage(string path, int defaultColorIndex)
        {
            Image image = Image.Load(path);
            return getImage(image, defaultColorIndex);
        }
        public static StoreBlocks getImage(Image image, int defaultColorIndex)
        {
            StoreBlocks storeBlocks = new StoreBlocks();
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    try
                    {
                        storeBlocks.Add(x, 0, y, Terrain.ReplaceData(72, (1 | color2colorInt[image.GetPixel(x, y)] << 1)));
                    }
                    catch
                    {
                        storeBlocks.Add(x, 0, y, Terrain.ReplaceData(72, (1 | defaultColorIndex << 1)));
                    }
                }
            }
            return storeBlocks;
        }
        //谢尔宾斯基三角形
        public static float sqrt3 = (float)Math.Sqrt(3);
        public static float sqrt12 = (float)(Math.Sqrt(12));
        public static float sqrt48 = (float)(Math.Sqrt(48));
        public static StoreBlocks get3DSierpinskiTriangle(Vector3 bottomCenter, int value, float height, int targetTimes)
        {
            StoreBlocks storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = true;
            StoreBlocks pyramid = getPyramid(Vector3.Zero, height / (float)(Math.Pow(2, targetTimes)), value);
            foreach (Vector3 v1 in get3DSierpinskiTriangleNextBotomCenters(bottomCenter, height, 0, targetTimes))
            {
                storeBlocks.AddRange(pyramid.Translate3D(StaticCommonMethod.RoundV3toP3(v1)));
            }
            return storeBlocks;
        }
        public static List<Vector3> get3DSierpinskiTriangleNextBotomCenters(Vector3 bottomCenter, float height, int nowTimes, int targetTimes)
        {
            List<Vector3> nexts = new List<Vector3>();
            if (nowTimes >= targetTimes)
            {
                nexts.Add(bottomCenter);
                return nexts;
            }
            nexts.Add(new Vector3(bottomCenter.X, bottomCenter.Y, bottomCenter.Z + height / sqrt12));
            nexts.Add(new Vector3(bottomCenter.X + height / 4f, bottomCenter.Y, bottomCenter.Z - height / sqrt48));
            nexts.Add(new Vector3(bottomCenter.X - height / 4f, bottomCenter.Y, bottomCenter.Z - height / sqrt48));
            nexts.Add(new Vector3(bottomCenter.X, bottomCenter.Y + height / 2, bottomCenter.Z));
            if (++nowTimes < targetTimes)
            {
                for (int i = 0; i < 4; i++)
                {
                    nexts.AddRange(get3DSierpinskiTriangleNextBotomCenters(nexts[0], height / 2, nowTimes, targetTimes));
                    nexts.RemoveAt(0);
                }
            }
            return nexts;
        }
        public static StoreBlocks getPyramid(Vector3 bottomCenter, float height, int value)
        {
            StoreBlocks storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = true;
            Vector3 P = new Vector3(bottomCenter.X, bottomCenter.Y + height, bottomCenter.Z);
            Vector3 A = new Vector3(bottomCenter.X, bottomCenter.Y, bottomCenter.Z + height / sqrt3);
            Vector3 B = new Vector3(bottomCenter.X + height / 2f, bottomCenter.Y, bottomCenter.Z - height / sqrt12);
            Vector3 C = new Vector3(bottomCenter.X - height / 2f, bottomCenter.Y, bottomCenter.Z - height / sqrt12);
            storeBlocks.AddRange(StaticCommonMethod.getTriangle(P, A, B, value));
            storeBlocks.AddRange(StaticCommonMethod.getTriangle(P, A, C, value));
            storeBlocks.AddRange(StaticCommonMethod.getTriangle(P, B, C, value));
            storeBlocks.AddRange(StaticCommonMethod.getTriangle(A, B, C, value));
            return storeBlocks.ClearDuplicatePosition();
        }
        public static int[] halfHeights = new int[5] { 40, 13, 4, 1, 0 };
        public static StoreBlocks getKoch90(CellFace bottomCenter, int value, int targetTimes)
        {
            targetTimes = targetTimes < 6 ? targetTimes : 5;
            StoreBlocks storeBlocks = new StoreBlocks();
            storeBlocks.IsAbsolute = true;
            List<CellFace> nowBottomCenters = new List<CellFace>();
            List<CellFace> nextBottomCenters = new List<CellFace>();
            nowBottomCenters.Add(bottomCenter);
            for (int x = -121; x <= 121; x++)
            {
                for (int z = -121; z <= 121; z++)
                {
                    storeBlocks.Add(bottomCenter.X + x, bottomCenter.Y - 1, bottomCenter.Z + z, value);
                }
            }
            for (int nowTimes = 0; nowTimes < targetTimes; nowTimes++)
            {
                int nowHalfHeight = halfHeights[nowTimes];
                int nowDoubleHalfHeight = 2 * nowHalfHeight;
                nextBottomCenters.Clear();
                foreach (CellFace nowBottomCenter in nowBottomCenters)
                {
                    int face = nowBottomCenter.Face;
                    Point3 direction = CellFace.FaceToPoint3(face);
                    /*实心
                    for (int x = nowHalfHeight * (-1 + direction.X); x <= nowHalfHeight * (1 + direction.X); x++)
                    {
                        for (int y = nowHalfHeight * (-1 + direction.Y); y <= nowHalfHeight * (1 + direction.Y); y++)
                        {
                            for (int z = nowHalfHeight * (-1 + direction.Z); z <= nowHalfHeight * (1 + direction.Z); z++)
                            {
                                storeBlocks.Add(nowBottomCenter.X + x, nowBottomCenter.Y + y, nowBottomCenter.Z + z, value);
                            }
                        }
                    }*/
                    if (nowHalfHeight == 0)
                    {
                        storeBlocks.Add(nowBottomCenter.X, nowBottomCenter.Y, nowBottomCenter.Z, value);
                    }
                    else
                    {
                        for (int x = nowHalfHeight * (-1 + direction.X) + 1; x < nowHalfHeight * (1 + direction.X); x++)
                        {
                            for (int y = nowHalfHeight * (-1 + direction.Y); y <= nowHalfHeight * (1 + direction.Y); y++)
                            {
                                storeBlocks.Add(nowBottomCenter.X + x, nowBottomCenter.Y + y, nowBottomCenter.Z + nowHalfHeight * (-1 + direction.Z), value);
                                storeBlocks.Add(nowBottomCenter.X + x, nowBottomCenter.Y + y, nowBottomCenter.Z + nowHalfHeight * (1 + direction.Z), value);
                            }
                            for (int z = nowHalfHeight * (-1 + direction.Z) + 1; z < nowHalfHeight * (1 + direction.Z); z++)
                            {
                                storeBlocks.Add(nowBottomCenter.X + x, nowBottomCenter.Y + nowHalfHeight * (-1 + direction.Y), nowBottomCenter.Z + z, value);
                                storeBlocks.Add(nowBottomCenter.X + x, nowBottomCenter.Y + nowHalfHeight * (1 + direction.Y), nowBottomCenter.Z + z, value);
                            }
                        }
                        for (int y = nowHalfHeight * (-1 + direction.Y); y <= nowHalfHeight * (1 + direction.Y); y++)
                        {
                            for (int z = nowHalfHeight * (-1 + direction.Z); z <= nowHalfHeight * (1 + direction.Z); z++)
                            {
                                storeBlocks.Add(nowBottomCenter.X + nowHalfHeight * (-1 + direction.X), nowBottomCenter.Y + y, nowBottomCenter.Z + z, value);
                                storeBlocks.Add(nowBottomCenter.X + nowHalfHeight * (1 + direction.X), nowBottomCenter.Y + y, nowBottomCenter.Z + z, value);
                            }
                        }
                    }
                    nextBottomCenters.Add(new CellFace(nowBottomCenter.X + direction.X * (nowDoubleHalfHeight + 1), nowBottomCenter.Y + direction.Y * (nowDoubleHalfHeight + 1), nowBottomCenter.Z + direction.Z * (nowDoubleHalfHeight + 1), nowBottomCenter.Face));
                    foreach (int otherFace in FaceToSurround(face))
                    {
                        Point3 other = CellFace.FaceToPoint3(otherFace);
                        nextBottomCenters.Add(new CellFace(nowBottomCenter.X + other.X * ( nowDoubleHalfHeight + 1), nowBottomCenter.Y + other.Y * (nowDoubleHalfHeight + 1), nowBottomCenter.Z + other.Z * (nowDoubleHalfHeight + 1), nowBottomCenter.Face));
                        nextBottomCenters.Add(new CellFace(nowBottomCenter.X + (direction.X + other.X) * nowHalfHeight + other.X, nowBottomCenter.Y + (direction.Y + other.Y) * nowHalfHeight + other.Y, nowBottomCenter.Z + (direction.Z + other.Z) * nowHalfHeight + +other.Z, otherFace));
                    }
                    foreach(Point3 diagonal in FaceToDiagonal(face))
                    {
                        nextBottomCenters.Add(new CellFace(nowBottomCenter.X + diagonal.X * (nowDoubleHalfHeight + 1), nowBottomCenter.Y + diagonal.Y * (nowDoubleHalfHeight + 1), nowBottomCenter.Z + diagonal.Z * (nowDoubleHalfHeight + 1), nowBottomCenter.Face));
                    }
                }
                nowBottomCenters.Clear();
                nowBottomCenters.AddRange(nextBottomCenters);
            }
            return storeBlocks;
        }
        public static int[] FaceToSurround(int face)
        {
            if(face ==4 || face == 5)
            {
                return new int[4]
                    {
                        0,1,2,3
                    };
            }else if(face ==1 || face == 3)
            {
                return new int[4]
                    {
                        0,2,4,5
                    };
            }
            else if (face == 0 || face == 2)
            {
                return new int[4]
                    {
                        1,3,4,5
                    };
            }
            else
            {
                return new int[0];
            }
        }
        public static Point3[] FaceToDiagonal(int face)
        {
            if (face == 4 || face == 5)
            {
                return new Point3[4]
                    {
                        new Point3(1,0,1),
                        new Point3(1,0,-1),
                        new Point3(-1,0,1),
                        new Point3(-1,0,-1)
                    };
            }
            else if (face == 1 || face == 3)
            {
                return new Point3[4]
                    {
                        new Point3(0,1,1),
                        new Point3(0,1,-1),
                        new Point3(0,-1,1),
                        new Point3(0,-1,-1)
                    };
            }
            else if (face == 0 || face == 2)
            {
                return new Point3[4]
                    {
                        new Point3(1,1,0),
                        new Point3(1,-1,0),
                        new Point3(-1,1,0),
                        new Point3(-1,-1,0)
                    };
            }
            else
            {
                return new Point3[0];
            }
        }
    }
}
