using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace TwoDEngine.Physics
{
    public interface Collider
    {
        bool CollidesWith(Collider c2);

        void SetPosition(Vector2 pos);

        void SetRotation(float r);

        void SetTransform(Matrix m);

    }
}
