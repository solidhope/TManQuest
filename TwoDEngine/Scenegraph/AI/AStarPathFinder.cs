using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoDEngine.AI {
    class NodeRec : IComparable {
		public int x,y,score;
		public NodeRec(int x, int y,int score){
			this.x =x;
			this.y =y;
			this.score = score;
		}
			
		public int CompareTo(object other){
			return score - ((NodeRec)other).score;	
		}
	}

    public static class AStarPathFinder
    {
        
        public static List<Vector2> FindPath(Vector2 start, Vector2 end, bool [,] blocking, 
			bool weightCorners=false, bool terminateEarly=false){
            int[,] grid = GetScoreGrid(start,end,blocking,weightCorners,terminateEarly);
            return FindPathTo(grid, end);
        }

        private static int[,] GetScoreGrid (Vector2 start, Vector2 end, bool [,] blocking, 
			bool weightCorners=false, bool terminateEarly=false){
		
            int startX = (int)start.X;
            int startY = (int)start.Y;
            int goalX = (int)end.X;
            int goalY = (int)end.Y;
            int[,] scoreGrid = new int[blocking.GetLength(0),blocking.GetLength(1)];
            // init score grid to max value to start
            for(int y=0;y<scoreGrid.GetLength(1);y++){
                for(int x=0;x<scoreGrid.GetLength(0);x++){
                    if (blocking[x,y]){
                        scoreGrid[x,y] = int.MinValue;
                    } else {
                        scoreGrid[x,y] = int.MaxValue;
                    }
                }
            }
            List<NodeRec> openList = new List<NodeRec>();
            scoreGrid[startX,startY]=0;  // our start is value 0
            openList.Add(new NodeRec(startX,startY,0));
            // now do A* weighted fill
            do {
                // sort to order by current scores and take first lowest
			    openList.Sort();
			    NodeRec testNode = openList[0];
			    openList.RemoveAt(0);
			    if (terminateEarly){
				    if ((testNode.x == goalX) && (testNode.y == goalY)){
	
					    return scoreGrid;
				    }
			    }
			    int num = scoreGrid[testNode.x,testNode.y];
			    for (int y= testNode.y-1;y<testNode.y+2;y++){
				    for (int x=testNode.x-1;x<testNode.x+2;x++){
					    if ((x>=0)&&(x<scoreGrid.GetLength(0))&&(y>=0)&&(y<scoreGrid.GetLength(1))&&
						    ((x!=testNode.x)||(y!=testNode.y))){
						    int newNum;
						    if (!weightCorners){
							    newNum = num+1;
						    } else {
							    if (Math.Abs(testNode.x-x) == Math.Abs(testNode.y-y)) {// is a corner	
								    newNum = num+3;	
							    } else {
								    newNum = num+2;	
							    }
						    }
						    // add manhatten bias
						    int md = Math.Abs(goalX-x)+Math.Abs(goalY-y);
						    if (!weightCorners){
							    newNum = newNum+md;
						    } else {
							    newNum = newNum+(md*2);
						    }
						    //
						    if (scoreGrid[x,y]>newNum){
							    scoreGrid[x,y]=newNum;
							    openList.Add(new NodeRec(x,y,newNum));
						    }
					    }
				    }
			    }
            } while (openList.Count>0); 
            return scoreGrid;
		}

        private static List<Vector2> FindPathTo(int[,] grid, Vector2 end, bool randomChoice=false)
        {
            // search back from end
            int x = (int)end.X;
            int y = (int)end.Y;
            List<Vector2> path = new List<Vector2>();
            while (grid[x, y] > 0)
            { // not at start yet
                int lowest = int.MaxValue;
                int sx = -1, sy = -1; // just to make C# happy
                for (int ty = y - 1; ty < y + 2; ty++)
                {
                    for (int tx = x - 1; tx < x + 2; tx++)
                    {
                        if ((tx >= 0) && (tx < grid.GetLength(0)) && (ty >= 0) && (ty < grid.GetLength(1)))
                        {
                            int tnum = grid[tx, ty];
                            if (tnum > int.MinValue)
                            { // not a wall
                                if (tnum < lowest)
                                {
                                    lowest = tnum;
                                    sx = tx;
                                    sy = ty;
                                }
                            }
                        }
                    }
                }
                x = sx;
                y = sy;
                path.Add(new Vector2(x, y));
            }
            path.Reverse() ;
            return path;
        }
    }
}
