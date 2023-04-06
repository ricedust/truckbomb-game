using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : StaticInstance<EnvironmentManager>
{
    [field: SerializeField] public float[] laneXPositions { get; private set; } = new float[5];
    [field: SerializeField] public float laneWidth { get; private set; }

    public bool IsValidLane(int lane) => lane >= 0 && lane < laneXPositions.Length; 
    
}