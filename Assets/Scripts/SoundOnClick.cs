using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SoundOnClick : MonoBehaviour
{
    public AudioClip clip;
    private AudioSource source;

    void Start()
    {
        source = EventSystem.current.GetComponent<AudioSource>();
        var button = GetComponent<Button>();
        button.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        source.PlayOneShot(clip);
    }
}