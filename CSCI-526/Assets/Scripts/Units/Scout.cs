using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Units
{
    public class Scout : PieceMono
    {
        public override string getUnitInfo()
        {
            return "Grants vision.";
        }

        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

            //Circle moves like a king (delta(x) + delta(y) <= 2)
			var pos = this.transform.position;
			int x = (int)pos.x;
			int y = (int)pos.y;
            var lvlMono = LevelMono.Instance;

            //add else block
            var availableMoves = new List<Tuple<int, int>>();
            availableMoves.Add(new Tuple<int, int>(x - 1, y - 1));
            availableMoves.Add(new Tuple<int, int>(x - 1, y));
            availableMoves.Add(new Tuple<int, int>(x - 1, y + 1));
            availableMoves.Add(new Tuple<int, int>(x, y - 1));
            availableMoves.Add(new Tuple<int, int>(x, y + 1));
            availableMoves.Add(new Tuple<int, int>(x + 1, y - 1));
            availableMoves.Add(new Tuple<int, int>(x + 1, y));
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
                // Diamond moves first, circle must not
                legalSpots.Clear();
            }
            else if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 1)
            {
                // Circle must move closer to enemies
                legalSpots.Clear();
                var availableMove = new Tuple<int, int>(3, 3);
                legalSpots.Add(availableMove);
            }
            else if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 2)
            {
                // Circle must capture an enemy
                legalSpots.Clear();
                var availableMove = new Tuple<int, int>(3, 4);
                legalSpots.Add(availableMove);
            }
            else if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 2)
            {
                // Free form movement for last capture.
            }
            
            return legalSpots;
        }

        public List<Tuple<int, int>> GetVisionRange()
        {
            List<Tuple<int, int>> area = new List<Tuple<int, int>>();
            return area;
        }
        
    }

}