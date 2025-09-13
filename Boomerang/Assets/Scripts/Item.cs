using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A�C�e��
/// </summary>
public class Item : MonoBehaviour
{
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
    /// �A�C�e���̎��
    /// </summary>
    public ItemSort sort;
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
    /// <summary>
    /// �擾�\���ǂ���
    /// </summary>
    public bool valid;
    /// <summary>
    /// �v���C���[�Ƀq�b�g�����Ƃ��̏���
    /// </summary>
    public void Hit()
    {
        switch(sort)
        {
        case ItemSort.Ring:
            GameObject.Find("Player").GetComponent<Player>().AddPower(power);
            power = 0;
            break;
        case ItemSort.Crystal:
            GameObject.Find("Player").GetComponent<Player>().SetElement(element);
            break;
        case ItemSort.Fruit:
            GameObject.Find("Player").GetComponent<Player>().HPCure(cureRate);
            break;
        }
        turnCount = 0;
        valid = false;
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
                if(power<MaxPower) power++;
            }
        }
        else
        {
            turnCount++;
            if(turnCount == validationTurn)
            {
                power = 1;
                turnCount = 0;
                valid = true;
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
        power = InitialPower;
        turnCount = 0;
        switch(sort)
        {
        case ItemSort.Ring:
            validationTurn = RingValidationTurn;
            if(sizePattern == 1)
            {
                CollisionRadius = func.metrecalc(10);
            }
            else if(sizePattern == 2)
            {
                CollisionRadius = func.metrecalc(8);
            }
            else if(sizePattern == 3)
            {
                CollisionRadius = func.metrecalc(5);
            }
            break;
        case ItemSort.Crystal:
            validationTurn = CrystalValidationTurn;
            CollisionRadius = func.metrecalc(5);
            break;
        case ItemSort.Fruit:
            validationTurn = FruitValidationTurn;
            CollisionRadius = func.metrecalc(5);
            break;
        }
        valid = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
