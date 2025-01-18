using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject congratsText;
    public GameObject LevelSelectionUI;
    public Transform levelMenuParent;
    public GameObject levelButtonPrefab;
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
    public void GenerateLevelMenu(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject buttonObj = Instantiate(levelButtonPrefab, levelMenuParent);
            LevelButton button = buttonObj.GetComponent<LevelButton>();
            button.Initialize(i);
        }
    }
}
