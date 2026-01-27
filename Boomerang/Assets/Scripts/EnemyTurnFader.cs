using UnityEngine;

/// <summary>
/// 敵ターンを示すオーバーレイ
/// </summary>
public class EnemyTurnFader : MonoBehaviour
{
    /// <summary>
    /// 表示状態一覧
    /// </summary>
    private enum State
    {
        /// <summary>非表示</summary>
        Invalid,
        /// <summary>フェードイン</summary>
        FadeIn,
        /// <summary>待機</summary>
        Wait,
        /// <summary>フェードアウト</summary>
        FadeOut,
    };
    /// <summary>
    /// 不透明度最大値
    /// </summary>
    private const float MaxAlpha = 0.45f;
    /// <summary>
    /// フェードにかかるフレーム数
    /// </summary>
    private const int FadeTime = 10;
    /// <summary>
    /// 表示状態
    /// </summary>
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
    /// 表示状態を変更する
    /// </summary>
    /// <param name="isVisible">trueなら表示開始、falseなら表示終了</param>
    public void SetVisibility(bool isVisible)
    {
        if(isVisible && state == State.Invalid || !isVisible && state == State.Wait)
        {
            state = isVisible ? State.FadeIn : State.FadeOut;
            time = 0;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Invalid;
        time = 0;
        alpha = 0;
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 0, 0, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
        case State.Invalid:
            time = 0;
            alpha = 0;
            break;
        case State.FadeIn:
            time++;
            alpha = MaxAlpha * (float)time / FadeTime;
            if(time == FadeTime)
            {
                state = State.Wait;
                time = 0;
                alpha = MaxAlpha;
            }
            break;
        case State.Wait:
            time = 0;
            alpha = MaxAlpha;
            state = State.FadeOut;
            break;
        case State.FadeOut:
            time++;
            alpha = MaxAlpha * (1.0f - (float)time / FadeTime);
            if(time == FadeTime)
            {
                state = State.Invalid;
                time = 0;
                alpha = 0;
            }
            break;
        }
        sr.color = new Color(1, 0, 0, alpha);
    }
}
