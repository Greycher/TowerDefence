using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CellIndexBounds : IEnumerable<Vector3Int>
{
    public Vector3Int Min;
    public Vector3Int Max;

    public CellIndexBounds(Vector3Int min, Vector3Int max)
    {
        Min = min;
        Max = max;
    }

    public bool Contains(Vector3Int point)
    {
        return point.x >= Min.x && point.x <= Max.x 
                                && point.z >= Min.z && point.z <= Max.z;
    }

    public Vector3Int GetClosestCellPosition(Vector3Int cellPosition)
    {
        return new Vector3Int(
            Mathf.Clamp(cellPosition.x, Min.x, Max.x),
            0,
            Mathf.Clamp(cellPosition.z, Min.z, Max.z));
    }

    public IEnumerator<Vector3Int> GetEnumerator()
    {
        return new CellEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}