using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoDEngine.Scenegraph;
using TwoDEngine.Scenegraph.SceneObjects;
using Microsoft.Xna.Framework.Input; 

namespace TankDemo
{
    /// <summary>
    /// The tank is implemented as a BasicTilemapSprite with no image.
    /// It has the tank body as a child sprite, which in turns has the turret as its child
    /// </summary>
    public class Tank : BasicTilemapSprite
    {
        BasicSprite tankBody;
        BasicSprite tankTurret;
        TileMap tileMap;
        float pixelPerSecSpeed=30.0f;
        Vector2 destinationPixel;
        Vector2 velocityPixPerSec;
        Boolean moving = false;
        float turretTurnSpeed = 1.0f;


        /// <summary>
        /// This is the constructor for a Tank object
        /// </summary>
        /// <param name="game">The Game to use to load the assets</param>
        /// <param name="parent">The parent scene object to make the tank
        /// a child of</param>
        /// <param name="tileMap">The tilemap to scroll when we get too close to an edge</param>
        public Tank(Game game, SceneObjectParent parent,TileMap tileMap):base(tileMap,parent)
        {
            //Load the images of the tank body and turret
            Texture2D tankBodyImage = game.Content.Load<Texture2D>(@"Textures/TankBody");
            Texture2D tankTurretImage = game.Content.Load<Texture2D>(@"Textures/TankTurret");
            // Now make the tank body a basicSprite that is a child of the Tank
            tankBody = new BasicSprite(this, tankBodyImage);
            // Now make the tank turret a BasicSprite that is a child of the body
            tankTurret = new BasicSprite(tankBody, tankTurretImage);
            // set the rotation and translation origin of the tank body at its center
            tankBody.SetHandle(new Vector2(tankBodyImage.Width / 2, tankBodyImage.Height / 2));
            // set the turret's origin at the approximate center of the round part
            tankTurret.SetHandle(new Vector2(7, 7));
            // set the turret's position as being at the center of the tank
            tankTurret.SetLocalPosition(new Vector2(tankBodyImage.Width / 2, tankBodyImage.Height / 2));
            // save the tile map
            this.tileMap = tileMap;
        }

        /// <summary>
        /// This method rotates the turret on the tank
        /// </summary>
        /// <param name="r"></param>
        public void SetTurretRotation(float r)
        {
            tankTurret.SetLocalRotation(r);
        }

        /// <summary>
        /// This method is called to start the tank rolling towards another cell on the tile map.
        /// The coordinates are converted to pixel coordinates and sent to the real movment code
        /// </summary>
        /// <param name="cellDestination">The coordinates of the destination cell</param>
        public void MoveToCell(Vector2 cellDestination)
        {
           MoveToPixel(CellToPixel(cellDestination));
        }

        /// <summary>
        /// This method starts the tnak rolling towards a destination pixel
        /// </summary>
        /// <param name="dest">The parent-local coordinates of the destination pixel</param>
        private void MoveToPixel(Vector2 dest)
        {
            Vector2 pos = GetLocalPosition(); //get our current pixel position
            destinationPixel = dest;  // set the destination for later refernce
            float dx = dest.X - pos.X; // the difference between start and end X coords -- the adjacent of theta
            float dy = dest.Y - pos.Y; // the difference between start and end X coords -- the opposite of theta
            double theta = Math.Atan2(dy, dx); // find theta, Tangent = O/A
            // find the velocity in X and Y per second based on theta and the total movement rate per second
            velocityPixPerSec = new Vector2((float)Math.Cos(theta)*pixelPerSecSpeed, (float)Math.Sin(theta) * pixelPerSecSpeed);
            // tell update we are now moving
            moving = true;
        }

