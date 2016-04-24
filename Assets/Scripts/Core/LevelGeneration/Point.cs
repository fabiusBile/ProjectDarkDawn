using System;


public struct Point
{
	public int x;
	public int y;

	public static Point Up {
		get {
			return new Point (0, 1);
		}
	}

	public static Point Right {
		get {
			return new Point (1, 0);
		}
	}

	public Point (int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public static Point operator +(Point p1, Point p2){
		return new Point (p1.x + p2.x, p2.y + p1.y);
	}
	public static Point operator -(Point p1, Point p2){
		return new Point (p1.x - p2.x, p2.y - p1.y);
	}
}


