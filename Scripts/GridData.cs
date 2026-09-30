using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    private Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionsToOccupy, ID, placedObjectIndex);

        foreach (Vector3Int position in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(position))
            {
                throw new Exception($"Dictionary already contains this cell position: {position}");
            }

            placedObjects[position] = data;
        }
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        if (placedObjects.TryGetValue(gridPosition, out PlacementData data))
        {
            foreach (Vector3Int position in data.occupiedPositions)
            {
                placedObjects.Remove(position);
            }
        }
        else
        {
            throw new Exception($"No object found at position: {gridPosition}");
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positions = new();

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                positions.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }

        return positions;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);

        foreach (Vector3Int position in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(position))
            {
                return false;
            }
        }

        return true;
    }
}

internal class PlacementData
{
    public List<Vector3Int> occupiedPositions;

    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int ID, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        this.ID = ID;
        PlacedObjectIndex = placedObjectIndex;
    }
}