using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public Image O;
    public Image X;
    public BlocksController blocksController;
    public int row;
    public int col;
    private RectTransform rectTransform;
    private 
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    public void Init(int row, int col, Vector2 pos, Vector2 size, BlocksController controller)
    {
        blocksController = controller;
        rectTransform.SetParent(controller.panel);
        rectTransform.localScale = Vector3.one;
        rectTransform.sizeDelta = size;
        rectTransform.localPosition = Vector3.zero;
        rectTransform.anchoredPosition = pos;
        this.row = row;
        this.col = col;
        O.gameObject.SetActive(false);
        X.gameObject.SetActive(false);
    }
    
    public void Click()
    {
        blocksController.Place(row, col);
    }

    public void PlayerPlace()
    {
        O.gameObject.SetActive(true);
    }
    
    public void RobotPlace()
    {
        X.gameObject.SetActive(true);
    }

    public void Rollback()
    {
        O.gameObject.SetActive(false);
        X.gameObject.SetActive(false);
    }
}