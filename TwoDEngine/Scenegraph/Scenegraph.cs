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


namespace TwoDEngine.Scenegraph
{
    /// <summary>
    /// This is the scene graph manager It registeres itself as a DrawableGameComponent to
    /// receieve XNA Update and Draw calls.  It also registers itself both with XNA's service
    /// registry and TwoDEngine' (superior) Registry.
    /// </summary>
    public class Scenegraph : Microsoft.Xna.Framework.DrawableGameComponent, SceneObjectParent
    {
       
        /// <summary>
        /// A sprite batch used to  render all SceneObjects in the scene graph
        /// </summary>
        SpriteBatch sceneGraphBatch;

        SpriteSortMode spriteSortMode;
        BlendState spriteBlendState;
        SamplerState spriteSamplerState;
        DepthStencilState spriteDepthStencilState;
        Effect spriteEffect;
        RasterizerState spriteRasterizerState;
        

        /// <summary>
        /// The scenegraph itself is the root of the scenegraph tree.  This holds a list of
        /// its' children
        /// </summary>
        List<SceneObject> children = new List<SceneObject>();

        /// <summary>
        /// When true, this stops all scenegraph updating
        /// </summary>
        bool freeze =false;

        /// <summary>
        /// The list of chidlren to remove at the end of the child updates
        /// </summary>
        List<SceneObject> childRemovalList = new List<SceneObject>();
        /// <summary>
        /// The list of chidlren to add at the end of the child updates
        /// </summary>
        List<SceneObject> childAdditionList = new List<SceneObject>();
        
        /// <summary>
        /// Constructs a scenegraph for the passed in Game.
        /// Registers itself both with XNA's Services registry and TwoDEngine' (superior) Registry.
        /// </summary>
        /// <param name="game"></param>
        public Scenegraph(Game game,SpriteSortMode sortMode= SpriteSortMode.Deferred,BlendState blendState=null,
            SamplerState samplerState = null,DepthStencilState depthStencilState = null,
                RasterizerState rasterizerState = null, Effect effect=null)
            : base(game)
        {
            //
            spriteBlendState = blendState==null?BlendState.AlphaBlend:blendState; //necc because cannot be default param
            spriteSamplerState = samplerState==null?SamplerState.LinearClamp:samplerState; // as above
            spriteDepthStencilState = depthStencilState==null?DepthStencilState.None:depthStencilState; // as above
            spriteRasterizerState = rasterizerState==null? RasterizerState.CullCounterClockwise:rasterizerState;
            spriteEffect = effect;
            // TODO: Construct any child components here\\Components.Add(scenegraph);
            game.Services.AddService(typeof(Scenegraph), this);
            game.Components.Add(this);
            Registry.Register(this);
        }

        



        /// <summary>
        /// Creates the SpriteBatch used for rendering
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            sceneGraphBatch = new SpriteBatch(this.Game.GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// This is called by teh XNA engine once per frame. It in turn calls 
        /// all its children's Update mthods.  (Which in turn does internal hosuekeeping
        /// and then calls the SceneObject sub-type specific UpdateMe call.)
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // let every scenegraph object do its update stuff
            if (!freeze)
            {
                foreach (SceneObject obj in children)
                {
                    obj.Update(gameTime, this);
                }
            }
            
            // update children lists on entire graph
            foreach (SceneObject child in childRemovalList)
            {
                children.Remove(child);
            }
            foreach (SceneObject child in childAdditionList)
            {
                children.Add(child);
            }
            childRemovalList.Clear();
            childAdditionList.Clear();
            foreach (SceneObject child in children)
            {
                child.UpdateChildLists();
            }
           
            
        }


        public void SetFreeze(bool b)
        {
            freeze = b;
        }

        /// <summary>
        /// Draws the scenegraph to the screen by starting a top down descent of
        /// game objects.  Each game object handles its own children.  Each child calls
        /// its GameObject subtype specific DrawMe to render to the sprite batch
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            sceneGraphBatch.Begin(spriteSortMode,spriteBlendState,spriteSamplerState,spriteDepthStencilState,spriteRasterizerState,
                spriteEffect);
            foreach (SceneObject obj in children)
            {
                obj.Draw(gameTime, sceneGraphBatch);
            }
            sceneGraphBatch.End();
        }

        /// <summary>
        /// Returns the gobal tramsform matrix of this object which is always the Identity
        /// matrix.
        /// </summary>
        /// <returns>The Identity matrix</returns>
        public Matrix GetGlobalMatrix()
        {
            return Matrix.Identity;
        }

        /// <summary>
        /// Adds a child to this object's child list
        /// </summary>
        /// <param name="child">A SceneObject to parent to the root</param>
        public void AddChild(SceneObject child)
        {
            childAdditionList.Add(child);
        }

        /// <summary>
        /// Removes a child to this object's child list
        /// </summary>
        /// <param name="child">A SceneObject to parent to the root</param>
        public void RemoveChild(SceneObject child)
        {
            childRemovalList.Add(child);
        }
    }
}
