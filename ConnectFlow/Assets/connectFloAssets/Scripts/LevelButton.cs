using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelButton : MonoBehaviour
{
    private Button button;
    private int value;
    public TMP_Text textUI;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void Initialize(int value)
    {
        this.value = value;
        textUI.text = value.ToString();
    }
    public void OnClick()
    {
        GameManager.instance.SetLevel(value);
    }
}
