using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Circle : Piece
{
    public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
    {
        List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

        //Circle moves like a king (delta(x) + delta(y) <= 2)
        var pos = new Vector2Int ((int)transform.position.x, (int)transform.position.y);
        bool changeMovement = this.CircleMovementCheck(pos);
		var lvlModel = GridManager.Instance.levelModel;
        if (changeMovement == true)
        {
            //Debug.Log("Circle should have queen movement");

			int maxRange = 3;
			// left 
			var range = UnityEngine.Mathf.Min(pos.x, maxRange);
			for (int i = 1; i <= range; i++) 
			{
				var availableMove = new Tuple<int, int>(pos.x - i, 	pos.y);
				if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 == this.isWhite)
				{
					break;
				} else if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 != this.isWhite){
					legalSpots.Add(availableMove);
					break;
				}
				else { legalSpots.Add(availableMove); }	
				// var availableMove = new Tuple<int, int>(pos.x - i, 	pos.y);
				// if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite == this.isWhite) 
				// { 
				// 	break; 
				// 	}
				// else if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite != this.isWhite) 
				// { 
				// 	legalSpots.Add(availableMove);
				// 	break; 
				// }
				// else { legalSpots.Add(availableMove); }	
			}

			// right
			range = UnityEngine.Mathf.Min(boardWidth - pos.x - 1, maxRange);
			for (int i = 1; i <= range; i++) 
			{
				var availableMove = new Tuple<int, int>(pos.x + i, 	pos.y);
                if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 == this.isWhite)
                {
                    break;
                }
                else if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 != this.isWhite)
                {
                    legalSpots.Add(availableMove);
                    break;
                }
                else { legalSpots.Add(availableMove); }
            }

			// up
			range = UnityEngine.Mathf.Min(boardHeight - pos.y - 1, maxRange);
			for (int j = 1; j <= range; j++) 
			{
				var availableMove = new Tuple<int, int>(pos.x, 	pos.y+j);
                if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 == this.isWhite)
                {
                    break;
                }
                else if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 != this.isWhite)
                {
                    legalSpots.Add(availableMove);
                    break;
                }
                else { legalSpots.Add(availableMove); }
            }

			// down
			range = UnityEngine.Mathf.Min(pos.y, maxRange);
			for (int j = 1; j <= range; j++) 
			{
				var availableMove = new Tuple<int, int>(pos.x, 	pos.y-j);
                if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 == this.isWhite)
                {
                    break;
                }
                else if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 != this.isWhite)
                {
                    legalSpots.Add(availableMove);
                    break;
                }
                else { legalSpots.Add(availableMove); }
            }

			// top left 
			var minIndex = UnityEngine.Mathf.Min(pos.x, boardHeight - pos.y - 1);
			range = UnityEngine.Mathf.Min(minIndex, maxRange);
			for (int i = 1; i <= range; i++) 
			{
				var availableMove = new Tuple<int, int>(pos.x - i, pos.y + i);
                if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 == this.isWhite)
                {
                    break;
                }
                else if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 != this.isWhite)
                {
                    legalSpots.Add(availableMove);
                    break;
                }
                else { legalSpots.Add(availableMove); }
            }

			// top right 
			minIndex = UnityEngine.Mathf.Min(boardWidth - pos.x - 1, boardHeight - pos.y - 1);
			range = UnityEngine.Mathf.Min(minIndex, maxRange);
			for (int i = 1; i <= range; i++) 
			{
				var availableMove = new Tuple<int, int>(pos.x + i, pos.y + i);
                if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 == this.isWhite)
                {
                    break;
                }
                else if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 != this.isWhite)
                {
                    legalSpots.Add(availableMove);
                    break;
                }
                else { legalSpots.Add(availableMove); }
            }

			// bottom left 
			minIndex = UnityEngine.Mathf.Min(pos.x, pos.y);
			range = UnityEngine.Mathf.Min(minIndex, maxRange);
			for (int i = 1; i <= range; i++) 
			{
				var availableMove = new Tuple<int, int>(pos.x - i, pos.y - i);
                if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 == this.isWhite)
                {
                    break;
                }
                else if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 != this.isWhite)
                {
                    legalSpots.Add(availableMove);
                    break;
                }
                else { legalSpots.Add(availableMove); }
            }

			// bottom right 
			minIndex = UnityEngine.Mathf.Min(boardWidth - pos.x - 1, pos.y);
			range = UnityEngine.Mathf.Min(minIndex, maxRange);
			for (int i = 1; i <= range; i++) 
			{
				var availableMove = new Tuple<int, int>(pos.x + i, pos.y - i);
                if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 == this.isWhite)
                {
                    break;
                }
                else if (lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2) != null && lvlModel.TryGetUnit(availableMove.Item1, availableMove.Item2).Item1 != this.isWhite)
                {
                    legalSpots.Add(availableMove);
                    break;
                }
                else { legalSpots.Add(availableMove); }
            }
			
        }
        else
        {
            //add else block
			var availableMoves = new List<Tuple<int, int>>();
			availableMoves.Add(new Tuple<int, int>(pos.x-1, pos.y-1));
			availableMoves.Add(new Tuple<int, int>(pos.x-1, pos.y));
			availableMoves.Add(new Tuple<int, int>(pos.x-1, pos.y+1));
			availableMoves.Add(new Tuple<int, int>(pos.x, pos.y-1));
			availableMoves.Add(new Tuple<int, int>(pos.x, pos.y+1));
			availableMoves.Add(new Tuple<int, int>(pos.x+1, pos.y-1));
			availableMoves.Add(new Tuple<int, int>(pos.x+1, pos.y));
			availableMoves.Add(new Tuple<int, int>(pos.x+1, pos.y+1));
			foreach (Tuple<int, int> move in availableMoves) 
			{
				if (move.Item1 >= 0 && move.Item1 < boardWidth && move.Item2 >= 0 && move.Item2 < boardHeight) 
				{
					if (lvlModel.TryGetUnit(move.Item1, move.Item2) != null && lvlModel.TryGetUnit(move.Item1, move.Item2).Item1 == this.isWhite) 
					{
						continue;
					}
					legalSpots.Add(move);
				}
			}

        }

		
		if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 1) 
		{
			legalSpots.Clear();
			var availableMove = new Tuple<int, int>(3, 3);
			legalSpots.Add(availableMove);
		} 
		else if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 2)
		{
			// designate move
		} 

        return legalSpots;
    }

    public override List<Tuple<int, int>> adjacentAllies(Tuple<int, int> unitPosition)
    { 
        Debug.Log("Unit Position: " + unitPosition.Item1 + ", " + unitPosition.Item2);
        var adjacentList = new List<Tuple<int, int>>();
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2));
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2));
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 + 1));
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 - 1));

        var adjAlly = new List<Tuple<int, int>>();
        var lvlModel = GridManager.Instance.levelModel;
        foreach (Tuple<int, int> coord in adjacentList)
        {
            if (coord.Item1 >= 0 && coord.Item1 < GridManager.Instance._width && coord.Item2 >= 0 && coord.Item2 < GridManager.Instance._height)
            {
                if (lvlModel.TryGetUnit(coord.Item1, coord.Item2) != null && lvlModel.TryGetUnit(coord.Item1, coord.Item2).Item1 == this.isWhite)
                {
                    adjAlly.Add(coord);
                }
            }
        }
        if (adjAlly.Count == 0)
        {
            return null;
        }
        return adjAlly;
    }

    //fix later
    private bool CircleMovementCheck(Vector2Int pos)
    {
        var adjList = this.adjacentAllies(new Tuple<int, int>(pos.x, pos.y));
        var lvlModel = GridManager.Instance.levelModel;
        if (adjList != null)
        {
            foreach (Tuple<int, int> coord in adjList)
            {
                //Debug.Log("" + coord.Item1 + " " + coord.Item2);
                if (lvlModel.TryGetUnit(coord.Item1, coord.Item2).Item2 == Assets.Scripts.Units.UnitType.Diamond)
                {
                    //Debug.Log("Make Circle move like Queen");
                    return true;
                }
            }
        }
        return false;
    }

    public override void UpdateLocation(Vector3 location)
    {
        this.transform.position = location;
    }
}
