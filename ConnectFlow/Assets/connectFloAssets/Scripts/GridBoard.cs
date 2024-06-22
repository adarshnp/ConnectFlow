using UnityEngine;
public class GridBoard : MonoBehaviour
{
    public int gridSize = 5; // Size of the grid (5x5 in this case)
    public float nodeSize = 1f; // Size of each grid node
    public float nodeScale = 7.5f; // Scale of each grid node
    public Transform gridOrigin; // Position of the grid's origin point
    public GameObject gridLinePrefab; // Prefab for the grid line or square
                                      // public GameObject dotPrefab; // Prefab for the grid line or square
    public Transform gridParent; // Prefab for the grid line or square

    [HideInInspector]
    public Node[,] grid; // 2D array to store the nodes
    [HideInInspector]
    public Node currentParentNode;
    [HideInInspector]
    public Node previousNode;
    [HideInInspector]
    public int colorCount = 0;
    [HideInInspector]
    public int currentColorCounter = 0;
    private void Start()
    {
        CreateGrid();
        GameManager.instance.onLevelSelectionEvent.AddListener(StartGridInitialization);
    }
    private void StartGridInitialization()
    {
        PopulateColorDots(GameManager.instance.boardData);
    }
    private void CreateGrid()
    {
        grid = new Node[gridSize, gridSize]; // Initialize the grid array
        int id = 0;
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                Vector3 nodePosition = gridOrigin.position + new Vector3(col * nodeSize, -row * nodeSize, 0f);
                GameObject gridLine = Instantiate(gridLinePrefab, nodePosition, Quaternion.identity, gridParent);
                gridLine.transform.localScale = new Vector3(nodeScale, nodeScale, 1f);

                Node node = gridLine.GetComponent<Node>();
                node.Initialize(this, Color.white, row, col, id); // Pass the GridManager reference and initial color
                grid[row, col] = node;
                id++;
            }
        }
    }
    private void PopulateColorDots(BoardData boardData)
    {
        foreach (Node node in grid)
        {
            node.parentNode = null;
            if (node.dot != null)
            {
                Destroy(node.dot.gameObject);
                node.dot = null;
            }
            node.isMarked = false;
            node.flow.FlowOFF();
        }
        currentParentNode = null;
        previousNode = null;
        colorCount = 0;
        currentColorCounter = 0;
        foreach (DotColor dot in boardData.boardDataList)
        {
            Node node1 = GetNode(dot.dot1ID);
            Node node2 = GetNode(dot.dot2ID);
            if(node1.dot==null)
                node1.AddColorDot(GameManager.instance.GetColor(dot.color));
            if (node2.dot == null)
                node2.AddColorDot(GameManager.instance.GetColor(dot.color));
            colorCount++;
        }

    }
    public Node GetNode(int id)
    {
        Node node = null;
        foreach (Node n in grid)
        {
            if (id == n.id)
            {
                node = n;
            }
        }
        return node;
    }
    public Node GetNode(int row, int col)
    {
        if (row >= 0 && row < gridSize && col >= 0 && col < gridSize)
        {
            return grid[row, col];
        }
        else
        {
            Debug.LogWarning("Invalid grid position!");
            return null;
        }
    }
    public bool CheckWin()
    {
        if (currentColorCounter == colorCount)
        {
            return true;
        }
        return false;
    }
    public Node GetNodeofSameColor(int id)
    {
        foreach (DotColor dot in GameManager.instance.boardData.boardDataList)
        {
            if (dot.dot1ID == id)
            {
                return GetNode(dot.dot2ID);
            }
            if (dot.dot2ID == id)
            {
                return GetNode(dot.dot1ID);
            }
        }
        return new Node();
    }
}
