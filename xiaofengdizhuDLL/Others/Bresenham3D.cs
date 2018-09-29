using System;

namespace Game
{
    //from https://sites.google.com/site/proyectosroboticos/bresenham-3d
    internal class Bresenham3D
    {
        public static void DoLine(int startX, int startY, int startZ, int endX, int endY, int endZ, Action<int, int, int> callback)
        {
            int Cont, dx, dy, dz, Adx, Ady, Adz, x_inc, y_inc, z_inc,
               err_1, err_2, dx2, dy2, dz2,
               Xnew = endX, Ynew = endY, Znew = endZ;

            //while (true)
            //{
            dx = Xnew - startX;
            dy = Ynew - startY;
            dz = Znew - startZ;

            if (dx < 0)
            {
                x_inc = -1;
            }
            else
            {
                x_inc = 1;
            }

            if (dy < 0)
            {
                y_inc = -1;
            }
            else
            {
                y_inc = 1;
            }

            if (dz < 0)
            {
                z_inc = -1;
            }
            else
            {
                z_inc = 1;
            }

            Adx = Math.Abs(dx);
            Ady = Math.Abs(dy);
            Adz = Math.Abs(dz);

            dx2 = Adx * 2;
            dy2 = Ady * 2;
            dz2 = Adz * 2;

            if ((Adx >= Ady) && (Adx >= Adz))
            {
                err_1 = dy2 - Adx;
                err_2 = dz2 - Adx;

                for (Cont = 0; Cont <= Adx - 1; Cont++)
                {
                    if (err_1 > 0)
                    {
                        startY += y_inc;
                        err_1 -= dx2;
                    }

                    if (err_2 > 0)
                    {
                        startZ += z_inc;
                        err_2 -= dx2;
                    }

                    err_1 += dy2;
                    err_2 += dz2;
                    startX += x_inc;

                    callback(startX, startY, startZ);
                }
            }

            if ((Ady > Adx) && (Ady >= Adz))
            {
                err_1 = dx2 - Ady;
                err_2 = dz2 - Ady;

                for (Cont = 0; Cont <= Ady - 1; Cont++)
                {
                    if (err_1 > 0)
                    {
                        startX += x_inc;
                        err_1 -= dy2;
                    }

                    if (err_2 > 0)
                    {
                        startZ += z_inc;
                        err_2 -= dy2;
                    }

                    err_1 += dx2;
                    err_2 += dz2;
                    startY += y_inc;

                    callback(startX, startY, startZ);
                }
            }

            if ((Adz > Adx) && (Adz > Ady))
            {
                err_1 = dy2 - Adz;
                err_2 = dx2 - Adz;

                for (Cont = 0; Cont <= Adz - 1; Cont++)
                {
                    if (err_1 > 0)
                    {
                        startY += y_inc;
                        err_1 -= dz2;
                    }

                    if (err_2 > 0)
                    {
                        startX += x_inc;
                        err_2 -= dz2;
                    }

                    err_1 += dy2;
                    err_2 += dx2;
                    startZ += z_inc;

                    callback(startX, startY, startZ);
                }
            }
            //}
        }
    }
}