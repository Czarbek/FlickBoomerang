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
    private int gaugeNum;
    /// <summary>
    /// 定常の拡大率
    /// </summary>
    private float standardScale;
    /// <summary>
    /// 弱点属性へのダメージ倍率
    /// </summary>
    private const float WeakRate = 2;
    /// <summary>
    /// 耐性属性へのダメージ倍率
    /// </summary>
    private const float ResistRate = 0.5f;
    /// <summary>
    /// 当たり判定の半径
    /// </summary>
    private readonly float[] CollisionRadius = new float[4] { func.metrecalc(15), func.metrecalc(10), func.metrecalc(8), func.metrecalc(5) };
    /// <summary>
    /// 画面外にいるときのy座標
    /// </summary>
    public const float ScreenOutY = func.camHeight*2 + 1.0f;
    private readonly int[] PixelSize = new int[4] { 360, 240, 192, 120 };
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
    /// ボス演出段階一覧
    /// </summary>
    public enum BossEffect
    {
        /// <summary>波紋</summary>
        Wave,
        /// <summary>ぼかし解除</summary>
        Blur,
        /// <summary>ゲージ出現</summary>
        Gauge,
        /// <summary>終了</summary>
        End,
        /// <summary>攻撃時縮小</summary>
        AttackShrink,
        /// <summary>攻撃時振動</summary>
        AttackOscillation,
        /// <summary>攻撃時拡縮</summary>
        AttackExpand,
        /// <summary>撤退前振動</summary>
        AscendOscillation,
        /// <summary>撤退前待機</summary>
        AscendWait,
        /// <summary>撤退　画面外退避</summary>
        Ascend,
        /// <summary>テキスト表示中</summary>
        TextWait,
        /// <summary>撃破演出</summary>
        Defeat,
        /// <summary>死亡</summary>
        Invalid,
    };
    /// <summary>
    /// ボス演出段階
    /// </summary>
    private BossEffect bossEffect;
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
    /// 死亡演出へ移行するかどうか
    /// </summary>
    private bool goDying;
    /// <summary>
    /// 死亡処理中かどうか
    /// </summary>
    private bool dying;
    /// <summary>
    /// 生きているか
    /// </summary>
    private bool alive;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    /// <summary>
    /// ボスの画像リスト
    /// </summary>
    private List<Sprite> spriteList;
    /// <summary>
    /// ボスの画像枚数
    /// </summary>
    private const int SpriteNum = 6;
    /// <summary>
    /// ボスの画像リストインデックス
    /// </summary>
    private int spriteIndex;
    /// <summary>
    /// 波紋の数
    /// </summary>
    public const int WaveNum = 3;
    /// <summary>
    /// 波紋出現から次の波紋までの時間
    /// </summary>
    private const int WaveGapTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// 段階ごとのぼかし解除の時間
    /// </summary>
    private const int BlurTime = (int)(500.0f / (SpriteNum-1) / func.FRAMETIME);
    /// <summary>
    /// ボスの撃破演出の時間
    /// </summary>
    private const int BossDefeatTime = (int)(2000.0f / func.FRAMETIME);
    /// <summary>
    /// ボス撃破時、下降開始までの時間
    /// </summary>
    private const int BossDescendTime = (int)(1000.0f / func.FRAMETIME);
    /// <summary>
    /// ボス撃破時の下降の深さ
    /// </summary>
    private const float BossDescendDepth = 0.4f;
    /// <summary>
    /// ボス撃破時の振動の振幅(片側)
    /// </summary>
    private const float BossOscillation = 0.3f;
    /// <summary>
    /// ボス撤退時の振動時間
    /// </summary>
    private const int BossOscillationTime = (int)(1000.0f / func.FRAMETIME);
    /// <summary>
    /// ボス撤退時の待機時間
    /// </summary>
    private const int BossWaitTime = (int)(200.0f / func.FRAMETIME);
    /// <summary>
    /// ボス撤退時の画面外退避の時間
    /// </summary>
    private const int BossAscendTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// ボス攻撃時の縮小時間
    /// </summary>
    private const int BossAttackShrinkTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// ボス攻撃時の待機時間
    /// </summary>
    private const int BossAttackWaitTime = (int)(300.0f / func.FRAMETIME);
    /// <summary>
    /// ボス攻撃時の振動時間(総計)
    /// </summary>
    private const int BossAttackOscillationTime = (int)(450.0f / func.FRAMETIME);
    /// <summary>
    /// ボス攻撃時の拡縮時間(拡大または縮小1回あたり)
    /// </summary>
    private const int BossAttackScaleTime = (int)(300.0f / func.FRAMETIME);
    /// <summary>
    /// ボス攻撃時の振動回数
    /// </summary>
    private const int BossAttackOscillationNum = 3;
    /// <summary>
    /// ボス攻撃時の拡縮回数(拡大縮小それぞれ)
    /// </summary>
    private const int BossAttackScaleNum = 3;
    /// <summary>
    /// ボス攻撃時、最初の縮小率
    /// </summary>
    private const float BossAttackFirstScale = 0.8f;
    /// <summary>
    /// ボス攻撃時の拡大率
    /// </summary>
    private const float BossAttackExpandScale = 1.4f;
    /// <summary>
    /// ボス演出時間
    /// </summary>
    private int time;
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
    /// ボスの攻撃演出を開始する
    /// </summary>
    public void StartAttack()
    {
        time = 0;
        bossEffect = BossEffect.AttackShrink;
    }
    /// <summary>
    /// このターン攻撃を受けたかどうかを判定する
    /// </summary>
    /// <returns>hit</returns>
    public bool IsHit()
    {
        return hit;
    }
    /// <summary>
    /// 攻撃を受けたときの処理
    /// </summary>
    /// <param name="atk"></param>
    public void SetHit(int atk, Element element)
    {
        int damage;
        float originalDamage = atk * CalcDamageRate(element, this.element);
        if(atk % 2 == 0)
        {
            damage = (int)originalDamage;
        }
        else
        {
            damage = (int)originalDamage + 1;
        }
        if(boss)
        {
            gauge.GetComponent<BossGauge>().SetDecrease(damage);
        }
        else
        {
            gauge.GetComponent<EnemyGauge>().SetDecrease(damage);
        }
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.BoomerangHit);
        hp -= damage;
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
                    if(boss)
                    {
                        time = 0;
                        bossEffect = BossEffect.AttackShrink;
                    }
                    else
                    {
                        GameObject bullet = (GameObject)Resources.Load("EnemyBullet");
                        bullet = Instantiate(bullet);
                        bullet.transform.position = this.transform.position;
                        bullet.GetComponent<EnemyBullet>().atk = atk;
                        bullet.GetComponent<EnemyBullet>().parent = this.gameObject;
                        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.EnemyAttack);
                    }
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
    public bool IsDying()
    {
        return goDying;
    }
    /// <summary>
    /// 生死判定
    /// </summary>
    /// <returns>生きているか</returns>
    public bool IsAlive()
    {
        return alive;
    }
    /// <summary>
    /// デバッグ用機能　死亡処理へ移行する
    /// </summary>
    public void SetDie_debug()
    {
        if(boss)
        {
            gauge.GetComponent<BossGauge>().Die();
        }
        else
        {
            gauge.GetComponent<EnemyGauge>().Die();
        }
        turnCounter.GetComponent<TurnCounter>().Die();
        Destroy(gameObject);
    }
    /// <summary>
    /// 死亡処理への移行をセットする
    /// </summary>
    public void SetDie()
    {
        goDying = true;
        if(boss)
        {
            time = 0;
            manager.GetComponent<BattleManager>().SetWait();
            if(manager.GetComponent<BattleManager>().GetLastFloor() == manager.GetComponent<BattleManager>().GetFloor())
            {
                bossEffect = BossEffect.Defeat;
            }
            else
            {
                bossEffect = BossEffect.AscendOscillation;
            }
        }
    }
    /// <summary>
    /// 死亡処理を開始する
    /// </summary>
    private void Die()
    {
        GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        for(int i = 1; i > -2; i -= 2)
        {
            int pixelSize = PixelSize[sizePattern];
            GameObject mask = (GameObject)Resources.Load("EnemyMask");
            mask = Instantiate(mask);
            mask.transform.position = new Vector2(transform.position.x + i * func.pxcalc((int)(pixelSize * transform.localScale.x)), transform.position.y);
            float maskScale = (float)pixelSize / 1024;
            mask.transform.localScale = new Vector2(maskScale, maskScale);
            mask.GetComponent<EnemyMask>().SetDirection(i);
            mask.GetComponent<EnemyMask>().parent = this.gameObject;
        }
        gauge.GetComponent<EnemyGauge>().Die();
        turnCounter.GetComponent<TurnCounter>().Die();
    }
    /// <summary>
    /// 死亡判定を更新する
    /// </summary>
    public void Inactivate()
    {
        alive = false;
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        manager.GetComponent<BattleManager>().AllowChangeFloor();
        if(boss)
        {
            manager.GetComponent<BattleManager>().EndWait();
        }
    }
    /// <summary>
    /// 波紋演出を終了する
    /// </summary>
    public void EndWave()
    {
        time = 0;
        bossEffect = BossEffect.Blur;
    }
    /// <summary>
    /// ゲージ演出を終了する
    /// </summary>
    public void EndGauge()
    {
        time = 0;
        bossEffect = BossEffect.End;
    }
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        startX = transform.position.x;
        startY = transform.position.y;
        transform.position = new Vector2(startX, ScreenOutY);

        turnCount = maxCount;
        collisionr = CollisionRadius[sizePattern];
        gaugeOffsetY = -(collisionr + func.metrecalc(3));
        hit = false;
        changeTurn = false;
        turnProcess = false;
        goDying = false;
        dying = false;
        alive = true;

        bossEffect = BossEffect.Wave;
        spriteIndex = 0;
        time = 0;

        switch(sizePattern)
        {
        case 0:
            spriteList = new List<Sprite>();
            if(boss)
            {
                if(element == Element.Fire)
                {
                    spriteList.Add(Resources.Load<Sprite>("BossFireBlur1"));
                    spriteList.Add(Resources.Load<Sprite>("BossFireBlur2"));
                    spriteList.Add(Resources.Load<Sprite>("BossFireBlur3"));
                    spriteList.Add(Resources.Load<Sprite>("BossFireBlur4"));
                    spriteList.Add(Resources.Load<Sprite>("BossFireBlur5"));
                    spriteList.Add(Resources.Load<Sprite>("boss_fire"));
                }
                else if(element == Element.Aqua)
                {
                    spriteList.Add(Resources.Load<Sprite>("BossAquaBlur1"));
                    spriteList.Add(Resources.Load<Sprite>("BossAquaBlur2"));
                    spriteList.Add(Resources.Load<Sprite>("BossAquaBlur3"));
                    spriteList.Add(Resources.Load<Sprite>("BossAquaBlur4"));
                    spriteList.Add(Resources.Load<Sprite>("BossAquaBlur5"));
                    spriteList.Add(Resources.Load<Sprite>("boss_aqua"));
                }
                else if(element == Element.Leaf)
                {
                    spriteList.Add(Resources.Load<Sprite>("BossLeafBlur1"));
                    spriteList.Add(Resources.Load<Sprite>("BossLeafBlur2"));
                    spriteList.Add(Resources.Load<Sprite>("BossLeafBlur3"));
                    spriteList.Add(Resources.Load<Sprite>("BossLeafBlur4"));
                    spriteList.Add(Resources.Load<Sprite>("BossLeafBlur5"));
                    spriteList.Add(Resources.Load<Sprite>("boss_leaf"));
                }
                sr.sprite = spriteList[spriteIndex];
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            }
            else
            {
                if(element == Element.Fire)
                {
                    sr.sprite = Resources.Load<Sprite>("eA_fire");
                }
                else if(element == Element.Aqua)
                {
                    sr.sprite = Resources.Load<Sprite>("eA_aqua");
                }
                else if(element == Element.Leaf)
                {
                    sr.sprite = Resources.Load<Sprite>("eA_leaf");
                }
            }
            transform.position = new Vector2(startX, startY);
            break;
        case 1:
            if(element == Element.Fire)
            {
                sr.sprite = Resources.Load<Sprite>("eB_fire");
            }
            else if(element == Element.Aqua)
            {
                sr.sprite = Resources.Load<Sprite>("eB_aqua");
            }
            else if(element == Element.Leaf)
            {
                sr.sprite = Resources.Load<Sprite>("eB_leaf");
            }
            break;
        case 2:
            if(element == Element.Fire)
            {
                sr.sprite = Resources.Load<Sprite>("eC_fire");
            }
            else if(element == Element.Aqua)
            {
                sr.sprite = Resources.Load<Sprite>("eC_aqua");
            }
            else if(element == Element.Leaf)
            {
                sr.sprite = Resources.Load<Sprite>("eC_leaf");
            }
            break;
        case 3:
            if(element == Element.Fire)
            {
                sr.sprite = Resources.Load<Sprite>("eD_fire");
            }
            else if(element == Element.Aqua)
            {
                sr.sprite = Resources.Load<Sprite>("eD_aqua");
            }
            else if(element == Element.Leaf)
            {
                sr.sprite = Resources.Load<Sprite>("eD_leaf");
            }
            break;
        default:
            break;
        }
        standardScale = 1;
        transform.localScale = new Vector3(standardScale, standardScale);

        manager = GameObject.Find("BattleManager");

        if(boss)
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.BossAppear);

            gauge = (GameObject)Resources.Load("BossGauge");
            gauge = Instantiate(gauge);
            gauge.transform.position = new Vector2(transform.position.x, transform.position.y);
            gauge.GetComponent<BossGauge>().parent = this;
            gauge.GetComponent<BossGauge>().hp = this.hp;
            gauge.GetComponent<BossGauge>().maxhp = this.hp;
            gauge.GetComponent<BossGauge>().SetGaugeNum(manager.GetComponent<BattleManager>().GetLastFloor() - manager.GetComponent<BattleManager>().GetFloor());
            transform.localScale = new Vector2(360.0f / 1024.0f, 360.0f / 1024.0f);
        }
        else
        {
            gauge = (GameObject)Resources.Load("EnemyGauge");
            gauge = Instantiate(gauge);
            gauge.transform.position = new Vector2(transform.position.x, transform.position.y + gaugeOffsetY);
            gauge.GetComponent<EnemyGauge>().parent = this;
            gauge.GetComponent<EnemyGauge>().hp = this.hp;
            gauge.GetComponent<EnemyGauge>().maxhp = this.hp;
        }

        turnCounter = (GameObject)Resources.Load("TurnCounter");
        turnCounter = Instantiate(turnCounter);
        turnCounter.GetComponent<TurnCounter>().parent = this;

        /*
        if(func.DEBUG)
        {
            GameObject cc = Instantiate((GameObject)Resources.Load("CollisionCircle"));
            cc.GetComponent<CollisionCircle>().Init(collisionr, gameObject);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        if(boss)
        {
            if(manager.GetComponent<BattleManager>().GetState() == BattleManager.State.BossAppear)
            {
                time++;
                switch(bossEffect)
                {
                case BossEffect.Wave:
                    if(time % WaveGapTime == 0 && time <= WaveGapTime * WaveNum)
                    {
                        GameObject wave = Instantiate((GameObject)Resources.Load("BossAppearEffect"));
                        wave.transform.position = transform.position;
                        wave.GetComponent<BossAppearEffect>().SetIndex((int)(time / WaveGapTime) - 1, gameObject);
                    }
                    break;
                case BossEffect.Blur:
                    if(time <= BlurTime && spriteIndex == 0)
                    {
                        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (float)time / BlurTime);
                    }
                    if(time == BlurTime)
                    {
                        time = 0;
                        spriteIndex++;
                        sr.sprite = spriteList[spriteIndex];
                        if(spriteIndex == SpriteNum - 1)
                        {
                            time = 0;
                            transform.localScale = new Vector2(1, 1);
                            gauge.GetComponent<BossGauge>().SetVisibility();
                            turnCounter.GetComponent<TurnCounter>().SetVisibility(boss);
                            bossEffect = BossEffect.Gauge;
                        }
                    }
                    break;
                case BossEffect.Gauge:
                    break;
                case BossEffect.End:
                    manager.GetComponent<BattleManager>().EndBossAppear();
                    break;
                }
            }
            else
            {
                switch(bossEffect)
                {
                case BossEffect.Defeat:
                    time++;
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f - (float)time / BossDefeatTime);
                    float descendY = 0.0f;
                    if(time > BossDescendTime)
                    {
                        descendY = BossDescendDepth * ((float)(time - BossDescendTime) / (BossDefeatTime - BossDescendTime));
                    }
                    transform.position = new Vector2(startX + func.sin(time * 36) * BossOscillation, startY - descendY);
                    if(time == BossDefeatTime)
                    {
                        gauge.GetComponent<BossGauge>().Die();
                        turnCounter.GetComponent<TurnCounter>().Die();
                        bossEffect = BossEffect.Invalid;
                    }
                    break;
                case BossEffect.AscendOscillation:
                    time++;
                    transform.position = new Vector2(startX + func.sin((float)time / BossOscillationTime * 360 * 3) * BossOscillation, startY);
                    if(time == BossOscillationTime)
                    {
                        time = 0;
                        bossEffect = BossEffect.AscendWait;
                    }
                    break;
                case BossEffect.AscendWait:
                    time++;
                    if(time == BossWaitTime)
                    {
                        time = 0;
                        bossEffect = BossEffect.Ascend;
                    }
                    break;
                case BossEffect.Ascend:
                    time++;
                    transform.position = new Vector2(startX, startY + (ScreenOutY - startY) * time / BossAscendTime);
                    if(time == BossAscendTime)
                    {
                        time = 0;
                        gauge.GetComponent<BossGauge>().Die();
                        turnCounter.GetComponent<TurnCounter>().Die();
                        GameObject.Find("AscendTx").GetComponent<AscendTx>().SetText();
                        bossEffect = BossEffect.TextWait;
                    }
                    break;
                case BossEffect.TextWait:
                    time++;
                    if(GameObject.Find("AscendTx").GetComponent<AscendTx>().EndText())
                    {
                        //manager.GetComponent<BattleManager>().EndWait();
                        Inactivate();
                        bossEffect = BossEffect.Invalid;
                    }
                    break;
                case BossEffect.AttackShrink:
                    time++;
                    float scale = 1.0f - (1.0f - BossAttackFirstScale) * (float)time / BossAttackShrinkTime;
                    if(time >= BossAttackShrinkTime)
                    {
                        scale = BossAttackFirstScale;
                    }
                    transform.localScale = new Vector2(standardScale * scale, standardScale * scale);
                    if(time == BossAttackShrinkTime + BossAttackWaitTime)
                    {
                        time = 0;
                        bossEffect = BossEffect.AttackOscillation;
                    }
                    break;
                case BossEffect.AttackOscillation:
                    time++;
                    transform.position = new Vector2(startX + func.sin((float)time / BossAttackOscillationTime * 360 * BossAttackOscillationNum) * BossOscillation, startY);
                    if(time == BossAttackOscillationTime)
                    {
                        time = 0;
                        bossEffect = BossEffect.AttackExpand;
                    }
                    break;
                case BossEffect.AttackExpand:
                    time++;
                    int currentTime = time % (BossAttackScaleTime*2);
                    float scale_ = 1.0f;
                    if(time < BossAttackScaleTime)
                    {
                        scale_ = BossAttackFirstScale + (BossAttackExpandScale - BossAttackFirstScale) * (float)currentTime / BossAttackScaleTime;
                    }
                    else if(time < BossAttackScaleTime * BossAttackScaleNum * 2)
                    {
                        if(currentTime < BossAttackScaleTime)
                        {
                            scale_ = 1.0f + (BossAttackExpandScale - 1.0f) * (float)currentTime / BossAttackScaleTime;
                        }
                        else
                        {
                            scale_ = BossAttackExpandScale - (BossAttackExpandScale - 1.0f) * (float)(currentTime-BossAttackScaleTime) / BossAttackScaleTime;
                        }
                    }
                    else
                    {
                        scale_ = 1.0f;
                        if(time == BossAttackScaleTime * BossAttackScaleNum * 2 + BossAttackWaitTime)
                        {
                            time = 0;
                            GameObject bullet = (GameObject)Resources.Load("EnemyBullet");
                            bullet = Instantiate(bullet);
                            bullet.transform.position = this.transform.position;
                            bullet.GetComponent<EnemyBullet>().atk = atk;
                            bullet.GetComponent<EnemyBullet>().parent = this.gameObject;
                            if(element == Element.Fire)
                            {
                                GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.BossAttackFire);
                            }
                            else if(element == Element.Aqua)
                            {
                                GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.BossAttackAqua);
                            }
                            else if(element == Element.Leaf)
                            {
                                GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.BossAttackLeaf);
                            }
                            bossEffect = BossEffect.End;
                        }
                    }
                    transform.localScale = new Vector2(standardScale * scale_, standardScale * scale_);
                    break;
                default:
                    break;
                }
            }
        }
        else
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
                        turnCounter.GetComponent<TurnCounter>().SetVisibility(boss);
                    }
                }
            }
            if(goDying && !dying && gauge != null)
            {
                if(gauge.GetComponent<EnemyGauge>().IsProcessing())
                {
                    Die();
                }
            }
        }
    }
}
