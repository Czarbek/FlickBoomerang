using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float ScaleX = 1.6f;
    /// <summary>
    /// 縦幅
    /// </summary>
    public float ScaleY = 0.15f;
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
    /// フェードにかかる時間(ミリ秒)
    /// </summary>
    public int FadeMiliSec;
    /// <summary>
    /// フェードにかかる時間(フレーム)
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
    /// 属性表示オブジェクト
    /// </summary>
    GameObject elem;
    /// <summary>
    /// 減少線オブジェクト
    /// </summary>
    GameObject gaugeLine;
    /// <summary>
    /// 表示を開始する
    /// </summary>
    public void SetVisibility()
    {
        state = State.FadeIn;
        transform.position = new Vector2(parent.transform.position.x, parent.transform.position.y + parent.GetComponent<Enemy>().gaugeOffsetY);
        centerX = transform.position.x;
        centerY = transform.position.y;
        transform.localScale = new Vector2(ScaleX, ScaleY);
        frame = (GameObject)Resources.Load("GaugeFrame");
        frame = Instantiate(frame);
        frame.transform.position = transform.position;
        frame.transform.localScale = new Vector2(frameScaleX, frameScaleY);
        elem = (GameObject)Resources.Load("ElementDsp");
        elem = Instantiate(elem);
        elem.transform.position = new Vector2(centerX - frameScaleX / 2 - func.metrecalc(ElementDsp.MetreSize), centerY);
        elem.GetComponent<ElementDsp>().SetElement(parent.element);
        elem.GetComponent<ElementDsp>().SetExpand(1);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        sr.color = new Color(col.r, col.g, col.b, 0);
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
        gaugeLine.transform.position = new Vector2((float)dspMinHP / maxhp * ScaleX + (centerX - ScaleX / 2), centerY);
    }
    /// <summary>
    /// 非表示にする
    /// </summary>
    public void Die()
    {
        state = State.FadeOut;
        elem.GetComponent<ElementDsp>().Die();
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
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;

        //HP同期
        hp = parent.hp;
        if(state != State.Decrease) dspHP = hp;

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
                Destroy(frame);
                Destroy(gameObject);
            }
            break;
        case State.Invalid:
            break;
        }

        float posx = dspHP / maxhp * ScaleX / 2 + (centerX - ScaleX / 2);
        float posy = centerY;
        float scalex = dspHP / maxhp * ScaleX;

        transform.position = new Vector2(posx, posy);
        transform.localScale = new Vector2(scalex, ScaleY);
    }
}
