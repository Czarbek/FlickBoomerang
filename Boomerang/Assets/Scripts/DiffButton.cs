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
    /// 閉じるボタンのピクセル単位の横幅
    /// </summary>
    private const int ButtonPxSizeX_close = 600;
    /// <summary>
    /// 閉じるボタンのピクセル単位の縦幅
    /// </summary>
    private const int ButtonPxSizeY_close = 240;
    /// <summary>
    /// ステージ数
    /// </summary>
    public const int StageNum = 3;
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
    private Sprite[] sp_diff = new Sprite[6];
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

        sp_diff[0] = Resources.Load<Sprite>("button_stage01");
        sp_diff[1] = Resources.Load<Sprite>("button_stage02");
        sp_diff[2] = Resources.Load<Sprite>("button_stage03");
        sp_diff[3] = Resources.Load<Sprite>("button_close");
        sp_diff[4] = Resources.Load<Sprite>("button_stage02_locked");
        sp_diff[5] = Resources.Load<Sprite>("button_stage03_locked");
        GetComponent<SpriteRenderer>().sprite = sp_diff[index];
        if(index > 0 && index < 3)
        {
            if(!ClearData.IsCleared(index - 1))
            {
                GetComponent<SpriteRenderer>().sprite = sp_diff[index+3];
            }
        }
        if(index == 3)
        {
            BSizeX = func.pxcalc(ButtonPxSizeX_close) / 2;
            BSizeY = func.pxcalc(ButtonPxSizeY_close) / 2;
        }
        else
        {
            BSizeX = func.pxcalc(ButtonPxSizeX) / 2;
            BSizeY = func.pxcalc(ButtonPxSizeY) / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool touchOnObj = Application.isEditor ? func.MouseCollision(transform.position, BSizeX, BSizeY, true) : func.MouseCollision(transform.position, BSizeX, BSizeY, true)||func.TouchCollision(transform.position, BSizeX, BSizeY, true);
        bool touched = Application.isEditor ? Input.GetMouseButtonDown(0) : Input.GetMouseButtonDown(0)||func.getTouch() == 1;
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
            if(touchOnObj)
            {
                if(touched && Fader.IsEnd() && lagt == 0)
                {
                    switch(index)
                    {
                    case 0:
                        stageInfo.GetComponent<StageInfo>().LoadStageInfo(index);
                        Fader.SetFader(20, true, "Stage");
                        break;
                    case 1:
                    case 2:
                        if(ClearData.IsCleared(index - 1))
                        {
                            stageInfo.GetComponent<StageInfo>().LoadStageInfo(index);
                            Fader.SetFader(20, true, "Stage");
                        }
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
