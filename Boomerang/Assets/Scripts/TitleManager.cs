using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル画面管理、スタートボタン
/// </summary>
public class TitleManager : MonoBehaviour
{
    /// <summary>
    /// ピクセル単位の横幅
    /// </summary>
    private const int ButtonPxSizeX = 640;
    /// <summary>
    /// ピクセル単位の縦幅
    /// </summary>
    private const int ButtonPxSizeY = 190;
    /// <summary>
    /// 横幅(半分)
    /// </summary>
    public float BSizeX;
    /// <summary>
    /// 縦幅(半分)
    /// </summary>
    public float BSizeY;
    /// <summary>
    /// 難易度ボタンのy座標
    /// </summary>
    private readonly float[] DiffButtonY = new float[4];
    /// <summary>
    /// モード一覧
    /// </summary>
    public enum State
    {
        /// <summary>タイトル</summary>
        Title,
        /// <summary>ヘルプ</summary>
        Help,
        /// <summary>難易度選択</summary>
        Select,
        /// <summary>選択不可表示</summary>
        Caution,
    };
    /// <summary>
    /// 表示状態
    /// </summary>
    public enum DspState
    {
        /// <summary>開始前待機</summary>
        Wait,
        /// <summary>フェードイン</summary>
        FadeIn,
        /// <summary>表示中</summary>
        Process,
        /// <summary>フェードアウト</summary>
        FadeOut,
        /// <summary>ヘルプ移行時フェードアウト</summary>
        HelpFadeOut,
        /// <summary>フェードイン2回目以降</summary>
        FadeIn2,
        /// <summary>非表示</summary>
        Invalid,
    };
    /// <summary>
    /// モード
    /// </summary>
    public State state;
    /// <summary>
    /// 表示状態
    /// </summary>
    public DspState dspState;
    /// <summary>
    /// フェードインにかかる時間
    /// </summary>
    protected const int FadeInTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// 2回目以降のフェードインにかかる時間
    /// </summary>
    protected const int FadeInTime2 = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// フェードアウトにかかる時間
    /// </summary>
    protected const int FadeOutTime = (int)(1000.0f / func.FRAMETIME);
    /// <summary>
    /// ヘルプモード移行時フェードアウトにかかる時間
    /// </summary>
    protected const int HelpFadeOutTime = (int)(250.0f / func.FRAMETIME);
    /// <summary>
    /// 点滅時間
    /// </summary>
    private const int BlinkTime = (int)(1500.0f / func.FRAMETIME);
    /// <summary>
    /// 縮小率
    /// </summary>
    protected const float ShrinkRate = 0.4f;
    /// <summary>
    /// 処理時間
    /// </summary>
    protected int time;
    /// <summary>
    /// 
    /// </summary>
    private bool selected;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    /// <summary>
    /// ステージ選択ボタン
    /// </summary>
    private GameObject[] button = new GameObject[4];

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);
        sr.sortingOrder = 2;
        transform.position = new Vector2(0, StageInfo.ycalc(50));
        BSizeX = func.pxcalc(ButtonPxSizeX) / 2;
        BSizeY = func.pxcalc(ButtonPxSizeY) / 2;
        DiffButtonY[0] = StageInfo.ycalc(110); //2.5
        DiffButtonY[1] = StageInfo.ycalc(80); //0.625
        DiffButtonY[2] = StageInfo.ycalc(50); //-1.25
        DiffButtonY[3] = StageInfo.ycalc(17.5f);

        state = State.Title;
        dspState = DspState.Wait;
        selected = false;

        if(Initializer.GetRetry())
        {
            SetState(State.Select);
            dspState= DspState.Wait;
            selected = true;
            sr.color = new Color(1, 1, 1, 0);
            for(int i = 0; i < 4; i++)
            {
                GameObject button = (GameObject)Resources.Load("DiffButton");
                button = Instantiate(button);
                button.transform.position = new Vector2(0, DiffButtonY[i]);
                button.GetComponent<DiffButton>().SetSprite(i);
            }
        }
    }
    /// <summary>
    /// 状態を取得する
    /// </summary>
    /// <returns>状態</returns>
    public State GetState()
    {
        return state;
    }
    /// <summary>
    /// 状態を変更する
    /// </summary>
    /// <param name="state">変更先の状態</param>
    public void SetState(State state)
    {
        this.state = state;
    }
    /// <summary>
    /// 表示状態を変更する
    /// </summary>
    /// <param name="state">変更先の表示状態</param>
    public void SetDspState(DspState dspState)
    {
        time = 0;
        this.dspState = dspState;
        if(dspState == DspState.FadeIn2)
        {
            transform.localScale = new Vector2(1, 1);
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
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(func.mouse());
        }
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
                sr.color = new Color(col.r, col.g, col.b, func.abs(func.sin((float)time / BlinkTime * 180 + 90)));
                if(touchOnObj && touched && Fader.IsEnd())
                {
                    GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.Button);
                    SetDspState(DspState.FadeOut);
                    GameObject.Find("TitleLogo").GetComponent<TitleLogo>().SetState(TitleLogo.State.FadeOut);
                    GameObject.Find("HelpButton").GetComponent<HelpButton>().SetDspState(DspState.FadeOut);
                }
                break;
            case DspState.FadeOut:
                sr.color = new Color(col.r, col.g, col.b, 1.0f - (float)time / FadeOutTime);
                float scale = 1.0f - (1.0f - ShrinkRate) * (float)time / FadeOutTime;
                transform.localScale = new Vector2(scale, scale);
                if(time == FadeOutTime)
                {
                    if(!selected)
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            button[i] = (GameObject)Resources.Load("DiffButton");
                            button[i] = Instantiate(button[i]);
                            button[i].transform.position = new Vector2(0, DiffButtonY[i]);
                            button[i].GetComponent<DiffButton>().SetSprite(i);
                        }
                        selected = true;
                    }
                    else
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            button[i].GetComponent<DiffButton>().SetDspState(DspState.FadeIn);
                        }
                    }
                    state = State.Select;
                    GameObject.Find("SelectBack").GetComponent<SelectBack>().SetFadeIn();
                    SetDspState(DspState.Invalid);
                }
                break;
            case DspState.HelpFadeOut:
                sr.color = new Color(col.r, col.g, col.b, 1.0f - (float)time / HelpFadeOutTime);
                if(time == HelpFadeOutTime)
                {
                    SetDspState(DspState.Invalid);
                }
                break;
            case DspState.Invalid:
                break;
            }
            break;
        case State.Help:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        case State.Select:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        case State.Caution:
            break;
        }
    }
}
