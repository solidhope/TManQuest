using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;

namespace TwoDEngine.Physics.Colliders
{
    public class CircleCollider: AbstractCollider
    {
        public CircleCollider(float radius, float density=0.5f):base(new CircleShape(radius,density)){}

        public override void SetScale(Vector2 vec)
        {
            throw new NotImplementedException();
        }
    }

   
}
