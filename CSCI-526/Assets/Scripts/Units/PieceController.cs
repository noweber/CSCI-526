using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Units;
using Assets.Scripts.Piece;

public class PieceController : MonoBehaviour
{
    public Color UnitColor;

    private IPiece pieceModel;

    public UnitType TypeOfUnit;

    public bool IsHumanUnit;

    // TODO: Ask what this is used for.
    public List<Tuple<int, int>> highlightedMoves = new();

    private void Awake()
    {
        Tuple<int, int> position = new((int)transform.position.x, (int)transform.position.y);
        pieceModel = TypeOfUnit switch
        {
            UnitType.Circle => new CircleModel(position, IsHumanUnit),
            UnitType.Diamond => new DiamondModel(position, IsHumanUnit),
            _ => new TriangleModel(position, IsHumanUnit),
        };
        gameObject.GetComponent<SpriteRenderer>().color = UnitColor;
    }

    public List<Tuple<int, int>> GetAdjacentAllies(Tuple<int, int> unitPosition)
    {
        return pieceModel.AdjacentAllies(unitPosition);
    }

    public List<Tuple<int, int>> GetLegalMoves(int boardWidth, int boardHeight)
    {
        return pieceModel.LegalMoves(boardWidth, boardHeight);
    }

    public bool HasMoved()
    {
        return pieceModel.HasMoved();
    }

    public void SetMoveState(bool state)
    {
        pieceModel.SetMoveState(state);
    }

    public bool IsControlledByHuman()
    {
        return pieceModel.IsControlledByHuman();
    }

    public virtual string Name()
    {
        return pieceModel.Name();
    }

    public string Summary()
    {
        return pieceModel.Summary();
    }

    public void UpdateLocation(Vector3 location)
    {
        this.transform.position = location;
        pieceModel.UpdateLocation(new Tuple<int, int>((int)location.x, (int)location.y));
    }
}
