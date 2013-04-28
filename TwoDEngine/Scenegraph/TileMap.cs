using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Squared.Tiled; // open source contributed Tiled map loader
using TiledMap =  Squared.Tiled.TileMap; // open source contributed Tiled map loader

namespace TwoDEngine.Scenegraph.SceneObjects
{
    /// <summary>
    /// This is a special kind of sprite that uses the rendering of a tile map as its imagery
    /// and provides query functions on the tile map data
    /// Other then that its just like a BasicSprite
    /// </summary>
    public class TileMap : AbstractSceneObject
    {
        /// <summary>
        /// This is an instance of a (modified) thrid party class that actually loads the TileD map information and
        /// holds it for us
        /// </summary>
        TiledMap tiledMap;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="parent"></param>
        /// <param name="tilemapFile"> The name of a TMX (tiled XML) format file. Note that Tiled MUST be set in preferences to
        /// store level data as uncompressed XML</param>
        public TileMap(Game game, SceneObjectParent parent,String tilemapFile)
            : base(parent)
        {
            tiledMap = TiledMap.Load(tilemapFile, game.Content);
        }

        /// <summary>
        /// Returns the size in pixels of a cell on the tile map
        /// </summary>
        /// <returns>the size of a single cell in pixels, x==width  y===height</returns>
        public Vector2 GetCellSize()
        {
            return new Vector2(tiledMap.TileWidth, tiledMap.TileHeight);
        }

        /// <summary>
        /// Returns the tile index number of a tile cell at the passed cell coordinate in the passed
        /// in layer.
        /// </summary>
        /// <param name="layerName">The name of the layer to be read</param>
        /// <param name="pos">The coordinates of the cell in cell coordinates</param>
        /// <returns>0 if the specified cell position is empty</returns>
        /// <exception cref="NonExistantLayerException">if the passed in layer is not in the current tile map</exception>
        public int GetTileIndex(string layerName, Vector2 pos)
        {
            Layer layer = tiledMap.GetLayer(layerName);
            if (layer == null)
            {
                throw new NonExistantLayerException("Layer " + layerName + " not found in current tile map.");
            }
            else
            {
                return layer.GetTile((int)pos.X, (int)pos.Y);
            }
        }

        /// <summary>
        /// This method gets all cells in the passed in layer whose tiles have the passed in property
        /// </summary>
        /// <param name="layerName">the name of the layer to query</param>
        /// <param name="key">A key to match in the tile parameters</param>
        /// <param name="value">A value to match, or null to match all values</param>
        /// <returns>An array of the cell coordinates of all the macthing cells</returns>
        /// <exception cref="NonExistantLayerException">if the passed in layer is not in the current tile map</exception>
        public Vector2[] GetCellsWithProperty(string layerName, string key, string value=null)
        {
            Layer layer = tiledMap.GetLayer(layerName);
            if (layer == null)
            {
                throw new NonExistantLayerException("Layer " + layerName + " not found in current tile map.");
            }
            else
            {
                List<Vector2> cellList = new List<Vector2>();
                layer.ForEveryCell((Vector2 pos) =>
                {
                    int tileNum = layer.GetTile((int)pos.X, (int)pos.Y);
                    if (tileNum > 0)
                    { // there is a tile in this cell
                        Tileset tileset = tiledMap.GetTilesetForTile(tileNum);
                        Tileset.TilePropertyList properties = tileset.GetTileProperties(tileNum);
                        if (properties.ContainsKey(key))
                        {
                            if ((value == null) || (properties[key] == value))
                            {
                                cellList.Add(new Vector2(pos.X, pos.Y));
                            }
                        }
                    }
                });

                return cellList.ToArray();
            }
        }

        /// <summary>
        /// Returns the dimensions of the entire map in pixels
        /// </summary>
        /// <returns>the size in pixes</returns>
        public Vector2 GetPixelSize()
        {
            return new Vector2(tiledMap.Width * tiledMap.TileWidth, tiledMap.Height * tiledMap.TileHeight);
        }