        /// <summary>
        /// The scenegraph itself gets an update epr frame.  It does some internal things 
        /// and then descends the tree calling each node's UpdateMe
        /// </summary>
        /// <param name="gameTime"> The elapsed time since the last update</param>
        /// <param name="graph">The scenegraph that is calling this UpdateMe</param>
        protected override void UpdateMe(GameTime gameTime, Scenegraph graph)
        {
            base.UpdateMe(gameTime, graph); // let any base classes do their updating
            Vector2 cellPos = GetLocalCellPosition(); // get our current position for movement calculation
           
            // get input keys and update  based on them
            KeyboardState state = Keyboard.GetState();
            if (!moving) // if we are moving then we dont want to repsond to keys until we get where we are going
            {
                #region Input code
                if (state.IsKeyDown(Keys.W))
                {
                    cellPos.Y--; // set the destination one up from where we are
                    tankBody.SetLocalRotation((float)-Math.PI / 2); // point the body the right way
                    moving = true; // tell future updates we are moving
                }
                if (state.IsKeyDown(Keys.S))
                {
                    cellPos.Y++; // set the destination one down from where we are
                    tankBody.SetLocalRotation((float)Math.PI / 2);  // point the body the right way
                    moving = true;// tell future updates we are moving
                }
                if (state.IsKeyDown(Keys.A))
                {
                    cellPos.X--;  // set the destination one left from where we are
                    tankBody.SetLocalRotation((float)Math.PI); // point the body the right way
                    moving = true;// tell future updates we are moving
                }
                if (state.IsKeyDown(Keys.D))
                {
                    cellPos.X++;  // set the destination one right from where we are
                    tankBody.SetLocalRotation(0); // point the body the right way
                    moving = true;// tell future updates we are moving
                }

               
                //check tilemap blocking, if the destination tile is blocked, then abort the move
                if (tileMap.GetTileIndex("blocking", cellPos) > 0)
                { // blocking tile at this square
                    moving = false;
                }
                // If were still moving then we werent blocked so go ahead and set the movement
                if (moving)
                { // not blocked
                    MoveToCell(cellPos);
                }
                #endregion
            }
            // turn turret based on Q and E
            double deltaT = gameTime.ElapsedGameTime.TotalSeconds; // get time delta to use below
            if (state.IsKeyDown(Keys.Q))
            {
                tankTurret.SetLocalRotation(tankTurret.GetLocalRotation() + // turn clockwise
                    (float)(deltaT* turretTurnSpeed)); 
            }
            if (state.IsKeyDown(Keys.E))
            {
                tankTurret.SetLocalRotation(tankTurret.GetLocalRotation() - // turn counter clockwise
                    (float)(deltaT * turretTurnSpeed));
            }
            
            // now do motion
            if (moving)
            {
               
                // check to see if we overshot and end motion
                // calculate distance to goal from where we are
                // We use a trick to avoid the square root in the distance claculation
                // movement speed is always >=0
                // if X& Y are both >=0  then IFF (X*X)>(Y*Y) THEN X>Y
                // so the comaprison is the same whether we take the quare root or not
                // as long as we comapre against another squred value
                Vector2 pos = GetLocalPosition();
                float dx = destinationPixel.X - pos.X;
                float dy = destinationPixel.Y - pos.Y;
                // Note this is a distance claculation without the final SQRT
                float distanceToGoalSqd = (dx * dx) + (dy * dy);
                // find the distance traveled since last update
                float distanceTraveled = (float)(deltaT * pixelPerSecSpeed);
                // calculate the distance squared for comparesion (much cheaper then SQRT)
                float distanceTraveledSqd = distanceTraveled * distanceTraveled;
                // compare.  If we moved a distance equal to or greater then the distance to the goal
                // for our last position, then we have reached our goal so just set us there and
                //end the move
                if (distanceToGoalSqd <= distanceTraveledSqd)
                {
                    SetLocalPosition(destinationPixel);
                    moving = false;
                } else {
                    // otehrwise move closer
                    SetLocalPosition(new Vector2(
                        pos.X + (float)(velocityPixPerSec.X * deltaT),
                        pos.Y + (float)(velocityPixPerSec.Y * deltaT)));
                }
            }
        }


    }
}
