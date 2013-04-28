using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoDEngine.Scenegraph
{
    /// <summary>
    /// This interface defiens a  public facing set of methods that all scene objects in the
    /// scene graph must implement
    /// </summary>
    public interface SceneObject
    {
        /// <summary>
        /// This method is called by the Scemegraph to allow for update of
        /// the object's state.  It is called once per frame.
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime, Scenegraph graph);

        /// <summary>
        /// This method is called post update to give the grap ha chance to update
        /// its actual chidl lists.
        /// This has to be seperated from Update so the child lists are not mutated in the
        /// middle of scanning for child updates
        /// </summary>
        void UpdateChildLists();

        /// <summary>
        /// This method is called to draw the object to the screen, it is called once per frame.
        /// </summary>
        /// <param name="gameTime"></param>
        void Draw(GameTime gameTime, SpriteBatch batch);
     
        /// <summary>
        ///  This sets or resets the obejct's draw priority in the scenegraph draw list.
        ///  Higher numbered objects are drawn first
        /// </summary>
        /// <param name="drawPriority"></param>
        void SetPriority(int drawPriority);

        /// <summary>
        /// returns priority for comparison purposes
        /// </summary>
        /// <returns></returns>
        int GetPriority();

        /// <summary>
        /// This sets the object's local transformation matrix
        /// </summary>
        /// <param name="matrix"></param>
        void SetLocalMatrix(Matrix matrix);

        /// <summary>
        /// Called when parent matrix changes
        /// </summary>
        /// <param name="parentMatrix">the global matrix of this object's parent</param>
        void UpdateGlobalMatrix(Matrix parentMatrix);

        /// <summary>
        /// removes the object from the scenegraph and frees all resources
        /// </summary>
        void Destroy();
    }
}
