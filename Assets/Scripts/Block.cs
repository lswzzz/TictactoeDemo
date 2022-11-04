using System.Collections;
using DG.Tweening;
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
    private float animationTime = 0.5f;
    
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
        ShowAnimation();
    }

    void ShowAnimation()
    {
        rectTransform.localScale = Vector3.zero;
        rectTransform.DOLocalRotate(new Vector3(0, 0, 360), 0.5f).SetRelative(true);
        rectTransform.DOScale(new Vector3(1, 1, 1), 0.5f);
    }
    
    public void Click()
    {
        blocksController.Place(row, col);
    }

    public void PlayerPlace()
    {
        O.gameObject.SetActive(true);
        rectTransform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        rectTransform.DOScale(new Vector3(1, 1, 1), 0.5f);
    }
    
    public void RobotPlace()
    {
        X.gameObject.SetActive(true);
        rectTransform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        rectTransform.DOScale(new Vector3(1, 1, 1), 0.5f);
    }

    public void Rollback()
    {
        O.gameObject.SetActive(false);
        X.gameObject.SetActive(false);
        rectTransform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        rectTransform.DOScale(new Vector3(1, 1, 1), 0.5f);
    }
}