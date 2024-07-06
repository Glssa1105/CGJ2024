using System;
using System.Collections.Generic;
using UnityEngine;

public class GridRotater
{
    public static EDirType RotateDirMap(EDirType dirMap, EGridRotate currentRotation)
    {
        EDirType rotatedDirMap = 0;

        foreach (EDirType dir in Enum.GetValues(typeof(EDirType)))
        {
            if (dirMap.HasFlag(dir))
            {
                EDirType rotatedDir = RotateDirection(dir, currentRotation);
                rotatedDirMap |= rotatedDir;
            }
        }

        return rotatedDirMap;
    }

    private static EDirType RotateDirection(EDirType direction, EGridRotate rotation)
    {
        switch (rotation)
        {
            case EGridRotate.UP:
                return direction;
            case EGridRotate.RIGHT:
                return direction switch
                {
                    EDirType.UP => EDirType.RIGHT,
                    EDirType.RIGHT => EDirType.DOWN,
                    EDirType.DOWN => EDirType.LEFT,
                    EDirType.LEFT => EDirType.UP,
                    _ => direction
                };
            case EGridRotate.DOWN:
                return direction switch
                {
                    EDirType.UP => EDirType.DOWN,
                    EDirType.RIGHT => EDirType.LEFT,
                    EDirType.DOWN => EDirType.UP,
                    EDirType.LEFT => EDirType.RIGHT,
                    _ => direction
                };
            case EGridRotate.LEFT:
                return direction switch
                {
                    EDirType.UP => EDirType.LEFT,
                    EDirType.RIGHT => EDirType.UP,
                    EDirType.DOWN => EDirType.RIGHT,
                    EDirType.LEFT => EDirType.DOWN,
                    _ => direction
                };
            default:
                return direction;
        }
    }
    
    
    
    private static Vector2Int GetNewPosition(Vector2Int currentPosition, EDirType direction)
    {
        switch (direction)
        {
            case EDirType.UP:
                return new Vector2Int(currentPosition.x, currentPosition.y + 1);
            case EDirType.DOWN:
                return new Vector2Int(currentPosition.x, currentPosition.y - 1);
            case EDirType.LEFT:
                return new Vector2Int(currentPosition.x - 1, currentPosition.y);
            case EDirType.RIGHT:
                return new Vector2Int(currentPosition.x + 1, currentPosition.y);
            default:
                return currentPosition;
        }
    }

    public static Dictionary<EDirType,Grid> AccessMultiplePoints(Vector2Int currentPosition, EDirType dirMap,Grid[,] grids)
    {
        Dictionary<EDirType,Grid> gridList = new Dictionary<EDirType,Grid>();
        foreach (EDirType direction in System.Enum.GetValues(typeof(EDirType)))
        {
//                Debug.Log(dirMap+""+direction);
            if ((dirMap & direction) == direction)
            {
                Vector2Int newPosition = GetNewPosition(currentPosition, direction);
                newPosition = new Vector2Int(newPosition.y, newPosition.x);
               // Debug.Log(newPosition);
                if (IsValidPosition(newPosition,grids))
                {
                    Grid value = grids[newPosition.x, newPosition.y];
                    gridList.Add(direction,value);
                    Debug.Log(value);
                }
            }
        }

        return gridList;
    }
    private static bool IsValidPosition(Vector2Int position,Grid[,] grid)
    {

        
        return position.x >= 0 && position.x < grid.GetLength(0) &&
               position.y >= 0 && position.y < grid.GetLength(1);
    }
}