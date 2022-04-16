using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridSystem : MonoBehaviour
{
    [SerializeField] private float cellSizeX = 1;
    [SerializeField] private float cellSizeZ = 1;
    [SerializeField] private float cellGapX = 0.2f;
    [SerializeField] private float cellGapZ = 0.2f;
    [SerializeField] private MeshRenderer _gridMeshRenderer;
    [SerializeField] private MeshRenderer[] _gridBlockerMeshRenderers;
    
    [Header("Gizmos")] 
    [SerializeField] private Color _cellColor = Color.green;
    [SerializeField] private Color _blockedCellColor = Color.red;
    [SerializeField] private Vector3 _drawOffset = Vector3.up * 0.02f;
    [Tooltip("When toggled off, the gizmo is only drawn when the object is selected. " +
             "If toggle on, the gizmo is drawn whether the object is selected or not.")]
    [SerializeField] private bool _drawGizmosAlways;

    private BitArray2D _blockedCellMap;
    private Grid _grid;
    private Vector3Int _cellPosArrayOffset;

    private void GetAndPrepareGrid()
    {
        _grid = GetComponent<Grid>();
        _grid.cellSize = new Vector3(cellSizeX, 0, cellSizeZ);
        _grid.cellGap = new Vector3(cellGapX, 0, cellGapZ);
        _grid.cellLayout = GridLayout.CellLayout.Rectangle;
        _grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;
    }

    private void OnDrawGizmos()
    {
        if (_drawGizmosAlways)
        {
            DrawGizmos();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!_drawGizmosAlways)
        {
            DrawGizmos();
        }
    }

    private void DrawGizmos()
    {
        GetAndPrepareGrid();
        var cellBounds = DetectOverlappedCells(_gridMeshRenderer);

        _cellPosArrayOffset = new Vector3Int(-cellBounds.Min.x, 0, -cellBounds.Min.z);
        _blockedCellMap = new BitArray2D(cellBounds.Max.x - cellBounds.Min.x + 1, cellBounds.Max.z - cellBounds.Min.z + 1);
        
        foreach (var renderer in _gridBlockerMeshRenderers)
        {
            var blockedBounds = DetectOverlappedCells(renderer, true);
            foreach (var cellPosition in blockedBounds)
            {
                if (cellBounds.Contains(cellPosition))
                {
                    var arrIndexPos = cellPosition + _cellPosArrayOffset;
                    _blockedCellMap[arrIndexPos.x, arrIndexPos.z] = true;
                }
            }
        }

        foreach (var cellPosition in cellBounds)
        {
            var cellCenterWorldPos = _grid.GetCellCenterWorld(cellPosition);
            var arrIndexPos = cellPosition + _cellPosArrayOffset;
            var isBlocked = _blockedCellMap[arrIndexPos.x, arrIndexPos.z];
            var color = isBlocked ? _blockedCellColor : _cellColor;
            var oldColor = Gizmos.color;
            Gizmos.color = color;
            {
                Gizmos.DrawCube(cellCenterWorldPos + _drawOffset, _grid.cellSize);
            }
            Gizmos.color = oldColor;
        }
    }

    private CellIndexBounds DetectOverlappedCells(MeshRenderer renderer, bool includeOverflows = false)
    {
        var bounds = renderer.bounds;
        var min = _grid.WorldToCell(bounds.min);
        var max = _grid.WorldToCell(bounds.max);

        if (!includeOverflows)
        {
            var cellExtent = new Vector3(cellSizeX / 2, 0, cellSizeZ / 2);
            var leftBottomPointOfLeftBottomCell = _grid.GetCellCenterWorld(min) - cellExtent;
            if (leftBottomPointOfLeftBottomCell.x < bounds.min.x)
            {
                min.x++;
            }

            if (leftBottomPointOfLeftBottomCell.z < bounds.min.z)
            {
                min.z++;
            }

            var rightTopPointOfRightTopCell = _grid.GetCellCenterWorld(max) + cellExtent;
            if (rightTopPointOfRightTopCell.x > bounds.max.x)
            {
                max.x--;
            }

            if (rightTopPointOfRightTopCell.z > bounds.max.z)
            {
                max.z--;
            }
        }

        return new CellIndexBounds(min, max);
    }
    
    private struct CellIndexBounds : IEnumerable<Vector3Int>
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

        public IEnumerator<Vector3Int> GetEnumerator()
        {
            return new CellEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    private struct CellEnumerator : IEnumerator<Vector3Int>
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
}
