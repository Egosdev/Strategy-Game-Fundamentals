using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
public class MapCreator : MonoBehaviour
{
    #region Singleton
    public static MapCreator Instance;

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

    [SerializeField] GameObject tile;
    [SerializeField] Color tileOne;
    [SerializeField] Color tileTwo;
    [SerializeField] Vector2 matrix;
    public List<Vector3Int> playgroundList;
    [SerializeField] GameObject generateInputPanel;
    [SerializeField] GameObject inGamePanel;
    [SerializeField] TextMeshProUGUI errorText;
    [SerializeField] TMP_InputField rowInputField;
    [SerializeField] TMP_InputField columnInputField;

    private void Start() //don't forget to delete
    {
        //GenerateMap();
    }

    public void TryCreateMap()
    {

        string rowInput = rowInputField.text.ToString();
        string columunInput = columnInputField.text.ToString();

        if (int.TryParse(rowInput, out int row) && int.TryParse(columunInput, out int column))
        {
            if (row >= 5 && column >= 5)
            {
                matrix = new Vector2(row, column);

                GenerateMap();
                generateInputPanel.SetActive(false);
                inGamePanel.SetActive(true);
            }
            else
            {
                errorText.text = "Row and column values must be bigger than or equal to 5";
            }
        }
        else
        {
            errorText.text = "Please enter an integer for both row and column.";
        }
    }

    private void GenerateMap()
    {
        for (int i = 0; i < matrix.y; i++)
        {
            for (int j = 0; j < matrix.x; j++)
            {
                GameObject newTile = Instantiate(tile, new Vector3Int(i, j, 0), Quaternion.identity);
                newTile.transform.SetParent(transform);
                playgroundList.Add(Vector3Int.FloorToInt(newTile.transform.position));
                if ((i + j) % 2 == 0)
                {
                    newTile.GetComponent<SpriteRenderer>().color = tileOne;
                }
                else
                {
                    newTile.GetComponent<SpriteRenderer>().color = tileTwo;
                }
            }
        }
        Camera.main.transform.position = new Vector3(matrix.y / 2 - 0.5f, matrix.x / 2 - 0.5f, Camera.main.transform.position.z);
        CameraMove.cameraCenteredCoords = new Vector2(matrix.y / 2 - 0.5f, matrix.x / 2 - 0.5f);

        if(matrix.y - 10 > 0)
        {
            CameraMove.panLimit.x = ((matrix.y - 10) / 2) + 0.5f;
        }
        if (matrix.x - 10 > 0)
        {
            CameraMove.panLimit.y = ((matrix.x - 10) / 2) + 0.5f;
        }
    }
}
