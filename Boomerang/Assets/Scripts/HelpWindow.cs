using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpWindow : MonoBehaviour
{
    /// <summary>
    /// 表示状態
    /// </summary>
    public enum State
    {
        Invalid,
        FadeIn,
        Process,
        FadeOut,
    };
    /// <summary>
    /// 現在の状態
    /// </summary>
    private State state;
    /// <summary>
    /// ページ数
    /// </summary>
    static private int MaxPage = 5;
    /// <summary>
    /// フェードインにかかる時間
    /// </summary>
    private const int FadeInTime = (int)(100.0f / func.FRAMETIME);
    /// <summary>
    /// フェードアウトにかかる時間
    /// </summary>
    private const int FadeOutTime = (int)(250.0f / func.FRAMETIME);
    private const float TriangleX = 0.8f;
    private const float TriangleY = -2.6f;
    private const float TriangleSize = 0.5f;
    /// <summary>
    /// 処理時間
    /// </summary>
    private int time;
    /// <summary>
    /// 現在のぺージ
    /// </summary>
    private int page;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    private Sprite[] helpTxList = new Sprite[MaxPage];
    private Sprite[] helpExpList = new Sprite[MaxPage];
    private Sprite[] helpImgList = new Sprite[MaxPage];
    private Sprite[] helpImg2List = new Sprite[MaxPage];

    public void SetState(State state)
    {
        time = 0;
        this.state = state;
        if(state == State.FadeOut)
        {
            SetPage(0);
            SetInvisible();
        }
        else if(state == State.Process)
        {
            SetPage(page);
        }
    }
    public void SetInvisible()
    {

        GameObject.Find("HelpTx").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        GameObject.Find("HelpExp").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        GameObject.Find("HelpImg").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        GameObject.Find("HelpImg2").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        GameObject.Find("PageNum").GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 0);
        GameObject.Find("NextPage").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        GameObject.Find("PreviousPage").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }
    public void SetPage(int page)
    {
        this.page = page;
        GameObject.Find("HelpTx").GetComponent<SpriteRenderer>().sprite = helpTxList[page];
        GameObject.Find("HelpExp").GetComponent<SpriteRenderer>().sprite = helpExpList[page];
        GameObject.Find("HelpImg").GetComponent<SpriteRenderer>().sprite = helpImgList[page];
        GameObject.Find("HelpImg2").GetComponent<SpriteRenderer>().sprite = helpImg2List[page];
        GameObject.Find("PageNum").GetComponent<TextMeshProUGUI>().text = "" + (page + 1);
        GameObject.Find("HelpTx").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GameObject.Find("HelpExp").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GameObject.Find("HelpImg").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GameObject.Find("HelpImg2").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GameObject.Find("PageNum").GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 1);
        GameObject.Find("NextPage").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        GameObject.Find("PreviousPage").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        if(page == 0)
        {
            GameObject.Find("NextPage").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
            GameObject.Find("PreviousPage").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
        else if(page == MaxPage - 1)
        {
            GameObject.Find("NextPage").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            GameObject.Find("PreviousPage").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        }
        else
        {
            GameObject.Find("NextPage").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
            GameObject.Find("PreviousPage").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        }
        if(page == 3)
        {
            GameObject.Find("HelpImg").transform.localScale = new Vector3(0.85f, 1.0f, 1.0f);
            GameObject.Find("HelpImg2").transform.localScale = new Vector3(0.85f, 1.0f, 1.0f);
        }
        else
        {
            GameObject.Find("HelpImg").transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            GameObject.Find("HelpImg2").transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);

        helpTxList[0] = Resources.Load<Sprite>("helptx_01");
        helpTxList[1] = Resources.Load<Sprite>("helptx_02");
        helpTxList[2] = Resources.Load<Sprite>("helptx_03");
        helpTxList[3] = Resources.Load<Sprite>("helptx_04");
        helpTxList[4] = Resources.Load<Sprite>("helptx_05");
        helpExpList[0] = Resources.Load<Sprite>("helpexp_01");
        helpExpList[1] = Resources.Load<Sprite>("helpexp_02");
        helpExpList[2] = Resources.Load<Sprite>("helpexp_03");
        helpExpList[3] = Resources.Load<Sprite>("helpexp_04");
        helpExpList[4] = Resources.Load<Sprite>("helpexp_05");
        helpImgList[0] = Resources.Load<Sprite>("help1_1");
        helpImgList[1] = Resources.Load<Sprite>("help2_1");
        helpImgList[2] = Resources.Load<Sprite>("help3_1");
        helpImgList[3] = Resources.Load<Sprite>("help4_1");
        helpImgList[4] = Resources.Load<Sprite>("help5_1");
        helpImg2List[0] = Resources.Load<Sprite>("help1_2");
        helpImg2List[1] = Resources.Load<Sprite>("help2_2");
        helpImg2List[2] = Resources.Load<Sprite>("help3_2");
        helpImg2List[3] = Resources.Load<Sprite>("help4_2");
        helpImg2List[4] = Resources.Load<Sprite>("help5_2");

        SetPage(0);
        SetInvisible();
        SetState(State.Invalid);
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        switch(state)
        {
        case State.Invalid:
            break;
        case State.FadeIn:
            sr.color = new Color(1, 1, 1, (float)time / FadeInTime);
            if(time == FadeInTime)
            {
                SetState(State.Process);
            }
            break;
        case State.Process:
            if(page < MaxPage - 1)
            {
                if(Input.GetMouseButtonDown(0) || func.getTouch() == 1)
                {
                    float touchedx = Application.isEditor ? func.mouse().x : func.getTouchPosition().x;
                    float touchedy = Application.isEditor ? func.mouse().y : func.getTouchPosition().y;
                    if(func.CircleCollision(touchedx, touchedy, 0.01f, TriangleX, TriangleY, TriangleSize))
                    {
                        SetPage(page + 1);
                    }
                }

            }
            if(page > 0)
            {
                if(Input.GetMouseButtonDown(0) || func.getTouch() == 1)
                {
                    float touchedx = Application.isEditor ? func.mouse().x : func.getTouchPosition().x;
                    float touchedy = Application.isEditor ? func.mouse().y : func.getTouchPosition().y;
                    if(func.CircleCollision(touchedx, touchedy, 0.01f, -TriangleX, TriangleY, TriangleSize))
                    {
                        SetPage(page - 1);
                    }
                }
            }
            break;
        case State.FadeOut:
            sr.color = new Color(1, 1, 1, 1.0f - (float)time / FadeInTime);
            if(time == FadeOutTime)
            {
                SetState(State.Invalid);
            }
            break;
        }
    }
}
