using System;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private int ShipProductionRate;

    public Owner Owner { get; private set; }

    public int ShipsCount { get; private set; }

    public float Radius { get; private set; }

    public event Action<int> ShipsCountChanged;
    public event Action<Owner> PlanetOwnerChanged;
    public Action PlanetSelected;
    public Action PlanetUnSelected;

    private float time;

    private void Start()
    {
        GlobalAction.ShipLanded += TakeShip;

        if (Owner == Owner.Player || Owner == Owner.AI)
        {
            ShipsCount = 50;
        }
        else if (Owner == Owner.None)
        {
            ShipsCount = UnityEngine.Random.Range(10, 50);
        }
        ShipsCountChanged(ShipsCount);
    }

    private void Update()
    {
        if(Owner==Owner.Player || Owner == Owner.AI)
        {
            ProduceShips();
        }
    }

    public void TakeShip(Owner shipOwner,Transform planetTransform)
    {
        if (shipOwner == Owner && planetTransform == transform)
        {
            AddShipCount(1);
        }
        else if(planetTransform == transform)
        {
            DecreaseShipCount(1);
            if(ShipsCount<=0)
            {
                SetOwner(shipOwner);
            }
        }
    }

    public int LaunchShips()
    {
        ShipsCount = ShipsCount / 2;
        ShipsCountChanged.Invoke(ShipsCount);
        return ShipsCount;
    }

    public void AddShipCount(int count)
    {
        ShipsCount += count;
        ShipsCountChanged.Invoke(ShipsCount);
    }

    public void DecreaseShipCount(int count)
    {
        ShipsCount -= count;
        ShipsCountChanged.Invoke(ShipsCount);
    }

    public void SetOwner(Owner owner)
    {
        this.Owner = owner;
        PlanetOwnerChanged.Invoke(this.Owner);
    }

    public void SetStartShipsCount(int count)
    {
        ShipsCount = count;
    }

    public void SetRadius(int radius)
    {
        transform.localScale = Vector3.one * radius * 2;
        Radius = transform.localScale.x / 2;
    }

    private void ProduceShips()
    {
        time += Time.deltaTime;
        if(time>=1f)
        {
            if (Owner != Owner.None)
            {
                AddShipCount(ShipProductionRate);
                ShipsCountChanged.Invoke(ShipsCount);
 
                time = 0;
            }
        }
    }
}
