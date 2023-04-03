using UnityEngine;
using TMPro;
using System.Collections;
using Random = UnityEngine.Random;

public class BoltManager : StaticInstance<BoltManager>
{
    [Header("References")]
    [SerializeField] private ObjectPooler boltPool;
    [SerializeField] private TextMeshProUGUI boltsCollectedText;
    [Header("Spawn Parameters")]
    [SerializeField] private float spawnIntervalInSeconds;
    [SerializeField] private float spawnYPosition;

    public int boltsCollected { get; private set; }

    // attach and detach listener to OnBoltCollected event
    private void OnEnable() => Bolt.OnBoltCollected += UpdateBoltsCollected;
    private void OnDisable() => Bolt.OnBoltCollected -= UpdateBoltsCollected;

    public void StartSpawningBolts()
    {
        StartCoroutine(SpawnBoltsOverTime());
    }

    private IEnumerator SpawnBoltsOverTime()
    {
        while (GameManager.instance.state == GameState.inGame)
        {
            yield return new WaitForSeconds(spawnIntervalInSeconds);
            GameObject bolt = boltPool.Spawn();
            bolt.transform.position = GenerateSpawnPosition();
        }
    }

    /// <summary>
    /// Returns a Vector2 spawn position at the top of a random lane
    /// </summary>
    private Vector2 GenerateSpawnPosition()
    {
        int laneIndex = Random.Range(0, EnvironmentManager.instance.laneXPositions.Length);
        float laneXPos = EnvironmentManager.instance.laneXPositions[laneIndex];

        return new Vector2(laneXPos, spawnYPosition);
    }

    private void UpdateBoltsCollected()
    {
        boltsCollected++;
        boltsCollectedText.text = "BOLTS: " + boltsCollected;
    }
}
