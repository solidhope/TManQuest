using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SidescrollerDemo
{
    public interface MOBAI
    {
        void UpdateAI(float elapsedTime);
        void EnterCell(Vector2 cell);
        void ExitCell(Vector2 cell);
        void PositionReset();

    }
}
