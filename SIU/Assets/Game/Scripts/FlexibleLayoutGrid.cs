using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class FlexibleLayoutGrid : MonoBehaviour
{
    public int rows;
    public int columns;
    public Vector2 spacing;
    public bool fitX;
    public bool fitY;

    private GridLayoutGroup gridLayout;

    void Start()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        AdjustCells();
    }

    void Update()
    {
        AdjustCells();
    }

    void AdjustCells()
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float parentHeight = GetComponent<RectTransform>().rect.height;

        if (fitX && columns > 0)
        {
            float cellWidth = (parentWidth - ((columns - 1) * spacing.x)) / columns;
            gridLayout.cellSize = new Vector2(cellWidth, gridLayout.cellSize.y);
        }
        
        if (fitY && rows > 0)
        {
            float cellHeight = (parentHeight - ((rows - 1) * spacing.y)) / rows;
            gridLayout.cellSize = new Vector2(gridLayout.cellSize.x, cellHeight);
        }

        gridLayout.spacing = spacing;
    }
}
