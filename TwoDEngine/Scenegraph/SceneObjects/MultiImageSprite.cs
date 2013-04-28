using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoDEngine.Scenegraph;
using TwoDEngine.Scenegraph.SceneObjects;
using Microsoft.Xna.Framework.Graphics;


namespace TwoDEngine.Scenegraph.SceneObjects
{
    public class MultiImageSprite:BasicSprite 
    {
        private SpriteImage[] images;
        public MultiImageSprite(SceneObjectParent parent, SpriteImage simage):base(parent,simage){}

        public MultiImageSprite(SceneObjectParent parent, SpriteImage[] simage) : base(parent, simage[0]) {
            images = simage;
        }

        /// <summary>
        /// This creates a sprite with no image
        /// Note that the Scenegraph itself is a SceneObjectParent and represents
        /// the root of the scenegraph
        /// </summary>
        /// <param name="parent">The parent scene graph object</param>
        public MultiImageSprite(SceneObjectParent parent):base(parent){}

        /// <summary>
        /// This creates a sprite with the passed in image
        /// </summary>
        /// <param name="parent">The parent scene graph object</param>
        /// <param name="image">The image to draw for the sprite</param>
        public MultiImageSprite(SceneObjectParent parent, Texture2D image) : base(parent, image) { }

        public int GetImageCount()
        {
            return images.Length;
        }

        public void SetCurrentImage(int idx)
        {
            SetImage(images[idx]);
        }

    }
}
