using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Diamond : PieceMono
    {
        private List<Tuple<int, int>> adjAllies = new List<Tuple<int, int>>();

        public override string getUnitInfo()
        {
            return "Capture Enemies: Yes \nAbility: When adjacent to an ally Scout or Fighter, increases that unit's movement range.";
        }

        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

            var lvlMono = LevelMono.Instance;
            var pos = this.transform.position;
            int x = (int)pos.x;
            int y = (int)pos.y;
            // var adjacentList = new List<Tuple<int, int>>();
            // adjacentList.Add(new Tuple<int, int>(x + 1, y)); //right
            // adjacentList.Add(new Tuple<int, int>(x - 1, y)); //left
            // adjacentList.Add(new Tuple<int, int>(x, y + 1)); //up
            // adjacentList.Add(new Tuple<int, int>(x, y - 1)); //down


            int maxRange = 2;
            // left 
            var range = UnityEngine.Mathf.Min(x, maxRange);
            for (int i = 1; i <= range; i++)
            {
                var availableMove = new Tuple<int, int>(x - i, y);
                var availablePiece = lvlMono.GetPiece(availableMove);
                if (availablePiece != null && (!this.IsEnemyOf(availablePiece) || availablePiece.IsTriangle()))
                {
                    break;
                }
                else if (availablePiece != null && this.IsEnemyOf(availablePiece))
                {
                    legalSpots.Add(availableMove);
                    break;
                }
                else { legalSpots.Add(availableMove); }
            }

            // right
            range = UnityEngine.Mathf.Min(boardWidth - x - 1, maxRange);
            for (int i = 1; i <= range; i++)
            {
                var availableMove = new Tuple<int, int>(x + i, y);
                var availablePiece = lvlMono.GetPiece(availableMove);
                if (availablePiece != null && (!this.IsEnemyOf(availablePiece) || availablePiece.IsTriangle()))
                {
                    break;
                }
                else if (availablePiece != null && this.IsEnemyOf(availablePiece))
                {
                    legalSpots.Add(availableMove);
                    break;
                }
                else { legalSpots.Add(availableMove); }
            }

            // up
            range = UnityEngine.Mathf.Min(boardHeight - y - 1, maxRange);
            for (int j = 1; j <= range; j++)
            {
                var availableMove = new Tuple<int, int>(x, y + j);
                var availablePiece = lvlMono.GetPiece(availableMove);
                if (availablePiece != null && (!this.IsEnemyOf(availablePiece) || availablePiece.IsTriangle()))
                {
                    break;
                }
                else if (availablePiece != null && this.IsEnemyOf(availablePiece))
                {
                    legalSpots.Add(availableMove);
                    break;
                }
                else { legalSpots.Add(availableMove); }
            }

            // down
            range = UnityEngine.Mathf.Min(y, maxRange);
            for (int j = 1; j <= range; j++)
            {
                var availableMove = new Tuple<int, int>(x, y - j);
                var availablePiece = lvlMono.GetPiece(availableMove);
                if (availablePiece != null && (!this.IsEnemyOf(availablePiece) || availablePiece.IsTriangle()))
                {
                    break;
                }
                else if (availablePiece != null && this.IsEnemyOf(availablePiece))
                {
                    legalSpots.Add(availableMove);
                    break;
                }
                else { legalSpots.Add(availableMove); }
            }

            var availableMoves = new List<Tuple<int, int>>();
            availableMoves.Add(new Tuple<int, int>(x - 1, y - 1));
            availableMoves.Add(new Tuple<int, int>(x - 1, y + 1));
            availableMoves.Add(new Tuple<int, int>(x + 1, y - 1));
            availableMoves.Add(new Tuple<int, int>(x + 1, y + 1));
            foreach (Tuple<int, int> move in availableMoves)
            {
                if (move.Item1 >= 0 && move.Item1 < boardWidth && move.Item2 >= 0 && move.Item2 < boardHeight)
                {
                    if (lvlMono.GetPiece(move) != null && (!this.IsEnemyOf(lvlMono.GetPiece(move)) || (lvlMono.GetPiece(move).IsTriangle())))
                    {
                        continue;
                    }
                    legalSpots.Add(move);
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
            else if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 2)
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

        public override List<Tuple<int, int>> GetVisibleArea(int range)
        {
            List<Tuple<int, int>> visibleArea = new List<Tuple<int, int>>();
            return visibleArea;
        }
    }
}