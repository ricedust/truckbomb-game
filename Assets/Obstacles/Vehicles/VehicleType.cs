using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vehicle Type", menuName = "Scriptable Objects/Vehicle Type")]
public class VehicleType : ScriptableObject
{
    [field: SerializeField] public int probability { get; private set; }
    [field: SerializeField] public List<Vector2> colliderPoints { get; private set; }
    [field: SerializeField] public List<SpriteProbabilityPair> sprites { get; private set; }
    
    [Serializable]
    public struct SpriteProbabilityPair
    {
        [field: SerializeField] public Sprite sprite { get; private set; }
        [field: SerializeField] public int probability { get; private set; }
    }
}