using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// �{�X�ł��邩
    /// </summary>
    public bool boss;
    /// <summary>
    /// �{�XHP�Q�[�W�̖{��
    /// </summary>
    public int gaugeNum;
    /// <summary>
    /// ��_�����ւ̃_���[�W�{��
    /// </summary>
    private const float WeakRate = 2;
    /// <summary>
    /// �ϐ������ւ̃_���[�W�{��
    /// </summary>
    private const float ResistRate = 1;
    /// <summary>
    /// �����蔻��̔��a
    /// </summary>
    private readonly float[] CollisionRadius = new float[4] { func.metrecalc(15), func.metrecalc(10), func.metrecalc(8), func.metrecalc(5) };
    public const float ScreenOutY = func.camHeight*2 + 0.5f;
    /// <summary>
    /// �����ꗗ
    /// </summary>
    public enum Element
    {
        /// <summary>��</summary>
        Fire,
        /// <summary>��</summary>
        Aqua,
        /// <summary>��</summary>
        Leaf,
        /// <summary>�Ȃ�</summary>
        None,
    };
    /// <summary>
    /// �o��t���A
    /// </summary>
    public int floor;
    /// <summary>
    /// �����ʒu��x���W
    /// </summary>
    public float startX;
    /// <summary>
    /// �����ʒu��y���W
    /// </summary>
    public float startY;
    /// <summary>
    /// HP�Q�[�W�̕\���ʒu
    /// </summary>
    public float gaugeOffsetY;
    /// <summary>
    /// �q�b�g�|�C���g
    /// </summary>
    public int hp;
    /// <summary>
    /// ����
    /// </summary>
    public Element element;
    /// <summary>
    /// �U����
    /// </summary>
    public int atk;
    /// <summary>
    /// �U���܂ł̃^�[��
    /// </summary>
    public int maxCount;
    /// <summary>
    /// �s���^�[��
    /// </summary>
    public int turnCount;
    /// <summary>
    /// �T�C�Y�̃p�^�[��
    /// </summary>
    public int sizePattern;
    /// <summary>
    /// �����蔻��̔��a
    /// </summary>
    private float collisionr;
    /// <summary>
    /// ���̃^�[���U�����󂯂���
    /// </summary>
    private bool hit;
    /// <summary>
    /// �s���I��������
    /// </summary>
    private bool changeTurn;
    /// <summary>
    /// �^�[���s�����s������
    /// </summary>
    private bool turnProcess;
    /// <summary>
    /// ���S���������ǂ���
    /// </summary>
    private bool dying;
    /// <summary>
    /// �����Ă��邩
    /// </summary>
    private bool alive;
    /// <summary>
    /// �o�g���}�l�[�W���[�I�u�W�F�N�g
    /// </summary>
    GameObject manager;
    /// <summary>
    /// HP�o�[
    /// </summary>
    GameObject gauge;
    /// <summary>
    /// �^�[���J�E���^�[
    /// </summary>
    GameObject turnCounter;
    /// <summary>
    /// ���̃^�[���U�����󂯂����ǂ����𔻒肷��
    /// </summary>
    /// <returns>hit</returns>
    public bool isHit()
    {
        return hit;
    }
    /// <summary>
    /// �U�����󂯂��Ƃ��̏���
    /// </summary>
    /// <param name="atk"></param>
    public void SetHit(int atk)
    {
        hp -= atk;
        if(hp<0) hp = 0;
        hit = true;
    }
    /// <summary>
    /// �����蔻��̔��a���擾����
    /// </summary>
    /// <returns>�����蔻��̔��a</returns>
    public float GetCollision()
    {
        return collisionr;
    }
    /// <summary>
    /// ������������_���[�W�{�����v�Z����
    /// </summary>
    /// <param name="attackers">�U�����̑���</param>
    /// <param name="defenders">�팂���̑���</param>
    /// <returns>�_���[�W�{��</returns>
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
    /// �G�^�[���J�n���̏���
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
    /// �G�^�[���I��������
    /// </summary>
    public void EndTurn()
    {
        changeTurn = false;
        turnProcess = false;
    }
    /// <summary>
    /// �^�[���J�E���g�����Z�b�g����
    /// </summary>
    public void ResetTurn()
    {
        turnCount = maxCount;
    }
    /// <summary>
    /// �^�[���I���̉�
    /// </summary>
    /// <returns>changeTurn</returns>
    public bool CanChangeTurn()
    {
        return changeTurn;
    }
    /// <summary>
    /// �^�[���I����������
    /// </summary>
    public void SetChange()
    {
        changeTurn = true;
    }
    /// <summary>
    /// ���S���������ǂ����𔻒肷��
    /// </summary>
    /// <returns>�������Ȃ�true</returns>
    public bool isDying()
    {
        return dying;
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <returns>�����Ă��邩</returns>
    public bool isAlive()
    {
        return alive;
    }
    /// <summary>
    /// ���S�������J�n����
    /// </summary>
    public void Die()
    {
        Debug.Log("die");
        GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        for(int i = 1; i > -2; i -= 2)
        {
            GameObject mask = (GameObject)Resources.Load("EnemyMask");
            mask = Instantiate(mask);
            mask.transform.position = new Vector2(transform.position.x+i*func.pxcalc((int)(1024*transform.localScale.x)), transform.position.y);
            mask.transform.localScale = this.transform.localScale;
            mask.GetComponent<EnemyMask>().SetDirection(i);
            mask.GetComponent<EnemyMask>().parent = this.gameObject;
        }
        gauge.GetComponent<EnemyGauge>().Die();
        Destroy(turnCounter.gameObject);
    }
    /// <summary>
    /// ���S������X�V����
    /// </summary>
    public void Inactivate()
    {
        alive = false;
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;
        startY = transform.position.y;
        transform.position = new Vector2(startX, ScreenOutY);

        turnCount = maxCount;
        collisionr = CollisionRadius[sizePattern];
        hit = false;
        changeTurn = false;
        turnProcess = false;
        dying = false;
        alive = true;

        switch(sizePattern)
        {
        case 0:
            if(element == Element.Fire)
            {
                GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("eA_fire");
            }
            else if(element == Element.Aqua)
            {
                GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("eA_aqua");
            }
            else if(element == Element.Leaf)
            {
                GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("eA_leaf");
            }
            break;
        case 1:
            if(element == Element.Fire)
            {
                GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("eB_fire");
            }
            else if(element == Element.Aqua)
            {
                GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("eB_aqua");
            }
            else if(element == Element.Leaf)
            {
                GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("eB_leaf");
            }
            break;
        case 2:
            if(element == Element.Fire)
            {
                GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("eC_fire");
            }
            else if(element == Element.Aqua)
            {
                GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("eC_aqua");
            }
            else if(element == Element.Leaf)
            {
                GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("eC_leaf");
            }
            break;
        case 3:
            if(element == Element.Fire)
            {
                GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("eD_fire");
            }
            else if(element == Element.Aqua)
            {
                GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("eD_aqua");
            }
            else if(element == Element.Leaf)
            {
                GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("eD_leaf");
            }
            break;
        default:
            break;
        }
        transform.localScale = new Vector3((collisionr * 2) / func.pxcalc(1024), (collisionr * 2) / func.pxcalc(1024));

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
