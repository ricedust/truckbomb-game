using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class BoltManager : StaticInstance<BoltManager>
{
    [Header("References")]
    [SerializeField] private Bolt boltPrefab;
    [SerializeField] private TextMeshProUGUI boltsCollectedText;
    [Header("Spawn Parameters")]
    [SerializeField] private float spawnIntervalInSeconds;
    [SerializeField] private float spawnYPosition;
    [Header("Pool Parameters")]
    [SerializeField] private int defaultPoolCapacity;
    [SerializeField] private int maxPoolCapacity;

    private ObjectPool<Bolt> boltPool;
    private int boltsCollected;

    // attach and detach listener to OnBoltCollected event
    private void OnEnable() => Bolt.OnBoltCollected += UpdateBoltsCollected;
    private void OnDisable() => Bolt.OnBoltCollected -= UpdateBoltsCollected;

    private void Start()
    {
        boltPool = new ObjectPool<Bolt>(() =>
        { // create function
            Bolt bolt = Instantiate(boltPrefab, transform);
            bolt.Initialize(DespawnBolt);
            return bolt;
        }, bolt =>
        { // action on get
            // Debug.Log("Bolt activated");
            bolt.gameObject.SetActive(true);
        }, bolt =>
        { // action on release
            // Debug.Log("Bolt deactivated");
            bolt.gameObject.SetActive(false);
        }, bolt =>
        { // action on destroy
            // Debug.Log("Bolt destroyed");
            Destroy(bolt.gameObject);
        }, false, defaultPoolCapacity, maxPoolCapacity);
    }

    /// <summary>
    /// Despawn action for bolt objects so that bolts can despawn themselves
    /// </summary>
    private void DespawnBolt(Bolt bolt) => boltPool.Release(bolt);

    public void SpawnBoltsInInterval()
    {
        InvokeRepeating("SpawnBolt", 0, spawnIntervalInSeconds);
    }

    private void SpawnBolt()
    {
        Bolt bolt = boltPool.Get();
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
