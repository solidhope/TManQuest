using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

namespace TwoDEngine.Physics.Colliders
{
    
    public class BoxCollider : AbstractCollider
    {
        Vector2 size;
        Vector2 scale=new Vector2(1,1);

        public BoxCollider(float density = 0.5f) : base(new PolygonShape(density)) { }

        public BoxCollider(Vector2 sz, float density = 0.5f) : base(new PolygonShape(density)) {
            this.size = sz;
            CalculateBox();
        }

        private static Vertices MakeVertices(Vector2 sz)
        {
            Vertices v = new Vertices();
            v.Add(new Vector2(0, 0));
            v.Add(new Vector2(sz.X, 0));
            v.Add(new Vector2(sz.X, sz.Y));
            v.Add(new Vector2(0,sz.Y));
            return v;
        }

        public void SetSize(Vector2 sz)
        {
            this.size = sz;
        }

        public override void SetScale(Vector2 scale)
        {
            this.scale = scale;
            CalculateBox();
        }

        private void CalculateBox()
        {
            Vertices v =((PolygonShape)shape).Vertices;
            Vector2 sz = size * scale;
            v.Clear();
            v.Add(new Vector2(0, 0));
            v.Add(new Vector2(sz.X, 0));
            v.Add(new Vector2(sz.X, sz.Y));
            v.Add(new Vector2(0, sz.Y));
        }
    }
  
}
