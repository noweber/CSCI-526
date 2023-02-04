using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Circle : Piece
{

    public override bool IsMoveLegal(Vector2 destination)
    {
        //Circle moves like a king (if x and/or y change by 1)
        var pos = transform.position;
        if (Mathf.Abs(destination.x - pos.x) == 1 && Mathf.Abs(destination.y - pos.y) <= 1)
        {
            return true;
        }
        else if (Mathf.Abs(destination.x - pos.x) <= 1 && Mathf.Abs(destination.y - pos.y) == 1)
        {
            return true;
        }

        return false;
    }

    public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
    {
        List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

        //Circle moves like a king (delta(x) + delta(y) <= 2)
        var pos = new Vector2Int ((int)transform.position.x, (int)transform.position.y);
        bool changeMovement = this.CircleMovementCheck(pos);
		var lvlModel = GridManager.Instance.levelModel;
        if (changeMovement == true)
        {
            Debug.Log("Circle should have queen movement");

			int maxRange = 3;
			// left 
			var range = UnityEngine.Mathf.Min(pos.x, maxRange);
			for (int i = 1; i <= range; i++) 
			{
				var availableMove = new Tuple<int, int>(pos.x - i, 	pos.y);
				if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] == this.isWhite)
				{
					break;
				} else if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] != this.isWhite){
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
				if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] == this.isWhite)
				{
					break;
				} else if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] != this.isWhite){
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
				if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] == this.isWhite)
				{
					break;
				} else if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] != this.isWhite){
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
				if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] == this.isWhite)
				{
					break;
				} else if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] != this.isWhite){
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
				if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] == this.isWhite)
				{
					break;
				} else if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] != this.isWhite){
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
				if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] == this.isWhite)
				{
					break;
				} else if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] != this.isWhite){
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
				if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] == this.isWhite)
				{
					break;
				} else if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] != this.isWhite){
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
				if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] == this.isWhite)
				{
					break;
				} else if (lvlModel.TryGetUnit(i, j) != null && lvlModel.TryGetUnit(i, j)[0] != this.isWhite){
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
				if (move[0] >= 0 && move[0] < boardWidth && move[1] >= 0 && move[1] < boardHeight) 
				{
					if (lvlModel.TryGetUnit(move[0],move[1]) != null && lvlModel.TryGetUnit(move[0],move[1])[0] == this.isWhite) 
					{
						continue;
					}
					legalSpots.Add(move);
				}
			}

        }

        return legalSpots;
    }

    public override List<Tuple<int, int>> adjacentAllies(Tuple<int, int> unitPosition)
    {
        var adjacentList = new List<Tuple<int, int>>();
        adjacentList.Add(new Tuple<int, int>(unitPosition.x + 1, unitPosition.y));
        adjacentList.Add(new Tuple<int, int>(unitPosition.x - 1, unitPosition.y));
        adjacentList.Add(new Tuple<int, int>(unitPosition.x, unitPosition.y + 1));
        adjacentList.Add(new Tuple<int, int>(unitPosition.x, unitPosition.y - 1));

        var adjAlly = new List<Tuple<int, int>>();
        var lvlModel = GridManager.Instance.levelModel;
        foreach (Tuple<int, int> coord in adjacentList)
        {
            if (lvlModel.TryGetUnit(coord[0], coord[1])[0] == this.isWhite)
            {
                adjAlly.Add(coord);
            }
        }
        if (adjAlly.Count == 0)
            return null;
        return adjAlly;
    }

    private bool CircleMovementCheck(Vector2Int pos)
    {
        var adjList = this.adjacentAllies(new Tuple<int, int>(pos.x, pos.y));
        if (adjList != null)
        {
            foreach (Tuple<int, int> coord in adjList)
            {
                if (GridManager.Instance.GetPiece(coord).unitName == "Diamond")
                {
                    Debug.Log("Make Circle move like Queen");
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
