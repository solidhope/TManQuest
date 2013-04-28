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
    /// This defines a sprite image that is a series of image frames
    /// chnaged over time
    /// </summary>
    public class AnimatedSpriteImage: SpriteImage
    {
        /// <summary>
        /// The image that contains all the frames
        /// </summary>
        Texture2D image;

        /// <summary>
        /// This is an array of frames within the image
        /// a single frame sprite has only one entry
        /// </summary>
        Rectangle[] frames;

        /// <summary>
        /// This is the seconds per frame on an animated sprite
        /// A single image sprite has this set to MAX FLOAT;
        /// </summary>
        float secPerFrame = float.MaxValue;

        /// <summary>
        /// This is the frame index of the currently displayed frame.  In a single image
        /// sprite this is always 0
        /// </summary>
        int frameIndex = 0;

        /// <summary>
        /// Tracks the smount of time since the last frame change
        /// </summary>
        double elapsedFrameTime = 0f;


        BoxCollider collider;

        /// <summary>
        /// This creates a sprite image with the passed in image strip, frame size and
        /// frame coordinates
        /// </summary>
        /// <param name="parent">The parent scene graph object</param>
        /// <param name="image">The image to draw for the sprite</param>
        /// <param name="frameSize">The width and height of each frame in the strip</param>
       
        public AnimatedSpriteImage(Texture2D image, Rectangle[] frames, float secPerFrame)
        {
            this.image = image;
            this.frames = frames;
            this.secPerFrame = secPerFrame;
            this.collider = new BoxCollider();
            ResetColliderSize();
        }

        private void ResetColliderSize()
        {
            collider.SetSize(GetCurrentImageSize());
        }

        private static Rectangle[] MakeRegularFrames(Texture2D image, int numberOfFrames)
        {
            Rectangle[] rects = new Rectangle[numberOfFrames];
            for (int i = 0; i < numberOfFrames; i++)
            {
                rects[i] = new Rectangle(i * (image.Width / numberOfFrames), 0, image.Width / numberOfFrames, image.Height);
                Console.WriteLine("rect " + i + ": " + rects[i]);
            }
            return rects;
        }

        /// This creates a spriteimage  with the passed in image strip that contains numberOfFrames equally sized frames
        ///
        /// </summary>
        /// <param name="parent">The parent scene graph object</param>
        /// <param name="image">The image to draw for the sprite</param>
        /// <param name="frameSize">The width and height of each frame in the strip</param>
        public AnimatedSpriteImage(Texture2D image, int numberOfFrames, float secPerFrame)
            : this(image,MakeRegularFrames(image,numberOfFrames),secPerFrame)
        {
           
        }

         /// <summary>
        /// This update just advances frames.
        /// </summary>
        /// <param name="gameTime"> the elapsed time since last call</param>
        /// <param name="graph">the scenegraph this SceneObject is attached to</param>
        public void Update(GameTime gameTime, Scenegraph graph)
        {
            elapsedFrameTime += gameTime.ElapsedGameTime.TotalSeconds;
            
            while (elapsedFrameTime >= secPerFrame)
            {
                elapsedFrameTime -= secPerFrame;
                frameIndex = (frameIndex + 1) % frames.Length;
                ResetColliderSize();
            }
        }
    
        /// <summary>
        /// This draws the current frame
        /// </summary>
         /// <param name="batch">the sprite batch to use to render the image</param>
        /// <param name="dest">a rectangle in screen pixel coordinates that indicates where to draw the image</param>
        /// <param name="rotation">a rotation for the image</param>
        /// <param name="priority">the priority of this draw operation</param>
        public void Draw(SpriteBatch batch, Vector2 position, float rotation, Vector2 scale, int priority)
        {

            batch.Draw(image, position, frames[frameIndex], Color.White, rotation,
                    Vector2.Zero, scale, SpriteEffects.None, priority);
        }

        /// <summary>
        /// This gets the wisth and height of the last frame drawn
        /// </summary>
        /// <returns>a Vector2 where X = image width and Y = image height</returns>
        public Vector2 GetCurrentImageSize()
        {
            return new Vector2(frames[frameIndex].Width, frames[frameIndex].Height);
        }



        //TODO: This duyplicates code in SimpleSprite, shoudl be refactored to an abstract parent
        public Physics.Collider GetCollider()
        {
            return collider;
        }

        public void SetColliderTransform(Matrix m)
        {
            if (collider != null)
            { //HACK, REMOVE AFTER WE REFACTOR IMAGE SPRITE
                collider.SetTransform(m);
            }
        }
    }
}
