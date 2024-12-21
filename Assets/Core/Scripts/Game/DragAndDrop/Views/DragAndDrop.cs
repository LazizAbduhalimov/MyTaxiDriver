using Client.Game;
using LGrid;
using UnityEngine;

public class DragAndDrop : MouseHoldCount
{
    public TaxiBase TaxiBase;
    public Grid Grid;

    private Vector3 _offset;
    private Camera _mainCamera;
    private Vector3 _initialPoint;

    void Start()
    {
        var cellToWorld = GetSnappedPosition(transform.position);
        FillCell(Vector3Int.RoundToInt(cellToWorld));
        _mainCamera = Camera.main;
    }

    protected override bool ShouldHandleClick()
    {
        return PassedTime >= SleepTime && !TaxiBase.IsDriving;
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
            transform.position = GetMouseWorldPosition2();
    }

    protected override void HandleClick()
    {
        var cellToWorld = GetSnappedPosition(GetMouseWorldPosition2());
        if (!Map.Instance.IsCellExists(cellToWorld, out var cell))
        {
            cell = Map.Instance.CreateCell(Vector3Int.RoundToInt(cellToWorld));
        }

        if (cellToWorld == _initialPoint)
        {
            transform.position = cellToWorld;
            return;
        }
        
        if (cell.TaxiBase == null)
        {
            if (Map.Instance.IsCellExists(GetSnappedPosition(_initialPoint), out var initialCell))
            {
                initialCell.TaxiBase = null;
            }

            cell.TaxiBase = TaxiBase;
            transform.position = cellToWorld;
            return;
        }

        if (cell.TaxiBase.Level == TaxiBase.Level)
        {
            Debug.Log($"Upgrade! from level {TaxiBase.Level} to {TaxiBase.Level+1}");
            if (Map.Instance.IsCellExists(GetSnappedPosition(_initialPoint), out var initialCell))
            {
                Destroy(initialCell.TaxiBase.gameObject);
                initialCell.TaxiBase = null;
            }
            Destroy(cell.TaxiBase.gameObject);
            cell.TaxiBase = null;
        }
    }

    private void FillCell(Vector3Int cellPosition)
    {
        if (!Map.Instance.IsCellExists(cellPosition, out var cell))
        {
            cell = Map.Instance.CreateCell(cellPosition);
        }
        if (TaxiBase == null) Debug.LogError("Null taxi");
        cell.TaxiBase = TaxiBase;
        transform.position = cellPosition;
    }
    
    private Vector3 GetMouseWorldPosition2()
    {
        var mouseScreenPosition = Input.mousePosition;
        var plane = new Plane(Vector3.up, Vector3.zero);
        var ray = _mainCamera.ScreenPointToRay(mouseScreenPosition);
        return plane.Raycast(ray, out var distance) ? ray.GetPoint(distance) : Vector3.zero;
    }

    private Vector3 GetSnappedPosition(Vector3 position)
    {
        return Grid.CellToWorld(Grid.WorldToCell(position));
    }
}