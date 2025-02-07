using System.Collections.Generic;
using System.Linq;
using Core.Scripts;
using UnityEngine;

public class UnitSelectionBox : MonoBehaviour
{
    public RectTransform selectionBox;
    private Vector2 startMousePos;
    private List<Unit> _allUnits;
    private List<Unit> _selectedUnits = new ();

    void Start()
    {
        selectionBox.gameObject.SetActive(false);
        _allUnits = FindObjectsOfType<Unit>().ToList();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = Input.mousePosition;
            selectionBox.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0)) 
        {
            UpdateSelectionBox(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0)) // Отпускаем кнопку
        {
            SelectUnits();
            selectionBox.gameObject.SetActive(false);
            DisplaySelectedUnits();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!TryGetRayPointFromScreenToMouse(out var position)) return; 
            if (!TryGetRayPointFromScreenToMouse(out var startPosition, startMousePos)) return;
            var positions = GetCirclePositions(_selectedUnits.Count, position);
            for (var i = 0; i < _selectedUnits.Count; i++)
            {
                _selectedUnits[i].Walk(positions[i]);
            }
        }
    }

    void UpdateSelectionBox(Vector2 currentMousePos)
    {
        var boxStart = startMousePos;
        var boxEnd = currentMousePos;

        var width = Mathf.Abs(boxEnd.x - boxStart.x);
        var height = Mathf.Abs(boxEnd.y - boxStart.y);
        
        var minX = Mathf.Min(boxStart.x, boxEnd.x);
        var minY = Mathf.Min(boxStart.y, boxEnd.y);

        selectionBox.sizeDelta = new Vector2(width, height);
        selectionBox.anchoredPosition = new Vector2(minX, minY + height);
    }

    void SelectUnits()
    {
        var endMouse = (Vector2)Input.mousePosition;
        _selectedUnits.Clear();
        var min = new Vector2(Mathf.Min(endMouse.x, startMousePos.x), Mathf.Min(endMouse.y, startMousePos.y));
        var max = new Vector2(Mathf.Max(endMouse.x, startMousePos.x), Mathf.Max(endMouse.y, startMousePos.y));

        foreach (var unit in _allUnits)
        {
            var screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x &&
                screenPos.y > min.y && screenPos.y < max.y)
            {
                _selectedUnits.Add(unit);
                unit.GetComponentInChildren<Renderer>().material.color = Color.green; // Подсвечиваем
            }
            else
            {
                unit.GetComponentInChildren<Renderer>().material.color = Color.white; // Сбрасываем цвет
            }
        }
    }

    private void DisplaySelectedUnits()
    {
        foreach (var unit in _selectedUnits)
        {
            Debug.Log(unit.name);
        }
    }
    
    private bool TryGetRayPointFromScreenToMouse(out Vector3 position, Vector3? mousePosition = null)
    {
        mousePosition ??= Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mousePosition.Value);
        if (Physics.Raycast(ray, out var hit, 100))
        {
            position = hit.point;
            return true;
        }

        position = default;
        return false;
    }
    
    public static Vector3[] GetCirclePositions(int totalPositions, Vector3 center, float radius = 2f)
    {
        Vector3[] positions = new Vector3[totalPositions];
        float angleStep = 360f / totalPositions;

        for (int i = 0; i < totalPositions; i++)
        {
            float angle = angleStep * i;
            float radians = Mathf.Deg2Rad * angle;

            float x = Mathf.Cos(radians) * radius + center.x;
            float z = Mathf.Sin(radians) * radius + center.z; // Используем center.x для X, center.z для Z

            positions[i] = new Vector3(x, center.y, z); // возвращаем позицию с центром по Y
        }

        return positions;
    }
    
    public static Vector3[] GetRectanglePositions(int totalPositions, Vector3 center, Vector3 startPoint, float stepX, float stepY)
    {
        // Вычисляем вектор направления от startPoint к center
        Vector3 direction = (center - startPoint).normalized;

        // Рассчитываем вектор, перпендикулярный направлению
        Vector3 perpendicularDirection = new Vector3(-direction.z, 0, direction.x); // Поворот на 90 градусов по оси Y

        // Вычисляем количество позиций по осям X и Y
        int positionsX = Mathf.CeilToInt(Mathf.Sqrt(totalPositions));
        int positionsY = Mathf.FloorToInt((float)totalPositions / positionsX);

        // Если позиций больше, чем можно разместить по ширине, увеличиваем Y
        if (positionsX * positionsY < totalPositions)
            positionsY++;

        // Рассчитываем размеры прямоугольника
        float width = (positionsX - 1) * stepX;
        float height = (positionsY - 1) * stepY;

        Vector3[] positions = new Vector3[totalPositions];

        int index = 0;
        for (int i = 0; i < positionsX; i++)
        {
            for (int j = 0; j < positionsY; j++)
            {
                if (index >= totalPositions)
                    return positions;

                // Вычисляем позицию, перемещая вдоль направления и перпендикулярного направления
                Vector3 offset = direction * (i * stepX - width / 2) + perpendicularDirection * (j * stepY - height / 2);
                positions[index] = center + offset;
                index++;
            }
        }

        return positions;
    }
}
