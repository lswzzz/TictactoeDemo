using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IndexUIController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int size = 3;
        
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
    
    public void StartGame()
    {
        Global.Size = size;
        StartCoroutine(DelayLoad());
    }

    WaitForSeconds wait = new WaitForSeconds(0.5f);
    IEnumerator DelayLoad()
    {
        yield return wait;
        SceneManager.LoadScene("GameScene");
    }
}