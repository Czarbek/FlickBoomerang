using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�g����ʊǗ�
/// </summary>
public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// �X�v���C�g��x�����̊g�嗦
    /// </summary>
    private const float Sizex = func.camWidth * 4;
    /// <summary>
    /// �X�v���C�g��y�����̊g�嗦
    /// </summary>
    private const float Sizey = func.camHeight * 4;
    /// <summary>
    /// �t���A�ő吔
    /// </summary>
    public const int MaxFloor = 10;
    /// <summary>
    /// �����t���A
    /// </summary>
    public const int InitialFloor = 1;
    /// <summary>
    /// �X�e�[�W�J�n�O�̈Â�
    /// </summary>
    private const float InitialAlpha = 0.5f;
    /// <summary>
    /// �X�e�[�W�J�n�O�̑ҋ@����(�t���[����)
    /// </summary>
    private const int StartWaitTime = 60;
    /// <summary>
    /// �X�e�[�W�J�n���̖��]����
    /// </summary>
    private const int FadeInTime = 20;
    /// <summary>
    /// �G���X���C�h�C�����鎞��(�t���[����)
    /// </summary>
    public const int SlideTime = (int)(200.0f / func.FRAMETIME);
    /// <summary>
    /// �Q�[���I�[�o�[���̈Ó]����(�t���[����)
    /// </summary>
    private const int GOFadeOutTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// ��Ԉꗗ
    /// </summary>
    public enum State
    {
        /// <summary>�X�e�[�W�J�n�O���擾</summary>
        Load,
        /// <summary>�X�e�[�W�J�n�O�ҋ@</summary>
        StartWait,
        /// <summary>�{�X�o�ꉉ�o</summary>
        BossAppear,
        /// <summary>�X�e�[�W�i�s��</summary>
        Process,
        /// <summary>�^�[���؂�ւ��ҋ@</summary>
        TurnWait,
        /// <summary>�X�e�[�W�I���O�ҋ@</summary>
        EndWait,
        /// <summary>�t���A�؂�ւ�</summary>
        Change,
        /// <summary>�X�e�[�W�I��</summary>
        End,
        /// <summary>�Q�[���I�[�o�[</summary>
        GameOver,
        /// <summary>�N���A</summary>
        StageClear,
    };
    /// <summary>
    /// �^�[���ꗗ
    /// </summary>
    public enum Turn
    {
        /// <summary>�v���C���[�^�[��</summary>
        Player,
        /// <summary>�G�^�[��</summary>
        Enemy,
        /// <summary>�^�[���؂�ւ�</summary>
        Change,
    };
    /// <summary>
    /// ���
    /// </summary>
    public State state;
    /// <summary>
    /// ���̏��
    /// </summary>
    private State nextState;
    /// <summary>
    /// �^�[��
    /// </summary>
    static private Turn turn;
    /// <summary>
    /// ���̃^�[��
    /// </summary>
    static private Turn nextTurn;
    /// <summary>
    /// �t�F�[�_�[�I�u�W�F�N�g
    /// </summary>
    private GameObject fader;
    /// <summary>
    /// StageInfo�I�u�W�F�N�g(���X�g�Q�Ɨp)
    /// </summary>
    private GameObject stageInfo;
    /// <summary>
    /// FloorCounter�I�u�W�F�N�g
    /// </summary>
    private GameObject floorCount;
    /// <summary>
    /// �G�I�u�W�F�N�g���X�g
    /// </summary>
    private GameObject[] enemy;
    /// <summary>
    /// �G�̐�
    /// </summary>
    private int enemyCount;
    /// <summary>
    /// �A�C�e���I�u�W�F�N�g���X�g
    /// </summary>
    private GameObject[] item;
    /// <summary>
    /// �G�̐�
    /// </summary>
    private int itemCount;
    /// <summary>
    /// �t���A
    /// </summary>
    private int floor;
    /// <summary>
    /// �ŏI�t���A
    /// </summary>
    private int lastFloor;
    /// <summary>
    /// �o�ߎ���
    /// </summary>
    private int time;
    /// <summary>
    /// ��`�̐F��R�l
    /// </summary>
    private float r;
    /// <summary>
    /// ��`�̐F��G�l
    /// </summary>
    private float g;
    /// <summary>
    /// ��`�̐F��B�l
    /// </summary>
    private float b;
    /// <summary>
    /// ��`�̕s�����x
    /// </summary>
    private float alpha;
    /// <summary>
    /// �I�u�W�F�N�g�̐������ς�ł��邩
    /// </summary>
    private bool prepared;
    /// <summary>
    /// ���݂̏�Ԃ��擾����
    /// </summary>
    /// <returns>���</returns>
    public State GetState()
    {
        return state;
    }
    /// <summary>
    /// ���݂̃^�[�����擾����
    /// </summary>
    /// <returns></returns>
    public Turn GetTurn()
    {
        return turn;
    }
    /// <summary>
    /// ���݂̌o�ߎ��Ԃ��擾����
    /// </summary>
    /// <returns></returns>
    public int GetTime()
    {
        return time;
    }
    /// <summary>
    /// ���݂̃t���A���擾����
    /// </summary>
    /// <returns>���݂̃t���A�̔ԍ�</returns>
    public int GetFloor()
    {
        return floor;
    }
    /// <summary>
    /// �Ō�̃t���A���擾����
    /// </summary>
    /// <returns>�Ō�̃t���A�̔ԍ�</returns>
    public int GetLastFloor()
    {
        return lastFloor;
    }
    /// <summary>
    /// ���j���o�Ȃǂ̂��߂̑ҋ@��Ԃ�ݒ肷��
    /// </summary>
    public void SetWait()
    {
        state = State.TurnWait;
    }
    /// <summary>
    /// �ҋ@��Ԃ���������
    /// </summary>
    public void EndWait()
    {
        state = nextState;
    }
    /// <summary>
    /// �^�[�����I������
    /// </summary>
    /// <param name="currentTurn">���݂̃^�[��</param>
    public void EndTurn(Turn currentTurn)
    {
        turn = Turn.Change;
        nextTurn = currentTurn == Turn.Player ? Turn.Enemy : Turn.Player;
        if(nextTurn == Turn.Player)
        {
            GameObject.Find("Player").GetComponent<Player>().SetState(Player.State.Wait);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.StartTurn);
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
            for(int i = 0; i < enemyCount; i++)
            {
                if(enemy[i].GetComponent<Enemy>().hp <= 0 && enemy[i].GetComponent<Enemy>().IsAlive()) enemy[i].GetComponent<Enemy>().SetDie();
                if(enemy[i].GetComponent<Enemy>().IsAlive())
                {
                    if(enemy[i].GetComponent<Enemy>().IsDying())
                    {

                    }
                    else
                    {
                        doContinue = true;
                    }
                }
            }
            if(!doContinue)
            {
                if(floor == lastFloor)
                {
                    nextState = State.End;
                }
                else
                {
                    nextState = State.Change;
                }
                state = State.EndWait;
            }
        }
        if(GameObject.Find("Player").GetComponent<Player>().isDead())
        {
            GameObject.Find("Player").GetComponent<Player>().SetState(Player.State.NoInput);
            state = State.GameOver;
            time = 0;
        }
    }
    /// <summary>
    /// �{�X�o�ꉉ�o���I������
    /// </summary>
    public void EndBossAppear()
    {
        GameObject.Find("Player").GetComponent<Player>().SetState(Player.State.Wait);
        state = State.Process;
    }
    /// <summary>
    /// ���o�҂����I�����t���A�ڍs����
    /// </summary>
    public void AllowChangeFloor()
    {
        bool doContinue = false;
        for(int i = 0; i < enemyCount; i++)
        {
            if(enemy[i].GetComponent<Enemy>().IsAlive())
            {
                doContinue = true;
            }
        }
        if(state == State.EndWait && !doContinue)
        {
            if(floor < lastFloor)
            {
                state = State.Change;
            }
            else
            {
                GameObject.Find("ClearTx").GetComponent<ClearTx>().SetText();
                state = State.StageClear;
            }
        }
    }
    /// <summary>
    /// �G�̐����擾����
    /// </summary>
    /// <returns></returns>
    public int GetEnemyCount()
    {
        return enemyCount;
    }
    /// <summary>
    /// �A�C�e���̐����擾����
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
                nextState = State.Process;
                if(floor == InitialFloor)
                {
                    GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusic(MusicManager.BGM.Stage);
                }
                if(!prepared)
                {
                    StageInfo.ObjInfo info = stageInfo.GetComponent<StageInfo>().GetStageInfo(floor);
                    for(int i = 0; i < info.loaderIndex; i++)
                    {
                        GameObject obj;
                        bool isEnemy;
                        bool boss = false;
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
                            nextState = State.BossAppear;
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
                            obj.GetComponent<Enemy>().boss = boss;
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
                GameObject.Find("FloorDsp").GetComponent<FloorDsp>().SetText(floor, lastFloor);
                state = State.StartWait;
            }
            break;
        case State.StartWait:
            if(time >= StartWaitTime)
            {
                alpha = InitialAlpha * (float)(FadeInTime - (time - StartWaitTime)) / FadeInTime;
                if(alpha <= 0)
                {
                    alpha = 0;
                    time = 0;
                    floorCount.GetComponent<FloorCount>().DeleteText();
                    if(nextState == State.Process)
                    {
                        GameObject.Find("Player").GetComponent<Player>().SetState(Player.State.Wait);
                    }
                    state = nextState;
                }
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                sr.color = new Color(r, g, b, alpha);
            }
            break;
        case State.BossAppear:
            break;
        case State.Process:
            nextState = State.Process;
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
        case State.TurnWait:

            break;
        case State.EndWait:
            //GameObject.Find("Player").GetComponent<Player>().SetState(Player.State.NoInput);
            break;
        case State.Change:
            Fader.SetFader(20);
            if(time > fader.GetComponent<Fader>().FadeTime)
            {
                time = 0;
                alpha = InitialAlpha;
                GetComponent<SpriteRenderer>().color = new Color(r, g, b, alpha);
                floor++;
                state = State.Load;
                turn = Turn.Player;
                //�����Ƀt���A�J�ڎ��̃��Z�b�g����
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
                GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.MoveFloor);
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
                GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusic(MusicManager.BGM.GameOver);
            }
            break;
        case State.StageClear:

            break;
        }
    }
}
