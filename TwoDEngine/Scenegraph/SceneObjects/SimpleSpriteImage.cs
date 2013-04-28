using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoDEngine.Physics.Colliders;

namespace TwoDEngine.Scenegraph.SceneObjects
{
    /// <summary>
    /// This class defines a sprite image that is a single static image
    /// </summary>
    public class SimpleSpriteImage: SpriteImage
    {
        /// <summary>
        /// The image to draw
        /// </summary>

        Texture2D image;

        BoxCollider collider;

        /// <summary>
        /// A constructor that takes an image and returns a SpriteImage object
        ///
        /// </summary>
        /// <param name="image">the  image this object will draw</param>
        public SimpleSpriteImage(Texture2D image)
        {
            this.image = image;
            if (image != null)
            { //HACK: need to move image handlign above tilemap sprite in hirearchy
                collider = new BoxCollider(new Vector2(image.Width, image.Height));
            }
        }

        /// <summary>
        /// Called to update the image for the passage of time.  
        /// Since a SimpleSpriteImage represents a single static image, it is a NOP
        /// </summary>
        /// <param name="gameTime">elapsed time since last call</param>
        /// <param name="graph">the scene graph that is controlling the render of this sprite image</param>
        public void Update(GameTime gameTime, Scenegraph graph)
        {
            //does nothing in a  simple sprite image
        }

        /// <summary>
        /// This draws the static image at the passed in location and rotation
        /// </summary>
        /// <param name="batch">the sprite batch to use to render the image</param>
        /// <param name="dest">a rectangle in screen pixel coordinates that indicates where to draw the image</param>
        /// <param name="rotation">a rotation for the image</param>
        /// <param name="priority">the priority of this draw operation</param>
        public void Draw(SpriteBatch batch,Vector2 position, float rotation, Vector2 scale, int priority)
        {
            if (image != null)
            {
                batch.Draw(image, position, new Rectangle(0, 0, image.Width, image.Height), Color.White, rotation,
                       Vector2.Zero, scale, SpriteEffects.None, priority);
            }
        }

        /// <summary>
        /// Returns the size of this static image
        /// </summary>
        /// <returns>a Vector2 where X = image width and Y = image height</returns>
        public Vector2 GetCurrentImageSize()
        {
            if (image == null)
            {
                return Vector2.Zero;
            }
            else
            {
                return new Vector2(image.Width, image.Height);
            }
        }

        public void SetColliderTransform(Matrix m)
        {
            if (collider != null)
            { //HACK, REMOVE AFTER WE REFACTOR IMAGE SPRITE
                collider.SetTransform(m);
            }
        }


        public Physics.Collider GetCollider()
        {
            return collider;
        }
    }
}
