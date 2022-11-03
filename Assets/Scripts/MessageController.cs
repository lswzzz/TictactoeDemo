using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MessageController : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public AudioClip winSound;
    public AudioClip loseSound;
    private AudioSource source;
    public TextMeshProUGUI msgText;
    private WaitForSeconds wait = new WaitForSeconds(1f);
    private WaitForSeconds wait2 = new WaitForSeconds(3f);
    private Canvas canvas;
    
    void Awake()
    {
        source = EventSystem.current.GetComponent<AudioSource>();
        canvas = GetComponent<Canvas>();
    }

    public void ShowMessage(string message)
    {
        msgText.text = message;
        resultText.text = "";
        StartCoroutine(DelayHide1());
    }

    IEnumerator DelayHide1()
    {
        canvas.enabled = true;
        yield return wait;
        canvas.enabled = false;
    }

    IEnumerator DelayHide2()
    {
        canvas.enabled = true;
        yield return wait2;
        canvas.enabled = false;
    }
    
    public void ShowResult(int result)
    {
        msgText.text = "";
        switch (result)
        {
            case 0:
                resultText.text = "DRAW";
                source.PlayOneShot(loseSound);
                break;
            case 1:
                resultText.text = "YOU WIN";
                source.PlayOneShot(winSound);
                break;
            case 2:
                resultText.text = "YOU LOSE";
                source.PlayOneShot(loseSound);
                break;
        }
        StartCoroutine(DelayHide2());
    }
}