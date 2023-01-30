using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override List<Vector2> LegalMoves(int boardWidth, int boardHeight)
    {
        List<Vector2> legalSpots = new List<Vector2>();

        //Circle moves like a king (delta(x) + delta(y) <= 2)
        var pos = transform.position;
        bool changeMovement = this.CircleMovementCheck(pos);

        if (changeMovement == true)
        {
            Debug.Log("Circle should have queen movement");
/*
            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    //Vertical Movement
                    if (pos.x == i)
                    {
                        var coord = new Vector2(i, j);
                        legalSpots.Add(coord);
                    }

                    //Diagonal Movement
                    else if (Mathf.Abs(i - pos.x) == Mathf.Abs(j - pos.y))
                    {
                        var coord = new Vector2(i, j);
                        legalSpots.Add(coord);
                    }
                }
            }
*/

			int maxRange = 3;
			// left 
			var range = UnityEngine.Mathf.Min(pos.x, maxRange);
			for (int i = 1; i <= range; i++) 
			{
				var availableMove = new Vector2(pos.x - i, 	pos.y);
				if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite == this.isWhite) { break; }
				else if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite != this.isWhite) 
				{ 
					legalSpots.Add(availableMove);
					break; 
				}
				else { legalSpots.Add(availableMove); }	
			}

			// right
			range = UnityEngine.Mathf.Min(boardWidth - pos.x - 1, maxRange);
			for (int i = 1; i <= range; i++) 
			{
				var availableMove = new Vector2(pos.x + i, 	pos.y);
				if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite == this.isWhite) { break; }
				else if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite != this.isWhite) 
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
				var availableMove = new Vector2(pos.x, 	pos.y+j);
				if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite == this.isWhite) { break; }
				else if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite != this.isWhite) 
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
				var availableMove = new Vector2(pos.x, 	pos.y-j);
				if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite == this.isWhite) { break; }
				else if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite != this.isWhite) 
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
				var availableMove = new Vector2(pos.x - i, pos.y + i);
				if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite == this.isWhite) { break; }
				else if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite != this.isWhite) 
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
				var availableMove = new Vector2(pos.x + i, pos.y + i);
				if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite == this.isWhite) { break; }
				else if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite != this.isWhite) 
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
				var availableMove = new Vector2(pos.x - i, pos.y - i);
				if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite == this.isWhite) { break; }
				else if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite != this.isWhite) 
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
				var availableMove = new Vector2(pos.x + i, pos.y - i);
				if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite == this.isWhite) { break; }
				else if (GridManager.Instance.GetPiece(availableMove) != null && GridManager.Instance.GetPiece(availableMove).isWhite != this.isWhite) 
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
/*
            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    if (Mathf.Abs(i - pos.x) == 1 && Mathf.Abs(j - pos.y) <= 1)
                    {
                        legalSpots.Add(new Vector2(i, j));
                    }
                    else if (Mathf.Abs(i - pos.x) <= 1 && Mathf.Abs(j - pos.y) == 1)
                    {
                        legalSpots.Add(new Vector2(i, j));
                    }
                }
            }
*/

			var availableMoves = new List<Vector2>();
			availableMoves.Add(new Vector2(pos.x-1, pos.y-1));
			availableMoves.Add(new Vector2(pos.x-1, pos.y));
			availableMoves.Add(new Vector2(pos.x-1, pos.y+1));
			availableMoves.Add(new Vector2(pos.x, pos.y-1));
			availableMoves.Add(new Vector2(pos.x, pos.y+1));
			availableMoves.Add(new Vector2(pos.x+1, pos.y-1));
			availableMoves.Add(new Vector2(pos.x+1, pos.y));
			availableMoves.Add(new Vector2(pos.x+1, pos.y+1));
			foreach (Vector2 move in availableMoves) 
			{
				if (move.x >= 0 && move.x < boardWidth && move.y >= 0 && move.y < boardHeight) 
				{
					if (GridManager.Instance.GetPiece(move) != null && GridManager.Instance.GetPiece(move).isWhite == this.isWhite) 
					{
						continue;
					}
					legalSpots.Add(move);
				}
			}

        }

        if (legalSpots.Count == 0)
            return null;

        return legalSpots;
    }

    public override List<Vector2> adjacentAllies(Vector2 unitPosition)
    {
        var adjacentList = new List<Vector2>();
        adjacentList.Add(new Vector2(unitPosition.x + 1, unitPosition.y));
        adjacentList.Add(new Vector2(unitPosition.x - 1, unitPosition.y));
        adjacentList.Add(new Vector2(unitPosition.x, unitPosition.y + 1));
        adjacentList.Add(new Vector2(unitPosition.x, unitPosition.y - 1));

        var adjAlly = new List<Vector2>();
        foreach (Vector2 coord in adjacentList)
        {
            if (GridManager.Instance._pieces.ContainsKey(coord))
            {
                if (GridManager.Instance._pieces[coord] != null && this.isWhite == GridManager.Instance._pieces[coord].isWhite )
                {
                    adjAlly.Add(coord);
                }
            }
        }
        if (adjAlly.Count == 0)
            return null;
        return adjAlly;
    }

    private bool CircleMovementCheck(Vector2 pos)
    {
        var adjList = this.adjacentAllies(pos);
        if (adjList != null)
        {
            foreach (Vector2 coord in adjList)
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
