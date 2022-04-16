using System.Collections.Generic;
using UnityEngine;

public struct CellEnumerator : IEnumerator<Vector3Int>
{
    private readonly CellIndexBounds _bounds;
    public Vector3Int _current;

    public CellEnumerator(CellIndexBounds bounds) : this()
    {
        _bounds = bounds;
        Reset();
    }

    public object Current => _current;
    Vector3Int IEnumerator<Vector3Int>.Current => _current;

    public bool MoveNext()
    {
        if (++_current.z > _bounds.Max.z)
        {
            _current.z = _bounds.Min.z;
            return ++_current.x <= _bounds.Max.x;
        }

        return true;
    }

    public void Reset()
    {
        _current = _bounds.Min - Vector3Int.forward;
    }

    public void Dispose() { }
}