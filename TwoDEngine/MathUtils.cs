using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoDEngine
{
    public static class MathUtils
    {

        
            //In a 2D grid, returns the angle to a specified point from the +X axis
            public static float ArcTanAngle(float X, float Y)
            {
                if (X == 0)
                {
                    if (Y == 1)
                        return (float)MathHelper.PiOver2;
                    else
                        return (float)-MathHelper.PiOver2;
                }
                else if (X > 0)
                    return (float)Math.Atan(Y / X);
                else if (X < 0)
                {
                    if (Y > 0)
                        return (float)Math.Atan(Y / X) + MathHelper.Pi;
                    else
                        return (float)Math.Atan(Y / X) - MathHelper.Pi;
                }
                else
                    return 0;
            }

            //returns Euler angles that point from one point to another
            public static Vector3 AngleTo(Vector3 from, Vector3 location)
            {
                Vector3 angle = new Vector3();
                Vector3 v3 = Vector3.Normalize(location - from);
                angle.X = (float)Math.Asin(v3.Y);
                angle.Y = ArcTanAngle(-v3.Z, -v3.X);
                return angle;
            }

            //converts a Quaternion to Euler angles (X = pitch, Y = yaw, Z = roll)
            public static Vector3 QuaternionToEuler(Quaternion rotation)
            {
                Vector3 rotationaxes = new Vector3();

                Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
                Vector3 up = Vector3.Transform(Vector3.Up, rotation);
                rotationaxes = AngleTo(new Vector3(), forward);
                if (rotationaxes.X == MathHelper.PiOver2)
                {
                    rotationaxes.Y = ArcTanAngle(up.Z, up.X);
                    rotationaxes.Z = 0;
                }
                else if (rotationaxes.X == -MathHelper.PiOver2)
                {
                    rotationaxes.Y = ArcTanAngle(-up.Z, -up.X);
                    rotationaxes.Z = 0;
                }
                else
                {
                    up = Vector3.Transform(up, Matrix.CreateRotationY(-rotationaxes.Y));
                    up = Vector3.Transform(up, Matrix.CreateRotationX(-rotationaxes.X));
                    rotationaxes.Z = ArcTanAngle(up.Y, -up.X);
                }
                return rotationaxes;
            }
    }
}
