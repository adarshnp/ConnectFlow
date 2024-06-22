using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public List<BoardData> levelDataList;
}
[System.Serializable]
public class BoardData 
{
    public List<DotColor> boardDataList;
}
[System.Serializable]
public class DotColor
{
    public string color;
    public int dot1ID;
    public int dot2ID;
}
