using UnityEngine;

public class SelectBack : MonoBehaviour
{
    /// <summary>
    /// 状態一覧
    /// </summary>
    public enum State
    {
        /// <summary>開始前待機</summary>
        Wait,
        /// <summary>フェードイン</summary>
        FadeIn,
        /// <summary>表示中</summary>
        Process,
        /// <summary>フェードアウト</summary>
        FadeOut,
    };
    /// <summary>
    /// フェードインにかかる時間
    /// </summary>
    private const int FadeInTime = (int)(200.0f / func.FRAMETIME);

    /// <summary>
    /// 状態
    /// </summary>
    private State state;
    /// <summary>
    /// 処理時間
    /// </summary>
    private int time;
    /// <summary>
    /// SpriteRendererコンポーネント
    /// </summary>
    private SpriteRenderer sr;

    public void SetFadeIn()
    {
        state = State.FadeIn;
    }
    public void SetFadeOut()
    {
        state = State.FadeOut;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Wait;
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
        case State.Wait:
            break;
        case State.FadeIn:
            time++;
            sr.color = new Color(1, 1, 1, (float)time/FadeInTime);
            if(time == FadeInTime)
            {
                time = 0;
                state = State.Process;
            }
            break;
        case State.Process:
            break;
        case State.FadeOut:
            time++;
            sr.color = new Color(1, 1, 1, 1.0f - (float)time / FadeInTime);
            if(time == FadeInTime)
            {
                time = 0;
                state = State.Wait;
            }
            break;
        }
    }
}
