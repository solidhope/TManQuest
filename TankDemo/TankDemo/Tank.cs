using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoDEngine.Scenegraph;
using TwoDEngine.Scenegraph.SceneObjects;
using Microsoft.Xna.Framework.Input;
using TwoDEngine.Physics;
using TwoDEngine.Physics.Colliders;

namespace TankDemo
{
    /// <summary>
    /// The tank is implemented as a BasicSprite with no image.
    /// It has the tank body as a child sprite, which in turns has the turret as its child
    /// </summary>
    public class Tank : BasicTilemapSprite
    {
       
        BasicSprite tankTurret;
        TileMap tileMap;
        float pixelPerSecSpeed=30.0f;
        Vector2 destinationPixel;
        Vector2 velocityPixPerSec;
        Boolean moving = false;
        float turretTurnSpeed = 1.0f;
        BasicSprite currentCollisionObject = null;

        public Tank(Game game, SceneObjectParent parent,TileMap tileMap):base(tileMap,parent,GetImage(game))
        {
            Texture2D tankTurretImage = game.Content.Load<Texture2D>(@"Textures/TankTurret");
           
            tankTurret = new BasicSprite(this, tankTurretImage);
            this.SetHandle(new Vector2(GetImage(game).Width / 2, GetImage(game).Height / 2));
            tankTurret.SetLocalPosition(new Vector2(GetImage(game).Width / 2, GetImage(game).Height / 2));
            tankTurret.SetHandle(new Vector2(7, 7));
            tankTurret.SetLocalRotation((float)Math.PI / 4);
            this.tileMap = tileMap;
        }

        public static Texture2D GetImage(Game game)
        {
            return game.Content.Load<Texture2D>(@"Textures/TankBody");
        }

     

        
        public void SetTurretRotation(float r)
        {
            tankTurret.SetLocalRotation(r);
        }

        public void MoveToCell(Vector2 cellDestination)
        {
           MoveToPixel(CellToPixel(cellDestination));
        }

        private void MoveToPixel(Vector2 dest)
        {
            Vector2 pos = GetLocalPosition();
            destinationPixel = dest;
            float dx = dest.X - pos.X;
            float dy = dest.Y-pos.Y;
            double theta = Math.Atan2(dy, dx);
            velocityPixPerSec = new Vector2((float)Math.Cos(theta)*pixelPerSecSpeed, (float)Math.Sin(theta) * pixelPerSecSpeed);
            moving = true;
        }

        protected override void UpdateMe(GameTime gameTime, Scenegraph graph)
        {
            base.UpdateMe(gameTime, graph);
            // get input
            Vector2 currentPos = GetLocalPosition();
            KeyboardState state = Keyboard.GetState();
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            #region Input code
            if (state.IsKeyDown(Keys.W))
            {
                currentPos.Y -= pixelPerSecSpeed*time;
                this.SetLocalRotation((float)-Math.PI / 2);
                moving = true;
            }
            if (state.IsKeyDown(Keys.S))
            {
                currentPos.Y += pixelPerSecSpeed * time;
                this.SetLocalRotation((float)Math.PI / 2);
                moving = true;
            }
            if (state.IsKeyDown(Keys.A))
            {
                currentPos.X -= pixelPerSecSpeed * time;
                this.SetLocalRotation((float)Math.PI);
                moving = true;
            }
            if (state.IsKeyDown(Keys.D))
            {
                currentPos.X += pixelPerSecSpeed * time;
                this.SetLocalRotation(0);
                moving = true;
            }

            
            //check tilemap blocking
            Vector2 size = GetSize();
            Vector2 position = GetLocalPosition();
            #endregion

            // move turret
            #region Move Turret
            double deltaT = gameTime.ElapsedGameTime.TotalSeconds;
            if (state.IsKeyDown(Keys.Q))
            {
                tankTurret.SetLocalRotation(tankTurret.GetLocalRotation() + 
                    (float)(deltaT* turretTurnSpeed));
            }
            if (state.IsKeyDown(Keys.E))
            {
                tankTurret.SetLocalRotation(tankTurret.GetLocalRotation() - 
                    (float)(deltaT * turretTurnSpeed));
            }
            #endregion

            Vector2 cellPos = PixelToCell(currentPos);
            if (tileMap.GetTileIndex("blocking",cellPos)==0){
                SetLocalPosition(currentPos);
            }
            // do pickup
            if (state.IsKeyDown(Keys.Space) && (currentCollisionObject != null))
            {
                PickupCurrentCollisionObject();
            }
        }

        private void PickupCurrentCollisionObject()
        {
            currentCollisionObject.Destroy();
        }

        public override void OnCollisionEnterWith(BasicSprite other)
        {
            base.OnCollisionEnterWith(other);
            Console.WriteLine("Collision enter");
            currentCollisionObject = other;
        }

        public override void OnCollisionExitWith(BasicSprite other)
        {
            base.OnCollisionEnterWith(other);
            Console.WriteLine("Collision exit");
            currentCollisionObject = null;
        }
    }
}
