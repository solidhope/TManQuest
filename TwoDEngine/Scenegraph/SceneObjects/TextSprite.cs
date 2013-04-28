using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoDEngine.Scenegraph.SceneObjects
{
    public class TextSprite : BasicTilemapSprite
    {
        SpriteFont font;
        string text;
        Color color = Color.White;

        public TextSprite(TileMap map, SceneObjectParent parent, SpriteFont font, string text):base(map,parent){
            this.font = font;
            this.text = text;
        }

        protected override void DrawAt(SpriteBatch batch,Vector2 scale, float rotation, Vector2 translation, int priority)
        {
            batch.DrawString(font, text, translation, color, rotation, Vector2.Zero, scale, SpriteEffects.None, priority);
        }

        public override Vector2 GetSize()
        {
            return font.MeasureString(text);
        }
    }
}
