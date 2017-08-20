using UnityEngine;
using System.Collections;

namespace Level
{
    public class Tile : MonoBehaviour
    {

        public Point Position { get; set; }
        public TileTypes type;

        public bool IsWalkable(System.Object unused)
        {
            return type != TileTypes.wall;
        }

		public Tile(Point pos, TileTypes type = TileTypes.empty){
            this.Position = pos;
            this.type = type;
        }

		public Tile(int x, int y, TileTypes type = TileTypes.empty){
            Point pos = new Point(x, y);
            this.Position = pos;
            this.type = type;
        }


    }
}

