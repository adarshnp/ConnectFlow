using UnityEngine;

public class Node : MonoBehaviour
{
    private Color nodeColor;// Color of the node
    public Color NodeColor
    {
        get { return nodeColor; }
        set
        {
            nodeColor = value;
            UpdateColor(value);
        }
    }
    [HideInInspector]
    public int row; // Row index of the node in the grid
    [HideInInspector]
    public int col; // Column index of the node in the grid
    [HideInInspector]
    public int id; // id of the node in the grid
    public Flow flow;
    [HideInInspector]
    public Node parentNode;
    [HideInInspector]
    public Dots dot;
    public GameObject dotPrefab;
    private GridBoard gridManager; // Reference to the GridManager script
    [HideInInspector]
    public bool isMarked = false;
    public void Initialize(GridBoard manager, Color color, int r, int c, int i)
    {
        gridManager = manager;
        NodeColor = new Color(color.r, color.g, color.b, 0.025f);
        row = r;
        col = c;
        id = i;
        UpdateColor(nodeColor);
    }

    public void UpdateColor(Color color)
    {
        // Update the visual representation of the node based on its color
        GetComponent<SpriteRenderer>().color = color;
    }

    public void AddColorDot(Color col)
    {
        GameObject setDot = Instantiate(dotPrefab, this.GetComponent<Transform>());
        dot = setDot.GetComponent<Dots>();
        dot.color = col;
        dot.UpdateColor();
        dot.UpdatePath(id);
        isMarked = true;
    }
    public void AddPathnode(float angle)
    {
        isMarked = true;
        parentNode = gridManager.currentParentNode;
        parentNode.dot.UpdatePath(id);
        flow.FlowON();
        flow.Color = parentNode.dot.color;
        flow.Rotation(angle);
    }

    private void OnMouseDown()
    {
        if (isMarked)
        {
            //start new path
            GameManager.instance.isDrawingPath = true;
            gridManager.previousNode = this;

            if (dot != null)
            {
                //if  color node
                parentNode = this;
                gridManager.currentParentNode = this;

                //if this node or node of same color  have a path, remove its current path
                DeletePathNode(this);
            }
            else
            {
                //add starting node as parent node
                gridManager.currentParentNode = parentNode;

            }
        }
        else
        {
            //no action
        }
    }
    private void OnMouseUp()
    {
        //on color dot
        if (GameManager.instance.isDrawingPath)
        {
            GameManager.instance.isDrawingPath = false;
        }
    }
    private void OnMouseEnter()
    {
        if (GameManager.instance.isDrawingPath)
        {
            float rotation = GetDirection();
            if (rotation == -1) return;
            if (isMarked)
            {
                if (dot == null)
                {
                    if (parentNode != gridManager.currentParentNode)
                    {
                        DeletePathNode(parentNode);
                        parentNode = gridManager.currentParentNode;
                        AddPathnode(rotation);
                    }

                }
                else
                {
                    if (this != gridManager.currentParentNode && this.dot.color == gridManager.currentParentNode.dot.color)
                    {
                        AddPathnode(rotation);
                        //code for changing node color
                        GameManager.instance.isDrawingPath = false;
                        parentNode.dot.isComplete = true;
                        gridManager.currentColorCounter++;
                        if(gridManager.CheckWin())
                        {
                            GameManager.instance.onSuccessEvent.Invoke();
                        }
                    }
                }
            }
            else
            {
                AddPathnode(rotation);
            }
            gridManager.previousNode = this;
        }
    }
    private void DeletePathNode(Node parent)
    {
        //if current parent have no path..check the other node of same color
        Node selectedParent;
        if (parent.dot != null)
        {
            if (parent.dot.path.Count == 1)
            {
                selectedParent = gridManager.GetNodeofSameColor(parent.id);
            }
            else
            {
                selectedParent = parent;
            }
        }
        else { return; }
        int currentID;
        for (int i = selectedParent.dot.path.Count - 1; i >= 0; i--)
        {
            currentID = selectedParent.dot.path[i];
            Node node = gridManager.GetNode(currentID);
            if (node.dot == null)
            {
                node.isMarked = false;
            }
            node.flow.FlowOFF();
        }
        if (selectedParent.dot.isComplete) { gridManager.currentColorCounter--; }
    }
    private float GetDirection()
    {
        Node prevNode = gridManager.previousNode;
        int dir = 0;
        float rotation = -1; //default rotation is 0 => towards up
        if (prevNode.row == row)
        {
            dir = prevNode.col - col;   //if negative right if positive left
            if (dir == 1)
                rotation = -90;
            else if (dir == -1)
                rotation = 90;
        }

        else if (prevNode.col == col)
        {
            dir = prevNode.row - row;   //if negative downward if positive upward
            if (dir == 1)
                rotation = 180;
            else if (dir == -1)
                rotation = 0;
        }
        return rotation;
    }

}
