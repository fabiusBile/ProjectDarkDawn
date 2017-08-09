using System;
using UnityEngine;

[System.Serializable]
public struct Point
{
	public int x;
	public int y;


//	public int Y {
//		get {
//			return y;
//		}
//		set {
//			y = value;
//		}
//	}
//	public int X {
//		get {
//			return x;
//		}
//		set {
//			x = value;
//		}
//	}
//
	public static Point Up {
		get {
			return new Point (0, 1);
		}
	}
	public static Point Down {
		get {
			return new Point (0, -1);
		}
	}
	public static Point Right {
		get {
			return new Point (1, 0);
		}
	}
	public static Point Left {
		get {
			return new Point (-1, 0);
		}
	}
	public Point (int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public Point (Vector3 vector){
		this.x = Mathf.RoundToInt (vector.x);
		this.y = Mathf.RoundToInt (vector.z);
	}
	public Point(Vector2 vector){
		this.x = Mathf.RoundToInt(vector.x);
		this.y = Mathf.RoundToInt (vector.y);
	}
	public static Point operator +(Point p1, Point p2){
		return new Point (p1.x + p2.x, p2.y + p1.y);
	}
	public static Point operator -(Point p1, Point p2){
		return new Point (p1.x - p2.x, p2.y - p1.y);
	}
	public static Boolean operator ==(Point p1, Point p2){
		return (p1.x == p2.x && p1.y == p2.y);
	}
	public static Boolean operator !=(Point p1, Point p2){
		return (p1.x == p2.x && p1.y == p2.y);
	}
}


