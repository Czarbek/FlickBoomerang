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
    /// フェードインにかかる時間
    /// </summary>
    new private const int FadeInTime = (int)(100.0f / func.FRAMETIME);
    /// <summary>
    /// 縮小率
    /// </summary>
    new private const float ShrinkRate = 0.3f;
    /// <summary>
    /// 縮小にかかる時間
    /// </summary>
    private const int ShrinkTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// 縮小中かどうか
    /// </summary>
    private bool shrinking;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;

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
        shrinking = false;
        time = 0;
        lagt = 0;
        previousState = TitleManager.State.Title;
        manager = GameObject.Find("TitleManager");
        stageInfo = GameObject.Find("StageInfo");

        state = manager.GetComponent<TitleManager>().GetState();
        dspState = DspState.FadeIn;

        sr = GetComponent<SpriteRenderer>();
        sp_diff[0] = Resources.Load<Sprite>("button_stage01");
        sp_diff[1] = Resources.Load<Sprite>("button_stage02");
        sp_diff[2] = Resources.Load<Sprite>("button_stage03");
        sp_diff[3] = Resources.Load<Sprite>("button_close");
        sp_diff[4] = Resources.Load<Sprite>("button_stage02_locked");
        sp_diff[5] = Resources.Load<Sprite>("button_stage03_locked");
        sr.sprite = sp_diff[index];
        sr.color = new Color(1, 1, 1, 0);
        if(index > 0 && index < 3)
        {
            if(!ClearData.IsCleared(index - 1))
            {
                sr.sprite = sp_diff[index+3];
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

        if(Initializer.GetRetry())
        {
            SetState(State.Select);
            sr.color = new Color(1, 1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        bool touchOnObj = Application.isEditor ? func.MouseCollision(transform.position, BSizeX, BSizeY, true) : func.MouseCollision(transform.position, BSizeX, BSizeY, true)||func.TouchCollision(transform.position, BSizeX, BSizeY, true);
        bool touched = Application.isEditor ? Input.GetMouseButtonDown(0) : Input.GetMouseButtonDown(0)||func.getTouch() == 1;
        */
        bool touchOnObj = func.MouseCollision(transform.position, BSizeX, BSizeY, true);
        bool touched = Input.GetMouseButtonDown(0);

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
            time++;
            switch(dspState)
            {
            case DspState.Wait:
                break;
            case DspState.FadeIn:
                int currentTime = time > FadeInTime * index ? time - FadeInTime * index : 0;
                if(currentTime > FadeInTime)
                {
                    currentTime = FadeInTime;
                }
                sr.color = new Color(col.r, col.g, col.b, (float)currentTime / FadeInTime);
                if(time == FadeInTime * (StageNum + 1))
                {
                    SetDspState(DspState.Process);
                }
                break;
            case DspState.Process:
                if(touchOnObj)
                {
                    if(touched && Fader.IsEnd() && lagt == 0)
                    {
                        switch(index)
                        {
                        case 0:
                            stageInfo.GetComponent<StageInfo>().LoadStageInfo(index);
                            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.Button);
                            SetDspState(DspState.FadeOut);
                            Fader.SetFader(20, true, "Stage");
                            break;
                        case 1:
                        case 2:
                            if(ClearData.IsCleared(index - 1))
                            {
                                stageInfo.GetComponent<StageInfo>().LoadStageInfo(index);
                                GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.Button);
                                SetDspState(DspState.FadeOut);
                                Fader.SetFader(20, true, "Stage");
                            }
                            else
                            {
                                GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.Error);
                                Caution.SetVisibility(index - 1);
                                manager.GetComponent<TitleManager>().SetState(State.Caution);
                            }
                            break;
                        case 3:
                            manager.GetComponent<TitleManager>().SetState(State.Title);
                            GameObject.Find("TitleLogo").GetComponent<TitleLogo>().SetState(TitleLogo.State.FadeIn);
                            GameObject.Find("TitleManager").GetComponent<TitleManager>().SetDspState(TitleManager.DspState.FadeIn2);
                            GameObject.Find("HelpButton").GetComponent<HelpButton>().SetDspState(TitleManager.DspState.FadeIn2);
                            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.Button);
                            break;
                        }
                    }
                }
                break;
            case DspState.FadeOut:
                sr.color = new Color(col.r, col.g, col.b, 1.0f - (float)time / FadeOutTime);
                float scale = 1.0f - (1.0f - ShrinkRate) * (float)time / FadeOutTime;
                transform.localScale = new Vector2(scale, scale);
                if(time == FadeOutTime)
                {
                    SetDspState(DspState.Invalid);
                }
                break;
            case DspState.Invalid:
                break;
            }
            break;
        case State.Caution:
            break;
        }
        if(shrinking)
        {
            time++;
            float scale = ShrinkRate + (1 - ShrinkRate) * (1 - (float)time / ShrinkTime);
            transform.localScale = new Vector2(scale, scale);
            if(time == ShrinkTime)
            {
                shrinking = false;
            }
        }
        if(lagt>0) lagt--;
    }
}
