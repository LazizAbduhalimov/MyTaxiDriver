using System.Linq;
using Client;
using Client.Game;
using LGrid;
using UnityEngine;

public class DragAndDrop : MouseHoldCount
{
    private TaxiBase _taxiBase;
    private Grid Grid => Links.Instance.Grid;
    private Vector3 _offset;
    private Camera _mainCamera;
    private Vector3 _initialPoint;

    void Start()
    {
        _taxiBase = GetComponent<TaxiBase>();
        _mainCamera = Camera.main;
        if (_taxiBase == null) Debug.LogError("Null taxi");
        if (Time.frameCount != 1) return;
        var cellToWorld = GetSnappedPosition(transform.position);
        FillCell(Vector3Int.RoundToInt(cellToWorld));
    }

    private void OnEnable()
    {
        _mainCamera ??= Camera.main;
    }

    protected override bool ShouldHandleClick()
    {
        return PassedTime >= SleepTime;
    }

    protected override void HandleDown()
    {
        base.HandleDown();
        _initialPoint = transform.position; 
    }

    protected override void HandleDrag()
    {
        base.HandleDrag();
        if (ConditionVerified)
            transform.position = GetLimitedMousePosition();
    }

    protected override void HandleClick()
    {
        var cellToWorld = GetSnappedPosition(GetLimitedMousePosition());
        if (!Map.Instance.IsCellExists(cellToWorld, out var cell) ||
            cellToWorld == _initialPoint)
        {
            transform.position = _initialPoint;
            return;
        }

        if (cell.TaxiBase == null)
        {
            if (Map.Instance.IsCellExists(GetSnappedPosition(_initialPoint), out var initialCell))
            {
                initialCell.TaxiBase = null;
            }

            cell.TaxiBase = _taxiBase;
            transform.position = cellToWorld;
            return;
        }
        
        if (cell.TaxiBase.Level == _taxiBase.Level &&
            !cell.TaxiBase.IsDriving)
        {
            if (AllVehicles.Instance.CarsPool.Length > _taxiBase.Level)
                Debug.Log("Max Level Reached!");
            Debug.Log($"Upgrade! from level {_taxiBase.Level} to {_taxiBase.Level+1}");
            if (Map.Instance.IsCellExists(GetSnappedPosition(_initialPoint), out var initialCell))
            {
                initialCell.TaxiBase.gameObject.SetActive(false);
                initialCell.TaxiBase = null;
            }
            cell.TaxiBase.gameObject.SetActive(false);
            var pool = AllVehicles.Instance.CarsPool[_taxiBase.Level];
            var createdObject = pool.GetFromPool(cell.Position);
            cell.TaxiBase = createdObject.GetComponent<TaxiBase>();
        }
        else
        {
            transform.position = GetSnappedPosition(_initialPoint);
        }
    }

    private void FillCell(Vector3Int cellPosition)
    {
        if (!Map.Instance.IsCellExists(cellPosition, out var cell))
        {
            cell = Map.Instance.CreateCell(cellPosition);
        }
        else
        {
            if (cell.TaxiBase != null)
            {
                var emptyCell = Map.Instance.Cells.Values.FirstOrDefault(c => c.TaxiBase == null);
                if (emptyCell == null)
                {
                    Debug.LogError("Too many objects");
                    gameObject.SetActive(false);
                    return;
                }

                cell = emptyCell;
            }
        }
        cell.TaxiBase = _taxiBase;
        transform.position = cell.Position;
    }
    
    private Vector3 GetMouseWorldPosition()
    {
        var mouseScreenPosition = Input.mousePosition;
        var plane = new Plane(Vector3.up, Vector3.zero);
        var ray = _mainCamera.ScreenPointToRay(mouseScreenPosition);
        return plane.Raycast(ray, out var distance) ? ray.GetPoint(distance) : Vector3.zero;
    }

    private Vector3 GetLimitedMousePosition()
    {
        var mousePosition = GetMouseWorldPosition();
        return mousePosition;
    }

    private Vector3 GetSnappedPosition(Vector3 position)
    {
        return Grid.CellToWorld(Grid.WorldToCell(position));
    }
}