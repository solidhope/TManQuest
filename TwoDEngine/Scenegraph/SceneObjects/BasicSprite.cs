using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoDEngine.Physics;

namespace TwoDEngine.Scenegraph.SceneObjects
{
    /// <summary>
    /// This class defines a basic sprite with a single image that cna be added and moved
    /// around the scenegraph
    /// </summary>
    public class BasicSprite : AbstractSceneObject
    {
        class CollisionRecord
        {
            public bool collidedWith;
            public BasicSprite sprite;

            public CollisionRecord(BasicSprite sprite)
            {
                collidedWith = false;
                this.sprite = sprite;
            }

            
        }
        /// <summary>
        /// The image to draw for this sprite
        /// </summary>
        SpriteImage image;
        

        /// <summary>
        /// List of other sprites to check for collision with
        /// </summary>
        List<CollisionRecord> collisionList = new List<CollisionRecord>();

        /// <summary>
        

        /// <summary>
        /// This creates a sprite with no image
        /// Note that the Scenegraph itself is a SceneObjectParent and represents
        /// the root of the scenegraph
        /// </summary>
        /// <param name="parent">The parent scene graph object</param>
        public BasicSprite(SceneObjectParent parent):base(parent){
            this.image = null;
        }

        /// <summary>
        /// This creates a sprite with the passed in image
        /// </summary>
        /// <param name="parent">The parent scene graph object</param>
        /// <param name="image">The image to draw for the sprite</param>
        public BasicSprite(SceneObjectParent parent, Texture2D image):base(parent)
        {
            this.image = new SimpleSpriteImage(image); 
        }

        /// <summary>
        /// This creates a sprite with the passed in image
        /// </summary>
        /// <param name="parent">The parent scene graph object</param>
        /// <param name="image">The image to draw for the sprite</param>
        public BasicSprite(SceneObjectParent parent, SpriteImage simage)
            : base(parent)
        {
            this.image = simage;

        }


        protected void SetImage(SpriteImage simage)
        {
            this.image = simage;
        }
       

        public SpriteImage GetSpriteImage()
        {
            return image;
        }

        public override Vector2 GetSize()
        {
            if (image == null)
            {
                return Vector2.Zero;
            }
            else
            {
                return image.GetCurrentImageSize()*GetScale();
            }
        }

        public virtual Collider GetCollider()
        {
            
            return image.GetCollider();
        }

        public override void UpdateGlobalMatrix(Matrix parentMatrix)
        {
            base.UpdateGlobalMatrix(parentMatrix);
            if (image != null)
            {// HACK need to move image handling lower down the class hriearchy
                image.SetColliderTransform(GetGlobalMatrix());
            }
        }

        public virtual void OnCollisionWith(BasicSprite other){
            //nop is default
        }

        public virtual void OnCollisionEnterWith(BasicSprite other)
        {
            //nop is default
        }

        public virtual void OnCollisionExitWith(BasicSprite other)
        {
            //nop is default
        }

        public void CollidesWith(BasicSprite other)
        {
            collisionList.Add(new CollisionRecord(other));
        }
       

        /// <summary>
        /// This is an abstract method that must be made concrete by any sub-class of AbstractSceneObject 
        /// it is responsible for drawing the imagery given the passed in scale, rotation and translation
        /// which are all in global screen coordinates
        /// </summary>
        /// <param name="batch"> The sprite batch to use to draw the sprite</param>
        /// <param name="scale"> A scale factor to apply to the sprite</param>
        /// <param name="rotation"> A rotation to apply to the drawn sprite</param>
        /// <param name="translation"> A translation to apply to the drawn sprite</param>
        /// <param name="priority"> The draw priority.  By default the only draw order gaurantee is that parent scene
        /// graph objects are drawn befoe their children, but this may be used to impose additional order constraints</param>
        protected override void DrawAt(SpriteBatch batch, Vector2 scale, float rotation, Vector2 translation,int priority)
        {
            
            if (image!=null){
                Vector2 imageSz = image.GetCurrentImageSize();
              
               // Console.WriteLine("Soure,dest: " + source + "," + dest);
                image.Draw(batch, translation, rotation, scale,  priority);
            }
        }


        /// <summary>
        /// This update just advances frames and checks for collisions
        /// </summary>
        /// <param name="gameTime"> the elapsed time since last call</param>
        /// <param name="graph">the scenegraph this SceneObject is attached to</param>
        protected override void UpdateMe(GameTime gameTime, Scenegraph graph)
        {
            if (image != null)
            {
                image.Update(gameTime, graph);
            }
            List<CollisionRecord> removalList= new List<CollisionRecord>();
            foreach (CollisionRecord rec in collisionList)
            {
                BasicSprite other = rec.sprite;
                if (other.IsDestroyed())
                {
                    removalList.Add(rec);
                }
                else
                {
                    if (GetCollider().CollidesWith(other.GetCollider()))
                    {
                        if (!rec.collidedWith)
                        {
                            rec.collidedWith = true;
                            OnCollisionEnterWith(other);
                        }
                        OnCollisionWith(other);
                    }
                    else
                    {
                        if (rec.collidedWith)
                        {
                            rec.collidedWith = false;
                            OnCollisionExitWith(other);
                        }
                    }
                }
            }
            foreach (CollisionRecord r in removalList)
            {
                collisionList.Remove(r);
            }
        }

        public override void OnDestroy()
        {
           //nop
        }
    }
}
