using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TwoDEngine.Scenegraph;
using TwoDEngine.Scenegraph.SceneObjects;

namespace SidescrollerDemo
{


    public class JumpingPlayer : Player
    {
        bool jumping = false;
        float jumpVelocity;
        float impulse = 200f;
        float gravity = -200f;

        public JumpingPlayer(SpriteFont font, TileMap map, SceneObjectParent parent, Texture2D image, float pixelPerSecSpeed = 0) :
            base(font, map, parent, new SimpleSpriteImage(image), pixelPerSecSpeed) { }

        public JumpingPlayer(SpriteFont font, TileMap map, SceneObjectParent parent, SpriteImage simage, float pixelPerSecSpeed = 0) :
            base(font, map, parent, simage, pixelPerSecSpeed) { }

        public JumpingPlayer(SpriteFont font, TileMap map, SceneObjectParent parent, SpriteImage[] simage, float pixelPerSecSpeed = 0) :
            base(font, map, parent, simage, pixelPerSecSpeed) { }

        // this begins a jump arc
        public void StartJump(float impulse)
        {
            jumping = true;
            jumpVelocity = impulse;
        }

        protected override void UpdateMe(Microsoft.Xna.Framework.GameTime gameTime, TwoDEngine.Scenegraph.Scenegraph scenegraph)
        {
            double deltaT = gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState state = Keyboard.GetState();


            // then get existing position in order to update with the Y velocity.
            Vector2 newPos = GetLocalPosition(); // relative to tile map parent
            // if we arent currently in a jump, echeck for jump key
            // if pressed then set our Y velocity "up" to the starting impulse
            if (!jumping)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    StartJump(impulse);
                }
            }
            // add in the effects of gravity to our current Y velocity
            jumpVelocity += (float)(gravity * deltaT);
            // set new position using Y velocity
            newPos.Y -= (float)(jumpVelocity * deltaT);


            // need to check head and feet, use extents
            // note that code currently assuems centered handle.  Will have to be
            // modified if handle is else where
            if (!PositionCollidesWithBlocking(newPos))
            {
                SetLocalPosition(newPos);
            }
            else
            {
                jumping = false;
                jumpVelocity = 0;
            }
            base.UpdateMe(gameTime, scenegraph);
        }
    }
}
