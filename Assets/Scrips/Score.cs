using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
    private static float maxScore;


    public static float MaxScore
    {
        get { return maxScore; }
        set { maxScore = Mathf.Max(maxScore,value); }
    }
}
