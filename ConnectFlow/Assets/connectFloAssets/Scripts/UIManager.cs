using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject congratsText;
    public static UIManager instance;
    public GameObject LevelSelectionUI;
    private void Start()
    {
        GameManager.instance.onSuccessEvent.AddListener(ShowSuccessMessage);
        GameManager.instance.onLevelSelectionUIUpdateEvent.AddListener(LevelSelectionAction);
    }
    public void ShowSuccessMessage()
    {
        congratsText.SetActive(true);
    }
    private void LevelSelectionAction()
    {
        LevelSelectionUI.SetActive(false);
    }
}
