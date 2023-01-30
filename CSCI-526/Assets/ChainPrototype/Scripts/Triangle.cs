using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : Piece
{

    public override bool IsMoveLegal(Vector2 destination)
    {
        /*triangle moves like a bishop (diagonal)
         *  |srcx - destx| == |srcy- desty|
        */
        var pos = transform.position;
        if (Mathf.Abs(destination.x - pos.x) == Mathf.Abs(destination.y - pos.y))
        {
            return true;
        }
        return false;
    }

    public override List<Vector2> LegalMoves(int boardWidth, int boardHeight)
    {
        List<Vector2> legalSpots = new List<Vector2>();

        /*triangle moves like a bishop (diagonal)
         *  delta(x) = delta(y)
        */
        var pos = transform.position;

        bool triangleAbilityCheck = this.TriangleAbilityCheck(pos);
        bool useAbility = false;

        if (triangleAbilityCheck == true && GameManager.Instance.UsedAbility != true)
        {
            //enable some ability option to add another movement turn
            //MenuManager Option pops up
            MenuManager.Instance.ShowAbilityButton();
            //if option is selected, ignore triangle legal moves + add 1 to NumMoves?

            //useAbility = true;
            //GameManager.Instance.UsedAbility = true;
        }
		
		int maxRange = 3;
		// top left 
		var minIndex = UnityEngine.Mathf.Min(pos.x, boardHeight - pos.y - 1);
		var range = UnityEngine.Mathf.Min(minIndex, maxRange);
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
                if (GridManager.Instance._pieces[coord] != null && this.isWhite == GridManager.Instance._pieces[coord].isWhite)
                {
                    adjAlly.Add(coord);
                }
            }
        }
        if (adjAlly.Count == 0)
            return null;
        return adjAlly;
    }


    private bool TriangleAbilityCheck(Vector2 pos)
    {
        var adjList = this.adjacentAllies(pos);
        if (adjList != null)
        {
            foreach (Vector2 coord in adjList)
            {
                if (GridManager.Instance.GetPiece(coord).unitName != "Triangle")
                {
                    Debug.Log("Set Triangle Ability True");
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
