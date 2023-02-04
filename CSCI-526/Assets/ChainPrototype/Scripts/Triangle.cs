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

    public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
    {
        List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

        /*triangle moves like a bishop (diagonal)
         *  delta(x) = delta(y)
        */
        var pos = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        bool triangleAbilityCheck = this.TriangleAbilityCheck(pos);
        //bool useAbility = false;
        //var lvlModel = GridManager.Instance.levelModel;

        if (triangleAbilityCheck == true && GameManagerChain.Instance.UsedAbility != true)
        {
            //enable some ability option to add another movement turn
            //MenuManager Option pops up
            MenuManager.Instance.ShowAbilityButton();
            //if option is selected, ignore triangle legal moves + add 1 to NumMoves?

            //useAbility = true;
            //GameManager.Instance.UsedAbility = true;
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


    private bool TriangleAbilityCheck(Vector2Int pos)
    {
        var adjList = this.adjacentAllies(new Tuple<int, int>(pos.x, pos.y));
        if (adjList != null)
        {
            foreach (Tuple<int, int> coord in adjList)
            {
                if (lvlModel.TryGetUnit(coord[0], coord[1])[1].unitName == "Circle")
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
