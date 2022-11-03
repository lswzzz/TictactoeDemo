using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingController : MonoBehaviour
{
    public TextMeshProUGUI text;
    
    public int size = 3;

    public void Start()
    {
        size = Global.Size;
        text.text = size.ToString();
    }
    
    public void SizeSub()
    {
        size = Math.Max(size - 1, Global.MinSize);
        text.text = size.ToString();
    }

    public void SizeAdd()
    {
        size = Math.Min(size + 1, Global.MaxSize);
        text.text = size.ToString();
    }
        
    public void ReStartGame()
    {
        Global.Size = size;
        EventManager.TriggerEvent(new CustomEvent(EvtType.Reset));
        GetComponent<Canvas>().enabled = false;
    }

    public void Exit()
    {
        GetComponent<Canvas>().enabled = false;
    }
}