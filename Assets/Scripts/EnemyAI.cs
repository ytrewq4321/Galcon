using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float timeBetweenAttacks = 5f;

    private List<Planet> filteredPlanets = new List<Planet>();

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
            Attack();
        }
    }

    private void Attack()
    {
        Planet startPlanet = GetRandomPlanet(Owner.AI, false);
        Planet targetPlanet = GetRandomPlanet(Owner.AI, true);

        if (startPlanet != null && targetPlanet != null)
        {
            LauchEnemyShips(startPlanet, targetPlanet);
        }
    }

    private Planet GetRandomPlanet(Owner owner, bool excludeOwner)
    {
        List<Planet> planets = GameController.Instance.planets;
        filteredPlanets.Clear();

        foreach (Planet planet in planets)
        {
            if ((excludeOwner && planet.Owner != owner) || (!excludeOwner && planet.Owner == owner))
            {
                filteredPlanets.Add(planet);
            }
        }

        if (filteredPlanets.Count > 0)
        {
            return filteredPlanets[Random.Range(0, filteredPlanets.Count)];
        }

        return null;
    }

    private Vector3 GetRandomPointOnCircle(Vector3 center, float radius)
    {
        float angle = UnityEngine.Random.Range(0f, 360f);
        float x = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = center.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector3(x, y, center.z);
    }

    private void LauchEnemyShips(Planet startPlanet, Planet targetPlanet)
    {
        int shipCount = startPlanet.LaunchShips();
        for (int i = 0; i < shipCount; i++)
        {
            Ship newShip = ShipPool.Instance.ShipObjectPool.Get();
            Vector3 randomSpawnPosition = GetRandomPointOnCircle(startPlanet.transform.position, startPlanet.Radius + 1f);
            newShip.transform.position = randomSpawnPosition;
            newShip.SetOwner(Owner.AI);
            newShip.LaunchShip(targetPlanet.transform);
        }
    }
}
