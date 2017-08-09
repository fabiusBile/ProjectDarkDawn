using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Level;

/* 		Author : Saad Khawaja
	 *  http://www.saadkhawaja.com
	 * 	http://www.twitter.com/saadskhawaja

	 *     This file is part of Grid Based A* - Tower Defense.

		    Grid Based A* - Tower Defense is free software: you can redistribute it and/or modify
		    it under the terms of the GNU General Public License as published by
		    the Free Software Foundation, either version 3 of the License, or
		    (at your option) any later version.

		    Grid Based A* - Tower Defense is distributed in the hope that it will be useful,
		    but WITHOUT ANY WARRANTY; without even the implied warranty of
		    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
		    GNU General Public License for more details.


	 * 
*/

public class EnemyAStar : MonoBehaviour
{


    public LevelGeneration LevelGenerator;
    public Tile nextNode;
    bool gray = false;
    public Tile[,] grid;



    public Point currentGridPosition = new Point();
    public Point startGridPosition = new Point();
    public Point endGridPosition = new Point();

    private Orientation gridOrientation = Orientation.Vertical;
    private bool allowDiagonals = true;
    private bool correctDiagonalSpeed = true;
    private Point input;
    private bool isMoving = true;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float t;
    private float factor;
    private Color myColor;



    public class MySolver<TPathNode, TUserContext> : SettlersEngine.SpatialAStar<TPathNode,
    TUserContext> where TPathNode : SettlersEngine.IPathNode<TUserContext>
    {
        protected override Double Heuristic(PathNode inStart, PathNode inEnd)
        {


            //int formula = GameManager.distance;
            int formula = 1;
            int dx = Math.Abs(inStart.X - inEnd.X);
            int dy = Math.Abs(inStart.Y - inEnd.Y);

            if (formula == 0)
                return Math.Sqrt(dx * dx + dy * dy); //Euclidean distance

            else if (formula == 1)
                return (dx * dx + dy * dy); //Euclidean distance squared

            else if (formula == 2)
                return Math.Min(dx, dy); //Diagonal distance

            else if (formula == 3)
                return (dx * dy) + (dx + dy); //Manhatten distance



            else
                return Math.Abs(inStart.X - inEnd.X) + Math.Abs(inStart.Y - inEnd.Y);

            //return 1*(Math.Abs(inStart.X - inEnd.X) + Math.Abs(inStart.Y - inEnd.Y) - 1); //optimized tile based Manhatten
            //return ((dx * dx) + (dy * dy)); //Khawaja distance
        }

        protected override Double NeighborDistance(PathNode inStart, PathNode inEnd)
        {
            return Heuristic(inStart, inEnd);
        }

        public MySolver(TPathNode[,] inGrid)
            : base(inGrid)
        {
        }
    }


    public IEnumerable<Tile> Search(Point start, Point end, Tile[,] grid)
    {
        startGridPosition = start;
        endGridPosition = end;
        MySolver<Tile, System.Object> aStar = new MySolver<Tile, System.Object>(grid);
        IEnumerable<Tile> path = aStar.Search(new Vector2(start.x,start.y), new Vector2(end.x, end.y), null);
        return path;

    }

    // Use this for initialization
    void Start()
    {

        myColor = getRandomColor();





        foreach (GameObject g in GameObject.FindGameObjectsWithTag("GridBox"))
        {
            g.GetComponent<Renderer>().material.color = Color.white;
        }


        updatePath();

        this.GetComponent<Renderer>().material.color = myColor;



    }



    public void findUpdatedPath(int currentX, int currentY)
    {


        MySolver<Tile, System.Object> aStar = new MySolver<Tile, System.Object>(LevelGenerator.TileMap);
        IEnumerable<Tile> path = aStar.Search(new Vector2(currentX, currentY), new Vector2(endGridPosition.x, endGridPosition.y), null);


        int x = 0;

        if (path != null)
        {

            foreach (Tile node in path)
            {
                if (x == 1)
                {
                    nextNode = node;
                    break;
                }

                x++;

            }

        }

    }


    Color getRandomColor()
    {
        Color tmpCol = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        return tmpCol;

    }

    // Update is called once per frame
    /**
    void Update()
    {

        if (!isMoving)
        {
            StartCoroutine(move());
        }
    }
**/


    public float moveSpeed;

    private enum Orientation
    {
        Horizontal,
        Vertical
    };

    /**
    public IEnumerator move()
    {
        isMoving = true;
        startPosition = transform.position;
        t = 0;

        if (gridOrientation == Orientation.Horizontal)
        {
            endPosition = new Point(startPosition.x + System.Math.Sign(input.x) * LevelGenerator.gridSize,
                                      startPosition.y);
            currentGridPosition.x += System.Math.Sign(input.x);
        }
        else
        {
            endPosition = new Point(startPosition.x + System.Math.Sign(input.x) * LevelGenerator.gridSize,
                                      startPosition.y + System.Math.Sign(input.y) * LevelGenerator.gridSize);

            currentGridPosition.x += System.Math.Sign(input.x);
            currentGridPosition.y += System.Math.Sign(input.y);
        }

        if (allowDiagonals && correctDiagonalSpeed && input.x != 0 && input.y != 0)
        {
            factor = 0.9071f;
        }
        else
        {
            factor = 1f;
        }


        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / LevelGenerator.gridSize) * factor;
            transform.position = Point.Lerp(startPosition, endPosition, t);
            yield return null;
        }



        isMoving = false;
        getNextMovement();

        yield return 0;





    }

**/

    void updatePath()
    {
        findUpdatedPath(currentGridPosition.x, currentGridPosition.y);
    }

    void getNextMovement()
    {
        updatePath();


        //StartCoroutine(move());
    }






}
