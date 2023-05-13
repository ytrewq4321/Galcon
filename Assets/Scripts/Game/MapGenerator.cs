using UnityEngine;
using System.Collections.Generic;

public class MapGenerator
{
    public int minPlanetCount = 5;
    public int maxPlanetCount = 15;
    public Vector2 mapSize = new Vector2(100, 40);
    public int minPlanetRadius = 5;
    public int maxPlanetRadius = 10;
    public int maxAttemptsPerPlanet = 100;

    public List<Planet> GenerateMap(GameObject planetPrefab)
    {
        List<Planet> planets = new List<Planet>();
        int planetCount = Random.Range(minPlanetCount, maxPlanetCount + 1);

        for (int i = 0; i < planetCount; i++)
        {
            int planetRadius = Random.Range(minPlanetRadius, maxPlanetRadius);
            Vector3 position = GetRandomValidPosition(planets, planetRadius);

            if (position != Vector3.zero)
            {
                GameObject newPlanetObject = Object.Instantiate(planetPrefab, position, Quaternion.identity);


                Planet newPlanet = newPlanetObject.GetComponent<Planet>();
                newPlanet.SetRadius(planetRadius);
                newPlanet.SetStartShipsCount(Random.Range(2, 10));

                planets.Add(newPlanet);
            }
        }
        return planets;
    }

    private Vector3 GetRandomValidPosition(List<Planet> planets, float planetRadius)
    {
        Vector3 position = Vector3.zero;
        int attempts = 0;

        while (attempts < maxAttemptsPerPlanet)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-mapSize.x, mapSize.x), Random.Range(-mapSize.y, mapSize.y), 0);
            bool isValid = true;

            foreach (Planet planet in planets)
            {
                float minDistance = planet.Radius + planetRadius;
                float distance = Vector3.Distance(randomPosition, planet.transform.position)- planet.Radius - planetRadius;

                if (distance < minDistance)
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                position = randomPosition;
                break;
            }

            attempts++;
        }
        return position;
    }
}
