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
        //GraphicsDeviceManager graphics;
        
        /// <summary>
        /// This is the initializer for the game object.  It sets
        /// up all global values and services
        /// </summary>
        public Game1()
        {
            // Create a Graphics Device Manager Service
            
            Registry.Register( new GraphicsDeviceManager(this));
            // Create a scenegraph service
            Registry.Register(new Scenegraph(this));
            // set the root directory for loading content
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
            // No furtehr initialization necessary
     
            base.Initialize();
        }

        /// <summary>
        /// This wil create the tile map and add it to the scenegraph,
        /// then create the tan kand add it as a child of the tilemap.
        /// </summary>
        protected override void LoadContent()
        {
            // This  creates the tilemap
            // The first paremeter is a refence to the game, used to load
            // content
            // The second parameter is the parent object of the tilemap in
            // the scenegraph.  The scenegraph itself acts as the root parent,
            // so we fetch it from the registry and pass it in
            TileMap map = new TileMap(this,Registry.Lookup<Scenegraph>(), @"Content\SampleTileMap.tmx");
            
            // This creates the tank. 
            // The first paremeter is a refence to the game, used to load
            // content
            // The second parameter is again the parent object,
            // in this case the tile map.
            // The third parameter is the TileMap this tank is on.
            // (It happens to be the parent but it wont encessarily alwaays be)
            // The tnak uses this to scroll the map when it gets near an edge
            Tank tank = new Tank(this,map,map);
            tank.SetLocalPosition(new Vector2(GraphicsDevice.Viewport.Width/2,GraphicsDevice.Viewport.Height/2));
          
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

            // All game drawing is done by the scene graph which
            // get called directly from the Xna engine since it is a
            // DrawableGameComponent

            base.Draw(gameTime);
        }
    }
}
