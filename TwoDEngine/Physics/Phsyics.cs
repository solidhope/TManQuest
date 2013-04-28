using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using System.Diagnostics;

namespace TwoDEngine.Physics
{
    public class Physics: GameComponent
    {
        Game game;
        World physicsWorld;
        System.Threading.Thread physicsThread;
        float timeStep = 1.0f / 60;
       

        public Physics(Game game,Rectangle worldDimensions, Vector2 gravity,bool allowSleep=true):base(game)
        {
            // TODO: Construct any child components here\\Components.Add(scenegraph);
            this.game = game;
            game.Services.AddService(typeof(Physics), this);
            game.Components.Add(this);
            Registry.Register(this);
            CreateWorld(gravity);
        }

        public void CreateWorld(Vector2 gravity)
        {
           
            physicsWorld = new World(gravity);
        }

        public Collider CreateBoxCollider(Vector2 sz)
        {
            return new Colliders.BoxCollider(sz);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            
            base.Initialize();
            physicsWorld.Step(timeStep);
           
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
          
        }

    }
}
