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
    private CellIndexBounds _cellBounds;
    private Vector3Int _cellPosArrayOffset;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        GetAndPrepareGrid();
        _cellBounds = DetectOverlappedCells(_gridMeshRenderer);

        _cellPosArrayOffset = new Vector3Int(-_cellBounds.Min.x, 0, -_cellBounds.Min.z);
        _blockedCellMap =
            new BitArray2D(_cellBounds.Max.x - _cellBounds.Min.x + 1, _cellBounds.Max.z - _cellBounds.Min.z + 1);

        foreach (var renderer in _gridBlockerMeshRenderers)
        {
            var blockedBounds = DetectOverlappedCells(renderer, true);
            foreach (var cellPosition in blockedBounds)
            {
                if (_cellBounds.Contains(cellPosition))
                {
                    SetCellBlocked(cellPosition, true);
                }
            }
        }
    }

    private void GetAndPrepareGrid()
    {
        _grid = GetComponent<Grid>();
        _grid.cellSize = new Vector3(cellSizeX, 0, cellSizeZ);
        _grid.cellGap = new Vector3(cellGapX, 0, cellGapZ);
        _grid.cellLayout = GridLayout.CellLayout.Rectangle;
        _grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;
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
    
    private bool GetCellBlocked(Vector3Int cellPosition)
    {
        var arrIndexPos = cellPosition + _cellPosArrayOffset;
        return _blockedCellMap[arrIndexPos.x, arrIndexPos.z];
    }

    public void SetCellBlocked(Vector3Int cellPosition, bool blocked)
    {
        var arrIndexPos = cellPosition + _cellPosArrayOffset;
        _blockedCellMap[arrIndexPos.x, arrIndexPos.z] = blocked;
    }
    
    public CellInfo GetClosestCelInfo(Vector3 point)
    {
        var cellPosition = _cellBounds.GetClosestCellPosition(_grid.WorldToCell(point));
        return new CellInfo(this, _grid.GetCellCenterWorld(cellPosition), cellPosition,
            GetCellBlocked(cellPosition));
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
        Initialize();

        foreach (var cellPosition in _cellBounds)
        {
            var cellCenterWorldPos = _grid.GetCellCenterWorld(cellPosition);
            var isBlocked = GetCellBlocked(cellPosition);
            var color = isBlocked ? _blockedCellColor : _cellColor;
            var oldColor = Gizmos.color;
            Gizmos.color = color;
            {
                Gizmos.DrawCube(cellCenterWorldPos + _drawOffset, _grid.cellSize);
            }
            Gizmos.color = oldColor;
        }
    }
    
    public static GridSystem GetFromCollider(Collider collider)
    {
        return collider.transform.parent.GetComponent<GridSystem>();
    }
}
