using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LevelGeneration
{
	public class LevelGeneration : MonoBehaviour
	{
		struct PossiblePlaceForRoom
		{
			public Point pos;
			public int origin;

			public PossiblePlaceForRoom (Point pos, int origin)
			{
				this.pos = pos;
				this.origin = origin;
			}
		}

		public Transform block;
		public Transform block2;
		public Transform wall;

		//Размер тайла
		public Vector2 tileSize;

		//Стороны таблицы для генерации
		public int gridDimensionsX, gridDimensionsY;

		//Размер ячейки таблицы
		//Максимальный размер комнаты не должен превышать
		//максимальный размер ячейки, но может
		//быть меньше него.
		public int cellSizeX, cellSizeY;

		//Число комнат
		public int roomsCount;

		//Максимальные и минимальные размеры комнаты
		public int maxRoomSize, minRoomSize;

		//Список комнат
		List<Room> rooms = new List<Room> ();

		//Список координат, в которые можно поставить комнату
		List<PossiblePlaceForRoom> possiblePlacesToPlaceRoom = new List<PossiblePlaceForRoom> ();

		//Таблица. Координаты соответствуют расположению комнат.
		//Число, находящееся по координате - номер комнаты, находящейся в этом месте.
		//-1 - отсутсвие комнаты.
		int[,] grid;
		//Количество ответвлений
		public int distortion = 3;
		//Массив, представляющий собой уровень
		int[,] map;

		void Awake ()
		{
			grid = new int[gridDimensionsX, gridDimensionsY];
			map = new int[(int)cellSizeX * gridDimensionsX + 6, (int)cellSizeY * gridDimensionsY + 6];

			//Генерация таблицы и заполнение ее пустыми клетками
			for (int i = 0; i != gridDimensionsX; i++) {
				for (int j = 0; j != gridDimensionsY; j++) {
					grid [i, j] = -1;
				}
			}

			//Генерация первой комнаты в случайном месте и запись ее координат в таблицу
			Point firstRoomPos = new Point (Random.Range (0, gridDimensionsX - 1), Random.Range (0, gridDimensionsY - 1));

			rooms.Add (new Room (maxRoomSize, minRoomSize, cellSizeX, cellSizeX, firstRoomPos, distortion));
			grid [firstRoomPos.x, firstRoomPos.y] = 0;
			//Записывает координаты пустых ячеек 
			AddCoordinatesOfFreePlacesToList (firstRoomPos, 0);


			//Пока не наберется достаточное число комнат,
			//проходится по массиву возможных мест для расположения и устанавливает там комнату
			while (rooms.Count < roomsCount) {
				int randomRoomNumber = (Random.Range (0, possiblePlacesToPlaceRoom.Count - 1));
				PossiblePlaceForRoom place = possiblePlacesToPlaceRoom [randomRoomNumber];
				rooms.Add (new Room (maxRoomSize, minRoomSize, cellSizeX, cellSizeX, place.pos, distortion));
				int lastRoom = rooms.Count - 1;
				rooms [lastRoom].connectedRooms.Add (place.origin);
				rooms [place.origin].connectedRooms.Add (lastRoom);
				grid [place.pos.x, place.pos.y] = lastRoom;
				AddCoordinatesOfFreePlacesToList (place.pos, lastRoom);
				possiblePlacesToPlaceRoom.RemoveAt (randomRoomNumber);
				string output2 = "";
				for (int i = 0; i!=gridDimensionsX;i++){
					for (int j = 0; j != gridDimensionsY; j++) {
						output2 += grid [i, j];
					}
					output2 += "\n";
				}
				Debug.Log (output2);
			}



			//Построение массива уровня
			for (int i = 0; i != gridDimensionsX; i++) {
				for (int j = 0; j != gridDimensionsY; j++) {
					for (int _i = 0; _i != cellSizeX; _i++) {
						for (int _j = 0; _j != cellSizeY; _j++) {
							int room = grid [i, j];
							if (room >= 0) {
								map [i * cellSizeX + _i, j * cellSizeY + _j] = rooms [room].GetBlock (_i, _j);
							} else {
								map [i * cellSizeX + _i, j * cellSizeY + _j] = 0;
							}
						}
					}
				}
			}

			foreach (Room origin in rooms) {
				foreach (int targetNum in origin.connectedRooms) {
					ConnectRooms (origin, rooms [targetNum]);
				}

			}
			string output = "";
			for (int i = 0; i!=map.GetLength(0);i++){
				for (int j = 0; j != map.GetLength (1); j++) {
					output += map [i, j];
				}
				output += "\n";
			}
			Debug.Log (output);

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

					if (map [i, j] > 0) {
						if (map [i, j] == 2)
							bl = block2;
						else if (map [i, j] == 3)
							bl = wall;
						else
							bl = block;
						Transform.Instantiate (bl, new Vector3 (-i * tileSize.x / 2 + xOffset + 3, j * tileSize.y / 4 + yOffset + 3, 0), bl.rotation);
					}
					xOffset += (tileSize.x / 2);
				}
				yOffset += (tileSize.y / 4);
			}
		}

		//Добавляет в массив возможных мест для размещения комнаты пустые соседи указанной клетки таблицы
		void AddCoordinatesOfFreePlacesToList (Point place, int origin)
		{
			//Возможные места для размещения комнаты

			PossiblePlaceForRoom[] possiblePlaces = { 
				new PossiblePlaceForRoom (place + Point.Up, origin), 
				new PossiblePlaceForRoom (place + Point.Right, origin),
				new PossiblePlaceForRoom (place + Point.Down, origin),
				new PossiblePlaceForRoom (place + Point.Left, origin)
			};
		
			//Проходит по всем возможным местам для размещения комнаты
			//Если такая ячейка существует и она пустая -- добавляет ее в массив возможных мест расположения комнаты
			for (int i = 0; i != 4; i++) {
				Point possiblePlace = possiblePlaces [i].pos;
				if (possiblePlace.x >= 0 && possiblePlace.x < gridDimensionsX
				   && possiblePlace.y >= 0 && possiblePlace.y < gridDimensionsY) {
					if (grid [possiblePlace.x, possiblePlace.y] == -1) {
						possiblePlacesToPlaceRoom.Add (possiblePlaces[i]);
						Debug.Log (possiblePlace.x + " " + possiblePlace.y);
						grid [possiblePlace.x, possiblePlace.y] = -2;
					}
				}
				
			}
		}

		void ConnectRooms (Room origin, Room target)
		{
			bool _dirX = origin.PosX < target.PosX;
			bool _dirY = origin.PosY < target.PosY;

			int dirX = _dirX ? 1 : -1;
			int dirY = _dirY ? 1 : -1;

			int targetX = target.PosX * cellSizeX + cellSizeX / 2;
			int targetY = target.PosY * cellSizeY + cellSizeY / 2;
			int roomX = origin.PosX * cellSizeY + cellSizeY / 2;
			int roomY = origin.PosY * cellSizeY + cellSizeY / 2;

			//Строим коридор
			for (int n = roomX - dirX; (n != (targetX + dirX * 2)); n += dirX) {
				if (n < map.GetLength (0) && map [n, roomY] != 1) {
					map [n, roomY] = 2;
					if (map [n, roomY + 1] != 1) {
						map [n, roomY + 1] = 2;
					}
					if (map [n, roomY + 2] == 0) {
						map [n, roomY + 2] = 3;
					}
					if (map [n, roomY - 1] != 1) {
						map [n, roomY - 1] = 2;
					}
					if (map [n, roomY - 2] == 0) {
						map [n, roomY - 2] = 3;
					}
				}
			}

			for (int j = roomY - dirY; (j != targetY + dirY * 2); j += dirY) {
				if (map [targetX, j] != 1) {
					map [targetX, j] = 2;

					if (map [targetX + 1, j] != 1) {
						map [targetX + 1, j] = 2;
					}
					if (map [targetX + 2, j] == 0) {
						map [targetX + 2, j] = 3;
					}
					if (map [targetX - 1, j] != 1) {
						map [targetX - 1, j] = 2;
					}
					if (map [targetX - 2, j] == 0) {
						map [targetX - 2, j] = 3;
					}

				}
			}
		}
	}
}