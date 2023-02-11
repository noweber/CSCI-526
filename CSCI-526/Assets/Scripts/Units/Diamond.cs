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
                    if (Mathf.Abs(i - x) == 2 && Mathf.Abs(j - y) == 0)
                    {
                        var availableMove = new Tuple<int, int>(i, j);
                        var availablePiece = lvlMono.GetPiece(availableMove);
                        if (availablePiece != null)
                        {
                            if (!this.IsEnemyOf(availablePiece)) { continue; }
                        }
                        legalSpots.Add(availableMove);
                    }
                    if (Mathf.Abs(i - x) == 0 && Mathf.Abs(j - y) == 2)
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

            // var Position = new Tuple<int, int>(x, y);
            // var triangleLegal = inTriangleRange(Position);
            // if (triangleLegal != null)
            // {                
            // }

            return legalSpots;
        }
    }
}
