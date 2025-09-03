using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

/// <summary>
/// 通常敵のHPゲージ
/// </summary>
public class EnemyGauge : MonoBehaviour
{
    /// <summary>
    /// 状態一覧
    /// </summary>
    private enum State
    {
        /// <summary>待機</summary>
        Wait,
        /// <summary>フェードイン</summary>
        FadeIn,
        /// <summary>表示中</summary>
        Process,
        /// <summary>フェードアウト</summary>
        FadeOut,
        /// <summary>非アクティブ</summary>
        Invalid,
    };
    /// <summary>
    /// 状態
    /// </summary>
    private State state;
    /// <summary>
    /// 横幅
    /// </summary>
    public float scaleX = 1.6f;
    /// <summary>
    /// 縦幅
    /// </summary>
    public float scaleY = 0.15f;
    /// <summary>
    /// 枠の横幅
    /// </summary>
    public float frameScaleX = 1.7f;
    /// <summary>
    /// 枠の縦幅
    /// </summary>
    public float frameScaleY = 0.25f;
    /// <summary>
    /// 依存先
    /// </summary>
    public Enemy parent;
    /// <summary>
    /// ヒットポイント
    /// </summary>
    public int hp;
    /// <summary>
    /// 最大ヒットポイント
    /// </summary>
    public int maxhp;
    /// <summary>
    /// ゲージの中心位置
    /// </summary>
    private float centerX;
    private float centerY;
    /// <summary>
    /// フェードインにかかる時間(ミリ秒)
    /// </summary>
    public int FadeMiliSec;
    /// <summary>
    /// フェードインにかかる時間(フレーム)
    /// </summary>
    private int FadeTime;
    /// <summary>
    /// フェード時の時間カウント(フレーム)
    /// </summary>
    private int time;
    /// <summary>
    /// 枠オブジェクト
    /// </summary>
    GameObject frame;
    /// <summary>
    /// 表示を開始する
    /// </summary>
    public void SetVisibility()
    {
        state = State.FadeIn;
        transform.position = new Vector2(parent.transform.position.x, parent.transform.position.y + parent.GetComponent<Enemy>().gaugeOffsetY);
        centerX = transform.position.x;
        centerY = transform.position.y;
        transform.localScale = new Vector2(scaleX, scaleY);
        frame = (GameObject)Resources.Load("GaugeFrame");
        frame = Instantiate(frame);
        frame.transform.position = transform.position;
        frame.transform.localScale = new Vector2(frameScaleX, frameScaleY);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        sr.color = new Color(col.r, col.g, col.b, 0);
    }
    /// <summary>
    /// 非表示にする
    /// </summary>
    public void Die()
    {
        state = State.FadeOut;
        time = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.Wait;

        time = 0;
        FadeTime = (int)(FadeMiliSec / func.FRAMETIME);
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        switch(state)
        {
        case State.Wait:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        case State.FadeIn:
            time++;
            sr.color = new Color(col.r, col.g, col.b, (float)time / FadeTime);
            Color framecol = frame.GetComponent<SpriteRenderer>().color;
            frame.GetComponent<SpriteRenderer>().color = new Color(framecol.r, framecol.g, framecol.b, (float)time / FadeTime);
            if(time == FadeTime)
            {
                state = State.Process;
            }
            break;
        case State.Process:
            sr.color = new Color(col.r, col.g, col.b, 1);
            break;
        case State.FadeOut:
            time++;
            sr.color = new Color(col.r, col.g, col.b, 1.0f - (float)time / FadeTime);
            framecol = frame.GetComponent<SpriteRenderer>().color;
            frame.GetComponent<SpriteRenderer>().color = new Color(framecol.r, framecol.g, framecol.b, 1.0f - (float)time / FadeTime);
            if(time == FadeTime)
            {
                Destroy(frame);
                Destroy(gameObject);
            }
            break;
        case State.Invalid:
            break;
        }
        hp = parent.hp;
        float posx = (float)hp / maxhp * scaleX / 2 + (centerX - scaleX / 2);
        float posy = centerY;
        float scalex = (float)hp / maxhp * scaleX;

        transform.position = new Vector2(posx, posy);
        transform.localScale = new Vector2(scalex, scaleY);
    }
}
