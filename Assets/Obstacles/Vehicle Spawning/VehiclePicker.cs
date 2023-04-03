using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehiclePicker : MonoBehaviour
{
    [SerializeField] private List<VehicleType> vehicleTypes;

    /// <summary>Cumulative weights for each vehicle type</summary>
    private List<int> cumulativeTypeWeights;

    /// <summary>The cumulative sprite weights for a given vehicle type</summary>
    private Dictionary<VehicleType, List<int>> cumulativeSpriteWeightsByType;

    private void Awake()
    {
        cumulativeTypeWeights = CreateCumulativeSumList(vehicleTypes.Select(type => type.probability).ToList());
        cumulativeSpriteWeightsByType = new Dictionary<VehicleType, List<int>>();
    }

    /// <summary>
    /// Takes a list of integers and returns a list where each element is the sum of all integers that came before it
    /// </summary>
    private static List<int> CreateCumulativeSumList(List<int> list)
    {
        int runningSum = 0;
        List<int> cumulativeList = new List<int>(new int[list.Count]);

        for (int i = 0; i < list.Count; i++)
        {
            runningSum += list[i];
            cumulativeList[i] = runningSum;
        }

        return cumulativeList;
    }

    /// <summary>
    /// Picks a vehicle type and sprite color based on weighted probabilities
    /// and then assigns a sprite and collider to the vehicle.
    /// </summary>
    public void Randomize(Vehicle vehicle)
    {
        VehicleType selectedType = GetWeightedVehicleType();
        Sprite selectedSprite = GetWeightedSpriteByType(selectedType);

        vehicle.GetComponent<SpriteRenderer>().sprite = selectedSprite;
        vehicle.GetComponent<PolygonCollider2D>().SetPath(0, selectedType.colliderPoints);
    }

    private VehicleType GetWeightedVehicleType()
    {
        // random number from 0 to the sum of all weights
        int randomThreshold = Random.Range(0, cumulativeTypeWeights.Last());

        int vehicleTypeIndex = cumulativeTypeWeights.FindIndex(weight => randomThreshold < weight);
        return vehicleTypes[vehicleTypeIndex];
    }

    private Sprite GetWeightedSpriteByType(VehicleType type)
    {
        AddSpriteWeightsToDictionary(type);

        // random number from 0 to the sum of all weights
        int randomThreshold = Random.Range(0, cumulativeSpriteWeightsByType[type].Last());

        int spriteIndex = cumulativeSpriteWeightsByType[type].FindIndex(weight => randomThreshold < weight);
        return type.sprites[spriteIndex].sprite;
    }

    private void AddSpriteWeightsToDictionary(VehicleType type)
    {
        // save the cumulative sprite weights for a type if it isn't already in the dictionary
        if (!cumulativeSpriteWeightsByType.ContainsKey(type))
        {
            cumulativeSpriteWeightsByType[type] =
                CreateCumulativeSumList(type.sprites.Select(sprite => sprite.probability).ToList());
        }
    }
}