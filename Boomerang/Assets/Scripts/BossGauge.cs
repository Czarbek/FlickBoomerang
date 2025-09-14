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
        /// <summary>減少中</summary>
        Decrease,
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
    public float MaxScaleX = func.metrecalc(70);
    /// <summary>
    /// 縦幅
    /// </summary>
    public float MaxScaleY = 0.3f;
    /// <summary>
    /// 枠の横幅
    /// </summary>
    public float FrameScaleX = func.metrecalc(74);
    /// <summary>
    /// 枠の縦幅
    /// </summary>
    public float FrameScaleY = 0.5f;
    /// <summary>
    /// ゲージの表示位置
    /// </summary>
    private readonly float CenterX = 0;
    private readonly float CenterY = func.metrecalc(142-70);
    /// <summary>
    /// 各色の値
    /// </summary>
    public int BlueColr;
    public int BlueColg;
    public int BlueColb;
    public int GreenColr;
    public int GreenColg;
    public int GreenColb;
    public int YellowColr;
    public int YellowColg;
    public int YellowColb;
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
    /// ゲージの生成位置
    /// </summary>
    private float StartX;
    private float StartY;
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
    /// ゲージの段階
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
    /// ゲージ減少にかかる時間(ミリ秒)
    /// </summary>
    public int DecMiliSec;
    /// <summary>
    /// ゲージ減少にかかる時間(フレーム)
    /// </summary>
    private int DecTime;
    /// <summary>
    /// ゲージ表示中のHP数値
    /// </summary>
    private float dspHP;
    /// <summary>
    /// ゲージ減少前のHP数値
    /// </summary>
    private int dspMaxHP;
    /// <summary>
    /// 枠オブジェクト
    /// </summary>
    GameObject frame;
    /// <summary>
    /// 次段階のゲージ
    /// </summary>
    GameObject underGauge;
    /// <summary>
    /// 減少線オブジェクト
    /// </summary>
    GameObject gaugeLine;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    SpriteRenderer sr;
    /// <summary>
    /// ゲージの段階を設定する
    /// </summary>
    /// <param name="num">段階</param>
    public void SetGaugeNum(int num)
    {
        gaugeNum = num;
        sr = GetComponent<SpriteRenderer>();
        switch(gaugeNum)
        {
        case 2:
            sr.color = new Color((float)BlueColr / 255, (float)BlueColg / 255, (float)BlueColb / 255, 0);
            break;
        case 1:
            sr.color = new Color((float)GreenColr / 255, (float)GreenColg / 255, (float)GreenColb / 255, 0);
            break;
        case 0:
            sr.color = new Color((float)YellowColr / 255, (float)YellowColg / 255, (float)YellowColb / 255, 0);
            break;
        default:
            break;
        }
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
        StartX = transform.position.x;
        StartY = transform.position.y;
        scaleX = 0;
        scaleY = 0;
        angle = 0;
        time = 0;
        transform.localScale = new Vector2(scaleX, scaleY);
        frame = (GameObject)Resources.Load("GaugeFrame");
        frame = Instantiate(frame);
        frame.transform.position = transform.position;
        frame.transform.localScale = new Vector2(0, 0);
    }
    /// <summary>
    /// ゲージ減少状態にする
    /// </summary>
    /// <param name="damage">ダメージ値</param>
    public void SetDecrease(int damage)
    {
        state = State.Decrease;
        dspMaxHP = hp;
        int dspMinHP = hp - damage;
        if(dspMinHP < 0)
        {
            dspMinHP = 0;
        }
        time = 0;

        gaugeLine = Instantiate((GameObject)Resources.Load("GaugeLine"));
        gaugeLine.transform.position = new Vector2((float)dspMinHP / maxhp * MaxScaleX + (CenterX - MaxScaleX / 2), CenterY);
    }
    /// <summary>
    /// 非表示にする
    /// </summary>
    public void Die()
    {
        state = State.FadeOut;
        time = 0;
    }
    /// <summary>
    /// 演出中でないかを判定する
    /// </summary>
    /// <returns>演出中でないならtrue</returns>
    public bool IsProcessing()
    {
        return state == State.Process;
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.Wait;

        time = 0;
        FadeTime = (int)(FadeMiliSec / func.FRAMETIME);
        DecTime = (int)(DecMiliSec / func.FRAMETIME);
    }

    // Update is called once per frame
    void Update()
    {
        //HP同期
        hp = parent.hp;
        if(state != State.Decrease) dspHP = hp;

        Color col = sr.color;
        switch(state)
        {
        case State.Wait:
            break;
        case State.FadeIn:
            time++;
            angle = (float)time / FadeTime * 360.0f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            frame.transform.rotation = Quaternion.Euler(0, 0, angle);
            sr.color = new Color(col.r, col.g, col.b, ((float)time) / FadeTime);
            Color framecol = frame.GetComponent<SpriteRenderer>().color;
            frame.GetComponent<SpriteRenderer>().color = new Color(framecol.r, framecol.g, framecol.b, (float)time / FadeTime);
            centerX = StartX + (CenterX - StartX) * time / FadeTime;
            centerY = StartY + (CenterY - StartY) * time / FadeTime;
            transform.position = new Vector2(centerX, centerY);
            frame.transform.position = new Vector2(centerX, centerY);
            float generalScale = (float)time / FadeTime;
            scaleX = generalScale * MaxScaleX;
            scaleY = generalScale * MaxScaleY;
            transform.localScale = new Vector2(scaleX, scaleY);
            float frameScaleX = generalScale * FrameScaleX;
            float frameScaleY = generalScale * FrameScaleY;
            frame.transform.localScale = new Vector2(frameScaleX, frameScaleY);
            if(time == FadeTime)
            {
                angle = 0;
                transform.eulerAngles = new Vector3(angle, 0, 0);
                frame.transform.eulerAngles = new Vector3(angle, 0, 0);
                parent.GetComponent<Enemy>().EndGauge();
                state = State.Process;
                sr.color = new Color(col.r, col.g, col.b, 1);

                if(gaugeNum > 0)
                {
                    underGauge = Instantiate((GameObject)Resources.Load("UnderGauge"));
                    underGauge.transform.position = transform.position;
                    underGauge.transform.localScale = transform.localScale;
                }
                switch(gaugeNum)
                {
                case 2:
                    underGauge.GetComponent<SpriteRenderer>().color = new Color(GreenColr, GreenColg, GreenColb);
                    break;
                case 1:
                    underGauge.GetComponent<SpriteRenderer>().color = new Color(YellowColr, YellowColg, YellowColb);
                    break;
                default:
                    break;
                }
            }
            break;
        case State.Process:
            break;
        case State.Decrease:
            dspHP = hp + (float)(dspMaxHP - hp) / DecTime * (DecTime - time);
            time++;
            if(time == DecTime)
            {
                Destroy(gaugeLine);
                state = State.Process;
            }
            break;
        case State.FadeOut:
            time++;
            sr.color = new Color(col.r, col.g, col.b, 1.0f - (float)time / FadeTime);
            framecol = frame.GetComponent<SpriteRenderer>().color;
            frame.GetComponent<SpriteRenderer>().color = new Color(framecol.r, framecol.g, framecol.b, 1.0f - (float)time / FadeTime);
            if(time == FadeTime)
            {
                Debug.Log("boss defeated");
                parent.GetComponent<Enemy>().Inactivate();
                Destroy(frame);
                Destroy(gameObject);
            }
            break;
        case State.Invalid:
            break;
        }
        if(state == State.Process || state == State.Decrease)
        {
            float posx = (float)(dspHP / maxhp) * MaxScaleX / 2.0f + (CenterX - MaxScaleX / 2.0f);
            float posy = centerY;
            float scalex = (float)(dspHP / maxhp) * MaxScaleX;

            transform.position = new Vector2(posx, posy);
            transform.localScale = new Vector2(scalex, MaxScaleY);
        }
    }
}
