using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoDEngine.Physics;

namespace TwoDEngine.Scenegraph.SceneObjects
{
    /// <summary>
    /// This class implements a Movable OBject, or MOB.
    /// This is a self moving sprite that has a veclocity and destination to move towards 
    /// </summary>
    public class MOB : BasicTilemapSprite
    {
        /// <summary>
        /// The movement speed of the sprite when in motion
        /// </summary>
        float pixelPerSecSpeed = 30.0f;
        /// <summary>
        /// The local pixel location we are moving towards
        /// </summary>
        Vector2 destinationPixel;
        /// <summary>
        /// This is a vector that holds the velocity and direction decomposed
        /// into dx and dy per second
        /// </summary>
        Vector2 velocityPixPerSec;
        /// <summary>
        /// A flag that says we are currently in motion
        /// </summary>
        Boolean moving = false;

        

        /// <summary>
        ///  Creates an MOB from the passed in image, parented to the passed in parent, that mvoes at the given speed
        /// </summary>
        ///
        /// <param name="map">The tilemap this sprite is a (potentially indirect) child of</param>
        /// <param name="parent">The parent scene graph object</param> /// <param name="image">The image to draw for the sprite</param>
        /// <param name="image">The image to draw for the sprite</param>
        /// <param name="pixelPerSecSpeed">The movement speed of the MOB</param>

        public MOB(TileMap map, SceneObjectParent parent, Texture2D image, float pixelPerSecSpeed = 0) : base(map, parent, image) {
            this.pixelPerSecSpeed = pixelPerSecSpeed;

        }

        public MOB(TileMap map, SceneObjectParent parent, SpriteImage[] simage, float pixelPerSecSpeed = 0)
            : base(map, parent, simage)
        {
            this.pixelPerSecSpeed = pixelPerSecSpeed;

        }

        public MOB(TileMap map, SceneObjectParent parent, SpriteImage simage, float pixelPerSecSpeed = 0)
            : base(map, parent, simage)
        {
            this.pixelPerSecSpeed = pixelPerSecSpeed;

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
        public void MoveToPixel(Vector2 dest)
        {
            Vector2 pos = GetLocalPosition(); //get our current pixel position
            destinationPixel = dest;  // set the destination for later refernce
            float dx = dest.X - pos.X; // the difference between start and end X coords -- the adjacent of theta
            float dy = dest.Y - pos.Y; // the difference between start and end X coords -- the opposite of theta
            double theta = Math.Atan2(dy, dx); // find theta, Tangent = O/A
            // find the velocity in X and Y per second based on theta and the total movement rate per second
            velocityPixPerSec = new Vector2((float)Math.Cos(theta) * pixelPerSecSpeed, (float)Math.Sin(theta) * pixelPerSecSpeed);
            // tell update we are now moving
            moving = true;
        }

        /// <summary>
        /// Returns the speed of this MOB
        /// </summary>
        /// <returns></returns>
        public float GetPPSSpeed()
        {
            return pixelPerSecSpeed;
        }

        /// <summary>
        /// Returns whether or not this MOB is currently in motion
        /// </summary>
        /// <returns>true if moving, else false</returns>
        public bool IsMoving()
        {
            return moving;
        }

        /// <summary>
        /// This method is responsible for the update of the MOB. It adds movement
        /// behavior to the  BasicTilemapSprite beahvior
        /// </summary>
        /// <param name="gameTime">elapsed tiem since last call</param>
        /// <param name="graph">scenegraph this MOb is (potentially indirectly) parented to</param>
        protected override void UpdateMe(GameTime gameTime, Scenegraph graph)
        {
            base.UpdateMe(gameTime, graph); // let any base classes do their updating
            
            // now do motion
            double deltaT = gameTime.ElapsedGameTime.TotalSeconds;
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
                }
                else
                {
                    // otehrwise move closer
                    SetLocalPosition(new Vector2(
                        pos.X + (float)(velocityPixPerSec.X * deltaT),
                        pos.Y + (float)(velocityPixPerSec.Y * deltaT)));
                }

                
            }
        }
    }
}
