using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム
/// </summary>
public class Item : MonoBehaviour
{
    public enum State
    {
        /// <summary>フェードイン</summary>
        FadeIn,
        /// <summary>表示中</summary>
        Process,
        /// <summary>フェードアウト</summary>
        FadeOut,
        /// <summary>非表示</summary>
        Invalid,
    }
    /// <summary>
    /// アイテムの種類一覧
    /// </summary>
    public enum ItemSort
    {
        /// <summary>リング</summary>
        Ring,
        /// <summary>クリスタル</summary>
        Crystal,
        /// <summary>黄金の果実</summary>
        Fruit,
    };
    /// <summary>
    /// 状態
    /// </summary>
    private State state;
    /// <summary>
    /// アイテムの種類
    /// </summary>
    public ItemSort sort;
    /// <summary>
    /// リングの画像のピクセル単位の縦幅
    /// </summary>
    private const int RingPx = 264;
    /// <summary>
    /// クリスタルのの画像のピクセル単位の縦幅
    /// </summary>
    private const int CrystalPx = 112;
    /// <summary>
    /// 黄金の果実の画像のピクセル単位の縦幅
    /// </summary>
    private const int FruitPx = 122;
    /// <summary>
    /// x座標
    /// </summary>
    public float x;
    /// <summary>
    /// y座標
    /// </summary>
    public float y;
    /// <summary>
    /// 当たり判定の半径
    /// </summary>
    public float CollisionRadius;
    /// <summary>
    /// サイズ
    /// </summary>
    public int sizePattern;
    /// <summary>
    /// パワーの最大値
    /// </summary>
    private const int MaxPower = 10;
    /// <summary>
    /// 初期時点でのパワー
    /// </summary>
    public int InitialPower;
    /// <summary>
    /// 現在時点でのパワー
    /// </summary>
    private int power;
    /// <summary>
    /// クリスタルの属性
    /// </summary>
    public Enemy.Element element;
    /// <summary>
    /// 黄金の果実のHP回復割合
    /// </summary>
    public float cureRate;
    /// <summary>
    /// 経過ターン
    /// </summary>
    public int turnCount;
    /// <summary>
    /// 復活にかかるターン
    /// </summary>
    private int validationTurn;
    /// <summary>
    /// リング復活にかかるターン
    /// </summary>
    private const int RingValidationTurn = 1;
    /// <summary>
    /// クリスタル復活にかかるターン
    /// </summary>
    private const int CrystalValidationTurn = 4;
    /// <summary>
    /// 黄金の果実復活にかかるターン
    /// </summary>
    private const int FruitValidationTurn = 3;
    /// <summary>
    /// クリスタル効果持続ターン
    /// </summary>
    private const int CrystalSustainTurn = 2;
    /// <summary>
    /// 黄金の果実復活回数
    /// </summary>
    private const int FruitValidationNum = 2;
    /// <summary>
    /// フェードインにかかる時間(フレーム)
    /// </summary>
    private const int FadeTime = (int)(300.0f / func.FRAMETIME);
    /// <summary>
    /// フェード時の時間カウント(フレーム)
    /// </summary>
    private int time;
    /// <summary>
    /// 復活回数
    /// </summary>
    private int validationNum;
    /// <summary>
    /// <summary>
    /// 取得可能かどうか
    /// </summary>
    public bool valid;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    /// <summary>
    /// プレイヤーにヒットしたときの処理
    /// </summary>
    public void Hit()
    {
        switch(sort)
        {
        case ItemSort.Ring:
            GameObject.Find("Player").GetComponent<Player>().AddPower(power);
            RingEffect.SetDsp();
            Instantiate((GameObject)Resources.Load("ArrowEffect"));
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.Ring);
            power = 0;
            break;
        case ItemSort.Crystal:
            GameObject.Find("Player").GetComponent<Player>().SetElement(element);
            GameObject.Find("Player").GetComponent<Player>().SetElementTurn(CrystalSustainTurn);
            ElementEffect.SetElement(element);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.Crystal);
            break;
        case ItemSort.Fruit:
            GameObject.Find("Player").GetComponent<Player>().HPCure(cureRate);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.Fruit);
            break;
        }
        turnCount = 0;
        valid = false;
        state = State.FadeOut;
    }
    /// <summary>
    /// 敵ターン終了時処理
    /// </summary>
    public void EndTurn()
    {
        if(valid)
        {
            if(sort == ItemSort.Ring)
            {
                if(power < MaxPower)
                {
                    power++;
                    sr.sprite = Initializer.GetRingImg(power);
                }
            }
        }
        else
        {

            if(sort == ItemSort.Fruit && validationNum == FruitValidationNum)
            {
            }
            else
            {

                turnCount++;
                if(turnCount == validationTurn)
                {
                    power = 1;
                    if(sort == ItemSort.Ring)
                    {
                        sr.sprite = Initializer.GetRingImg(power);
                    }
                    turnCount = 0;
                    validationNum++;
                    valid = true;
                    state = State.FadeIn;
                }
            }
        }
    }
    /// <summary>
    /// 当たり判定の半径を取得する
    /// </summary>
    /// <returns>当たり判定の半径</returns>
    public float GetCollision()
    {
        return CollisionRadius;
    }
    /// <summary>
    /// 有効かどうかを判定する
    /// </summary>
    /// <returns>有効ならtrue</returns>
    public bool IsValid()
    {
        return valid;
    }
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        power = InitialPower;
        turnCount = 0;
        validationNum = 0;
        switch(sort)
        {
        case ItemSort.Ring:
            validationTurn = RingValidationTurn;
            if(sizePattern == 1)
            {
                CollisionRadius = func.metrecalc(10);
                transform.localScale = func.scalecalc(10, RingPx);
            }
            else if(sizePattern == 2)
            {
                CollisionRadius = func.metrecalc(8);
                transform.localScale = func.scalecalc(8, RingPx);
            }
            else if(sizePattern == 3)
            {
                CollisionRadius = func.metrecalc(5);
                transform.localScale = func.scalecalc(5, RingPx);
            }
            sr.sprite = Initializer.GetRingImg(power);
            break;
        case ItemSort.Crystal:
            validationTurn = CrystalValidationTurn;
            CollisionRadius = func.metrecalc(5);
            transform.localScale = func.scalecalc(5, CrystalPx);
            if(element == Enemy.Element.Fire)
            {
                sr.sprite = Resources.Load<Sprite>("Crystal_fire");
            }
            else if(element == Enemy.Element.Aqua)
            {
                sr.sprite = Resources.Load<Sprite>("Crystal_aqua");
            }
            else if(element == Enemy.Element.Leaf)
            {
                sr.sprite = Resources.Load<Sprite>("Crystal_leaf");
            }
            break;
        case ItemSort.Fruit:
            validationTurn = FruitValidationTurn;
            CollisionRadius = func.metrecalc(5);
            transform.localScale = func.scalecalc(5, FruitPx);
            sr.sprite = Resources.Load<Sprite>("Fruit");
            break;
        }
        time = 0;
        valid = true;

        /*
        if(func.DEBUG)
        {
            GameObject cc = Instantiate((GameObject)Resources.Load("CollisionCircle"));
            cc.GetComponent<CollisionCircle>().Init(CollisionRadius, this.gameObject);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
        case State.FadeIn:
            time++;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (float)time / FadeTime);
            if(time == FadeTime)
            {
                state = State.Process;
                time = 0;
            }
            break;
        case State.Process:
            break;
        case State.FadeOut:
            time++;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f - (float)time / FadeTime);
            if(time == FadeTime)
            {
                state = State.Invalid;
                time = 0;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            }
            break;
        case State.Invalid:
            break;
        }
    }
}
