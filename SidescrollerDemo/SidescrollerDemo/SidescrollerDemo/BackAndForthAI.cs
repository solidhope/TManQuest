using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SidescrollerDemo
{
    public class BackAndForthAI:MOBAI
    {
        int range;
        Vector2 homeCell;
        Vector2 leftEnd;
        Vector2 rightEnd;
        bool moveLeft = true;
        Monster monster;

        public BackAndForthAI(Monster monster, Dictionary<string, string> properties)
        {
            range = int.Parse(properties["MoveRange"]);
            this.monster = monster;
            ResetAI();
        }

        private void ResetAI(){
            homeCell = monster.GetLocalCellPosition();
            /// find movement endpoints
           
            leftEnd = homeCell;
            for (int i = 1; i < range+1; i++)
            {
                leftEnd.X = homeCell.X-i;
                if ((leftEnd.X<0)||(monster.GetTileMap().GetTileIndex("blocking", leftEnd) > 0))
                {
                    leftEnd.X+= 1;
                    break;
                }
            }
            rightEnd = homeCell;
            for (int i = 1; i < range + 1; i++)
            {
                rightEnd.X = homeCell.X +i;
                if (monster.GetTileMap().GetTileIndex("blocking", rightEnd) > 0)
                {
                    rightEnd.X -= 1;
                    break;
                }
            }
        }

        public void UpdateAI(float elapsedTime)
        {
            if (!monster.IsMoving())
            {
                if (moveLeft)
                {
                    monster.MoveToCell(leftEnd);
                }
                else
                {
                    monster.MoveToCell(rightEnd);
                }
                moveLeft = !moveLeft;
            }
        }



        public void EnterCell(Vector2 cell)
        {
            //nop
        }

        public void ExitCell(Vector2 cell)
        {
            //nop
        }


        public void PositionReset()
        {
            ResetAI();
        }
    }
}
