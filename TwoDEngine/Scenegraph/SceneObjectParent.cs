using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoDEngine.Scenegraph
{
    /// <summary>
    /// This interface defines a set of public methods all obejcst that can have scene objects as
    /// children must implement.  All SceneObejcts are also SceneObjectParents, but not all
    /// SceneObjectParents are SceneObjects.  For instance, the Scenegraph is a SceneObjectParent but
    /// not a SceneObject itself.
    /// </summary>
    public interface SceneObjectParent
    {
        /// <summary>
        /// Returns the transformation matrix in the world space
        /// </summary>
        /// <returns></returns>
        Matrix GetGlobalMatrix();

        /// <summary>
        /// Adds a child to this SceneObjectParent's child list
        /// </summary>
        /// <param name="child">the child to add</param>
        void AddChild(SceneObject child);

        /// <summary>
        /// removes a child to this SceneObjectParent's child list
        /// </summary>
        /// <param name="child">the child to add</param>
        void RemoveChild(SceneObject child);
    }
}
