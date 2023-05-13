using UnityEngine;
using TMPro;

public class PlanetVisual : MonoBehaviour
{
    [SerializeField] private Planet planet;
    [SerializeField] private TextMeshProUGUI shipsCount;
    [SerializeField] private GameObject outline;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        planet.ShipsCountChanged += OnShipsCountChanged;
        planet.PlanetSelected += OnPlanetSelected;
        planet.PlanetUnSelected += OnPlanetUnSelected;
        planet.PlanetOwnerChanged += OnOwnerChanged;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnShipsCountChanged(int count)
    {
        shipsCount.SetText(count.ToString());
    }

    private void OnPlanetUnSelected()
    {
        outline.SetActive(false);
    }

    private void OnPlanetSelected()
    {
        outline.SetActive(true);
    }

    private void OnOwnerChanged(Owner owner)
    {
        switch (owner)
        {
            case Owner.Player:
                spriteRenderer.color = Color.green;
                break;
            case Owner.AI:
                spriteRenderer.color = Color.red;
                break;
            default:
                spriteRenderer.color = Color.white;
                break;
        }
    }

    private void OnDestroy()
    {
        planet.ShipsCountChanged -= OnShipsCountChanged;
        planet.PlanetSelected -= OnPlanetSelected;
        planet.PlanetUnSelected -= OnPlanetUnSelected;
        planet.PlanetOwnerChanged -= OnOwnerChanged;
    }
}
