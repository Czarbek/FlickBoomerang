using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ブーメラン
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの初期x座標
    /// </summary>
    public const float StartX = 0;
    /// <summary>
    /// プレイヤーの初期y座標
    /// </summary>
    private static float StartY = -func.camHeight * 2 + func.metrecalc(10);
    /// <summary>
    /// 待機状態のz軸回転
    /// </summary>
    private const float baseAngle = 0;
    /// <summary>
    /// プレイヤー移動の初速度
    /// </summary>
    public const float InitialSpeed = 1.0f;
    /// <summary>
    /// パワーの初期値
    /// </summary>
    public const int InitialPower = 1;
    /// <summary>
    /// 角度のパターン数
    /// </summary>
    public const int WidthPatternNum = 4;
    /// <summary>
    /// 飛距離のパターン数
    /// </summary>
    public const int HeightPatternNum = 3;
    /// <summary>
    /// 飛行時間
    /// </summary>
    public int FlyingTimeMax = 60;
    /// <summary>
    /// 飛行中の回転回数
    /// </summary>
    public int FlyingRotationNum = 3;
    /// <summary>
    /// パターンごとの楕円軌道の横径リスト
    /// </summary>
    public readonly float[] HorizontalRadius = new float[WidthPatternNum] {  func.metrecalc(80), func.metrecalc(60), func.metrecalc(40), func.metrecalc(20) };
    /// <summary>
    /// パターンごとの楕円軌道の縦径リスト
    /// </summary>
    public readonly float[] VerticalRadius = new float[HeightPatternNum] { func.metrecalc(70), func.metrecalc(110), func.metrecalc(140) };
    /// <summary>
    /// 楕円軌道の中心x座標
    /// </summary>
    public const float OvalCenterX = 0.0f;
    /// <summary>
    /// 楕円軌道の中心y座標
    /// </summary>
    public readonly float[] OvalCenterY = new float[HeightPatternNum] { StartY+ func.metrecalc(40), StartY+ func.metrecalc(60), StartY+ func.metrecalc(80) };
    /// <summary>
    /// フリック角度のパターンリスト
    /// </summary>
    public readonly float[] AnglePattern = new float[WidthPatternNum + 1] { 0.0f, 22.5f, 45.0f, 67.5f, 90.0f };
    /// <summary>
    /// フリック距離のパターンリスト
    /// </summary>
    public readonly float[] FlickDistance = new float[HeightPatternNum] { func.metrecalc(40), func.metrecalc(80), func.metrecalc(120) };
    /// <summary>
    /// 当たり判定の半径
    /// </summary>
    public float Collisionr = func.metrecalc(5);
    /// <summary>
    /// プレイヤーの状態一覧
    /// </summary>
    public enum State
    {
        Wait, //入力待機
        Touched, //タッチ中
        Flying, //飛行中
        NoInput, //演出待ち
    };
    /// <summary>
    /// プレイヤーの状態
    /// </summary>
    private State state;
    /// <summary>
    /// x座標
    /// </summary>
    private float posx;
    /// <summary>
    /// y座標
    /// </summary>
    private float posy;
    /// <summary>
    /// 飛行速度
    /// </summary>
    private float speed;
    /// <summary>
    /// タッチした点のx座標
    /// </summary>
    private float touchedx;
    /// <summary>
    /// タッチした点のy座標
    /// </summary>
    private float touchedy;
    /// <summary>
    /// タッチしてから離されるまでの時間(フレーム)
    /// </summary>
    private int flickTime;
    /// <summary>
    /// 離された点のx座標
    /// </summary>
    private float releasedx;
    /// <summary>
    /// 離された点のy座標
    /// </summary>
    private float releasedy;
    /// <summary>
    /// 飛行中の経過時間(フレーム)
    /// </summary>
    private int flyingTime;
    /// <summary>
    /// 楕円軌道のパターン
    /// </summary>
    private Vector3 orbit;
    /// <summary>
    /// BattleManager
    /// </summary>
    private GameObject bm;
    /// <summary>
    /// ヒットポイント最大値
    /// </summary>
    public int MaxHP;
    /// <summary>
    /// ヒットポイント
    /// </summary>
    private int hp;
    /// <summary>
    /// パワー
    /// </summary>
    public int power;
    /// <summary>
    /// 属性
    /// </summary>
    private Enemy.Element element;
    
    /// <summary>
    /// 楕円軌道のパターンを求める
    /// </summary>
    /// <param name="angle">フリック角度</param>
    /// <param name="distance">フリック距離</param>
    /// <param name="speed">フリック速度</param>
    /// <returns>楕円軌道のパターン(横幅パターン, 縦幅パターン, 反転の有無)</returns>
    private Vector3 orbitPattern(float angle, float distance, float speed)
    {
        int resultV = 0;
        int resultH = 0;
        int flip = 1;
        float judgeAngle = angle;
        if(angle > func.rad(90)) {
            judgeAngle = Mathf.PI - angle;
            flip = -1;
        }
        for(int i = WidthPatternNum; i > 0; i--)
        {
            if(judgeAngle < func.rad(AnglePattern[i]))
            {
                resultH = i-1;
            }
        }
        for(int i = 0; i < HeightPatternNum; i++)
        {
            if(distance >= FlickDistance[i])
            {
                resultV = i;
            }
        }
        return new Vector3(resultH, resultV, flip);
    }
    /// <summary>
    /// 楕円の方程式に基づき、2D座標を計算する
    /// </summary>
    /// <param name="orbitPattern">楕円軌道のパターン</param>
    /// <param name="time">経過時間</param>
    /// <returns>2D座標</returns>
    private Vector2 calcOrbit(Vector3 orbitPattern, int time)
    {
        int orbitX = (int)orbitPattern.x;
        int orbitY = (int)orbitPattern.y;
        float resultx = 0;
        float resulty = 0;

        float angle = (360.0f * (float)time / (float)FlyingTimeMax - 90.0f);

        resultx = (int)orbitPattern.z * (HorizontalRadius[orbitX] / 2 * func.cos(angle)) + OvalCenterX;
        resulty = VerticalRadius[orbitY] / 2 * func.sin(angle) + OvalCenterY[orbitY];

        Transform myTransform = this.transform;
        Vector3 vAngle = myTransform.eulerAngles;
        vAngle.z = (float)time / (float)FlyingTimeMax * 360.0f * FlyingRotationNum * (-orbitPattern.z);
        if(time >= FlyingTimeMax)
        {
            vAngle.z = baseAngle;
        }
        myTransform.eulerAngles = vAngle;

        Vector2 result = new Vector2(resultx, resulty);
        return result;
    }
    /// <summary>
    /// 衝突に関する処理
    /// </summary>
    private void hit()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] item = GameObject.FindGameObjectsWithTag("Item");
        int enemyCount = bm.GetComponent<BattleManager>().GetEnemyCount();
        int itemCount = bm.GetComponent<BattleManager>().GetItemCount();
        for(int i = 0; i < enemyCount; i++)
        {
            if(!enemy[i].GetComponent<Enemy>().IsHit()&&enemy[i].GetComponent<Enemy>().IsAlive())
            {
                if(func.CircleCollision(transform.position, Collisionr, enemy[i].transform.position, enemy[i].GetComponent<Enemy>().GetCollision())){
                    enemy[i].GetComponent<Enemy>().SetHit(power, element);
                }
            }
        }
        for(int i = 0; i < itemCount; i++)
        {
            if(item[i].GetComponent<Item>().IsValid())
            {
                if(func.CircleCollision(transform.position, Collisionr, item[i].transform.position, item[i].GetComponent<Item>().GetCollision()))
                {
                    item[i].GetComponent<Item>().Hit();
                }
            }
        }
    }
    /// <summary>
    /// 現在のヒットポイントを取得する
    /// </summary>
    /// <returns>現在のヒットポイント</returns>
    public int GetHP()
    {
        return hp;
    }
    /// <summary>
    /// 敵の攻撃が当たった時の処理
    /// </summary>
    /// <param name="atk">敵の攻撃力</param>
    public void DamageFromEnemy(int atk)
    {
        hp -= atk;
        if(hp < 0) hp = 0;
    }
    /// <summary>
    /// HPを回復する
    /// </summary>
    /// <param name="rate">最大HPに対する割合</param>
    public void HPCure(float rate)
    {
        hp += (int)(MaxHP * rate);
        if(hp > MaxHP) hp = MaxHP;
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.HPCure);
    }
    /// <summary>
    /// パワーの加算
    /// </summary>
    /// <param name="power">加算するパワーの値</param>
    public void AddPower(int power)
    {
        this.power += power;
    }
    /// <summary>
    /// 属性の変更
    /// </summary>
    /// <param name="element">変更先の属性</param>
    public void SetElement(Enemy.Element element) {
        this.element = element;
    }
    /// <summary>
    /// 状態を書き換える
    /// </summary>
    /// <param name="newstate">変更先の状態</param>
    public void SetState(State newstate)
    {
        state = newstate;
    }
    /// <summary>
    /// フロア移動時の処理
    /// </summary>
    public void MoveFloor()
    {
        power = InitialPower;
        element = Enemy.Element.None;
        ElementEffect.SetElement(Enemy.Element.None);
    }
    /// <summary>
    /// 生死判定
    /// </summary>
    /// <returns>HPが0以下の場合、true</returns>
    public bool isDead()
    {
        return hp <= 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        bm = GameObject.Find("BattleManager");

        //プレイヤー位置セット
        Vector2 pos = new Vector2(StartX, StartY);
        transform.position = pos;
        transform.localScale = func.scalecalc(10, 858);

        Transform myTransform = transform;
        Vector3 vAngle = myTransform.eulerAngles;
        vAngle.z = baseAngle;
        myTransform.eulerAngles = vAngle;

        //パワー表示セット
        GameObject powerCounter = (GameObject)Resources.Load("PowerCounter");
        for(int i = 0; i < 4; i++)
        {
            powerCounter = Instantiate(powerCounter);
            powerCounter.transform.position = pos;
            powerCounter.GetComponent<PowerCounter>().player = this;
            powerCounter.GetComponent<PowerCounter>().index = i;
        }

        //ゲージセット


        //プレイヤーパラメータセット
        hp = MaxHP;
        power = InitialPower;
        element = Enemy.Element.None;
        state = State.NoInput;
    }
    // Update is called once per frame
    void Update()
    {
        bool touchBegin = Application.isEditor ? Input.GetMouseButtonDown(0) : Input.GetMouseButtonDown(0)||func.getTouch() == 1;
        bool touchEnd = Application.isEditor ? Input.GetMouseButtonUp(0) : Input.GetMouseButtonUp(0)||func.getTouch() == -1;
        Vector2 pos = transform.position;
        if(bm.GetComponent<BattleManager>().GetTurn() == BattleManager.Turn.Player)
        {
            switch(state)
            {
            case State.Wait:
                pos.x = StartX;
                pos.y = StartY;
                if(touchBegin)
                {
                    /*
                    touchedx = Application.isEditor ? func.mouse().x : func.getTouchPosition().x;
                    touchedy = Application.isEditor ? func.mouse().y : func.getTouchPosition().y;
                    */
                    touchedx = func.mouse().x;
                    touchedy = func.mouse().y;

                    flickTime = 0;
                    flyingTime = 0;
                    state = State.Touched;
                }
                break;
            case State.Touched:
                flickTime++;
                if(touchEnd)
                {
                    /*
                    releasedx = Application.isEditor ? func.mouse().x : func.getTouchPosition().x;
                    releasedy = Application.isEditor ? func.mouse().y : func.getTouchPosition().y;
                    */
                    releasedx = func.mouse().x;
                    releasedy = func.mouse().y;

                    float flickDistance = func.dist(touchedx, touchedy, releasedx, releasedy);
                    float flickAngle = func.getAngle(touchedx, touchedy, releasedx, releasedy);
                    Debug.Log(flickAngle / Mathf.PI * 180);
                    speed = InitialSpeed;
                    orbit = orbitPattern(flickAngle, flickDistance, speed);
                    Debug.Log(orbit);
                    GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.Flick);
                    state = State.Flying;
                }
                break;
            case State.Flying:
                flyingTime++;
                pos = calcOrbit(orbit, flyingTime);
                hit();
                if(flyingTime >= FlyingTimeMax)
                {
                    bm.GetComponent<BattleManager>().EndTurn(BattleManager.Turn.Player);
                    state = State.NoInput;
                    pos = new Vector2(StartX, StartY);
                }
                break;
            case State.NoInput:
                break;
            }
        }

        transform.position = pos;
    }
}
