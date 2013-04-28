using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoDEngine.Scenegraph.SceneObjects
{
    public class NonExistantLayerException:Exception
    {
        public NonExistantLayerException(string message) : base(message) { }
    }
}
