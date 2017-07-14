using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondenserManager : MonoBehaviour
{
    public Maze mazePrefab;
    private Maze mazeInstance;
    public Player playerPrefab;
    private Player playerInstance;

    // Use this for initialization
    private void Start()
    {
        StartCoroutine(BeginGame());
    }

    private IEnumerator BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
    }

    public void EndGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
    }
}
