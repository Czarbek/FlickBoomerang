using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// ボスであるか
    /// </summary>
    public bool boss;
    /// <summary>
    /// ボスHPゲージの本数
    /// </summary>
    public int gaugeNum;
    /// <summary>
    /// 弱点属性へのダメージ倍率
    /// </summary>
    private const float WeakRate = 2;
    /// <summary>
    /// 耐性属性へのダメージ倍率
    /// </summary>
    private const float ResistRate = 1;
    /// <summary>
    /// 当たり判定の半径の基準値
    /// </summary>
    private const float CollisionRadius = 0.3f;
    /// <summary>
    /// 当たり判定の半径の倍率
    /// </summary>
    private readonly float[] CollisionRadiusRate = new float[4] { 1.0f, 1.6f, 2.0f, 3.0f };
    public const float ScreenOutY = func.camHeight*2 + 0.5f;
    /// <summary>
    /// 属性一覧
    /// </summary>
    public enum Element
    {
        /// <summary>炎</summary>
        Fire,
        /// <summary>水</summary>
        Aqua,
        /// <summary>草</summary>
        Leaf,
        /// <summary>なし</summary>
        None,
    };
    /// <summary>
    /// 登場フロア
    /// </summary>
    public int floor;
    /// <summary>
    /// 初期位置のx座標
    /// </summary>
    public float startX;
    /// <summary>
    /// 初期位置のy座標
    /// </summary>
    public float startY;
    /// <summary>
    /// HPゲージの表示位置
    /// </summary>
    public float gaugeOffsetY;
    /// <summary>
    /// ヒットポイント
    /// </summary>
    public int hp;
    /// <summary>
    /// 属性
    /// </summary>
    public Element element;
    /// <summary>
    /// 攻撃力
    /// </summary>
    public int atk;
    /// <summary>
    /// 攻撃までのターン
    /// </summary>
    public int maxCount;
    /// <summary>
    /// 行動ターン
    /// </summary>
    public int turnCount;
    /// <summary>
    /// サイズのパターン
    /// </summary>
    public int sizePattern;
    /// <summary>
    /// 当たり判定の半径
    /// </summary>
    private float collisionr;
    /// <summary>
    /// このターン攻撃を受けたか
    /// </summary>
    private bool hit;
    /// <summary>
    /// 行動終了したか
    /// </summary>
    private bool changeTurn;
    /// <summary>
    /// ターン行動を行ったか
    /// </summary>
    private bool turnProcess;
    /// <summary>
    /// 死亡処理中かどうか
    /// </summary>
    private bool dying;
    /// <summary>
    /// 生きているか
    /// </summary>
    private bool alive;
    /// <summary>
    /// バトルマネージャーオブジェクト
    /// </summary>
    GameObject manager;
    /// <summary>
    /// HPバー
    /// </summary>
    GameObject gauge;
    /// <summary>
    /// ターンカウンター
    /// </summary>
    GameObject turnCounter;
    /// <summary>
    /// このターン攻撃を受けたかどうかを判定する
    /// </summary>
    /// <returns>hit</returns>
    public bool isHit()
    {
        return hit;
    }
    /// <summary>
    /// 攻撃を受けたときの処理
    /// </summary>
    /// <param name="atk"></param>
    public void SetHit(int atk)
    {
        hp -= atk;
        if(hp<0) hp = 0;
        hit = true;
    }
    /// <summary>
    /// 当たり判定の半径を取得する
    /// </summary>
    /// <returns>当たり判定の半径</returns>
    public float GetCollision()
    {
        return collisionr;
    }
    /// <summary>
    /// 当たり判定の半径を計算する
    /// </summary>
    /// <param name="sizePattern">サイズのパターン</param>
    /// <returns>当たり判定の半径</returns>
    public float CalcCollision(int sizePattern)
    {
        return CollisionRadius * CollisionRadiusRate[sizePattern];
    }
    /// <summary>
    /// 属性相性からダメージ倍率を計算する
    /// </summary>
    /// <param name="attackers">攻撃側の属性</param>
    /// <param name="defenders">被撃側の属性</param>
    /// <returns>ダメージ倍率</returns>
    public float CalcDamageRate(Element attackers, Element defenders)
    {
        float result = 1;
        if(attackers == Element.None || defenders == Element.None)
        {
            result = 1;
        }
        else
        {
            switch(attackers)
            {
            case Element.Fire:
                if(defenders == Element.Aqua)
                {
                    result = ResistRate;
                }
                else if(defenders == Element.Leaf)
                {
                    result = WeakRate;
                }
                break;
            case Element.Aqua:
                if(defenders == Element.Fire)
                {
                    result = WeakRate;
                }
                else if(defenders == Element.Leaf)
                {
                    result = ResistRate;
                }
                break;
            case Element.Leaf:
                if(defenders == Element.Fire)
                {
                    result = ResistRate;
                }
                else if(defenders == Element.Aqua)
                {
                    result = WeakRate;
                }
                break;
            default:
                break;
            }
        }
        return result;
    }
    /// <summary>
    /// 敵ターン開始時の処理
    /// </summary>
    public void StartTurn()
    {
        if(!turnProcess)
        {
            hit = false;
            changeTurn = false;
            turnProcess = true;
            if(hp > 0)
            {
                turnCount--;
                if(turnCount == 0)
                {
                    GameObject bullet = (GameObject)Resources.Load("EnemyBullet");
                    bullet = Instantiate(bullet);
                    bullet.transform.position = this.transform.position;
                    bullet.GetComponent<EnemyBullet>().atk = atk;
                    bullet.GetComponent<EnemyBullet>().parent = this.gameObject;
                }
                else
                {
                    changeTurn = true;
                }
            }
            else
            {
                changeTurn = true;
            }
        }
    }
    /// <summary>
    /// 敵ターン終了時処理
    /// </summary>
    public void EndTurn()
    {
        changeTurn = false;
        turnProcess = false;
    }
    /// <summary>
    /// ターンカウントをリセットする
    /// </summary>
    public void ResetTurn()
    {
        turnCount = maxCount;
    }
    /// <summary>
    /// ターン終了の可否
    /// </summary>
    /// <returns>changeTurn</returns>
    public bool CanChangeTurn()
    {
        return changeTurn;
    }
    /// <summary>
    /// ターン終了を許可する
    /// </summary>
    public void SetChange()
    {
        changeTurn = true;
    }
    /// <summary>
    /// 死亡処理中かどうかを判定する
    /// </summary>
    /// <returns>処理中ならtrue</returns>
    public bool isDying()
    {
        return dying;
    }
    /// <summary>
    /// 生死判定
    /// </summary>
    /// <returns>生きているか</returns>
    public bool isAlive()
    {
        return alive;
    }
    /// <summary>
    /// 死亡処理を開始する
    /// </summary>
    public void Die()
    {
        Debug.Log("die");
        GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        for(int i = 1; i > -2; i -= 2)
        {
            GameObject mask = (GameObject)Resources.Load("EnemyMask");
            mask = Instantiate(mask);
            mask.transform.position = new Vector2(transform.position.x+i, transform.position.y);
            mask.transform.localScale = this.transform.localScale;
            mask.GetComponent<EnemyMask>().SetDirection(i);
            mask.GetComponent<EnemyMask>().parent = this.gameObject;
        }
        gauge.GetComponent<EnemyGauge>().Die();
        Destroy(turnCounter.gameObject);
    }
    /// <summary>
    /// 死亡判定を更新する
    /// </summary>
    public void DestroyThis()
    {
        alive = false;
        //Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;
        startY = transform.position.y;
        transform.position = new Vector2(startX, ScreenOutY);

        turnCount = maxCount;
        collisionr = CalcCollision(sizePattern);
        hit = false;
        changeTurn = false;
        turnProcess = false;
        dying = false;
        alive = true;

        manager = GameObject.Find("BattleManager");

        gauge = (GameObject)Resources.Load("EnemyGauge");
        gauge = Instantiate(gauge);
        gauge.transform.position = new Vector2(transform.position.x, transform.position.y + gaugeOffsetY);
        gauge.GetComponent<EnemyGauge>().parent = this;
        gauge.GetComponent<EnemyGauge>().hp = this.hp;
        gauge.GetComponent<EnemyGauge>().maxhp = this.hp;

        turnCounter = (GameObject)Resources.Load("TurnCounter");
        turnCounter = Instantiate(turnCounter);
        turnCounter.GetComponent<TurnCounter>().parent = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.GetComponent<BattleManager>().GetState() == BattleManager.State.StartWait)
        {
            int time = manager.GetComponent<BattleManager>().GetTime();
            if(time > 20 && time - 20 <= BattleManager.SlideTime)
            {
                float y = ScreenOutY - (ScreenOutY - startY) * (float)(time - 20) / BattleManager.SlideTime;
                transform.position = new Vector2(transform.position.x, y);
                if(time - 20 == BattleManager.SlideTime)
                {
                    gauge.GetComponent<EnemyGauge>().SetVisibility();
                }
            }
        }
    }
}
