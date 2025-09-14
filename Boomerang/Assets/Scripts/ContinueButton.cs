using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    /// <summary>
    /// ボタンの種類一覧
    /// </summary>
    public enum ButtonSort
    {
        /// <summary>コンティニューする</summary>
        Continue_Yes,
        /// <summary>コンティニューしない</summary>
        Continue_No,
        /// <summary>次に進む</summary>
        Clear_Next,
        /// <summary>タイトルに戻る</summary>
        Clear_Quit,
    };
    /// <summary>
    /// ボタンの状態一覧
    /// </summary>
    public enum State
    {
        /// <summary>非表示</summary>
        Invalid,
        /// <summary>フェードイン</summary>
        FadeIn,
        /// <summary>表示中</summary>
        Process,
        /// <summary>押された</summary>
        Pushed,
        /// <summary>押されなかった</summary>
        NotPushed,
    };
    /// <summary>
    /// ボタンの表示位置x
    /// </summary>
    private const float ButtonX = 0;
    /// <summary>
    /// 上ボタンの表示位置y
    /// </summary>
    private const float ButtonY_1 = -1.0f;
    /// <summary>
    /// 下ボタンの表示位置y
    /// </summary>
    private const float ButtonY_2 = -2.5f;
    /// <summary>
    /// ピクセル単位の横幅
    /// </summary>
    private const int ButtonPxSizeX = 600;
    /// <summary>
    /// ピクセル単位の縦幅
    /// </summary>
    private const int ButtonPxSizeY = 240;
    /// <summary>
    /// 横幅(半分)
    /// </summary>
    private readonly float BSizeX = func.pxcalc(ButtonPxSizeX) / 2;
    /// <summary>
    /// 縦幅(半分)
    /// </summary>
    private readonly float BSizeY = func.pxcalc(ButtonPxSizeY) / 2;
    /// <summary>
    /// ボタンの状態
    /// </summary>
    State state;
    /// <summary>
    /// ボタンの種類
    /// </summary>
    ButtonSort buttonSort;
    /// <summary>
    /// Component<SpriteRenderer>
    /// </summary>
    SpriteRenderer sr;
    /// <summary>
    /// 画像のr値
    /// </summary>
    private float r;
    /// <summary>
    /// 画像のg値
    /// </summary>
    private float g;
    /// <summary>
    /// 画像のb値
    /// </summary>
    private float b;
    /// <summary>
    /// 画像のalpha値
    /// </summary>
    private float alpha;
    /// <summary>
    /// 時間
    /// </summary>
    private int time;
    /// <summary>
    /// フェードインにかかる時間
    /// </summary>
    private const int FadeInTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// もう一方のボタンのオブジェクト
    /// </summary>
    private GameObject partner;
    /// <summary>
    /// ボタンの種類をセットする
    /// </summary>
    /// <param name="buttonSort"></param>
    public void SetButton(ButtonSort buttonSort)
    {
        this.buttonSort = buttonSort;
    }
    /// <summary>
    /// 相方をセットする
    /// </summary>
    /// <param name="partner"></param>
    public void SetPartner(GameObject partner)
    {
        this.partner = partner;
    }
    /// <summary>
    /// 状態を書き換える
    /// </summary>
    /// <param name="state">状態</param>
    public void SetState(State state) {
        this.state = state;
    }
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        r = sr.color.r;
        g = sr.color.g;
        b = sr.color.b;
        alpha = 0;
        sr.color = new Color(r, g, b, alpha);
        float x = ButtonX;
        float y = ButtonY_1;

        switch(buttonSort)
        {
        case ButtonSort.Continue_Yes:
            sr.sprite = Resources.Load<Sprite>("button_yes");
            y = ButtonY_1;
            break;
        case ButtonSort.Continue_No:
            sr.sprite = Resources.Load<Sprite>("button_no");
            y = ButtonY_2;
            break;
        case ButtonSort.Clear_Next:
            sr.sprite = Resources.Load<Sprite>("button_next");
            y = ButtonY_1;
            break;
        case ButtonSort.Clear_Quit:
            sr.sprite = Resources.Load<Sprite>("button_quit");
            y = ButtonY_2;
            break;
        }

        transform.position = new Vector2(x, y);

        state = State.FadeIn;
    }

    // Update is called once per frame
    void Update()
    {
        bool touchOnObj = Application.isEditor ? func.MouseCollision(transform.position, BSizeX, BSizeY, true) : func.MouseCollision(transform.position, BSizeX, BSizeY, true)||func.TouchCollision(transform.position, BSizeX, BSizeY, true);
        bool touched = Application.isEditor ? Input.GetMouseButtonDown(0) : Input.GetMouseButtonDown(0) || func.getTouch() == 1;
        switch(state)
        {
        case State.Invalid:
            break;
        case State.FadeIn:
            time++;
            alpha = 1.0f * time / FadeInTime;
            sr.color = new Color(r, g, b, alpha);
            if(time == FadeInTime)
            {
                state = State.Process;
            }
            break;
        case State.Process:
            if(touchOnObj)
            {
                if(touched && Fader.IsEnd())
                {
                    partner.GetComponent<ContinueButton>().SetState(State.NotPushed);
                    state = State.Pushed;
                    switch(buttonSort)
                    {
                    case ButtonSort.Continue_Yes:
                        Fader.SetFader(Fader.FadeWaitTime, true, "Title");
                        break;
                    case ButtonSort.Continue_No:
                        Fader.SetFader(Fader.FadeWaitTime, true, "Title");
                        break;
                    case ButtonSort.Clear_Next:
                        Fader.SetFader(Fader.FadeWaitTime, true, "Title");
                        break;
                    case ButtonSort.Clear_Quit:
                        Fader.SetFader(Fader.FadeWaitTime, true, "Title");
                        break;
                    }
                }
            }
            break;
        case State.Pushed:
            break;
        case State.NotPushed:
            break;
        }
        
    }
}
