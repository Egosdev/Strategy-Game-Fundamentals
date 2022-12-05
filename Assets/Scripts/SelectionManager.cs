using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class SelectionManager : MonoBehaviour
{
    #region Singleton
    public static SelectionManager Instance;

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
    }
    #endregion

    public GameObject buildingGhost;
    [SerializeField] Color cannotPlaceableColor;
    [SerializeField] Color canPlaceableColor;
    public static bool canPlaceable;
    private int nonPlaceableTileCount;
    public GameObject[] buildingPrefabs;
    public GameObject[] unitPrefabs; //soldier, tank, plane
    public int soldierNumber;
    public int tankNumber;
    public int planeNumber;
    public GameObject selectedUnit;
    public GameObject selection;

    public void SelectUnit(GameObject selected)
    {
        selectedUnit = selected;
        selection.transform.position = selectedUnit.transform.position;
        selection.SetActive(true);
    }

    public void MoveUnit(Vector3Int moveCoords)
    {
        MapCreator.Instance.playgroundList.Remove(moveCoords);
        MapCreator.Instance.playgroundList.Add(Vector3Int.RoundToInt(selectedUnit.transform.position));
        selectedUnit.transform.position = moveCoords;
        ClearSelection();
    }

    public void DeSelectUnit()
    {
        selectedUnit = null;
        selection.SetActive(false);
    }

    public void SelectBuilding(int buildingIndex)
    {
        ClearSelection();
        buildingGhost = buildingPrefabs[buildingIndex];
        buildingGhost.gameObject.SetActive(true);
        DetailsManager.Instance.SetDetails(buildingGhost);
    }
    private void ClearSelection()
    {
        if (buildingGhost != null)
        {
            buildingGhost.gameObject.SetActive(false);
            buildingGhost = null;
        }
        DetailsManager.Instance.ResetDetails();
        DeSelectUnit();
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (buildingGhost != null)
        {
            buildingGhost.transform.position = Vector3Int.RoundToInt(mousePosition + new Vector3(0, 0, 10));
            nonPlaceableTileCount = 0;
            foreach (Transform tile in buildingGhost.transform)
            {
                if (MapCreator.Instance.playgroundList.Contains(Vector3Int.RoundToInt(tile.transform.position)))
                {
                    tile.GetComponent<SpriteRenderer>().color = canPlaceableColor;
                }
                else
                {
                    tile.GetComponent<SpriteRenderer>().color = cannotPlaceableColor;
                    nonPlaceableTileCount++;
                }
            }
            if (nonPlaceableTileCount == 0) canPlaceable = true;
            else if (nonPlaceableTileCount != 0) canPlaceable = false;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                buildingGhost.transform.Rotate(0, 0, 90);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                buildingGhost.transform.Rotate(0, 0, -90);
            }
            if (Input.GetMouseButtonDown(0) && canPlaceable)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                GameObject newProduct = Instantiate(buildingGhost, buildingGhost.transform.position, buildingGhost.transform.rotation);
                newProduct.GetComponent<Clickable>().type++;
                foreach (Transform tile in newProduct.transform)
                {
                    tile.gameObject.GetComponent<SpriteRenderer>().color = buildingGhost.GetComponent<Clickable>().iconColor;
                    tile.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    MapCreator.Instance.playgroundList.Remove(Vector3Int.RoundToInt(tile.transform.position));
                }
                ClearSelection();
            }
        }

        if (selectedUnit != null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                Vector3Int clickedPos = Vector3Int.RoundToInt(mousePosition + new Vector3(0, 0, 10));
                if(MapCreator.Instance.playgroundList.Contains(clickedPos))
                {
                    MoveUnit(clickedPos);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearSelection();
        }
    }
}
