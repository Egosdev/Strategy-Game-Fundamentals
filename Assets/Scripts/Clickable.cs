using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour
{
    public Color iconColor;
    public string nameInfo;
    public string detailsInfo;
    public int type; //0 ghost, 1 building, 2 unit
    public bool canProduceUnit;
    [SerializeField] List<Vector3Int> producibleTiles;

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (type == 1 && SelectionManager.Instance.buildingGhost == null)
        {
            DetailsManager.Instance.SetBuildingDetails(gameObject);
        }
        if (type == 2 && SelectionManager.Instance.buildingGhost == null)
        {
            DetailsManager.Instance.SetUnitDetails(gameObject);
            SelectionManager.Instance.SelectUnit(gameObject);
        }
    }

    public void SpawnUnit()
    {
        CalculateAdjacentTiles();
        if (producibleTiles.Count > 0)
        {
            if(nameInfo == "Barracks")
            {
                Vector3Int suitablePosition = producibleTiles[Random.Range(0, producibleTiles.Count)];
                GameObject newUnit = Instantiate(SelectionManager.Instance.unitPrefabs[0], suitablePosition, Quaternion.identity);
                SelectionManager.Instance.soldierNumber++;
                newUnit.transform.GetChild(0).GetComponent<TextMeshPro>().text = "S" + SelectionManager.Instance.soldierNumber.ToString();
                newUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 5;
                MapCreator.Instance.playgroundList.Remove(suitablePosition);
            }
            if (nameInfo == "Factory")
            {
                Vector3Int suitablePosition = producibleTiles[Random.Range(0, producibleTiles.Count)];
                GameObject newUnit = Instantiate(SelectionManager.Instance.unitPrefabs[1], suitablePosition, Quaternion.identity);
                SelectionManager.Instance.tankNumber++;
                newUnit.transform.GetChild(0).GetComponent<TextMeshPro>().text = "T" + SelectionManager.Instance.tankNumber.ToString();
                newUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 5;
                MapCreator.Instance.playgroundList.Remove(suitablePosition);
            }
            if (nameInfo == "Airport")
            {
                Vector3Int suitablePosition = producibleTiles[Random.Range(0, producibleTiles.Count)];
                GameObject newUnit = Instantiate(SelectionManager.Instance.unitPrefabs[2], suitablePosition, Quaternion.identity);
                SelectionManager.Instance.planeNumber++;
                newUnit.transform.GetChild(0).GetComponent<TextMeshPro>().text = "P" + SelectionManager.Instance.planeNumber.ToString();
                newUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 5;
                MapCreator.Instance.playgroundList.Remove(suitablePosition);
            }
        }
    }
    private void CalculateAdjacentTiles()
    {
        producibleTiles.Clear();
        foreach (Transform tile in transform)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector3Int tilePos = Vector3Int.RoundToInt(tile.transform.position + new Vector3(i, j, 0));
                    if(MapCreator.Instance.playgroundList.Contains(tilePos) && !producibleTiles.Contains(tilePos))
                    {
                        producibleTiles.Add(tilePos);
                    }
                }
            }
        }
    }
}
