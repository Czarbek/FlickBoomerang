using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ヘルプ表示ボタン
/// </summary>
public class HelpButton : TitleManager
{
    /// <summary>
    /// ピクセル単位の横幅
    /// </summary>
    private const int ButtonPxSizeX = 128;
    /// <summary>
    /// ピクセル単位の縦幅
    /// </summary>
    private const int ButtonPxSizeY = 128;
    /// <summary>
    /// TitleManagerオブジェクト(状態同期用)
    /// </summary>
    private GameObject manager;
    /// <summary>
    /// ヘルプボタンのスプライト
    /// </summary>
    private Sprite sp_help;
    /// <summary>
    /// 閉じるボタンのスプライト
    /// </summary>
    private Sprite sp_close;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("TitleManager");
        sp_help = Resources.Load<Sprite>("button_help");
        sp_close = Resources.Load<Sprite>("button_close");

        transform.position = new Vector2(0, StageInfo.ycalc(10));

        BSizeX = func.pxcalc(ButtonPxSizeX) / 2;
        BSizeY = func.pxcalc(ButtonPxSizeY) / 2;

        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);
        sr.sortingOrder = 2;

        time = 0;
        state = State.Title;
        dspState = DspState.Wait;

        if(Initializer.GetRetry())
        {
            SetState(State.Select);
            sr.color = new Color(1, 1, 1, 0);
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
        sr.sprite = sp_help;
        state = manager.GetComponent<TitleManager>().state;

        switch(state)
        {
        case State.Title:
            time++;
            switch(dspState)
            {
            case DspState.Wait:
                
                break;
            case DspState.FadeIn:
                
                sr.color = new Color(col.r, col.g, col.b, (float)time / FadeInTime);
                if(time == FadeInTime)
                {
                    SetDspState(DspState.Process);
                }
                break;
            case DspState.FadeIn2:
                sr.color = new Color(col.r, col.g, col.b, (float)time / FadeInTime2);
                if(time == FadeInTime2)
                {
                    SetDspState(DspState.Process);
                }
                break;
            case DspState.Process:
                sr.sprite = sp_help;
                
                if(touchOnObj && touched && Fader.IsEnd())
                {
                    GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.Button);
                    manager.GetComponent<TitleManager>().SetDspState(DspState.HelpFadeOut);
                    SetDspState(DspState.HelpFadeOut);
                }
                break;
            case DspState.FadeOut:
                sr.color = new Color(col.r, col.g, col.b, 1.0f - (float)time / FadeOutTime);
                if(time == FadeOutTime)
                {
                    SetDspState(DspState.Invalid);
                }
                break;

            case DspState.HelpFadeOut:
                sr.color = new Color(col.r, col.g, col.b, 1.0f - (float)time / HelpFadeOutTime);
                float scale_ = 1.0f - (1.0f - ShrinkRate) * (float)time / HelpFadeOutTime;
                transform.localScale = new Vector2(scale_, scale_);
                if(time == HelpFadeOutTime)
                {
                    transform.localScale = new Vector2(1, 1);
                    manager.GetComponent<TitleManager>().SetState(State.Help);
                    GameObject.Find("HelpWindow").GetComponent<HelpWindow>().SetState(HelpWindow.State.FadeIn);
                    SetDspState(DspState.FadeIn2);
                }
                break;
            case DspState.Invalid:
                sr.color = new Color(col.r, col.g, col.b, 0);
                
                break;
            }
            break;
        case State.Help:
            sr.sprite = sp_close;
            time++;
            switch(dspState)
            {
            case DspState.FadeIn2:
                sr.color = new Color(col.r, col.g, col.b, (float)time / FadeInTime2);
                if(time == FadeInTime2)
                {
                    SetDspState(DspState.Process);
                }
                break;
            case DspState.Process:
                if(touchOnObj && touched && Fader.IsEnd())
                {
                    GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.Button);
                    manager.GetComponent<TitleManager>().SetDspState(DspState.HelpFadeOut);
                    GameObject.Find("HelpWindow").GetComponent<HelpWindow>().SetState(HelpWindow.State.FadeOut);
                    SetDspState(DspState.HelpFadeOut);
                }
                break;
            case DspState.HelpFadeOut:
                sr.color = new Color(col.r, col.g, col.b, 1.0f - (float)time / HelpFadeOutTime);
                float scale_ = 1.0f - (1.0f - ShrinkRate) * (float)time / HelpFadeOutTime;
                transform.localScale = new Vector2(scale_, scale_);
                if(time == HelpFadeOutTime)
                {
                    transform.localScale = new Vector2(1, 1);
                    manager.GetComponent<TitleManager>().SetState(State.Title);
                    manager.GetComponent<TitleManager>().SetDspState(DspState.FadeIn2);
                    SetDspState(DspState.FadeIn2);
                }
                break;
            case DspState.Invalid:
                break;
            default:
                break;
            }
            break;
        case State.Select:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        }
    }
}
