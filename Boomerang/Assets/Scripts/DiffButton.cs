using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度ボタン
/// </summary>
public class DiffButton : TitleManager
{
    /// <summary>
    /// ピクセル単位の横幅
    /// </summary>
    private const int ButtonPxSizeX = 820;
    /// <summary>
    /// ピクセル単位の縦幅
    /// </summary>
    private const int ButtonPxSizeY = 270;
    /// <summary>
    /// TitleManagerオブジェクト(状態同期用)
    /// </summary>
    private GameObject manager;
    /// <summary>
    /// StageInfoオブジェクト(ロード指示用)
    /// </summary>
    private GameObject stageInfo;
    /// <summary>
    /// 各難易度ボタンのスプライト
    /// </summary>
    private Sprite[] sp_diff = new Sprite[4];
    /// <summary>
    /// ボタンの番号
    /// </summary>
    private int index;
    /// <summary>
    /// 入力拒否時間
    /// </summary>
    private int lagt;
    /// <summary>
    /// 
    /// </summary>
    private TitleManager.State previousState;
    /// <summary>
    /// スプライトを設定する
    /// </summary>
    /// <param name="index">リストのインデックス</param>
    public void SetSprite(int index)
    {
        this.index = index;
    }
    // Start is called before the first frame update
    void Start()
    {
        lagt = 0;
        previousState = TitleManager.State.Title;
        manager = GameObject.Find("TitleManager");
        stageInfo = GameObject.Find("StageInfo");

        sp_diff[0] = Resources.Load<Sprite>("button_select1");
        sp_diff[1] = Resources.Load<Sprite>("button_select2");
        sp_diff[2] = Resources.Load<Sprite>("button_select3");
        sp_diff[3] = Resources.Load<Sprite>("button_close");
        GetComponent<SpriteRenderer>().sprite = sp_diff[index];

        BSizeX = (float)ButtonPxSizeX / func.SCW * func.camWidth * 4 * transform.localScale.x;
        BSizeY = (float)ButtonPxSizeY / func.SCH * func.camHeight * 4 * transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        state = manager.GetComponent<TitleManager>().GetState();
        if(previousState != state)
        {
            lagt = 5;
            previousState = state;
        }
        switch(state)
        {
        case State.Title:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        case State.Help:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        case State.Select:
            sr.color = new Color(col.r, col.g, col.b, 1);
            if(func.MouseCollision(transform.position, BSizeX, BSizeY, true))
            {
                if(Input.GetMouseButtonDown(0) && Fader.IsEnd() && lagt == 0)
                {
                    switch(index)
                    {
                    case 0:
                        stageInfo.GetComponent<StageInfo>().LoadStageInfo(0);
                        Fader.SetFader(20, true, "Stage");
                        break;
                    case 1:
                        Fader.SetFader(20, true, "Stage2");
                        break;
                    case 2:
                        Fader.SetFader(20, true, "Stage3");
                        break;
                    case 3:
                        manager.GetComponent<TitleManager>().SetState(State.Title);
                        break;
                    }
                }
            }
            break;
        }
        if(lagt>0) lagt--;
    }
}
