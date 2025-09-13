using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトル画面管理
/// </summary>
public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// スプライトのx方向の拡大率
    /// </summary>
    private const float Sizex = func.camWidth * 4;
    /// <summary>
    /// スプライトのy方向の拡大率
    /// </summary>
    private const float Sizey = func.camHeight * 4;
    /// <summary>
    /// フロア最大数
    /// </summary>
    public const int MaxFloor = 10;
    /// <summary>
    /// 初期フロア
    /// </summary>
    private const int InitialFloor = 1;
    /// <summary>
    /// ステージ開始前の暗さ
    /// </summary>
    private const float InitialAlpha = 0.5f;
    /// <summary>
    /// ステージ開始前の待機時間(フレーム数)
    /// </summary>
    private const int StartWaitTime = 60;
    /// <summary>
    /// ステージ開始時の明転時間
    /// </summary>
    private const int FadeInTime = 20;
    /// <summary>
    /// 敵がスライドインする時間(フレーム数)
    /// </summary>
    public const int SlideTime = (int)(200.0f / func.FRAMETIME);
    /// <summary>
    /// ゲームオーバー時の暗転時間(フレーム数)
    /// </summary>
    private const int GOFadeOutTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// 状態一覧
    /// </summary>
    public enum State
    {
        /// <summary>ステージ開始前情報取得</summary>
        Load,
        /// <summary>ステージ開始前待機</summary>
        StartWait,
        /// <summary>ステージ進行中</summary>
        Process,
        /// <summary>ステージ終了前待機</summary>
        EndWait,
        /// <summary>フロア切り替え</summary>
        Change,
        /// <summary>ステージ終了</summary>
        End,
        /// <summary>ゲームオーバー演出中</summary>
        GameOver,
        /// <summary>ゲームオーバー表示後セレクト</summary>
        GameOverSelect,
    };
    /// <summary>
    /// ターン一覧
    /// </summary>
    public enum Turn
    {
        /// <summary>プレイヤーターン</summary>
        Player,
        /// <summary>敵ターン</summary>
        Enemy,
        /// <summary>ターン切り替え</summary>
        Change,
    };
    /// <summary>
    /// 状態
    /// </summary>
    private State state;
    /// <summary>
    /// ターン
    /// </summary>
    static private Turn turn;
    /// <summary>
    /// 次のターン
    /// </summary>
    static private Turn nextTurn;
    /// <summary>
    /// フェーダーオブジェクト
    /// </summary>
    private GameObject fader;
    /// <summary>
    /// StageInfoオブジェクト(リスト参照用)
    /// </summary>
    private GameObject stageInfo;
    /// <summary>
    /// FloorCounterオブジェクト
    /// </summary>
    private GameObject floorCount;
    /// <summary>
    /// 敵オブジェクトリスト
    /// </summary>
    private GameObject[] enemy;
    /// <summary>
    /// 敵の数
    /// </summary>
    private int enemyCount;
    /// <summary>
    /// アイテムオブジェクトリスト
    /// </summary>
    private GameObject[] item;
    /// <summary>
    /// 敵の数
    /// </summary>
    private int itemCount;
    /// <summary>
    /// フロア
    /// </summary>
    private int floor;
    /// <summary>
    /// 最終フロア
    /// </summary>
    private int lastFloor;
    /// <summary>
    /// 経過時間
    /// </summary>
    private int time;
    /// <summary>
    /// 矩形の色のR値
    /// </summary>
    private float r;
    /// <summary>
    /// 矩形の色のG値
    /// </summary>
    private float g;
    /// <summary>
    /// 矩形の色のB値
    /// </summary>
    private float b;
    /// <summary>
    /// 矩形の不透明度
    /// </summary>
    private float alpha;
    /// <summary>
    /// オブジェクトの生成が済んでいるか
    /// </summary>
    private bool prepared;
    /// <summary>
    /// 現在の状態を取得する
    /// </summary>
    /// <returns>状態</returns>
    public State GetState()
    {
        return state;
    }
    /// <summary>
    /// 現在のターンを取得する
    /// </summary>
    /// <returns></returns>
    public Turn GetTurn()
    {
        return turn;
    }
    /// <summary>
    /// 現在の経過時間を取得する
    /// </summary>
    /// <returns></returns>
    public int GetTime()
    {
        return time;
    }
    /// <summary>
    /// ターンを終了する
    /// </summary>
    /// <param name="currentTurn">現在のターン</param>
    public void EndTurn(Turn currentTurn)
    {
        turn = Turn.Change;
        nextTurn = currentTurn == Turn.Player ? Turn.Enemy : Turn.Player;
        if(nextTurn == Turn.Player)
        {
            GameObject.Find("Player").GetComponent<Player>().SetState(Player.State.Wait);
            for(int i = 0; i < enemyCount; i++)
            {
                enemy[i].GetComponent<Enemy>().EndTurn();
            }
            for(int i = 0; i < itemCount; i++)
            {
                item[i].GetComponent<Item>().EndTurn();
            }
        }
        else if(nextTurn == Turn.Enemy)
        {
            bool doContinue = false;
            bool wait = true;
            for(int i = 0; i < enemyCount; i++)
            {
                if(enemy[i].GetComponent<Enemy>().hp <= 0 && enemy[i].GetComponent<Enemy>().isAlive()) enemy[i].GetComponent<Enemy>().Die();
                if(enemy[i].GetComponent<Enemy>().isAlive()) doContinue = true;
                if(!(enemy[i].GetComponent<Enemy>().isAlive() && enemy[i].GetComponent<Enemy>().isDying())) wait = false;
            }
            if(!doContinue) state = State.Change;
            if(wait) state = State.EndWait;
        }
        if(GameObject.Find("Player").GetComponent<Player>().isDead())
        {
            GameObject.Find("Player").GetComponent<Player>().SetState(Player.State.NoInput);
            state = State.GameOver;
            time = 0;
        }
    }
    /// <summary>
    /// 敵の数を取得する
    /// </summary>
    /// <returns></returns>
    public int GetEnemyCount()
    {
        return enemyCount;
    }
    /// <summary>
    /// アイテムの数を取得する
    /// </summary>
    /// <returns></returns>
    public int GetItemCount()
    {
        return itemCount;
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.Load;
        turn = Turn.Player;
        fader = GameObject.Find("Fader");
        stageInfo = GameObject.Find("StageInfo");
        floorCount = GameObject.Find("FloorCount");
        floor = InitialFloor;
        time = 0;
        prepared = false;

        r = 0.0f;
        g = 0.0f;
        b = 0.0f;
        alpha = 0.5f;
        transform.position = new Vector2(func.SCCX, func.SCCY);
        transform.localScale = new Vector3(Sizex, Sizey, 1.0f);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(r, g, b, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        if(Fader.IsEnd()) time++;
        switch(state)
        {
        case State.Load:
            if(Fader.IsEnd())
            {
                time = 0;
                if(!prepared)
                {
                    StageInfo.ObjInfo info = stageInfo.GetComponent<StageInfo>().GetStageInfo(floor);
                    for(int i = 0; i < info.loaderIndex; i++)
                    {
                        GameObject obj;
                        bool isEnemy;
                        bool boss;
                        Item.ItemSort itemSort = Item.ItemSort.Ring;
                        switch(info.sort[i])
                        {
                        case StageInfo.ObjSort.Enemy:
                            obj = (GameObject)Resources.Load("Enemy");
                            isEnemy = true;
                            boss = false;
                            break;
                        case StageInfo.ObjSort.Boss:
                            obj = (GameObject)Resources.Load("Enemy");
                            isEnemy = true;
                            boss = true;
                            break;
                        case StageInfo.ObjSort.Ring:
                            obj = (GameObject)Resources.Load("Item");
                            isEnemy = false;
                            itemSort = Item.ItemSort.Ring;
                            break;
                        case StageInfo.ObjSort.Crystal:
                            obj = (GameObject)Resources.Load("Item");
                            isEnemy = false;
                            itemSort = Item.ItemSort.Crystal;
                            break;
                        case StageInfo.ObjSort.Fruit:
                            obj = (GameObject)Resources.Load("Item");
                            isEnemy = false;
                            itemSort = Item.ItemSort.Fruit;
                            break;
                        default:
                            obj = (GameObject)Resources.Load("Enemy");
                            isEnemy = true;
                            boss = false;
                            break;
                        }
                        obj = Instantiate(obj);
                        obj.transform.position = new Vector2(info.x[i], info.y[i]);
                        if(isEnemy)
                        {
                            obj.GetComponent<Enemy>().hp = info.hp[i];
                            obj.GetComponent<Enemy>().element = info.element[i];
                            obj.GetComponent<Enemy>().atk = info.atk[i];
                            obj.GetComponent<Enemy>().maxCount = info.turn[i];
                            obj.GetComponent<Enemy>().sizePattern = info.size[i];
                        }
                        else
                        {
                            obj.GetComponent<Item>().sort = itemSort;
                            obj.GetComponent<Item>().element = info.element[i];
                            obj.GetComponent<Item>().InitialPower = info.atk[i];
                            obj.GetComponent<Item>().turnCount = info.turn[i];
                            obj.GetComponent<Item>().sizePattern = info.size[i];
                        }
                    }
                    lastFloor = stageInfo.GetComponent<StageInfo>().GetLastFloorNumber();
                    prepared = true;
                }
                enemy = GameObject.FindGameObjectsWithTag("Enemy");
                item = GameObject.FindGameObjectsWithTag("Item");
                enemyCount = enemy.Length;
                itemCount = item.Length;
                floorCount.GetComponent<FloorCount>().SetText(floor, lastFloor);
                state = State.StartWait;
            }
            break;
        case State.StartWait:
            if(time >= StartWaitTime)
            {
                alpha -= InitialAlpha / FadeInTime;
                if(alpha <= 0)
                {
                    alpha = 0;
                    time = 0;
                    floorCount.GetComponent<FloorCount>().DeleteText();
                    GameObject.Find("Player").GetComponent<Player>().SetState(Player.State.Wait);
                    state = State.Process;
                }
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                sr.color = new Color(r, g, b, alpha);
            }
            break;
        case State.Process:
            if(turn == Turn.Player)
            {

            }
            else if(turn == Turn.Enemy)
            {
                for(int i = 0; i < enemyCount; i++)
                {
                    enemy[i].GetComponent<Enemy>().StartTurn();
                }
                bool change = true;
                for(int i = 0; i < enemyCount; i++)
                {
                    if(!enemy[i].GetComponent<Enemy>().CanChangeTurn())
                    {
                        change = false;
                        break;
                    }
                }
                if(change)
                {
                    EndTurn(turn);
                }
            }
            else if(turn == Turn.Change)
            {
                turn = nextTurn;
            }
            break;
        case State.EndWait:
            //GameObject.Find("Player").GetComponent<Player>().SetState(Player.State.NoInput);
            break;
        case State.Change:
            Fader.SetFader(20);
            if(time > fader.GetComponent<Fader>().FadeTime)
            {
                time = 0;
                floor++;
                state = State.Load;
                //ここにフロア遷移時のリセット処理
                for(int i = 0; i < enemyCount; i++)
                {
                    Destroy(enemy[i]);
                }
                for(int i = 0; i < itemCount; i++)
                {
                    Destroy(item[i]);
                }
                prepared = false;
                GameObject.Find("Player").GetComponent<Player>().MoveFloor();
            }
            break;
        case State.End:
            break;
        case State.GameOver:
            if(time < GOFadeOutTime)
            {
                alpha += InitialAlpha / FadeInTime;
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                sr.color = new Color(r, g, b, alpha);
            }
            else if(time == GOFadeOutTime)
            {
                GameObject.Find("GameOverTx").GetComponent<GameOverTx>().SetText();
            }
            break;
        case State.GameOverSelect:
            break;
        }
    }
}
