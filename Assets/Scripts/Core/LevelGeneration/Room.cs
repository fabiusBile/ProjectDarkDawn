using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace LevelGeneration
{
	public class Room
	{

		public TileTypes[,] map;
		private int width;
		private int height;
		private Point pos;
		public List<int> connectedRooms;
		public bool isConnected;

		public int PosX {
			get {
				return pos.x;
			}
		}

		public int PosY {
			get {
				return pos.y;
			}
		}

		public int Height {
			get {
				return height;
			}
		}

		public int Width {
			get {
				return width;
			}
		}


		public TileTypes GetBlock (int x, int y)
		{
			if (x < map.GetLength (0) && y < map.GetLength (1))
				return map [x, y];
			else
				return TileTypes.empty;
		}


		public int Distance (Room target)
		{
			return Mathf.Abs (this.PosY - target.PosX) + Mathf.Abs (this.PosY - target.PosX);
		}


		private bool checkCell(int x, int y, TileTypes type = TileTypes.empty){
			try { 
				return (map[x,y]==type);
			} catch (System.IndexOutOfRangeException ex){
				return type == TileTypes.empty;
			}
		}

		public Room (int maxRoomSize, int minRoomSize, int cellSizeX, int cellSizeY, Point pos, int maxDistortion = 0)
		{
			connectedRooms = new List<int> ();
			this.pos = pos;
			this.width = Random.Range (minRoomSize, maxRoomSize);
			this.height = Random.Range (minRoomSize, maxRoomSize);

			map = new TileTypes[cellSizeX, cellSizeY];

			//Центрирование 
			int minBorderX = (cellSizeX - width) / 2;
			int maxBorderX = width + minBorderX;
			int minBorderY = (cellSizeY - height) / 2;
			int maxBorderY = height + minBorderY;

			//Генерация прямоугольника комнаты
			for (int i = 0; i != cellSizeX; i++) {
				for (int j = 0; j != cellSizeY; j++) {
					if ((j >= minBorderY && j <= maxBorderY) && (i >= minBorderX && i <= maxBorderX))
						map [i, j] = TileTypes.ground;
					else
						map [i, j] = TileTypes.empty;
				}
			}

			//Генерация ответвлений
			for (int i = 0; i != maxDistortion; i++) {
				//Стартовая координата 
				int startX = Random.Range (minBorderX, maxBorderX);
				int startY = Random.Range (minBorderY, maxBorderY);
				//Конечная координата
				int stopX = Random.Range (maxBorderX, maxRoomSize);
				int stopY = Random.Range (maxBorderY, maxRoomSize);

				for (int x = startX; x != stopX; x++) {
					for (int y = startY; y != stopY; y++) {
						map [x, y] = TileTypes.ground;
					}
				}
			}

			for (int i = 0; i != cellSizeX; i++) {
				for (int j = 0; j != cellSizeY; j++) {
					if (map [i, j] == TileTypes.ground) {
						if (checkCell(i+1,j)||
							checkCell(i,j+1)||
							checkCell(i+1,j+1)||
							checkCell(i-1,j)||
							checkCell(i,j-1)||
							checkCell(i-1,j-1)||
							checkCell(i-1,j+1)||
							checkCell(i+1,j-1)){
							map [i, j] = TileTypes.wall;
						}
					}
				}
			}
//			for (int i = 0; i != cellSizeX; i++) {
//				for (int j = 0; j != cellSizeY; j++) {
//					if (map [i, j] == TileTypes.wall &&
//					    (checkCell (i, j + 1) || checkCell (i, j + 1, TileTypes.wall)) &&
//					    (checkCell (i, j - 1, TileTypes.ground)) &&
//					    (checkCell (i, j - 2, TileTypes.ground))) {
//						map [i, j] = TileTypes.wall;
//						map [i, j - 1] = TileTypes.wallMiddle;
//						map [i, j - 2] = TileTypes.wallBottom;
//					}
//				}
//			}
		}
	}
}