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
    public const int MaxFloor = 3;
    /// <summary>
    /// �����t���A
    /// </summary>
    private const int InitialFloor = 1;
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
    /// �G���X���C�h�C�����鎞��
    /// </summary>
    public const int SlideTime = (int)(200.0f / func.FRAMETIME);
    /// <summary>
    /// ��Ԉꗗ
    /// </summary>
    public enum State
    {
        /// <summary>�X�e�[�W�J�n�O���擾</summary>
        Load,
        /// <summary>�X�e�[�W�J�n�O�ҋ@</summary>
        StartWait,
        /// <summary>�X�e�[�W�i�s��</summary>
        Process,
        /// <summary>�X�e�[�W�I���O�ҋ@</summary>
        EndWait,
        /// <summary>�t���A�؂�ւ�</summary>
        Change,
        /// <summary>�X�e�[�W�I��</summary>
        End,
        /// <summary>�Q�[���I�[�o�[</summary>
        GameOver,
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
    private State state;
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
    private bool preparated;
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
        floor = InitialFloor;
        time = 0;
        preparated = false;

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
                if(!preparated)
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
                        Debug.Log(new Vector2(i, info.loaderIndex));
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
                        }
                    }
                    preparated = true;
                }
                enemy = GameObject.FindGameObjectsWithTag("Enemy");
                item = GameObject.FindGameObjectsWithTag("Item");
                enemyCount = enemy.Length;
                itemCount = item.Length;
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
                //�����Ƀt���A�J�ڎ��̃��Z�b�g����
                for(int i = 0; i < enemyCount; i++)
                {
                    Destroy(enemy[i]);
                }
                for(int i = 0; i < itemCount; i++)
                {
                    Destroy(item[i]);
                }
                preparated = false;
                GameObject.Find("Player").GetComponent<Player>().MoveFloor();
            }
            break;
        case State.End:
            break;
        case State.GameOver:
            break;
        }
    }
}
