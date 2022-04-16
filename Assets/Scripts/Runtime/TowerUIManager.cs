using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Button _towerOneBtn;
    [SerializeField] private TowerData _towerOneData;
    [SerializeField] private Button _towerTwoBtn;
    [SerializeField] private TowerData _towerTwoData;
    [SerializeField] private Button _towerThreeBtn;
    [SerializeField] private TowerData _towerThreeData;
    [SerializeField] private LayerMask _gridLayerMask;

    private RaycastHit[] _hits = new RaycastHit[1];
    private TowerData _selectedTowerData;
    private TowerGridIndicator _gridIndicatorInstance;

    private bool HasSelectedTower => _selectedTowerData != null;
    
    private void Awake()
    {
        RegisterEvents();
    }

    private void Update()
    {
        if (HasSelectedTower)
        {
            Vector3 indicatorPos;
            if (TryGetPointedCellInfo(out CellInfo cellInfo))
            {
                indicatorPos = cellInfo.Center;
                _gridIndicatorInstance.UpdateIndication(cellInfo.Blocked);
            }
            else
            {
                var ray = GetMouseRay();
                var plane = new Plane(Vector3.up, Vector3.zero);
                plane.Raycast(ray, out float d);
                indicatorPos = ray.GetPoint(d);
            }

            _gridIndicatorInstance.transform.position = indicatorPos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (HasSelectedTower)
        {
            if (TryGetPointedCellInfo(out CellInfo cellInfo))
            {
                if (!cellInfo.Blocked)
                {
                    //TODO look for gold
                    cellInfo.GridSystem.SetCellBlocked(cellInfo.CellPosition, true);
                    Instantiate(_selectedTowerData.TowerPrefab, cellInfo.Center, Quaternion.identity);
                    _selectedTowerData = null;
                    UpdateIndicator();
                }
                else
                {
                    //TODO Float blocked text
                    
                }
            }
        }
    }

    private void RegisterEvents()
    {
        _towerOneBtn.onClick.AddListener(() => OnTowerButtonClicked(_towerOneData));
        _towerTwoBtn.onClick.AddListener(() => OnTowerButtonClicked(_towerTwoData));
        _towerThreeBtn.onClick.AddListener(() => OnTowerButtonClicked(_towerThreeData));
    }

    private Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    private bool TryGetPointedCellInfo(out CellInfo cellInfo)
    {
        var ray = GetMouseRay();
        if (Physics.RaycastNonAlloc(ray, _hits, 100, _gridLayerMask) > 0)
        {
            GridSystem gridSystem = GridSystem.GetFromCollider(_hits[0].collider);
            Assert.IsNotNull(gridSystem);
            cellInfo = gridSystem.GetClosestCelInfo(_hits[0].point);
            return true;
        }

        cellInfo = default;
        return false;
    }

    private void OnTowerButtonClicked(TowerData towerData)
    {
        if (_selectedTowerData != towerData)
        {
            _selectedTowerData = towerData;
            UpdateIndicator();
        }
    }

    private void UpdateIndicator()
    {
        if (_gridIndicatorInstance)
        {
            Destroy(_gridIndicatorInstance.gameObject);
        }

        if (HasSelectedTower)
        {
            _gridIndicatorInstance = Instantiate(_selectedTowerData.GridIndicator);
        }
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void UnregisterEvents()
    {
        _towerOneBtn.onClick.RemoveAllListeners();
        _towerTwoBtn.onClick.RemoveAllListeners();
        _towerThreeBtn.onClick.RemoveAllListeners();
    }
}
