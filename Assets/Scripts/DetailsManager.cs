using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DetailsManager : MonoBehaviour
{
    #region Singleton
    public static DetailsManager Instance;

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

    [SerializeField] GameObject icon;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI detailsText;
    [SerializeField] GameObject deleteButton;
    [SerializeField] GameObject produceButton;
    [SerializeField] GameObject showingObject;
    [SerializeField] Color iconVoidColor;

    private void Start()
    {
        ResetDetails();
    }

    public void SetDetails(GameObject ghost)
    {
        icon.GetComponent<RawImage>().color = ghost.GetComponent<Clickable>().iconColor;
        nameText.text = ghost.GetComponent<Clickable>().nameInfo;
        detailsText.text = ghost.GetComponent<Clickable>().detailsInfo;
    }
    public void SetBuildingDetails(GameObject clickedBuilding)
    {
        SetDetails(clickedBuilding);
        showingObject = clickedBuilding;
        SetActiveDeleteButton(true);
        if (showingObject.GetComponent<Clickable>().canProduceUnit) SetActiveProduceButton(true);
        else SetActiveProduceButton(false);
        SelectionManager.Instance.DeSelectUnit();
    }
    public void SetUnitDetails(GameObject clickedUnit)
    {
        ResetDetails();
        SetDetails(clickedUnit);
        showingObject = clickedUnit;
        SetActiveDeleteButton(true);
    }

    public void ResetDetails()
    {
        icon.GetComponent<RawImage>().color = iconVoidColor;
        nameText.text = "";
        detailsText.text = "";
        showingObject = null;
        SetActiveDeleteButton(false);
        SetActiveProduceButton(false);
    }

    public void DeleteObject()
    {
        foreach (Transform tileCoords in showingObject.transform)
        {
            MapCreator.Instance.playgroundList.Add(Vector3Int.RoundToInt(tileCoords.position));
        }

        Destroy(showingObject);
        ResetDetails();
        SelectionManager.Instance.DeSelectUnit();
    }

    private void SetActiveDeleteButton(bool active)
    {
        deleteButton.SetActive(active);
    }
    private void SetActiveProduceButton(bool active)
    {
        produceButton.SetActive(active);
    }

    public void ProduceUnitButton()
    {
        showingObject.GetComponent<Clickable>().SpawnUnit();
    }
}
