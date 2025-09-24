using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �u�[������
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// �v���C���[�̏���x���W
    /// </summary>
    public const float StartX = 0;
    /// <summary>
    /// �v���C���[�̏���y���W
    /// </summary>
    private static float StartY = -func.camHeight * 2 + func.metrecalc(10);
    /// <summary>
    /// �ҋ@��Ԃ�z����]
    /// </summary>
    private const float baseAngle = 0;
    /// <summary>
    /// �v���C���[�ړ��̏����x
    /// </summary>
    public const float InitialSpeed = 1.0f;
    /// <summary>
    /// �p���[�̏����l
    /// </summary>
    public const int InitialPower = 1;
    /// <summary>
    /// �p�x�̃p�^�[����
    /// </summary>
    public const int WidthPatternNum = 4;
    /// <summary>
    /// �򋗗��̃p�^�[����
    /// </summary>
    public const int HeightPatternNum = 3;
    /// <summary>
    /// ��s����
    /// </summary>
    public int FlyingTimeMax = 60;
    /// <summary>
    /// ��s���̉�]��
    /// </summary>
    public int FlyingRotationNum = 3;
    /// <summary>
    /// �p�^�[�����Ƃ̑ȉ~�O���̉��a���X�g
    /// </summary>
    public readonly float[] HorizontalRadius = new float[WidthPatternNum] {  func.metrecalc(80), func.metrecalc(60), func.metrecalc(40), func.metrecalc(20) };
    /// <summary>
    /// �p�^�[�����Ƃ̑ȉ~�O���̏c�a���X�g
    /// </summary>
    public readonly float[] VerticalRadius = new float[HeightPatternNum] { func.metrecalc(70), func.metrecalc(110), func.metrecalc(140) };
    /// <summary>
    /// �ȉ~�O���̒��Sx���W
    /// </summary>
    public const float OvalCenterX = 0.0f;
    /// <summary>
    /// �ȉ~�O���̒��Sy���W
    /// </summary>
    public readonly float[] OvalCenterY = new float[HeightPatternNum] { StartY+ func.metrecalc(40), StartY+ func.metrecalc(60), StartY+ func.metrecalc(80) };
    /// <summary>
    /// �t���b�N�p�x�̃p�^�[�����X�g
    /// </summary>
    public readonly float[] AnglePattern = new float[WidthPatternNum + 1] { 0.0f, 22.5f, 45.0f, 67.5f, 90.0f };
    /// <summary>
    /// �t���b�N�����̃p�^�[�����X�g
    /// </summary>
    public readonly float[] FlickDistance = new float[HeightPatternNum] { func.metrecalc(40), func.metrecalc(80), func.metrecalc(120) };
    /// <summary>
    /// �����蔻��̔��a
    /// </summary>
    public float Collisionr = func.metrecalc(5);
    /// <summary>
    /// �v���C���[�̏�Ԉꗗ
    /// </summary>
    public enum State
    {
        Wait, //���͑ҋ@
        Touched, //�^�b�`��
        Flying, //��s��
        NoInput, //���o�҂�
    };
    /// <summary>
    /// �v���C���[�̏��
    /// </summary>
    private State state;
    /// <summary>
    /// x���W
    /// </summary>
    private float posx;
    /// <summary>
    /// y���W
    /// </summary>
    private float posy;
    /// <summary>
    /// ��s���x
    /// </summary>
    private float speed;
    /// <summary>
    /// �^�b�`�����_��x���W
    /// </summary>
    private float touchedx;
    /// <summary>
    /// �^�b�`�����_��y���W
    /// </summary>
    private float touchedy;
    /// <summary>
    /// �^�b�`���Ă��痣�����܂ł̎���(�t���[��)
    /// </summary>
    private int flickTime;
    /// <summary>
    /// �����ꂽ�_��x���W
    /// </summary>
    private float releasedx;
    /// <summary>
    /// �����ꂽ�_��y���W
    /// </summary>
    private float releasedy;
    /// <summary>
    /// ��s���̌o�ߎ���(�t���[��)
    /// </summary>
    private int flyingTime;
    /// <summary>
    /// �ȉ~�O���̃p�^�[��
    /// </summary>
    private Vector3 orbit;
    /// <summary>
    /// BattleManager
    /// </summary>
    private GameObject bm;
    /// <summary>
    /// �q�b�g�|�C���g�ő�l
    /// </summary>
    public int MaxHP;
    /// <summary>
    /// �q�b�g�|�C���g
    /// </summary>
    private int hp;
    /// <summary>
    /// �p���[
    /// </summary>
    public int power;
    /// <summary>
    /// ����
    /// </summary>
    private Enemy.Element element;
    
    /// <summary>
    /// �ȉ~�O���̃p�^�[�������߂�
    /// </summary>
    /// <param name="angle">�t���b�N�p�x</param>
    /// <param name="distance">�t���b�N����</param>
    /// <param name="speed">�t���b�N���x</param>
    /// <returns>�ȉ~�O���̃p�^�[��(�����p�^�[��, �c���p�^�[��, ���]�̗L��)</returns>
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
    /// �ȉ~�̕������Ɋ�Â��A2D���W���v�Z����
    /// </summary>
    /// <param name="orbitPattern">�ȉ~�O���̃p�^�[��</param>
    /// <param name="time">�o�ߎ���</param>
    /// <returns>2D���W</returns>
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
    /// �Փ˂Ɋւ��鏈��
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
    /// ���݂̃q�b�g�|�C���g���擾����
    /// </summary>
    /// <returns>���݂̃q�b�g�|�C���g</returns>
    public int GetHP()
    {
        return hp;
    }
    /// <summary>
    /// �G�̍U���������������̏���
    /// </summary>
    /// <param name="atk">�G�̍U����</param>
    public void DamageFromEnemy(int atk)
    {
        hp -= atk;
        if(hp < 0) hp = 0;
    }
    /// <summary>
    /// HP���񕜂���
    /// </summary>
    /// <param name="rate">�ő�HP�ɑ΂��銄��</param>
    public void HPCure(float rate)
    {
        hp += (int)(MaxHP * rate);
        if(hp > MaxHP) hp = MaxHP;
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.HPCure);
    }
    /// <summary>
    /// �p���[�̉��Z
    /// </summary>
    /// <param name="power">���Z����p���[�̒l</param>
    public void AddPower(int power)
    {
        this.power += power;
    }
    /// <summary>
    /// �����̕ύX
    /// </summary>
    /// <param name="element">�ύX��̑���</param>
    public void SetElement(Enemy.Element element) {
        this.element = element;
    }
    /// <summary>
    /// ��Ԃ�����������
    /// </summary>
    /// <param name="newstate">�ύX��̏��</param>
    public void SetState(State newstate)
    {
        state = newstate;
    }
    /// <summary>
    /// �t���A�ړ����̏���
    /// </summary>
    public void MoveFloor()
    {
        power = InitialPower;
        element = Enemy.Element.None;
        ElementEffect.SetElement(Enemy.Element.None);
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <returns>HP��0�ȉ��̏ꍇ�Atrue</returns>
    public bool isDead()
    {
        return hp <= 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        bm = GameObject.Find("BattleManager");

        //�v���C���[�ʒu�Z�b�g
        Vector2 pos = new Vector2(StartX, StartY);
        transform.position = pos;
        transform.localScale = func.scalecalc(10, 858);

        Transform myTransform = transform;
        Vector3 vAngle = myTransform.eulerAngles;
        vAngle.z = baseAngle;
        myTransform.eulerAngles = vAngle;

        //�p���[�\���Z�b�g
        GameObject powerCounter = (GameObject)Resources.Load("PowerCounter");
        for(int i = 0; i < 4; i++)
        {
            powerCounter = Instantiate(powerCounter);
            powerCounter.transform.position = pos;
            powerCounter.GetComponent<PowerCounter>().player = this;
            powerCounter.GetComponent<PowerCounter>().index = i;
        }

        //�Q�[�W�Z�b�g


        //�v���C���[�p�����[�^�Z�b�g
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
