using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoDEngine.Scenegraph.SceneObjects
{
    /// <summary>
    /// This sub-class of BasicSprite adds TileMap awareness.
    /// Positoning can be done by either pixel or cell position, and
    /// conversion routines are provided to convert between the two coordinate systems
    /// 
    /// A test routine is also provided to see if the current image extents of the sprite
    /// impinge on a blocked tile.  This bases on there being a non zero entry on the map
    /// layer "blocking" for the blocked tile
    /// </summary>
    public class BasicTilemapSprite:MultiImageSprite
    {
        /// <summary>
        /// The tilemap this sprite is a  (maybe indirect) child of
        /// </summary>
        TileMap tileMap;

        /// <summary>
        ///  Creates a new BasicTilemapSprite parented to the passed in parent
        /// </summary>
        ///
        /// <param name="map">The tilemap this sprite is a (potentially indirect) child of</param>
        /// <param name="parent">The parent scene graph object</param>
        /// <param name="image">The image to draw for the sprite</param>
        public BasicTilemapSprite(TileMap map, SceneObjectParent parent, Texture2D image):base(parent,image){
            tileMap = map;
        }

        public BasicTilemapSprite(TileMap map, SceneObjectParent parent, SpriteImage[] simage)
            : base(parent, simage)
        {
            tileMap = map;
        }

        public BasicTilemapSprite(TileMap map, SceneObjectParent parent, SpriteImage simage)
            : base(parent, simage)
        {
            tileMap = map;
        }

        /// <summary>
        ///  Creates an empty (invisible) BasicTilemapSprite parented to the passed in parent
        /// </summary>
        ///
        /// <param name="map">The tilemap this sprite is a (potentially indirect) child of</param>
        /// <param name="parent">The parent scene graph object</param> /// <param name="image">The image to draw for the sprite</param>
        public BasicTilemapSprite(TileMap map, SceneObjectParent parent)
            : base(parent)
        {
            tileMap = map;
        }
        
        /// <summary>
        /// Sets the local position to the centroid of the cell passed in
        /// </summary>
        /// <param name="pos">The local positiuon in cell coordinates</param>
        public void SetLocalCellPosition(Vector2 pos)
        {
           
            SetLocalPosition(CellToPixel(pos));
        }

        /// <summary>
        /// COnverts a Vector2 from cell coordinates to the pixel coordinates of its centroid
        /// </summary>
        /// <param name="pos">the position in cell oordinates</param>
        /// <returns>the pixel coordinates of the cell center</returns>
        public Vector2 CellToPixel(Vector2 pos)
        {
            Vector2 tileSize = tileMap.GetCellSize();
            return new Vector2((pos.X + 0.5f) * tileSize.X, (pos.Y + 0.5f) * tileSize.Y);
        }

        /// <summary>
        /// Returns the local position of the sprite in cell coordinates
        /// </summary>
        /// <returns>The cell containg the sprite's local position</returns>
        public Vector2 GetLocalCellPosition()
        {
            Vector2 pixelPos = GetLocalPosition();
            return PixelToCell(pixelPos);
            
        }

        /// <summary>
        /// Converts a vector in pixel coordinates to the cell coordinates of its enclosing cell
        /// </summary>
        /// <param name="pixelPos">position in pixels</param>
        /// <returns>The cell containing the passed in pixel</returns>
        public Vector2 PixelToCell(Vector2 pixelPos)
        {
            Vector2 tileSize = tileMap.GetCellSize();
            return new Vector2((float)Math.Floor(pixelPos.X / tileSize.X), (float)Math.Floor(pixelPos.Y / tileSize.Y));
        }

        /// <summary>
        /// This method tests a the extents of the sprite for collision with a blockign sqaure or the edge of
        /// the tile map as if the sprite's local coordinates were newPos.
        /// Note:  The current test only tests the corners of the extents.  If a sprite is bigger then the
        /// cell size it is possible for a blocked cell to be wholly enclsoed and this will return false
        /// </summary>
        /// <param name="newPos">The position to test</param>
        /// <returns>true if a blocking cell is inetrsected</returns>

        public bool PositionCollidesWithBlocking(Vector2 newPos)
        {
            Vector2 imageSz = GetSpriteImage().GetCurrentImageSize();
            Vector2 topLeft = new Vector2(newPos.X- (imageSz.X / 2), newPos.Y - (imageSz.Y / 2));
            Vector2 topRight = new Vector2(newPos.X + (imageSz.X / 2), newPos.Y - (imageSz.Y / 2));
            Vector2 bottomLeft = new Vector2(newPos.X - (imageSz.X / 2), newPos.Y + (imageSz.Y / 2));
            Vector2 bottomRight = new Vector2(newPos.X + (imageSz.X / 2), newPos.Y + (imageSz.Y / 2));
            // check if off map, if so is automatically blocking
            Vector2 tileMapSz = GetTileMap().GetPixelSize();
            if ((topLeft.X < 0) || (bottomRight.X > tileMapSz.X) ||
                (topLeft.Y < 0) || (bottomRight.Y > tileMapSz.Y))
            {
                return true;
            }
            // check against blocking map
            return ((GetTileMap().GetTileIndex("blocking", PixelToCell(topLeft)) != 0)||
                    (GetTileMap().GetTileIndex("blocking", PixelToCell(topRight)) != 0)||
                    (GetTileMap().GetTileIndex("blocking", PixelToCell(bottomLeft)) != 0)||
                    (GetTileMap().GetTileIndex("blocking", PixelToCell(bottomRight)) != 0)); 
        }

        /// <summary>
        /// Acessor to get the TileMap set when this sprite was created
        /// </summary>
        /// <returns>the TileMap that is a (potentially indirect) parent of this sprite</returns>
        public TileMap GetTileMap()
        {
            return tileMap;
        }

        private Vector2 lastCell;
        protected override void UpdateMe(GameTime gameTime, Scenegraph graph)
        {
            
            base.UpdateMe(gameTime, graph);
            Vector2 newCell = GetLocalCellPosition();
            if (lastCell != newCell)
            {
                ExitedCell(lastCell);
                EnteredCell(newCell);
            }
            lastCell = newCell;
        }

        protected virtual void EnteredCell(Vector2 cellpos)
        {
           // default is nop
        }

        protected virtual void ExitedCell(Vector2 cellpos)
        {
            // default is nop
        }

       
    }
}
