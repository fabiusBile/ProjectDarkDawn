using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneration : MonoBehaviour
{

	public Transform block;
	public Transform block2;
	public Transform wall;
	//Размер тайла
	public Vector2 tileSize;
	//Стороны таблицы для генерации
	public int gridDimensionsX, gridDimensionsY;
	//Размер ячейки таблицы (максимальный размер комнаты)
	public int cellSizeX, cellSizeY;
	//Число комнат
	public int roomsCount;
	public int maxRoomSizeX, maxRoomSizeY;

	List<Room> rooms = new List<Room> ();
	int[,] map;
	// Use this for initialization

	void Awake ()
	{
		map = new int[(int)cellSizeX * gridDimensionsX + 6, (int)cellSizeY * gridDimensionsY + 6];

		int[,] grid = new int[gridDimensionsX, gridDimensionsY];
		for (int i = 0; i != gridDimensionsX; i++) {
			for (int j = 0; j != gridDimensionsY; j++) {
				grid [i, j] = -1;
			}
		}

		//Размещение и генерация комнат
		while (rooms.Count < roomsCount) {
			int x = Mathf.RoundToInt (Random.Range (0f, gridDimensionsX - 1));
			int y = Mathf.RoundToInt (Random.Range (0f, gridDimensionsY - 1));
			if (grid [x, y] == -1) {
				rooms.Add (new Room (maxRoomSizeX, maxRoomSizeY,cellSizeX,cellSizeY, x, y));
				grid [x, y] = rooms.Count - 1;
			}
		}


		//Построение массива уровня
		for (int i = 0; i != gridDimensionsX; i++) {
			for (int j = 0; j != gridDimensionsY; j++) {
				for (int _i = 0; _i != cellSizeX; _i++) {
					for (int _j = 0; _j != cellSizeY; _j++) {
						int room = grid [i, j];
						if (room != -1) {
							map [i * cellSizeX + _i, j * cellSizeY + _j] = rooms [room].GetBlock (_i, _j);
						} else {
							map [i * cellSizeX + _i, j * cellSizeY + _j] = 0;
						}
					}
				}
			}
		}
		for (int i = 0; i != (rooms.Count - 1); i++) {
			Room room = rooms [i];
		
			//int distance = int.MaxValue;
			Room target = rooms [i + 1];

//			foreach (Room _target in rooms) {
//				if (_target != room && _target.connectedRoom == null && room.Distance (_target) < distance) {
//					target = _target;
//					distance = room.Distance (_target);
//				}
//			}


			//room.connectedRoom = target;

			bool _dirX = room.PosX < target.PosX;
			bool _dirY = room.PosY < target.PosY;

			int dirX = _dirX ? 1 : -1;
			int dirY = _dirY ? 1 : -1;

			int targetX = target.PosX * cellSizeX + cellSizeX / 2;
			int targetY = target.PosY * cellSizeY + cellSizeY / 2;
			int roomX = room.PosX * cellSizeY + cellSizeY / 2;
			int roomY = room.PosY * cellSizeY + cellSizeY / 2;

			//Строим коридор
			for (int n = roomX-dirX; (n != (targetX+dirX*2)); n += dirX) {
				if (n<map.GetLength(0) && map [n, roomY] != 1) {
					map [n, roomY] = 2;
					if (map [n, roomY+1] != 1) {
						map [n, roomY+1] = 2;
					}
					if (map [n, roomY+2] == 0) {
						map [n, roomY+2] = 3;
					}
					if (map [n, roomY-1] != 1) {
						map [n, roomY-1] = 2;
					}
					if (map [n, roomY-2] == 0) {
						map [n, roomY-2] = 3;
					}
				}
			}

			if (map [targetX + dirX , roomY + (3 * dirY)]==0)
				map [targetX + dirX*2 , roomY + (2 * -dirY)]=3;
			
			for (int j = roomY-dirY; (j != targetY+dirY*2); j += dirY) {
				if (map [targetX, j] != 1 ) {
					map [targetX, j] = 2;

					if (map [targetX+1, j] != 1) {
						map [targetX+1, j] = 2;
					}
					if (map [targetX+2, j] == 0) {
						map [targetX+2, j] = 3;
					}
					if (map [targetX-1, j] != 1) {
						map [targetX-1, j] = 2;
					}
					if (map [targetX-2, j] == 0) {
						map [targetX-2, j] = 3;
					}

				}
			}
		}

		//for (int i=0;i!=
		Draw ();
	}
	//Строит уровень из массива
	void Draw ()
	{
		float yOffset = 0;
		for (int i = 0; i != map.GetLength (0); i++) {
			float xOffset = 0;
			for (int j = 0; j != map.GetLength (1); j++) {
				Transform bl;

				if (map [i, j] != 0) {
					if (map [i, j] == 2)
						bl = block2;
					else if (map [i, j] == 3)
						bl = wall;
					else
						bl = block;
					Transform.Instantiate (bl, new Vector3 (-i * tileSize.x / 2 + xOffset+3, j * tileSize.y / 4 + yOffset+3, 0), bl.rotation);
				}
				xOffset += (tileSize.x / 2);
			}
			yOffset += (tileSize.y / 4);
		}
	}
}
