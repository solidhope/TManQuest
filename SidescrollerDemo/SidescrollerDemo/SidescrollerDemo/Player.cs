using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoDEngine;
using TwoDEngine.Scenegraph;
using TwoDEngine.Scenegraph.SceneObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SidescrollerDemo
{
    public class Player : MOB
    {
        /// <summary>
        /// it is sometimes usefiul to disable border scroll for debugging purposes
        /// </summary>

        const bool SCROLLBORDERS = true;

        Vector2 pushBorder = new Vector2(50, 50); // edge of  screen/window that causes scrolling
        GraphicsDeviceManager gmgr;
        SpriteFont font;


        public Player(SpriteFont font, TileMap map, SceneObjectParent parent, SpriteImage simage, float pixelPerSecSpeed = 0) :
            base(map, parent, simage, pixelPerSecSpeed)
        {
            Vector2 sz = simage.GetCurrentImageSize();
            SetHandle(new Vector2(sz.X / 2, sz.Y / 2));
            Registry.Require<GraphicsDeviceManager>();
            gmgr = Registry.Lookup<GraphicsDeviceManager>();
            this.font = font;
        }

        public Player(SpriteFont font, TileMap map, SceneObjectParent parent, SpriteImage[] simage, float pixelPerSecSpeed = 0) :
            base(map, parent, simage, pixelPerSecSpeed)
        {
            Vector2 sz = simage[0].GetCurrentImageSize();
            SetHandle(new Vector2(sz.X / 2, sz.Y / 2));
            Registry.Require<GraphicsDeviceManager>();
            gmgr = Registry.Lookup<GraphicsDeviceManager>();
            this.font = font;
        }


        protected override void UpdateMe(GameTime gameTime, Scenegraph scenegraph)
        {
            double deltaT = gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState state = Keyboard.GetState();

            // Get local position and modify in keyed direction by speed
            Vector2 newPos = GetLocalPosition(); // relative to tile map parent
            bool moving = false;
            if (state.IsKeyDown(Keys.A))
            {
                newPos.X -= (float)(GetPPSSpeed() * deltaT);
                SetCurrentImage(1);
                moving = true;
            }
            if (state.IsKeyDown(Keys.D))
            {
                newPos.X += (float)(GetPPSSpeed() * deltaT);
                SetCurrentImage(2);
                moving = true;
            }

            if (!moving)
            {
                SetCurrentImage(0);
            }


            // Get the width and height of the entire tile map
            Vector2 mapSize = GetTileMap().GetPixelSize();
            // get the width and height of this image
            Vector2 imageSz = GetSpriteImage().GetCurrentImageSize();
            // Check to see if new position is still on map
            if ((newPos.X - (imageSz.X / 2) > 0) && (newPos.X + (imageSz.X / 2) < mapSize.X) &&
                (newPos.Y - (imageSz.Y / 2) > 0) && (newPos.Y + (imageSz.Y / 2) < mapSize.Y))
            {
                // check to see if blocked
                if (!PositionCollidesWithBlocking(newPos))
                { // not blocked
                    SetLocalPosition(newPos);
                }
                // check for chaarcter within scroll borders of screen and if so move tilemap
                Vector2 screenPosition = GetGlobalPosition(); // our pixel position on screen
                Vector2 tilemapLocalPos = GetTileMap().GetLocalPosition(); // current psition of tile map
                Rectangle viewport = gmgr.GraphicsDevice.Viewport.Bounds; // size of screen (window)
                Vector2 scaledPushBorder = pushBorder * GetGlobalScale();
                if (screenPosition.X < scaledPushBorder.X)
                {
                    tilemapLocalPos.X += (scaledPushBorder.X - screenPosition.X);
                }
                if (screenPosition.X > viewport.Width - scaledPushBorder.X)
                {
                    tilemapLocalPos.X -= screenPosition.X - (viewport.Width - scaledPushBorder.X);
                }
                if (screenPosition.Y < scaledPushBorder.Y)
                {
                    tilemapLocalPos.Y += (scaledPushBorder.Y - screenPosition.Y);
                }
                if (screenPosition.Y > viewport.Height - scaledPushBorder.Y)
                {
                    tilemapLocalPos.Y -= screenPosition.Y - (viewport.Height - scaledPushBorder.Y);
                }
                if (SCROLLBORDERS)
                {
                    // tilemapLocalPos.X = Math.Min(tilemapLocalPos.X, 0);
                    // tilemapLocalPos.Y = Math.Min(tilemapLocalPos.Y, 0);
                    GetTileMap().SetLocalPosition(tilemapLocalPos);
                }
            }
            base.UpdateMe(gameTime, scenegraph); // do sprite targeted movement update

        }

        protected override void EnteredCell(Vector2 cellpos)
        {
            int tilenum = GetTileMap().GetTileIndex("placement", cellpos);
            if ((tilenum > 0) &&
                (GetTileMap().GetTileProperties(tilenum).ContainsKey("DEATH")))
            {
                DoDie();
            }
        }

        private void DoDie()
        {
            Scenegraph sg = Registry.Lookup<Scenegraph>();
            TextSprite loser = new TextSprite(GetTileMap(), sg, font, "You Lose!");
            Vector2 txtSz = loser.GetSize();
            loser.SetHandle(txtSz / 2);
            loser.SetLocalPosition(new Vector2(gmgr.GraphicsDevice.Viewport.Width / 2, gmgr.GraphicsDevice.Viewport.Height / 2));
            sg.SetFreeze(true);
        }

        public override void OnCollisionWith(BasicSprite other)
        {
            DoDie();
        }

    }


}
