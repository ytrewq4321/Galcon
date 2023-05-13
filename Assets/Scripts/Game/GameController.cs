using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public List<Planet> planets;

    [SerializeField] private int startShipsCount;
    [SerializeField] private GameObject planetPrefab;
    private MapGenerator mapGenerator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        GeneratePlanets();
    }

    private void GeneratePlanets()
    {
        mapGenerator = new MapGenerator();
        planets = mapGenerator.GenerateMap(planetPrefab);

        int playerStartingPlanetIndex = Random.Range(0, planets.Count);
        Planet playerStartingPlanet = planets[playerStartingPlanetIndex];
        playerStartingPlanet.SetOwner(Owner.Player);
        playerStartingPlanet.SetStartShipsCount(startShipsCount);

        int enemyStartingPlanetIndex;
        do
        {
            enemyStartingPlanetIndex = Random.Range(0, planets.Count);
        }
        while (enemyStartingPlanetIndex == playerStartingPlanetIndex);

        Planet enemyStartingPlanet = planets[enemyStartingPlanetIndex];
        enemyStartingPlanet.SetOwner(Owner.AI);
        playerStartingPlanet.SetStartShipsCount(startShipsCount);
    }
}
