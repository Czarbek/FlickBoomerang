using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// フェード管理
/// </summary>
public class Fader : MonoBehaviour
{
    /// <summary>
    /// スプライトのx方向の拡大率
    /// </summary>
    private const float Sizex = func.camWidth * 6;
    /// <summary>
    /// スプライトのy方向の拡大率
    /// </summary>
    private const float Sizey = func.camHeight * 6;
    /// <summary>
    /// 状態一覧
    /// </summary>
    private enum State
    {
        /// <summary>待機</summary>
        Wait,
        /// <summary>フェードアウト</summary>
        FadeOut,
        /// <summary>フェードアウト後待機</summary>
        FadeWait,
        /// <summary>フェードイン</summary>
        FadeIn,
    };
    /// <summary>
    /// 状態
    /// </summary>
    static private State state;
    /// <summary>
    /// 矩形の色のR値
    /// </summary>
    private float r;
    /// <summary>
    /// 矩形の色のG値
    /// </summary>
    private float g;
    /// <summary>
    /// 矩形の色のB値
    /// </summary>
    private float b;
    /// <summary>
    /// 矩形の不透明度
    /// </summary>
    private float alpha;
    /// <summary>
    /// 状態が変化してからの経過時間(フレーム数)
    /// </summary>
    static private int time;
    /// <summary>
    /// フェードアウト後からフェードインに移行するまでの待機時間(フレーム数)
    /// </summary>
    static private int waitTime;
    /// <summary>
    /// 遷移先のシーン名
    /// </summary>
    static private string destination;

    /// <summary>
    /// フェードにかかる時間(フレーム数)
    /// </summary>
    public int FadeTime = 45;
    /// <summary>
    /// フェードアウトしてからフェードイン開始までのデフォルト待機時間
    /// </summary>
    static public int FadeWaitTime = 30;

    // Start is called before the first frame update
    void Start()
    {
        
        DontDestroyOnLoad(this);
        state = State.Wait;
        r = 0.0f;
        g = 0.0f;
        b = 0.0f;
        alpha = 0.0f;
        time = 0;
        transform.position = new Vector2(func.SCCX, func.SCCY);
        transform.localScale = new Vector3(Sizex, Sizey, 1.0f);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(r, g, b, alpha);
    }
    /// <summary>
    /// フェードの状態を変化させる
    /// </summary>
    /// <param name="waitTime_">フェードアウト後の待機時間</param>
    /// <param name="isFadeOut">フェードアウトに移行するかどうか</param>
    /// <param name="destinatedScene">遷移先のシーン名</param>
    /// <return>なし</return>
    static public void SetFader(int waitTime_, bool isFadeOut = true, string destinatedScene = null)
    {
        time = 0;
        waitTime = waitTime_;
        destination = destinatedScene;
        state = isFadeOut ? State.FadeOut : State.FadeIn;
    }
    /// <summary>
    /// フェード処理中でないかどうか判定する
    /// </summary>
    /// <returns>フェード処理中でないならtrueを返す</returns>
    static public bool IsEnd()
    {
        return state == State.Wait;
    }
    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        time++;
        switch(state)
        {
        case State.Wait:
            break;
        case State.FadeOut:
            alpha = 1.0f / FadeTime * time;
            if(time >= FadeTime)
            {
                time = 0;
                alpha = 1.0f;
                state = State.FadeWait;
            }
            break;
        case State.FadeWait:
            if(time >= waitTime)
            {
                time = 0;
                if(destination != null)
                {
                    SceneManager.LoadScene(destination);
                }
                state = State.FadeIn;
            }
            break;
        case State.FadeIn:
            alpha = 1.0f - 1.0f / FadeTime * time;
            if(time >= FadeTime)
            {
                time = 0;
                waitTime = 0;
                destination = null;
                alpha = 0.0f;
                state = State.Wait;
            }
            break;
        }
        sr.color = new Color(r, g, b, alpha);

        bool touch = Application.isEditor ? Input.GetMouseButtonDown(0) : Input.GetMouseButtonDown(0) || func.getTouch() == 1;
        Vector2 touchLoc = func.mouse();
        if(touch)
        {
            GameObject tapEffect = Instantiate((GameObject)Resources.Load("TapEffect"));
            tapEffect.transform.position = touchLoc;
            Debug.Log("touch");
        }
    }
}
