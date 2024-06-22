using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public bool isDrawingPath;
    [HideInInspector]
    public BoardData boardData;
    [HideInInspector]
    public LevelData levelData;
    [HideInInspector]
    public int levelIndex;
    [HideInInspector]
    public UnityEvent onSuccessEvent = new UnityEvent();
    [HideInInspector]
    public UnityEvent onLevelSelectionEvent = new UnityEvent();
    [HideInInspector]
    public UnityEvent onLevelSelectionUIUpdateEvent = new UnityEvent();

    private void Awake()
    {
        instance = this;
        FetchBoardData();
    }

    private void FetchBoardData()
    {
        //string jsonPath = Application.streamingAssetsPath + "/PathData.json";
        string jsonPath = Application.streamingAssetsPath + "/PathData.json";
        string jsonStr = File.ReadAllText(jsonPath);
        levelData = JsonUtility.FromJson<LevelData>(jsonStr);
    }
    public void SetLevel(int index)
    {
        levelIndex = index;
        boardData = levelData.levelDataList[levelIndex];
        onLevelSelectionEvent.Invoke();
        onLevelSelectionUIUpdateEvent.Invoke();
    }
    public Color GetColor(string color)
    {
        Color convertedColor;
        if (ColorUtility.TryParseHtmlString(color, out convertedColor))
        {
            return convertedColor;
        }
        else
        {
            Debug.LogError("Failed to convert color: " + color);
            return Color.white; // Default color if conversion fails
        }
    }

}
