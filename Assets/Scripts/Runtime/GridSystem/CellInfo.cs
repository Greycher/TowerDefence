
using UnityEngine;

public struct CellInfo
{
    public GridSystem GridSystem;
    public Vector3 Center;
    public Vector3Int CellPosition;
    public bool Blocked;

    public CellInfo(GridSystem gridSystem, Vector3 center, Vector3Int cellPosition, bool blocked)
    {
        GridSystem = gridSystem;
        Center = center;
        CellPosition = cellPosition;
        Blocked = blocked;
    }
}