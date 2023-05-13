using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlanetSelector planetSelector;

    private void Start()
    {
        planetSelector.TargetSelected += LaunchPlayerShips;
    }

    private void LaunchPlayerShips()
    {
        foreach (var planet in planetSelector.SelectedPlanets)
        {
            int shipCount = planet.LaunchShips();
            for (int i = 0; i < shipCount; i++)
            {
                Ship newShip = ShipPool.Instance.ShipObjectPool.Get();
                Vector3 randomSpawnPosition = GetRandomPointOnCircle(planet.transform.position, planet.Radius + 1f);
                newShip.transform.position = randomSpawnPosition;
                newShip.SetOwner(Owner.Player);
                newShip.LaunchShip(planetSelector.TargetPlanet.transform);
            }
        }
    }

    private Vector3 GetRandomPointOnCircle(Vector3 center, float radius)
    {
        float angle = Random.Range(0f, 360f);
        float x = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = center.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector3(x, y, 0);
    }

    private void OnDestroy()
    {
        planetSelector.TargetSelected -= LaunchPlayerShips;
    }
}
