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

namespace SidescrollerDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        JumpingPlayer player;
        Vector2 playerStartCell;
        TileMap tileMap;

        public Game1()
        {
            Registry.Register(this.Content);
            graphics = new GraphicsDeviceManager(this);
            Registry.Register(graphics);
            new Scenegraph(this);
            Content.RootDirectory = "Content";

            // for debuggign sometiems useful to see more screen
            //graphics.PreferredBackBufferHeight = 1000;
            //graphics.PreferredBackBufferWidth = 800;

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
            tileMap = new TileMap(this, Registry.Lookup<Scenegraph>(), @"Content\main.tmx");
            //tileMap.SetScale(new Vector2(2f, 2f));
            // tileMap.SetLocalPosition(new Vector2(32, 32));
            Console.WriteLine("TM pos= " + tileMap.GetLocalPosition());
            playerStartCell = tileMap.GetCellsWithProperty("placement", "Type", "Player")[0];
            playerStartCell.Y -= 2;

            // player movement speed
            SpriteFont font = Content.Load<SpriteFont>(@"ScreenFont");
            player = new JumpingPlayer(font, tileMap, tileMap, GetPlayerImages(), 75);
            //player = new JumpingPlayer(font, tileMap, tileMap, new SimpleSpriteImage(playerImage), 50);
            ResetPlayer();
            // load level monsters
            /* Vector2[] monsterCells = tileMap.GetCellsWithProperty("placement", "Type", "MOB");
             foreach (Vector2 monsterCell in monsterCells)
             {
                 int tileIndex = tileMap.GetTileIndex("placement",monsterCell);
                 Dictionary<string, string> monsterProperties = tileMap.GetTileProperties(tileIndex);
                 Monster monster = new Monster(tileMap,tileMap,playerImage, 50f,monsterProperties);
                 monster.SetLocalCellPosition(monsterCell);
                 player.CollidesWith(monster);
             }*/



            // TODO: use this.Content to load your game content here*/
        }

        private SpriteImage[] GetPlayerImages()
        {
            SpriteImage[] images = new SpriteImage[3];
            SimpleSpriteImage img = new SimpleSpriteImage(Content.Load<Texture2D>("Player"));
            images[0] = img;
            images[1] = img;
            images[2] = img;
            return images;
        }

        private void ResetPlayer()
        {
            player.SetLocalCellPosition(playerStartCell);
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
            ManuallyScrollTileMap(gameTime.ElapsedGameTime.TotalSeconds);
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void ManuallyScrollTileMap(double dt)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Left))
            {
                Vector2 pos = tileMap.GetLocalPosition();
                pos.X -= (float)(10 * dt);
                tileMap.SetLocalPosition(pos);
            }
            if (state.IsKeyDown(Keys.Right))
            {
                Vector2 pos = tileMap.GetLocalPosition();
                pos.X += (float)(10 * dt);
                tileMap.SetLocalPosition(pos);
            }
            if (state.IsKeyDown(Keys.Up))
            {
                Vector2 pos = tileMap.GetLocalPosition();
                pos.Y -= (float)(10 * dt);
                tileMap.SetLocalPosition(pos);
            }
            if (state.IsKeyDown(Keys.Down))
            {
                Vector2 pos = tileMap.GetLocalPosition();
                pos.Y += (float)(10 * dt);
                tileMap.SetLocalPosition(pos);
            }

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
