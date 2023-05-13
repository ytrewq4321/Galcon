using UnityEngine;
using UnityEngine.Pool;

public class ShipPool : MonoBehaviour
{
    public static ShipPool Instance { get; private set; }

    [SerializeField] private Ship shipPrefab;
    [SerializeField] private int initialPoolSize = 5000;

    public ObjectPool<Ship> ShipObjectPool { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ShipObjectPool = new ObjectPool<Ship>(() =>
        {
            Ship ship = Instantiate(shipPrefab);
            ship.gameObject.SetActive(false);
            return ship;
        },
        ship => ship.gameObject.SetActive(true),
        ship => ship.gameObject.SetActive(false),
        ship => Destroy(ship.gameObject),
        true,
        initialPoolSize);
    }
}
