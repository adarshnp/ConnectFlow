using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dots : MonoBehaviour
{
    [HideInInspector]
    public Color color;
    [HideInInspector]
    public bool isComplete=false;
    [HideInInspector]
    public List<int> path = new List<int>();
    public void UpdateColor()
    {
        // Update the visual representation of the node based on its color
        GetComponent<SpriteRenderer>().color = color;
    }
    public void UpdatePath(int id)
    {
        path.Add(id);
    }

}
