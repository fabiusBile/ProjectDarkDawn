using System.Collections;
using UnityEngine;

public class Room
{
	private int[,] map;
	private int width;
	private int height;
	private int posX, posY;
	public Room connectedRoom;

	public int PosX {
		get {
			return posX;
		}
	}

	public int PosY {
		get {
			return posY;
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


	public int GetBlock (int x, int y)
	{
		if (x < map.GetLength (0) && y < map.GetLength (1))
			return map [x, y];
		else
			return -1;
	}


	public int Distance (Room target)
	{
		return Mathf.Abs (this.PosY - target.PosX) + Mathf.Abs (this.PosY - target.PosX);
	}

	public Room ()
	{
		
	}

	public Room (int maxRoomSizeX, int maxRoomSizeY, int cellSizeX, int cellSizeY, int posX, int posY)
	{
		this.posX = posX;
		this.posY = posY;
		this.width = Mathf.RoundToInt (Random.Range (6f, maxRoomSizeX));
		this.height = Mathf.RoundToInt (Random.Range (6f, maxRoomSizeY));

		map = new int[cellSizeX, cellSizeY];

		for (int i = 0; i != cellSizeX; i++) {
			int minBorderX = (cellSizeX - width) / 2;
			int maxBorderX = width + minBorderX;
			for (int j = 0; j != cellSizeY; j++) {
				int minBorderY = (cellSizeY - height) / 2;
				int maxBorderY = height + minBorderY;
				if ((j > minBorderY && j < maxBorderY) && (i > minBorderX && i < maxBorderX))
					map [i, j] = 1;
				else
					map [i, j] = 0;
			}
		}

		for (int i = 0; i != cellSizeX; i++)
			for (int j = 0; j != cellSizeY; j++) {
				if (map [i, j] == 1 && (
					    map [i + 1, j] == 0 ||
					    map [i - 1, j] == 0 ||
					    map [i, j + 1] == 0 ||
					    map [i, j - 1] == 0))
					map [i, j] = 3;
			}

	}
}
