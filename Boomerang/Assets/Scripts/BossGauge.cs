using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスのHPゲージ
/// </summary>
public class BossGauge : MonoBehaviour
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
    };
    /// <summary>
    /// 状態
    /// </summary>
    private State state;
    /// <summary>
    /// 横幅
    /// </summary>
    public float MaxScaleX = 4.2f;
    /// <summary>
    /// 縦幅
    /// </summary>
    public float MaxScaleY = 0.3f;
    /// <summary>
    /// 枠の横幅
    /// </summary>
    public float frameScaleX = 4.6f;
    /// <summary>
    /// 枠の縦幅
    /// </summary>
    public float frameScaleY = 0.6f;
    /// <summary>
    /// 各色の値
    /// </summary>
    public float BlueColr;
    public float BlueColg;
    public float BlueColb;
    public float GreenColr;
    public float GreenColg;
    public float GreenColb;
    public float YellowColr;
    public float YellowColg;
    public float YellowColb;
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
    /// ゲージ幅
    /// </summary>
    private float scaleX;
    private float scaleY;
    /// <summary>
    /// 回転角度
    /// </summary>
    private float angle;
    /// <summary>
    /// ゲージの本数
    /// </summary>
    private int gaugeNum;
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
    /// ゲージの本数を設定する
    /// </summary>
    /// <param name="num">本数</param>
    public void SetGaugeNum(int num)
    {
        gaugeNum = num;
    }
    /// <summary>
    /// ゲージの表示を開始する
    /// </summary>
    public void SetVisibility()
    {
        state = State.FadeIn;
        transform.position = new Vector2(parent.transform.position.x, parent.transform.position.y);
        centerX = transform.position.x;
        centerY = transform.position.y;
        scaleX = 0;
        scaleY = 0;
        angle = 0;
        transform.localScale = new Vector2(scaleX, scaleY);
        frame = (GameObject)Resources.Load("GaugeFrame");
        frame = Instantiate(frame);
        frame.transform.position = transform.position;
        frame.transform.localScale = new Vector2(0, 0);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        sr.color = new Color(col.r, col.g, col.b, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.Wait;

        time = 0;
        FadeTime = (int)(FadeMiliSec / func.FRAMETIME);
        Debug.Log(FadeTime);
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
            angle = (float)time / FadeTime * 360.0f;
            sr.color = new Color(col.r, col.g, col.b, (float)time / FadeTime);
            Color framecol = frame.GetComponent<SpriteRenderer>().color;
            frame.GetComponent<SpriteRenderer>().color = new Color(framecol.r, framecol.g, framecol.b, (float)time / FadeTime);
            if(time == FadeTime)
            {
                scaleX = MaxScaleX;
                scaleY = MaxScaleY;
                angle = 0;
                state = State.Process;
            }
            break;
        case State.Process:
            sr.color = new Color(col.r, col.g, col.b, 1);
            break;
        }
        hp = parent.hp - maxhp * (gaugeNum - 1);
        while(hp < 0)
        {
            gaugeNum--;
            if(gaugeNum < 0)
            {
                hp = 0;
                break;
            }
            hp = parent.hp - maxhp * (gaugeNum - 1);
        }
        float posx = (float)hp / maxhp * MaxScaleX / 2 + (centerX - MaxScaleX / 2);
        float posy = centerY;
        float scalex = (float)hp / maxhp * MaxScaleX;

        transform.position = new Vector2(posx, posy);
        transform.localScale = new Vector2(scalex, scaleY);
    }
}
