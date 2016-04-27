using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathNode
{
	// Координаты точки на карте.
	public Point Position { get; set; }
	// Длина пути от старта (G).
	public int PathLengthFromStart { get; set; }
	// Точка, из которой пришли в эту точку.
	public PathNode CameFrom { get; set; }
	// Примерное расстояние до цели (H).
	public int HeuristicEstimatePathLength { get; set; }
	// Ожидаемое полное расстояние до цели (F).
	public int EstimateFullPathLength {
		get {
			return this.PathLengthFromStart + this.HeuristicEstimatePathLength;
		}
	}
}

public class AStarPathfinder  {
	public static List<Point> FindPath(int[,] field, Point start, Point goal){
		// Шаг 1.
		var closedSet = new List<PathNode>();
		var openSet = new List<PathNode>();
		// Шаг 2.
		PathNode startNode = new PathNode()
		{
			Position = start,
			CameFrom = null,
			PathLengthFromStart = 0,
			HeuristicEstimatePathLength = GetHeuristicPathLength(start, goal)
		};
		openSet.Add(startNode);
		while (openSet.Count > 0)
		{
			// Шаг 3.
			var currentNode = openSet.OrderBy(node => 
				node.EstimateFullPathLength).First();
			// Шаг 4.
			if (currentNode.Position == goal)
				return GetPathForNode(currentNode);
			// Шаг 5.
			openSet.Remove(currentNode);
			closedSet.Add(currentNode);
			// Шаг 6.
			foreach (var neighbourNode in GetNeighbours(currentNode, goal, field))
			{
				// Шаг 7.
				if (closedSet.Count(node => node.Position == neighbourNode.Position) > 0)
					continue;
				var openNode = openSet.FirstOrDefault(node =>
					node.Position == neighbourNode.Position);
				// Шаг 8.
				if (openNode == null)
					openSet.Add(neighbourNode);
				else
					if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart)
					{
						// Шаг 9.
						openNode.CameFrom = currentNode;
						openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
					}
			}
		}
		// Шаг 10.
		return null;
	}
	private static int GetDistanceBetweenNeighbours()
	{
		return 1;
	}
	private static int GetHeuristicPathLength(Point from, Point to)
	{
		return Mathf.Abs(from.x - to.y) + Mathf.Abs(from.y - to.y);
	}
	private static List<PathNode> GetNeighbours(PathNode pathNode, 
		Point goal, int[,] field)
	{
		var result = new List<PathNode>();

		// Соседними точками являются соседние по стороне клетки.
		Point[] neighbourPoints = new Point[4];
		neighbourPoints[0] = new Point(pathNode.Position.x + 1, pathNode.Position.y);
		neighbourPoints[1] = new Point(pathNode.Position.x - 1, pathNode.Position.y);
		neighbourPoints[2] = new Point(pathNode.Position.x, pathNode.Position.y + 1);
		neighbourPoints[3] = new Point(pathNode.Position.x, pathNode.Position.y - 1);

		foreach (var point in neighbourPoints)
		{
			// Проверяем, что не вышли за границы карты.
			if (point.x < 0 || point.x >= field.GetLength(0))
				continue;
			if (point.y < 0 || point.y >= field.GetLength(1))
				continue;
			// Проверяем, что по клетке можно ходить.
			if ((field[point.x, point.y] != 1) && (field[point.x, point.y] != 3))
				continue;
			// Заполняем данные для точки маршрута.
			var neighbourNode = new PathNode()
			{
				Position = point,
				CameFrom = pathNode,
				PathLengthFromStart = pathNode.PathLengthFromStart +
					GetDistanceBetweenNeighbours(),
				HeuristicEstimatePathLength = GetHeuristicPathLength(point, goal)
			};
			result.Add(neighbourNode);
		}
		return result;
	}
	private static List<Point> GetPathForNode(PathNode pathNode)
	{
		var result = new List<Point>();
		var currentNode = pathNode;
		while (currentNode != null)
		{
			result.Add(currentNode.Position);
			currentNode = currentNode.CameFrom;
		}
		result.Reverse();
		return result;
	}
}
