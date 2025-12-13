using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UIElements;

/// <summary>
/// 敵行動ターン表示
/// </summary>
public class TurnCounter : MonoBehaviour
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
    /// ターンカウントの表示位置x座標
    /// </summary>
    private readonly float CountOffsetX = func.metrecalc(-7.0f);
    /// <summary>
    /// ターンカウントの表示位置y座標
    /// </summary>
    private readonly float CountOffsetY = func.metrecalc(8.0f);
    /// <summary>
    /// ターンカウントの表示位置x座標
    /// </summary>
    private readonly float BossCountOffsetX = func.metrecalc(-12.0f);
    /// <summary>
    /// ターンカウントの表示位置y座標
    /// </summary>
    private readonly float BossCountOffsetY = func.metrecalc(14.0f);
    /// <summary>
    /// 拡大率
    /// </summary>
    private const float DefaultScale = 0.5f;
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
    /// 追従先のエネミー
    /// </summary>
    public Enemy parent;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;

    /// <summary>
    /// 表示を開始する
    /// </summary>
    public void SetVisibility(bool boss)
    {
        state = State.FadeIn;
        sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        sr.color = new Color(col.r, col.g, col.b, 0);
        if(boss)
        {
            transform.position = new Vector2(parent.transform.position.x + BossCountOffsetX, parent.transform.position.y + BossCountOffsetY);
            transform.localScale = new Vector2(DefaultScale, DefaultScale);
        }
        else
        {
            transform.position = new Vector2(parent.transform.position.x + CountOffsetX, parent.transform.position.y + CountOffsetY);
            transform.localScale = new Vector2(DefaultScale, DefaultScale);
        }
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
        sr = GetComponent<SpriteRenderer>();
        FadeTime = (int)(FadeMiliSec / func.FRAMETIME);
    }

    // Update is called once per frame
    void Update()
    {
        Color col = sr.color;
        switch(state)
        {
        case State.Wait:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        case State.FadeIn:
            time++;
            sr.color = new Color(col.r, col.g, col.b, (float)time / FadeTime);
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
            if(time == FadeTime)
            {
                Destroy(gameObject);
            }
            break;
        case State.Invalid:
            break;
        }

        sr.sprite = Font.GetFont(parent.turnCount);
    }
}
