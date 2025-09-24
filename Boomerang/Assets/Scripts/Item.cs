using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A�C�e��
/// </summary>
public class Item : MonoBehaviour
{
    public enum State
    {
        /// <summary>�t�F�[�h�C��</summary>
        FadeIn,
        /// <summary>�\����</summary>
        Process,
        /// <summary>�t�F�[�h�A�E�g</summary>
        FadeOut,
        /// <summary>��\��</summary>
        Invalid,
    }
    /// <summary>
    /// �A�C�e���̎�ވꗗ
    /// </summary>
    public enum ItemSort
    {
        /// <summary>�����O</summary>
        Ring,
        /// <summary>�N���X�^��</summary>
        Crystal,
        /// <summary>�����̉ʎ�</summary>
        Fruit,
    };
    /// <summary>
    /// ���
    /// </summary>
    private State state;
    /// <summary>
    /// �A�C�e���̎��
    /// </summary>
    public ItemSort sort;
    /// <summary>
    /// �����O�̉摜�̃s�N�Z���P�ʂ̏c��
    /// </summary>
    private const int RingPx = 363;
    /// <summary>
    /// �N���X�^���̂̉摜�̃s�N�Z���P�ʂ̏c��
    /// </summary>
    private const int CrystalPx = 112;
    /// <summary>
    /// �����̉ʎ��̉摜�̃s�N�Z���P�ʂ̏c��
    /// </summary>
    private const int FruitPx = 122;
    /// <summary>
    /// x���W
    /// </summary>
    public float x;
    /// <summary>
    /// y���W
    /// </summary>
    public float y;
    /// <summary>
    /// �����蔻��̔��a
    /// </summary>
    public float CollisionRadius;
    /// <summary>
    /// �T�C�Y
    /// </summary>
    public int sizePattern;
    /// <summary>
    /// �p���[�̍ő�l
    /// </summary>
    private const int MaxPower = 10;
    /// <summary>
    /// �������_�ł̃p���[
    /// </summary>
    public int InitialPower;
    /// <summary>
    /// ���ݎ��_�ł̃p���[
    /// </summary>
    private int power;
    /// <summary>
    /// �N���X�^���̑���
    /// </summary>
    public Enemy.Element element;
    /// <summary>
    /// �����̉ʎ���HP�񕜊���
    /// </summary>
    public float cureRate;
    /// <summary>
    /// �o�߃^�[��
    /// </summary>
    public int turnCount;
    /// <summary>
    /// �����ɂ�����^�[��
    /// </summary>
    private int validationTurn;
    /// <summary>
    /// �����O�����ɂ�����^�[��
    /// </summary>
    private const int RingValidationTurn = 1;
    /// <summary>
    /// �N���X�^�������ɂ�����^�[��
    /// </summary>
    private const int CrystalValidationTurn = 3;
    /// <summary>
    /// �����̉ʎ������ɂ�����^�[��
    /// </summary>
    private const int FruitValidationTurn = 3;
    /// <summary>
    /// �t�F�[�h�C���ɂ����鎞��(�t���[��)
    /// </summary>
    private const int FadeTime = (int)(300.0f / func.FRAMETIME);
    /// <summary>
    /// �t�F�[�h���̎��ԃJ�E���g(�t���[��)
    /// </summary>
    private int time;
    /// <summary>
    /// <summary>
    /// �擾�\���ǂ���
    /// </summary>
    public bool valid;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    /// <summary>
    /// �v���C���[�Ƀq�b�g�����Ƃ��̏���
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
    /// �G�^�[���I��������
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
            turnCount++;
            if(turnCount == validationTurn)
            {
                power = 1;
                if(sort == ItemSort.Ring)
                {
                    sr.sprite = Initializer.GetRingImg(power);
                }
                turnCount = 0;
                valid = true;
                state = State.FadeIn;
            }
        }
    }
    /// <summary>
    /// �����蔻��̔��a���擾����
    /// </summary>
    /// <returns>�����蔻��̔��a</returns>
    public float GetCollision()
    {
        return CollisionRadius;
    }
    /// <summary>
    /// �L�����ǂ����𔻒肷��
    /// </summary>
    /// <returns>�L���Ȃ�true</returns>
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
