using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoDEngine.Physics;

namespace TwoDEngine.Scenegraph.SceneObjects
{
    /// <summary>
    /// This interface defines an image for a sprite to draw.
    /// It is used to hide the difference between basic images and
    /// animations or other image effects
    /// </summary>
    public interface SpriteImage
    {
        /// <summary>
        /// This is called once per frame with the elapsed time so the imagery
        /// can be updated nased on time
        /// </summary>
        /// <param name="gameTime">elpased time sicne last drawn</param>
        /// <param name="graph">the scene graph rendering this object</param>
        void Update(GameTime gameTime, Scenegraph graph);

        /// <summary>
        /// This is called by the sprite code to actually draw the image
        /// </summary>
        /// <param name="batch"> a sprite batch to draw to</param>
        /// <param name="dest">position of the drawing origin in screen coordinates</param>
        /// <param name="rotation"> an amount to rotate the imagery by</param>
        /// <param name="priority">the draw priority of this imgery</param>
        void Draw(SpriteBatch batch, Vector2 position, float rotation, Vector2 scale, int priority);

        /// <summary>
        /// Returns the wdth and height of the last image drawn
        /// </summary>
        /// <returns></returns>
        Vector2 GetCurrentImageSize();

        /// <summary>
        /// Returns a physics collider that encompasses the current image
        /// </summary>
        /// <returns></returns>
        Collider GetCollider();

        /// <summary>
        /// Sets the current transform of the collider associated with this image
        /// </summary>
        /// <param name="m"> An XNA transformation matrix</param>
        void SetColliderTransform(Matrix m);
    }
}
