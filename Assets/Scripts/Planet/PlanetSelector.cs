using System.Collections.Generic;
using UnityEngine;
using System;

public class PlanetSelector : MonoBehaviour
{
    public List<Planet> SelectedPlanets { get; private set; } = new List<Planet>();
    public Planet TargetPlanet { get;  private set; }
    public event Action TargetSelected;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask planetLayerMask;

    private Color selectionBoxColor = new Color(0, 1, 0, 0.3f);
    private Vector2 selectionBoxStart;
    private Vector2 selectionBoxEnd;
    private bool isSelecting = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (SelectedPlanets.Count > 0)
            {
                SelectTargetPlanet();
                ClearSelectedPlanet();
                TargetPlanet = null;
            }
            else
            {
                isSelecting = true;
                selectionBoxStart = Input.mousePosition;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (isSelecting)
            {
                selectionBoxEnd = Input.mousePosition;
                SelectPlanetsInBox();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
        }
    }

    private void OnGUI()
    {
        if (isSelecting)
        {
            Rect selectionBox = new Rect(selectionBoxStart.x, Screen.height - selectionBoxStart.y, selectionBoxEnd.x - selectionBoxStart.x, -1 * (selectionBoxEnd.y - selectionBoxStart.y));
            GUI.color = selectionBoxColor;
            GUI.DrawTexture(selectionBox, Texture2D.whiteTexture);
            GUI.color = Color.white;
        }
    }

    private void SelectPlanetsInBox()
    {
        ClearSelectedPlanet();
        Collider2D[] planetColliders = Physics2D.OverlapAreaAll(mainCamera.ScreenToWorldPoint(selectionBoxStart), mainCamera.ScreenToWorldPoint(selectionBoxEnd), planetLayerMask);

        foreach (Collider2D planetCollider in planetColliders)
        {
            GameObject planetGO = planetCollider.gameObject;
            var planet = planetCollider.GetComponent<Planet>();
            if(planet.Owner==Owner.Player)
            {
                SelectedPlanets.Add(planet);
                planet.PlanetSelected.Invoke();
            }
        }
    }

    private void SelectTargetPlanet()
    {
        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPosition, planetLayerMask);

        if (hitCollider != null)
        {
            var planet = hitCollider.GetComponent<Planet>();

            if (SelectedPlanets.Contains(planet))
            {
                SelectedPlanets.Remove(planet);
                planet.PlanetUnSelected.Invoke();
            }

            TargetPlanet = planet;
            TargetSelected.Invoke();
        }
    }

    public void ClearSelectedPlanet()
    {
        if(SelectedPlanets.Count>0)
        {
            foreach (var planet in SelectedPlanets)
            {
                planet.PlanetUnSelected.Invoke();
            }
            SelectedPlanets.Clear();
        }     
    }
}
