using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoDEngine.Scenegraph
{
    /// <summary>
    /// This is a base class that has all the child logic for implementing scene nodes
    /// It supports local and global coordinates and cooperates with the Scenegraph for rendering
    /// 
    /// It defines two abstract methods that must be  implemented by concrete sub-classes:
    /// UpdateMe -- called once per frame to allow the obejct to upate itself
    /// DrawMe -- called once perame to allow the object to render itself
    /// (see complete definitions below)
    /// 
    /// This class implements a delayed child list mutation startegy to assure that changes dont 
    /// interract with scanning for update
    /// </summary>
    public abstract class AbstractSceneObject : SceneObject, SceneObjectParent
    {
        /// <summary>
        /// The local transform mtrix to transform from the parent's coordinate system
        /// to thsi child's coordinate system
        /// </summary>
        Matrix localMatrix = Matrix.Identity;
        /// <summary>
        /// The global matrix that holds the transform from gloabl space to
        /// this child's coordinate system
        /// </summary>
        Matrix globalMatrix = Matrix.Identity;

        Matrix inverseGlobalMatrix;
        /// <summary>
        /// The draw priority for screen ordering
        /// </summary>
        int drawPriority;
        /// <summary>
        /// This child's parent object.  The root of the tree is the Scenegraph object itself
        /// </summary>
        SceneObjectParent parent;
        /// <summary>
        /// A list of this node's child nodes
        /// </summary>
        private List<SceneObject> children = new List<SceneObject>();

        /// <summary>
        /// The list of chidlren to remove at the end of the child updates
        /// </summary>
        List<SceneObject> childRemovalList = new List<SceneObject>();
        /// <summary>
        /// The list of chidlren to add at the end of the child updates
        /// </summary>
        List<SceneObject> childAdditionList = new List<SceneObject>();

        /// <summary>
        /// The sprite's current local position in the parent-gloabl coordinate space
        /// </summary>
        Vector2 position = Vector2.Zero;
        /// <summary>
        /// The sprite's current local rotation in parent-gloabl coordinate space
        /// </summary>
        float rotation = 0;
        /// <summary>
        /// The handle by which the sprite is paled and moved in image-local coordinates
        /// </summary>
        Vector2 origin = Vector2.Zero;


        Vector2 scale = new Vector2(1,1);

        //  Global Values
        /// <summary>
        /// Current position in world space
        /// </summary>
        Vector2 globalPosition = Vector2.Zero;

        /// <summary>
        /// Current rotation in world space
        /// </summary>
        float globalRotation = 0f;

        /// <summary>
        /// currentscale in world space
        /// </summary>
        Vector2 globalScale = new Vector2(1,1);


        bool destroyed = false;
        /// <summary>
        /// Constructor that just stores the parent and adds this object to the parent's child list
        /// </summary>
        /// <param name="parent"></param>
        
        public AbstractSceneObject(SceneObjectParent parent)
        {
            this.parent = parent;
            parent.AddChild(this);
        }


        /// <summary>
        /// Used internally to propegate lcoal matrix chnages to children's global matricies
        /// </summary>
        private void UpdateChildMatrices()
        {
            foreach (SceneObject child in children)
            {
                child.UpdateGlobalMatrix(globalMatrix);
            }
            foreach (SceneObject child in childAdditionList) // update pending adds
            {
                child.UpdateGlobalMatrix(globalMatrix);
            }
        }

        /// <summary>
        /// Updates this node's gloabl matrix based on the parent's global matrix, which
        /// is passed in and this node's local matrix
        /// </summary>
        /// <param name="parentMatrix"></param>
        public virtual void UpdateGlobalMatrix(Matrix parentMatrix){
            globalMatrix = localMatrix * parentMatrix;
            inverseGlobalMatrix = Matrix.Invert(globalMatrix);
            Quaternion quat;
            Vector3 pos;
            Vector3 scl;
            globalMatrix.Decompose(out scl, out quat, out pos);
            globalPosition = new Vector2(pos.X, pos.Y);
            globalRotation = MathUtils.QuaternionToEuler(quat).Z;
            globalScale = new Vector2(scl.X, scl.Y);
            UpdateChildMatrices();
        }

        /// <summary>
        /// Returns this object's Global matrix
        /// </summary>
        /// <returns>the global matrix</returns>
        public Matrix GetGlobalMatrix()
        {
            return globalMatrix;
        }


        public Vector2 LocalToGlobal(Vector2 local)
        {
            return Vector2.Transform(local, globalMatrix);
        }


        public Vector2 GlobalToLocal(Vector2 global)
        {
            return Vector2.Transform(global, inverseGlobalMatrix);
        }
        /// <summary>
        /// Adds a child to this object's list of children
        /// </summary>
        /// <param name="child">the scene graph object to add as a child</param>
        public void AddChild(SceneObject child)
        {
            childAdditionList.Add(child);
            child.UpdateGlobalMatrix(globalMatrix);
        }

        /// <summary>
        /// Rempoves a child to this object's list of children
        /// </summary>
        /// <param name="child">the scene graph object to add as a child</param>
        public void RemoveChild(SceneObject child)
        {
            childRemovalList.Add(child);
        }

        /// <summary>
        /// Sets the draw priority of this object in comparision to other scene graph objects
        /// </summary>
        /// <param name="drawPriority">the priority as per SpriteBatch.Draw</param>
        public void SetPriority(int drawPriority)
        {
            this.drawPriority = drawPriority;
        }


        /// <summary>
        /// Returns the currently set priority
        /// </summary>
        /// <returns>the priority as per SpriteBatch.Draw</returns>
        public int GetPriority()
        {
            return drawPriority;
        }

        /// <summary>
        /// Returns the width and hiehgt of this sprite as drawn on the scren
        /// (not counting scaling)
        /// </summary>
        /// <returns></returns>
        public abstract Vector2 GetSize();

        /// <summary>
        /// This sets up the draw matrix and then calls the implementation
        /// specific draw code
        /// </summary>
        /// <param name="gameTime">elapsed time since last call</param>
        /// <param name="batch">the srpite batch to draw to</param>
        public void Draw(GameTime gameTime, SpriteBatch batch)
        {
            Vector3 scale, translation;
            Quaternion quat;
            float rotation;

            globalMatrix.Decompose(out scale,out quat, out translation);
            rotation = MathUtils.QuaternionToEuler(quat).Z; 
            DrawAt(batch, new Vector2(scale.X,scale.Y), rotation, new Vector2(translation.X,translation.Y),drawPriority);
            foreach(SceneObject child in children){
                child.Draw(gameTime, batch);        
            }
        }

       
   
        /// <summary>
        /// This MUST be overriden by sub-classes to do the actual drawing.
        /// In order to get the transformation from the global matrix, draw must
        /// be done using the passed in scale, translation and rotation
        /// </summary>
        /// <param name="batch">the sprite batch to draw to</param>
        /// <param name="scale">the scale to draw at</param>
        /// <param name="rotation">the rotation to draw with</param>
        /// <param name="translation"> to draw at</param>
        protected abstract void DrawAt(SpriteBatch batch,Vector2 scale,float rotation, Vector2 translation,int drawPriority);


        /// <summary>
        /// Calls the implementation specific UpdateMe and then dispacthes
        /// updates to all children
        /// </summary>
        /// <param name="gameTime">elapsed time since last call</param>
        /// <param name="graph">the scenegraph rendering this object</param>
        public void Update(GameTime gameTime, Scenegraph graph)
        {
            UpdateMe(gameTime, graph);
            foreach (SceneObject child in children)
            {
                child.Update(gameTime, graph);
            }

           
        }

        /// <summary>
        /// Scene object type specific update
        /// Must be overrriden to handle object update. implement as empty if there is no update logic
        /// </summary>
        /// <param name="gameTime">elapsed time since last call</param>
        /// <param name="graph">the scenegraph doing the updating</param>
        protected abstract void UpdateMe(GameTime gameTime, Scenegraph graph);


        /// <summary>
        /// This method is called post update to give the grap ha chance to update
        /// its actual chidl lists.
        /// This has to be seperated from Update so the child lists are not mutated in the
        /// middle of scanning for child updates
        /// </summary>
        public void UpdateChildLists()
        {
            foreach (SceneObject child in childRemovalList)
            {
                children.Remove(child);
            }
            foreach (SceneObject child in childAdditionList)
            {
                children.Add(child);
                child.UpdateGlobalMatrix(globalMatrix);
            }
            childRemovalList.Clear();
            childAdditionList.Clear();
            foreach (SceneObject child in children)
            {
                child.UpdateChildLists();
            }
            
        }

        /// <summary>
        /// Called to reset the local matrix.  Updates this node's matrix
        /// and then tells all children to update their global matrices to atch
        /// </summary>
        /// <param name="matrix"></param>
        public void SetLocalMatrix(Matrix matrix)
        {
            localMatrix = matrix;
            UpdateGlobalMatrix(parent.GetGlobalMatrix());
        }

        /// <summary>
        /// This method is used internally to reclaculate the transformation matrix
        /// whenever , rotation or origin change
        /// </summary>
        private void RecalculateLocalMatrix()
        {
            SetLocalMatrix( Matrix.CreateScale(new Vector3(scale.X,scale.Y,1))*
                            Matrix.CreateTranslation(-origin.X, -origin.Y, 0) *
                           Matrix.CreateRotationZ(rotation) *
                           Matrix.CreateTranslation(position.X, position.Y, 0)
                          );
        }

        /// <summary>
        /// This sets the position of this sprite's handle in the parent's global coordinate system
        /// </summary>
        /// <param name="pos">The position as an x,y vector</param>
        public virtual void SetLocalPosition(Vector2 pos)
        {
            position = pos;
            RecalculateLocalMatrix();
        }

        /// <summary> 
        /// This sets the handle of the sprite in sprite image-local coordinates
        /// </summary>
        /// <param name="orig">The handle pixel as an x,y vector</param>
        public void SetHandle(Vector2 orig)
        {
            origin = orig;
            RecalculateLocalMatrix();
        }

        /// <summary>
        /// This gets the current position in the parent-global coordinate system
        /// </summary>
        /// <returns>The current position as an x,y vector</returns>
        public virtual Vector2 GetLocalPosition()
        {
            return position;
        }

        public virtual Vector2 GetScale()
        {
            return scale;
        }

        public virtual Vector2 GetGlobalPosition()
        {
            return globalPosition;
        }

        public virtual float GetGlobalRotation()
        {
            return globalRotation;
        }

        public virtual Vector2 GetGlobalScale()
        {
            return globalScale;
        }


        /// <summary>
        /// Sets the rotation as a local offset to the parent's global rotation
        /// </summary>
        /// <param name="rot">The local rotation in radians</param>
        public virtual void SetLocalRotation(float rot)
        {
            rotation = rot;
            RecalculateLocalMatrix();
        }

        /// <summary>
        /// Sets the rotation as a local offset to the parent's global rotation in radians
        /// </summary>
        public virtual float GetLocalRotation()
        {
            return rotation;
        }

        public virtual void SetScale(Vector2 scale){
            this.scale = scale;
            RecalculateLocalMatrix();
        }

        public void Destroy()
        {
            parent.RemoveChild(this);
            destroyed = true;
            OnDestroy();
        }

        public bool IsDestroyed()
        {
            return destroyed;
        }

        public abstract void OnDestroy();
    }
}
