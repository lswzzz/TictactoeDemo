using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public Canvas popupPanel;

    public void Start()
    {
        EventManager.TriggerEvent(new CustomEvent(EvtType.Init));
    }
    
    public void Reset()
    {
        EventManager.TriggerEvent(new CustomEvent(EvtType.Reset));
    }

    public void BackStep()
    {
        EventManager.TriggerEvent(new CustomEvent(EvtType.RollBack));
    }

    public void Setting()
    {
        popupPanel.enabled = true;
    }
}