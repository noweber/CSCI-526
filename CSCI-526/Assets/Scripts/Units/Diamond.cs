using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Diamond : PieceMono
    {
        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

            var lvlMono = LevelMono.Instance;
			var pos = this.transform.position;
			int x = (int)pos.x;
			int y = (int)pos.y;

            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    //get diagonals - those are ok
                    if (Mathf.Abs(i - x) == 1 && Mathf.Abs(j - y) == 1)
                    {
                        var availableMove = new Tuple<int, int>(i, j);
						var availablePiece = lvlMono.GetPiece(availableMove);
                        if (availablePiece != null)
                        {
                            if (!this.IsEnemyOf(availablePiece)) { continue; }
                        }
                        legalSpots.Add(availableMove);
                    }

                    //get +1 to cardinal directions
                    if (Mathf.Abs(i - Position.Item1) == 2 && Mathf.Abs(j - Position.Item2) == 0)
                    {
                        var availableMove = new Tuple<int, int>(i, j);
                        var availablePiece = lvlMono.GetPiece(availableMove);
                        if (availablePiece != null)
                        {
                            if (!this.IsEnemyOf(availablePiece)) { continue; }
                        }
                        legalSpots.Add(availableMove);
                    }
                    if (Mathf.Abs(i - Position.Item1) == 0 && Mathf.Abs(j - Position.Item2) == 2)
                    {
                        var availableMove = new Tuple<int, int>(i, j);
                        var availablePiece = lvlMono.GetPiece(availableMove);
                        if (availablePiece != null)
                        {
                            if (!this.IsEnemyOf(availablePiece)) { continue; }
                        }
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

            /*var triangleLegal = inTriangleRange(Position);
            if (triangleLegal != null)
            {
                UnityEngine.Debug.Log("Unlimited movement around triangle");
                foreach (var adj in triangleLegal)
                {
                    if (!legalSpots.Contains(adj))
                        legalSpots.Add(adj);
                }
            }*/

            return legalSpots;
        }

        /*private List<Tuple<int, int>> inTriangleRange(Tuple<int, int> unitPosition)
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

            var lvlModel = LevelMono.Instance.LevelModel;
            foreach (Tuple<int, int> coord in adjacentList)
            {
                if (coord.Item1 >= 0 && coord.Item1 < LevelMono.Instance.LevelModel.GetWidth() && coord.Item2 >= 0 && coord.Item2 < LevelMono.Instance.LevelModel.GetHeight())
                {
                    if (lvlModel.TryGetUnit(coord) != null && lvlModel.TryGetUnit(coord).IsControlledByHuman() == base.IsControlledByHuman() && string.Equals(lvlModel.TryGetUnit(coord).Name(), UnitType.Triangle.ToString()))
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
        }*/
    }
}
