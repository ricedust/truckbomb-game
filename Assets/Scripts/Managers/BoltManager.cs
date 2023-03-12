using UnityEngine;
using TMPro;

public class BoltManager : StaticInstance<BoltManager>
{
    [Header("References")]
    [SerializeField] private ObjectPooler boltPool;
    [SerializeField] private TextMeshProUGUI boltsCollectedText;
    [Header("Spawn Parameters")]
    [SerializeField] private float spawnIntervalInSeconds;
    [SerializeField] private float spawnYPosition;

    private int boltsCollected;

    // attach and detach listener to OnBoltCollected event
    private void OnEnable() => Bolt.OnBoltCollected += UpdateBoltsCollected;
    private void OnDisable() => Bolt.OnBoltCollected -= UpdateBoltsCollected;

    public void SpawnBoltsInInterval()
    {
        // for some godforsaken reason, invoke repeating will fire twice at the beginning unless set to 0.1f
        InvokeRepeating("SpawnBolt", 0.1f, spawnIntervalInSeconds);
    }

    private void SpawnBolt()
    {
        GameObject bolt = boltPool.Get();
        bolt.transform.position = GenerateSpawnPosition();
    }

    /// <summary>
    /// Returns a Vector2 spawn position at the top of a random lane
    /// </summary>
    private Vector2 GenerateSpawnPosition()
    {
        int laneIndex = Random.Range(0, EnvironmentManager.instance.GetNumLanes());
        float laneXPos = EnvironmentManager.instance.GetLaneX(laneIndex);

        return new Vector2(laneXPos, spawnYPosition);
    }

    private void UpdateBoltsCollected()
    {
        boltsCollected++;
        boltsCollectedText.text = "BOLTS: " + boltsCollected;
    }
}
