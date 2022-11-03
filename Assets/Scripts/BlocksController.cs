using System.Collections;
using System.Drawing;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BlocksController : MonoBehaviour, EventListener<CustomEvent>
{
    public Block Prefab;
    public RectTransform panel;
    public int Gap = 20;
    private bool placeEnabled = true;
    private WaitForSeconds wait = new WaitForSeconds(0.5f);
    private WaitForSeconds wait2 = new WaitForSeconds(3f);
    private Block[] Blocks;
    public MessageController messageController;

    void OnEnable()
    {
        this.EventStartListening<CustomEvent>();
    }

    void OnDisable()
    {
        this.EventStopListening<CustomEvent>();
    }

    void Awake()
    {
        Blocks = new Block[Global.MaxSize * Global.MaxSize];
    }
    
    public void Init()
    {
        Tictactoe.Instance.Reset(Global.Size);
        placeEnabled = true;
        int n = Global.Size;
        float width = panel.rect.width;
        float height = panel.rect.height;
        float blockWidth = (width - (n + 1) * Gap) / n;
        float blockHeight = (height - (n + 1) * Gap) / n;
        var size = new Vector2(blockWidth, blockHeight);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                var item = Pools<GameObject>.Instance.Get();
                if (item == null)
                {
                    item = GameObject.Instantiate(Prefab).gameObject;
                }
                var x = blockWidth * j + blockWidth / 2 + Gap * (j + 1) - width/2;
                var y = blockHeight * i + blockHeight / 2 + Gap * (i + 1) - height/2;
                var pos = new Vector2(x, y);
                item.SetActive(true);
                var block = item.GetComponent<Block>();
                block.Init(i, j, pos, size, this);
                Blocks[i*n+j] = block;
            }
        }

    }

    public void Reset()
    {
        int count = panel.childCount;
        for (int i = 0; i < count; i++)
        {
            var o = (RectTransform) panel.GetChild(0);
            o.SetParent(null);
            o.gameObject.SetActive(false);
            Pools<GameObject>.Instance.Add(o.gameObject);
        }
        Init();
    }

    public void RollBack()
    {
        if (!Tictactoe.Instance.CanRollBack()) return;
        
        var last = Tictactoe.Instance.Last();
        Tictactoe.Instance.RollBack();
        Blocks[last.y * Global.Size + last.x].Rollback();
        
        last = Tictactoe.Instance.Last();
        Tictactoe.Instance.RollBack();
        Blocks[last.y * Global.Size + last.x].Rollback();
        messageController.ShowMessage("RollBack");
    }

    public void OnEvent(CustomEvent evt)
    {
        switch(evt.type)
        {
            case EvtType.Init:
                Init();
                break;
            case EvtType.Reset:
                Reset();
                messageController.ShowMessage("Reset");
                break;
            case EvtType.RollBack:
                RollBack();
                break;
        }
    }

    public void Place(int row, int col)
    {
        StartCoroutine(PlayerPlace(row, col));
    }
    
    IEnumerator PlayerPlace(int row, int col)
    {
        if (placeEnabled && Tictactoe.Instance.CanPlace(row, col))
        {
            var result = Tictactoe.Instance.PlayerRound(row, col);
            Blocks[row*Global.Size+col].PlayerPlace();
            if (result!=0 || Tictactoe.Instance.IsFull())
            {
                messageController.ShowResult(result);
                yield return wait2;
                Reset();
            }
            else
            {
                StartCoroutine(RobotPlace());
            }
        }
    }
    
    IEnumerator RobotPlace()
    {
        placeEnabled = false;
        yield return wait;
        var result = Tictactoe.Instance.RobotRound();
        var last = Tictactoe.Instance.Last();
        Blocks[last.y * Global.Size + last.x].RobotPlace();
        if (result!=0 || Tictactoe.Instance.IsFull())
        {
            messageController.ShowResult(result);
            yield return wait2;
            Reset();
        }
        else
        {
            yield return wait;
            placeEnabled = true;
        }
    }
}