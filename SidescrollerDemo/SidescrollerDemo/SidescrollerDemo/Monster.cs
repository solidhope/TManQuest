using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoDEngine;
using TwoDEngine.Physics;
using TwoDEngine.Scenegraph;
using TwoDEngine.Scenegraph.SceneObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SidescrollerDemo
{
    public class Monster: MOB
    {
        MOBAI ai;

        public Monster(TileMap map, SceneObjectParent parent, Texture2D image, 
            float moveSpeed, Dictionary<string, string> properties)
            :base(map,parent,image,moveSpeed)
        {
            SetHandle(new Vector2(image.Width/2,image.Height/2));
            foreach (string key in properties.Keys)
            {
                switch (key)
                {
                    case "AI":
                        switch (properties[key])
                        {
                            case "BackAndForth":
                                ai = new BackAndForthAI(this,properties);
                                break;
                        }
                        break;
                }
            }
        }

        public override void SetLocalPosition(Vector2 pos)
        {
            base.SetLocalPosition(pos);
            ai.PositionReset();
        }

        protected override void UpdateMe(GameTime gameTime, Scenegraph graph)
        {
            base.UpdateMe(gameTime, graph);
            ai.UpdateAI((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

    }
}
