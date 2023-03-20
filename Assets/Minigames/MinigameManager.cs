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

    public bool isBombOnScreen { get; set; } = false;

    private void OnEnable()
    {
        PlayerCollision.OnCarCollision += SpawnMinigame;
        Minigame.OnMinigameWon += FreeUpMinigamePosition;
    }
    private void OnDisable()
    {
        PlayerCollision.OnCarCollision -= SpawnMinigame;
        Minigame.OnMinigameWon -= FreeUpMinigamePosition;
    }

    private void Start()
    {
        availabilityByPosition = minigamePositions.ToDictionary(pos => pos, availability => true);
    }
    public void SpawnMinigame()
    {
        var openMinigamePositions = availabilityByPosition.Where(pos => pos.Value).Select(pos => pos.Key);
        if (openMinigamePositions.Any())
        {
            // choose random Vector2 from open minigame positions
            int randomIndex = Random.Range(0, openMinigamePositions.Count());
            Vector2 minigamePosition = openMinigamePositions.ElementAt(randomIndex);

            // assign false to availability at minigameposition
            availabilityByPosition[minigamePosition] = false;

            // pick minigame, reset it, and move it to the position
            Minigame minigame = PickMinigame();
            minigame.ResetState();
            minigame.SetPosition(minigamePosition);
        }
        else GameManager.instance.ChangeState(GameState.lose);
    }

    private Minigame PickMinigame()
    {
        // if a bomb is selected, return the bomb minigame
        if (WillGetBomb())
        {
            isBombOnScreen = true;
            return bombMinigamePool.Get().GetComponent<Minigame>();
        }
        // otherwise, pick a regular minigame
        return minigamePools[Random.Range(0, minigamePools.Count)].Get().GetComponent<Minigame>();
    }

    // returns true if a bomb is rolled and there isn't already a bomb on screen
    private bool WillGetBomb() => Random.value < bombMinigameChance && !isBombOnScreen;

    private void FreeUpMinigamePosition(Vector2 minigamePosition)
    {
        availabilityByPosition[minigamePosition] = true;
    }
}
