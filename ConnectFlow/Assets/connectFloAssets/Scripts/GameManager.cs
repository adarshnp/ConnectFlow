using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

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

    public UIManager uiManager;

    private void Awake()
    {
        instance = this;
#if UNITY_EDITOR
        FetchBoardDataFromLocal();
#else
        StartCoroutine(FetchBoardData());
#endif
    }
    private IEnumerator FetchBoardData()
    {
        string url = "https://raw.githubusercontent.com/adarshnp/ConnectFlow/refs/heads/main/ConnectFlow/Assets/StreamingAssets/Pathdata.json"; 

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            // Handle errors
            Debug.LogError(request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            levelData = JsonUtility.FromJson<LevelData>(json);
        }

    }
    private void FetchBoardDataFromLocal()
    {
        string jsonPath = Application.streamingAssetsPath + "/PathData.json";
        string jsonStr = File.ReadAllText(jsonPath);
        levelData = JsonUtility.FromJson<LevelData>(jsonStr);
        uiManager.GenerateLevelMenu(levelData.levelDataList.Count);
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
