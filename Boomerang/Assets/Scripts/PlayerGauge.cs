using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using System;

/// <summary>
/// 自機のHPゲージ
/// </summary>
public class PlayerGauge : MonoBehaviour
{
    /// <summary>
    /// 状態一覧
    /// </summary>
    private enum State
    {
        /// <summary>表示中</summary>
        Process,
        /// <summary>減少中</summary>
        Decrease,
        /// <summary>増加中</summary>
        Increase,
    };
    /// <summary>
    /// 状態
    /// </summary>
    private State state;
    /// <summary>
    /// 当たり判定の半径
    /// </summary>
    static public float Collisionr = 0.1f;
    /// <summary>
    /// max時のx座標
    /// </summary>
    public const float DefaultX = 1.65f;
    /// <summary>
    /// y座標
    /// </summary>
    public const float DefaultY = -4.5f;
    /// <summary>
    /// 横幅
    /// </summary>
    private readonly float ScaleX = func.metrecalc(25);
    /// <summary>
    /// 縦幅
    /// </summary>
    private readonly float ScaleY = 0.25f;
    /// <summary>
    /// 枠の横幅
    /// </summary>
    private readonly float frameScaleX = func.metrecalc(25+2);
    /// <summary>
    /// 枠の縦幅
    /// </summary>
    private readonly float frameScaleY = 0.35f;
    /// <summary>
    /// ヒットポイント最大値
    /// </summary>
    private int maxHP = 100;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject player;
    /// <summary>
    /// ヒットポイント
    /// </summary>
    private int hp;
    /// <summary>
    /// ゲージ減少にかかる時間(フレーム)
    /// </summary>
    private const int DecTime = (int)(1000.0f / func.FRAMETIME);
    /// <summary>
    /// ゲージ表示中のHP数値
    /// </summary>
    private float dspHP;
    /// <summary>
    /// ゲージ減少前のHP数値
    /// </summary>
    private int dspMaxHP;
    /// <summary>
    /// 処理時間
    /// </summary>
    private int time;
    /// <summary>
    /// 減少線オブジェクト
    /// </summary>
    GameObject gaugeLine;

    /// <summary>
    /// 敵弾がヒットした場合、プレイヤーのHPを減らす
    /// </summary>
    /// <param name="atk">敵の攻撃力</param>
    public void Hit(int atk)
    {
        SetDecrease(atk);
        player.GetComponent<Player>().DamageFromEnemy(atk);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.EnemyAttackHit);
    }
    /// <summary>
    /// ゲージ減少状態にする
    /// </summary>
    /// <param name="damage">ダメージ値</param>
    public void SetDecrease(int damage)
    {
        if(gaugeLine != null)
        {
            Destroy(gaugeLine);
        }
        state = State.Decrease;
        dspMaxHP = hp;
        int dspMinHP = hp - damage;
        if(dspMinHP < 0)
        {
            dspMinHP = 0;
        }
        time = 0;

        gaugeLine = Instantiate((GameObject)Resources.Load("GaugeLine"));
        gaugeLine.transform.position = new Vector2((float)dspMinHP / maxHP * ScaleX + gaugeLine.transform.localScale.x / 2 + (DefaultX - ScaleX / 2), DefaultY);
    }
    /// <summary>
    /// ゲージ減少状態にする
    /// </summary>
    /// <param name="damage">ダメージ値</param>
    public void SetIncrease(int cure)
    {
        if(gaugeLine != null)
        {
            Destroy(gaugeLine);
        }
        state = State.Increase;
        dspMaxHP = hp;
        int dspMinHP = hp + cure;
        if(dspMinHP > maxHP)
        {
            dspMinHP = maxHP;
        }
        time = 0;

        gaugeLine = Instantiate((GameObject)Resources.Load("GaugeLine"));
        gaugeLine.transform.position = new Vector2((float)dspMinHP / maxHP * ScaleX - gaugeLine.transform.localScale.x / 2 + (DefaultX - ScaleX / 2), DefaultY);
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.Process;
        time = 0;

        transform.position = new Vector2(DefaultX, DefaultY);
        transform.localScale = new Vector2(ScaleX, ScaleY);
        player = GameObject.Find("Player");
        maxHP = player.GetComponent<Player>().MaxHP;

        GameObject frame = (GameObject)Resources.Load("GaugeFrame");
        frame = Instantiate(frame);
        frame.transform.position = transform.position;
        frame.transform.localScale = new Vector2(frameScaleX, frameScaleY);
    }

    // Update is called once per frame
    void Update()
    {
        //HP同期
        hp = player.GetComponent<Player>().GetHP();
        if(state == State.Process) dspHP = hp;

        switch(state)
        {
        case State.Process:
            break;
        case State.Decrease:
            time++;
            dspHP = hp + (float)(dspMaxHP - hp) / DecTime * (DecTime - time);
            if(time == DecTime)
            {
                Destroy(gaugeLine);
                state = State.Process;
            }
            break;
        case State.Increase:
            time++;
            dspHP = hp + (float)(hp - dspMaxHP) / DecTime * (DecTime - time);
            if(time == DecTime)
            {
                Destroy(gaugeLine);
                state = State.Process;
            }
            break;
        }

        float posx = dspHP / maxHP * ScaleX / 2 + (DefaultX - ScaleX / 2);
        float posy = DefaultY;
        float scalex = dspHP / maxHP * ScaleX;

        transform.position = new Vector2(posx, posy);
        transform.localScale = new Vector2(scalex, ScaleY);
    }
}
