using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridBoard : MonoBehaviour
{

    #region fields
    [SerializeField]
    private int rows;
    [SerializeField]
    private int cols;
    [SerializeField]
    private Vector2 gridSize;
    [SerializeField]
    private Vector2 gridOffset;

    [SerializeField]
    private Sprite cellSprite;
    private Vector2 cellSize;
    private Vector2 cellScale;
    #endregion

    [SerializeField]
    private List<Color> colorList = new List<Color>(new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.grey });
    [SerializeField]
    private Transform gridPreFab;
    // Use this for initialization
    void Start()
    {
        initCells();
    }

    void initCells()
    {
        
        GameObject cellObject = new GameObject();

        cellObject.AddComponent<SpriteRenderer>().sprite = cellSprite;
        cellSize = cellSprite.bounds.size;

        Vector2 newCellSize = new Vector2(gridSize.x / (float)cols, gridSize.y / (float)rows);
        cellScale.x = newCellSize.x / cellSize.x;
        cellScale.y = newCellSize.y / cellSize.y;

        cellSize = newCellSize;

        cellObject.transform.localScale = new Vector2(cellScale.x, cellScale.y);

        gridOffset.x = -(gridSize.x / 2) + (cellSize.x / 2);
        gridOffset.y = -(gridSize.y / 2) + (cellSize.y / 2);

        Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/Temporary/" + cellObject.name + ".prefab");
        PrefabUtility.ReplacePrefab(cellObject, prefab, ReplacePrefabOptions.ConnectToPrefab);

        for (int row = 0; row < rows; row++)
            for (int col = 0; col < cols; col++)
            {
                Vector2 pos = new Vector2(col * cellSize.x + gridOffset.x + transform.position.x, row * cellSize.y + gridOffset.y + transform.position.y);
                RandomColor(ref cellObject);
                GameObject newGridTile = Instantiate(cellObject, pos, Quaternion.identity) as GameObject;
                
                newGridTile.transform.parent = transform;
            }

        Destroy(cellObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);
    }

    private void RandomColor(ref GameObject c0)
    {
        c0.GetComponent<SpriteRenderer>().color = colorList[Random.Range(0, colorList.Count)];
    }

    public void ReseedButton()
    {
        int _childCount = transform.childCount;

        for (int i = 0; i < _childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        initCells();
    }
}
