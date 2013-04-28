using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;

namespace TwoDEngine.Physics.Colliders
{
   
    public abstract class AbstractCollider : Collider
    {
        protected Shape shape;
        Transform transform = new Transform();
        

        public AbstractCollider(Shape shape)
        {
            this.shape = shape;
            
        }

        public virtual bool CollidesWith(Collider c2)
        {
            return AABB.TestOverlap(shape, 0, ((AbstractCollider)c2).shape, 0, ref transform,
                ref ((AbstractCollider)c2).transform);
        }

        public void SetPosition(Vector2 pos)
        {
            float r = transform.Angle;
            transform.Set(pos, r);
        }

        public void SetRotation(float r)
        {
            Vector2 pos = transform.Position;
            transform.Set(pos, r);
        }

        public abstract void SetScale(Vector2 vec);

        public virtual void SetTransform(Matrix m)
        {
            Vector3 pos;
            Quaternion quat;
            Vector3 scale;
            m.Decompose(out scale,out quat,out pos);
            Vector3 rot = MathUtils.QuaternionToEuler(quat);
            transform.Set(new Vector2(pos.X,pos.Y), (float)rot.Z);
            SetScale(new Vector2(scale.X,scale.Y));
        }

        public Shape GetShape()
        {
            return shape;
        }

    }
}
