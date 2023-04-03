using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinigameManager : StaticInstance<MinigameManager>
{
    [SerializeField] private List<Vector2> minigamePositions;
    [SerializeField] private List<ObjectPooler> minigamePools; // excluding bomb minigame pool
    [SerializeField] private ObjectPooler bombMinigamePool;
    [SerializeField] [Range(0, 1)] private float bombMinigameChance;

    private Dictionary<Vector2, bool> availabilityByPosition;

    public bool isBombOnScreen { get; set; }

    private void OnEnable()
    {
        PlayerCollision.OnPlayerDamaged += SpawnMinigame;
        Minigame.OnMinigameWon += FreeUpMinigamePosition;
    }
    private void OnDisable()
    {
        PlayerCollision.OnPlayerDamaged -= SpawnMinigame;
        Minigame.OnMinigameWon -= FreeUpMinigamePosition;
    }

    private void Start()
    {
        availabilityByPosition = minigamePositions.ToDictionary(pos => pos, availability => true);
    }

    public void SpawnMinigame()
    {
        List<Vector2> openMinigamePositions = availabilityByPosition.
            Where(pos => pos.Value).
            Select(pos => pos.Key).ToList();
        if (openMinigamePositions.Any())
        {
            // choose random Vector2 from open minigame positions
            int randomIndex = Random.Range(0, openMinigamePositions.Count);
            Vector2 minigamePosition = openMinigamePositions.ElementAt(randomIndex);

            // assign false to availability at minigameposition
            availabilityByPosition[minigamePosition] = false;

            // pick minigame and spawn it at the position
            Minigame minigame = PickMinigame();
            minigame.SetPosition(minigamePosition);
        }
        else
        {
            GameManager.instance.ChangeState(GameState.lose);
            PlayerCollision.OnPlayerDamaged -= SpawnMinigame;
        }
    }

    private Minigame PickMinigame()
    {
        // if a bomb is selected, return the bomb minigame
        if (WillGetBomb())
        {
            isBombOnScreen = true;
            return bombMinigamePool.Spawn().GetComponent<Minigame>();
        }
        // otherwise, pick a regular minigame
        return minigamePools[Random.Range(0, minigamePools.Count)].Spawn().GetComponent<Minigame>();
    }

    // returns true if a bomb is rolled and there isn't already a bomb on screen
    private bool WillGetBomb() => Random.value < bombMinigameChance && !isBombOnScreen;

    private void FreeUpMinigamePosition(Vector2 minigamePosition)
    {
        availabilityByPosition[minigamePosition] = true;
    }
}
