using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TwoDEngine;
using TwoDEngine.Scenegraph;
using TwoDEngine.Scenegraph.SceneObjects;

namespace TankDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Scenegraph scenegraph;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            scenegraph = new Scenegraph(this);
            Registry.Register(graphics);
       
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
     
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            TileMap map = new TileMap(this,Registry.Lookup<Scenegraph>(), @"Content\SampleTileMap.tmx");
            
            Tank tank = new Tank(this,map,map);
            tank.SetLocalPosition(new Vector2(GraphicsDevice.Viewport.Width/2,GraphicsDevice.Viewport.Height/2));
           // tank.SetTurretRotation((float)Math.PI / 8);
            foreach (Vector2 cellPos in map.GetCellsWithProperty("placement", "Item"))
            {
                int itemIdx = map.GetTileIndex("placement",cellPos);
                string itemType = map.GetTileProperties(itemIdx)["Item"];
                switch (itemType)
                {
                    case "Turret":
                        BasicTilemapSprite turret= new BasicTilemapSprite(map,map,Content.Load<Texture2D>(@"Textures/TankTurret"));
                        turret.SetLocalCellPosition(cellPos);
                        tank.CollidesWith(turret);
                        break;
                }
            }
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
