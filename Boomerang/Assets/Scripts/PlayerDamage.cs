using UnityEngine;

/// <summary>
/// プレイヤーへのダメージエフェクト
/// </summary>
public class PlayerDamage : MonoBehaviour

{
    private enum State
    {
        Invalid,
        FadeIn,
        Process,
        FadeOut,
    };

    private const int FadeTimeMiliSec = 500;

    /// <summary>
    /// フェード時間(フレーム数)
    /// </summary>
    private int FadeTime;
    private State state;
    /// <summary>
    /// 処理時間
    /// </summary>
    private int time;
    /// <summary>
    /// 不透明度
    /// </summary>
    private float alpha;
    /// <summary>
    /// SpriteRendererコンポーネント
    /// </summary>
    private SpriteRenderer sr;

    /// <summary>
    /// 状態を変更する
    /// </summary>
    /// <param name="state">変更先の状態</param>
    private void SetState(State state)
    {
        this.state = state;
        this.time = 0;
    }
    public void SetVisibility(bool visible)
    {
        if(visible)
        {
            if(state == State.Invalid)
            {
                SetState(State.FadeIn);
            }
        }
        else
        {
            if(state == State.Process)
            {
                SetState(State.FadeOut);
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Invalid;
        FadeTime = (int)(FadeTimeMiliSec / func.FRAMETIME);

        alpha = 0.0f;
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        switch(state)
        {
        case State.Invalid:
            alpha = 0;
            break;
        case State.Process:
            alpha = 1.0f;
            break;
        case State.FadeIn:
            alpha = (float)time / FadeTime;
            if(time == FadeTime)
            {
                time = 0;
                state = State.Process;
            }
            break;
        case State.FadeOut:
            alpha = 1.0f - (float)time / FadeTime;
            if(time == FadeTime)
            {
                time = 0;
                SetState(State.Invalid);
            }
            break;
        }
        sr.color = new Color(1, 1, 1, alpha);
    }
}
