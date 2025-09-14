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
    private int gaugeNum;
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
    /// �{�X���o�i�K�ꗗ
    /// </summary>
    public enum BossEffect
    {
        /// <summary>�g��</summary>
        Wave,
        /// <summary>�ڂ�������</summary>
        Blur,
        /// <summary>�Q�[�W�o��</summary>
        Gauge,
        /// <summary>�I��</summary>
        End,
        /// <summary>���j���o</summary>
        Defeat,
        /// <summary>���S</summary>
        Invalid,
    };
    /// <summary>
    /// �{�X���o�i�K
    /// </summary>
    private BossEffect bossEffect;
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
    /// ���S���o�ֈڍs���邩�ǂ���
    /// </summary>
    private bool goDying;
    /// <summary>
    /// ���S���������ǂ���
    /// </summary>
    private bool dying;
    /// <summary>
    /// �����Ă��邩
    /// </summary>
    private bool alive;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    /// <summary>
    /// �{�X�̉摜���X�g
    /// </summary>
    private List<Sprite> spriteList;
    /// <summary>
    /// �{�X�̉摜����
    /// </summary>
    private const int SpriteNum = 6;
    /// <summary>
    /// �{�X�̉摜���X�g�C���f�b�N�X
    /// </summary>
    private int spriteIndex;
    /// <summary>
    /// �g��̐�
    /// </summary>
    public const int WaveNum = 3;
    /// <summary>
    /// �g��o�����玟�̔g��܂ł̎���
    /// </summary>
    private const int WaveGapTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// �i�K���Ƃ̂ڂ��������̎���
    /// </summary>
    private const int BlurTime = (int)(500.0f / (SpriteNum-1) / func.FRAMETIME);
    /// <summary>
    /// �{�X�̌��j���o�̎���
    /// </summary>
    private const int BossDefeatTime = (int)(2000.0f / func.FRAMETIME);
    /// <summary>
    /// �{�X���j���A���~�J�n�܂ł̎���
    /// </summary>
    private const int BossDescendTime = (int)(1000.0f / func.FRAMETIME);
    /// <summary>
    /// �{�X���j���̉��~�̐[��
    /// </summary>
    private const float BossDescendDepth = 0.4f;
    /// <summary>
    /// �{�X���j���̐U���̐U��(�Б�)
    /// </summary>
    private const float BossOscillation = 0.3f;
    /// <summary>
    /// �{�X���o����
    /// </summary>
    private int time;
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
    public bool IsHit()
    {
        return hit;
    }
    /// <summary>
    /// �U�����󂯂��Ƃ��̏���
    /// </summary>
    /// <param name="atk"></param>
    public void SetHit(int atk)
    {
        if(boss)
        {
            gauge.GetComponent<BossGauge>().SetDecrease(atk);
        }
        else
        {
            gauge.GetComponent<EnemyGauge>().SetDecrease(atk);
        }
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
    public bool IsDying()
    {
        return goDying;
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <returns>�����Ă��邩</returns>
    public bool IsAlive()
    {
        return alive;
    }
    /// <summary>
    /// ���S�����ւ̈ڍs���Z�b�g����
    /// </summary>
    public void SetDie()
    {
        goDying = true;
        if(boss)
        {
            bossEffect = BossEffect.Defeat;
        }
    }
    /// <summary>
    /// ���S�������J�n����
    /// </summary>
    private void Die()
    {
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
        turnCounter.GetComponent<TurnCounter>().Die();
    }
    /// <summary>
    /// ���S������X�V����
    /// </summary>
    public void Inactivate()
    {
        alive = false;
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        manager.GetComponent<BattleManager>().AllowChangeFloor();
    }
    /// <summary>
    /// �g�䉉�o���I������
    /// </summary>
    public void EndWave()
    {
        time = 0;
        bossEffect = BossEffect.Blur;
    }
    /// <summary>
    /// �Q�[�W���o���I������
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
            if(element == Element.Fire)
            {
                spriteList.Add(Resources.Load<Sprite>("BossFireBlur1"));
                spriteList.Add(Resources.Load<Sprite>("BossFireBlur2"));
                spriteList.Add(Resources.Load<Sprite>("BossFireBlur3"));
                spriteList.Add(Resources.Load<Sprite>("BossFireBlur4"));
                spriteList.Add(Resources.Load<Sprite>("BossFireBlur5"));
                spriteList.Add(Resources.Load<Sprite>("eA_fire"));
            }
            else if(element == Element.Aqua)
            {
                spriteList.Add(Resources.Load<Sprite>("BossAquaBlur1"));
                spriteList.Add(Resources.Load<Sprite>("BossAquaBlur2"));
                spriteList.Add(Resources.Load<Sprite>("BossAquaBlur3"));
                spriteList.Add(Resources.Load<Sprite>("BossAquaBlur4"));
                spriteList.Add(Resources.Load<Sprite>("BossAquaBlur5"));
                spriteList.Add(Resources.Load<Sprite>("eA_aqua"));
            }
            else if(element == Element.Leaf)
            {
                spriteList.Add(Resources.Load<Sprite>("BossLeafBlur1"));
                spriteList.Add(Resources.Load<Sprite>("BossLeafBlur2"));
                spriteList.Add(Resources.Load<Sprite>("BossLeafBlur3"));
                spriteList.Add(Resources.Load<Sprite>("BossLeafBlur4"));
                spriteList.Add(Resources.Load<Sprite>("BossLeafBlur5"));
                spriteList.Add(Resources.Load<Sprite>("eA_leaf"));
            }
            sr.sprite = spriteList[spriteIndex];
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
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
        transform.localScale = new Vector3((collisionr * 2) / func.pxcalc(1024), (collisionr * 2) / func.pxcalc(1024));

        manager = GameObject.Find("BattleManager");

        if(boss)
        {
            gauge = (GameObject)Resources.Load("BossGauge");
            gauge = Instantiate(gauge);
            gauge.transform.position = new Vector2(transform.position.x, transform.position.y);
            gauge.GetComponent<BossGauge>().parent = this;
            gauge.GetComponent<BossGauge>().hp = this.hp;
            gauge.GetComponent<BossGauge>().maxhp = this.hp;
            gauge.GetComponent<BossGauge>().SetGaugeNum(manager.GetComponent<BattleManager>().GetLastFloor() - manager.GetComponent<BattleManager>().GetFloor());
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
                            gauge.GetComponent<BossGauge>().SetVisibility();
                            turnCounter.GetComponent<TurnCounter>().SetVisibility();
                            bossEffect = BossEffect.Gauge;
                        }
                    }
                    break;
                case BossEffect.Gauge:
                    break;
                case BossEffect.End:
                    manager.GetComponent<BattleManager>().EndBossAppear();
                    break;
                default:
                    break;
                }
            }
            else
            {
                if(bossEffect == BossEffect.Defeat)
                {
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
                        turnCounter.GetComponent<TurnCounter>().SetVisibility();
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
