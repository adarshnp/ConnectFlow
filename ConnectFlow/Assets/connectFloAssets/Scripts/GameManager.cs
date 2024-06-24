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
#if UNITY_EDITOR
        string jsonPath = Application.streamingAssetsPath + "/PathData.json";
#else
        string jsonPath = "StreamingAssets/Pathdata.json";
#endif
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
        if (ColorUtility.TryParseHtmlString(color, out Color convertedColor))
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
