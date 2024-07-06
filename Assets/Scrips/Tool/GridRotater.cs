using System;

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
}