        public Dictionary<string, string> GetTileProperties(int tileNum)
        {
            Tileset tileset = tiledMap.GetTilesetForTile(tileNum);
            return tileset.GetTileProperties(tileNum);
        }

        /// <summary>
        /// This overrides the BasicSprite draw that draws one or a squence of image with code
        /// that draws multiple layered arrays of tiles.
        /// It will not draw layers that have ahd their visibility toggled off in  the tile editor
        /// </summary>
        /// <param name="batch">a SpriteBatch to draw the tiles to</param>
        /// <param name="scale">a scale to scale all images to</param>
        /// <param name="rotation">a rotation to rotate the tiles</param>
        /// <param name="translation">a pixel position to start draw at</param>
        /// <param name="priority"> a priority to draw all the tiles at</param>
        protected override void DrawAt(SpriteBatch batch, Vector2 scale, float rotation, Vector2 translation, int priority)
        {
            

            //TODO: rewrite draw to allow for scaling
            //tiledMap.Draw(batch, new Rectangle(0, 0,tiledMap.Width*tiledMap.TileWidth, tiledMap.Height*tiledMap.TileHeight), vp); 
            // drawing logic
            Vector2 screenOrigin = translation; // already scaled by matrix, do not scale!
            Viewport vp = Registry.Lookup<GraphicsDeviceManager>().GraphicsDevice.Viewport;
            Rectangle screenRect = new Rectangle(-(int)screenOrigin.X,-(int)screenOrigin.Y, vp.Width, vp.Height);
            Vector2 mapPixelSize = GetPixelSize()*scale;
            Vector2 tileSize = GetCellSize()*scale;
            Rectangle clippedTMCoords = Rectangle.Intersect(new Rectangle(0, 0, 
                (int)mapPixelSize.X,(int) mapPixelSize.Y),screenRect);
            Rectangle clippedTMTileCoords = new Rectangle((int)Math.Floor(clippedTMCoords.X / tileSize.X),
                                                          (int)Math.Floor(clippedTMCoords.Y / tileSize.Y),
                                                          (int)Math.Ceiling(clippedTMCoords.Width / tileSize.X)+1,
                                                          (int)Math.Ceiling(clippedTMCoords.Height / tileSize.Y)+1);
            foreach(Layer layer in tiledMap.GetLayersInOrder()){
                if ((layer.Visible==1)) for (int y = 0; y < clippedTMTileCoords.Height; y++)
                {
                    for (int x =0; x < clippedTMTileCoords.Width; x++)
                    {
                        int tx = clippedTMTileCoords.X + x;
                        int ty = clippedTMTileCoords.Y + y;
                        
                        
                        if ((tx >= 0) && (tx < tiledMap.Width)&&
                            (ty >= 0) && (ty < tiledMap.Height))
                        {
                            int tileIdx = layer.GetTile(tx,ty);
                            if (tileIdx != 0)
                            {
                                Tileset tset = tiledMap.GetTilesetForTile(tileIdx);
                                Rectangle tileLocationInImage = new Rectangle();
                                if (tset.MapTileToRect(tileIdx, ref tileLocationInImage))
                                {

                                    Vector2 pos = new Vector2((tx*tileSize.X)+screenOrigin.X,(ty*tileSize.Y)+screenOrigin.Y);
                                    batch.Draw(tset.Texture,// tileset image
                                             new Rectangle((int)pos.X, (int)pos.Y, (int)tileSize.X, (int)tileSize.Y),
                                             tileLocationInImage, Color.White);
                                }
                                else
                                {
                                    Console.WriteLine("error, did not find tile number " + tileIdx + " in tile set " + tset.Name);
                                }
                            }
                        }
                    } 
                }
            }
        }

        /// <summary>
        /// currently does nothing
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="graph"></param>
        protected override void UpdateMe(GameTime gameTime, Scenegraph graph)
        {
            
            //TODO Could put support for animated tiles here...
        }


        public override Vector2 GetSize()
        {
            return GetPixelSize();
        }

        public override void OnDestroy()
        {
            throw new NotImplementedException();
        }
    }
}
