using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogo : MonoBehaviour
{
    /// <summary>
    /// 状態一覧
    /// </summary>
    public enum State
    {
        /// <summary>開始前待機</summary>
        Wait,
        /// <summary>登場演出</summary>
        JumpIn,
        /// <summary>表示中</summary>
        Process,
        /// <summary>フェードアウト</summary>
        FadeOut,
        /// <summary>フェードイン</summary>
        FadeIn,
        /// <summary>非表示</summary>
        Invalid,
    };
    /// <summary>
    /// 状態
    /// </summary>
    private State state;
    /// <summary>
    /// 初期位置x座標
    /// </summary>
    private const float StartX = -func.camWidth * 4;
    /// <summary>
    /// x座標
    /// </summary>
    private readonly float CenterX = 0;
    /// <summary>
    /// y座標
    /// </summary>
    private readonly float CenterY = StageInfo.ycalc(120);
    /// <summary>
    /// 出現にかかる時間
    /// </summary>
    private const int JumpInTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// 回転時間
    /// </summary>
    private const int RotateTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// フェードアウトにかかる時間
    /// </summary>
    private const int FadeOutTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// フェードインにかかる時間
    /// </summary>
    private const int FadeInTime = (int)(150.0f / func.FRAMETIME);
    /// <summary>
    /// 処理時間
    /// </summary>
    private int time;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;

    /// <summary>
    /// 状態を変更する
    /// </summary>
    /// <param name="state">変更先の状態</param>
    public void SetState(State state)
    {
        time = 0;
        this.state = state;
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("testLogo");
        transform.position = new Vector2(StartX, CenterY);

        SetState(State.Wait);

        if(Initializer.GetRetry())
        {
            SetState(State.Invalid);
            sr.color = new Color(1, 1, 1, 0);
            transform.position = new Vector2(CenterX, CenterY);
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusic(MusicManager.BGM.Opening);
        }

    }

    // Update is called once per frame
    void Update()
    {
        time++;
        switch(state)
        {
        case State.Wait:
            if(Fader.IsEnd())
            {
                GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusic(MusicManager.BGM.Opening);
                SetState(State.JumpIn);
            }
            break;
        case State.JumpIn:
            if(time <= JumpInTime)
            {
                transform.position = new Vector2(StartX + (CenterX - StartX) * func.sin((float)time / JumpInTime * 90.0f), CenterY);
            }
            transform.rotation = Quaternion.Euler(0, 0, func.sin((float)time / RotateTime * 90.0f) * 360.0f);
            if(time == RotateTime)
            {
                SetState(State.Process);
                GameObject.Find("TitleManager").GetComponent<TitleManager>().SetDspState(TitleManager.DspState.FadeIn);
                GameObject.Find("HelpButton").GetComponent<TitleManager>().SetDspState(TitleManager.DspState.FadeIn);
            }
            break;
        case State.Process:
            break;
        case State.FadeOut:
            sr.color = new Color(1, 1, 1, 1.0f - (float)time / FadeOutTime);
            if(time == FadeOutTime)
            {
                SetState(State.Invalid);
            }
            break;
        case State.FadeIn:
            sr.color = new Color(1, 1, 1, (float)time / FadeInTime);
            if(time == FadeInTime)
            {
                SetState(State.Process);
            }
            break;
        case State.Invalid:
            break;
        }
    }
}
