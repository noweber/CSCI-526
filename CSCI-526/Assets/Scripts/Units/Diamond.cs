using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Diamond : PieceMono
    {
        public override string getUnitInfo()
        {
            return "Can Capture Enemies. When adjacent to Circle, Circle gains increased range.";
        }

        public override List<Tuple<int, int>> LegalMoves(int boardHeight, int boardWidth)
        {
            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

            var lvlMono = LevelMono.Instance;
			var pos = this.standingOnTile.gridLocation;//this.transform.position;
            int x = (int)pos.x;
            int y = (int)pos.y;

            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    //get diagonals - those are ok
                    if (Mathf.Abs(i - x) == 1 && Mathf.Abs(j - y) == 1)
                    {
                        Debug.Log("forward: " + i + ", " + j);
                        var availableMove = new Tuple<int, int>(i, j);
						var availablePiece = lvlMono.GetPiece(availableMove);
                        if (availablePiece != null)
                        {
                            if (!this.IsEnemyOf(availablePiece)) { continue; }
                        }
                        //Debug.Log("Diamond coord Check: " + i + ", " + j);
                        legalSpots.Add(availableMove);
                    }

                    //get +1 to cardinal directions
                    if (Mathf.Abs(i - x) == 2 && Mathf.Abs(j - y) == 0)
                    {
                        Debug.Log("forward: " + i + ", " + j);
                        var availableMove = new Tuple<int, int>(i, j);
                        var availablePiece = lvlMono.GetPiece(availableMove);
                        if (availablePiece != null)
                        {
                            if (!this.IsEnemyOf(availablePiece)) { continue; }
                        }
                        //Debug.Log("Diamond coord Check: " + i + ", " + j);
                        legalSpots.Add(availableMove);
                    }
                    if (Mathf.Abs(i - x) == 0 && Mathf.Abs(j - y) == 2)
                    {
                        Debug.Log("forward: " + i + ", " + j);
                        var availableMove = new Tuple<int, int>(i, j);
                        var availablePiece = lvlMono.GetPiece(availableMove);
                        if (availablePiece != null)
                        {
                            if (!this.IsEnemyOf(availablePiece)) { continue; }
                        }
                        //Debug.Log("Diamond coord Check: " + i + ", " + j);
                        legalSpots.Add(availableMove);
                    }
                }
            }

            if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 0)
            {
                // first move of tutorial
                Debug.Log("FIRST MOVE");
                var availableMove = new Tuple<int, int>(1, 0);
                legalSpots.Clear();
                legalSpots.Add(availableMove);
            }
            else if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 1)
            {
                legalSpots.Clear();
            }

            var Position = new Tuple<int, int>(x, y);
            var triangleLegal = inTriangleRange(Position);
            if (triangleLegal != null)
            {
                UnityEngine.Debug.Log("Unlimited movement around triangle");
                foreach (var adj in triangleLegal)
                {
                    if (!legalSpots.Contains(adj))
                        legalSpots.Add(adj);
                }
            }
            var posCheck = this.standingOnTile.gridLocation;//this.transform.position;
            if (!legalSpots.Contains(new Tuple<int,int>(posCheck.x, posCheck.y + 2)))
            {
                if (posCheck.x > 0 && posCheck.x < boardHeight && posCheck.y > 0 && posCheck.y < boardWidth)
                {
                    legalSpots.Add(new Tuple<int, int>(posCheck.x, posCheck.y + 2));
                }
            }

            return legalSpots;
        }

        private List<Tuple<int, int>> inTriangleRange(Tuple<int, int> unitPosition)
        {
            var adjacentList = new List<Tuple<int, int>>();
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2)); //right
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2)); //left
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 + 1)); //up
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 - 1)); //down
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2 + 1)); //right up diag
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2 + 1)); //left  up diag
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2 - 1)); //right down diag
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2 - 1)); //left down diag

            var lvlMono = LevelMono.Instance;
            foreach (Tuple<int, int> coord in adjacentList)
            {
                if (coord.Item1 >= 0 && coord.Item1 < LevelMono.Instance.GetWidth() && coord.Item2 >= 0 && coord.Item2 < LevelMono.Instance.GetHeight())
                {
                    if (lvlMono.GetPiece(coord) != null && !this.IsEnemyOf(lvlMono.GetPiece(coord)) && lvlMono.GetPiece(coord).IsTriangle())
                    {
                        var triangleRange = new List<Tuple<int, int>>();
                        triangleRange.Add(new Tuple<int, int>(coord.Item1 + 1, coord.Item2)); //right
                        triangleRange.Add(new Tuple<int, int>(coord.Item1 - 1, coord.Item2)); //left
                        triangleRange.Add(new Tuple<int, int>(coord.Item1, coord.Item2 + 1)); //up
                        triangleRange.Add(new Tuple<int, int>(coord.Item1, coord.Item2 - 1)); //down
                        triangleRange.Add(new Tuple<int, int>(coord.Item1 + 1, coord.Item2 + 1)); //right up diag
                        triangleRange.Add(new Tuple<int, int>(coord.Item1 - 1, coord.Item2 + 1)); //left  up diag
                        triangleRange.Add(new Tuple<int, int>(coord.Item1 + 1, coord.Item2 - 1)); //right down diag
                        triangleRange.Add(new Tuple<int, int>(coord.Item1 - 1, coord.Item2 - 1)); //left down diag
                        return triangleRange;
                    }
                }
            }    
            return null;
        }
    }
}
