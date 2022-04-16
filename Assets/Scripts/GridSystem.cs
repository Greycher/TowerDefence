using System;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridSystem : MonoBehaviour
{
    [SerializeField] private float cellSizeX;
    [SerializeField] private float cellSizeZ;
    [SerializeField] private float cellGapX;
    [SerializeField] private float cellGapZ;
    [SerializeField] private MeshRenderer _meshRenderer;

    [Header("Gizmos")] 
    [SerializeField] private Color _validCellColor = Color.green;
    [Tooltip("When toggled off, the gizmo is only drawn when the object is selected. " +
             "If toggle on, the gizmo is drawn whether the object is selected or not.")]
    [SerializeField] private bool _drawGizmosAlways;

    private Grid _grid;

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
        var bounds = _meshRenderer.bounds;
        var minCellPos = _grid.WorldToCell(bounds.min);
        var maxCellPos = _grid.WorldToCell(bounds.max);

        var cellExtent = new Vector3(cellSizeX / 2, 0, cellSizeZ / 2);

        var leftBottomPointOfLeftBottomCell = _grid.GetCellCenterWorld(minCellPos) - cellExtent;
        if (leftBottomPointOfLeftBottomCell.x < bounds.min.x)
        {
            minCellPos.x++;
        }

        if (leftBottomPointOfLeftBottomCell.z < bounds.min.z)
        {
            minCellPos.z++;
        }

        var rightTopPointOfRightTopCell = _grid.GetCellCenterWorld(maxCellPos) + cellExtent;
        if (rightTopPointOfRightTopCell.x > bounds.max.x)
        {
            maxCellPos.x--;
        }

        if (rightTopPointOfRightTopCell.z > bounds.max.z)
        {
            maxCellPos.z--;
        }

        for (var x = minCellPos.x; x <= maxCellPos.x; x++)
        {
            for (var z = minCellPos.z; z <= maxCellPos.z; z++)
            {
                var cellPosition = new Vector3Int(x, 0, z);
                var cellCenterWorldPos = _grid.GetCellCenterWorld(cellPosition);
                var oldColor = Gizmos.color;
                Gizmos.color = _validCellColor;
                {
                    Gizmos.DrawCube(cellCenterWorldPos + Vector3.up * 0.01f, _grid.cellSize);
                }
                Gizmos.color = oldColor;
            }
        }
    }
}
