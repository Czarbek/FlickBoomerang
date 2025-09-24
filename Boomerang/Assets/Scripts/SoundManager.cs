using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// ���ʉ�
    /// </summary>
    public enum Se
    {
        /// <summary>�{�^���^�b�v��</summary>
        Button,
        /// <summary>�u�[�������t���b�N��</summary>
        Flick,
        /// <summary>�����O�l����</summary>
        Ring,
        /// <summary>�u�[�������q�b�g��</summary>
        BoomerangHit,
        /// <summary>�G�U����</summary>
        EnemyAttack,
        /// <summary>�G�̍U���q�b�g��</summary>
        EnemyAttackHit,
        /// <summary>���{�X�U����</summary>
        BossAttackFire,
        /// <summary>�t���A�؂�ւ�����</summary>
        MoveFloor,
        /// <summary>�^�[���J�n����</summary>
        StartTurn,
        /// <summary>�G�o�ꉹ</summary>
        EnemyAppear,
        /// <summary>�{�X�o�ꉹ</summary>
        BossAppear,
        /// <summary>�G���j����</summary>
        EnemyDefeat,
        /// <summary>�N���X�^���擾����</summary>
        Crystal,
        /// <summary>�����̉ʎ��擾����</summary>
        Fruit,
        /// <summary>�G���[��</summary>
        Error,
        /// <summary>HP�񕜎�</summary>
        HPCure,
        /// <summary>�u�[��������]����</summary>
        BoomerangRotate,
        /// <summary>���{�X�U����</summary>
        BossAttackAqua,
        /// <summary>���{�X�U����</summary>
        BossAttackLeaf,
    }
    /// <summary>
    /// ���ʉ����X�g�̐�
    /// </summary>
    private const int SeNum = 19;
    /// <summary>
    /// ���ʉ����X�g
    /// </summary>
    private List<AudioClip> seList;

    public void PlaySound(Se se)
    {
        Debug.Log((int)se);
        GetComponent<AudioSource>().PlayOneShot(seList[(int)se]);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        seList = new List<AudioClip>
        {
            (AudioClip)Resources.Load("se_01"),
            (AudioClip)Resources.Load("se_02"),
            (AudioClip)Resources.Load("se_03"),
            (AudioClip)Resources.Load("se_04"),
            (AudioClip)Resources.Load("se_05"),
            (AudioClip)Resources.Load("se_06"),
            (AudioClip)Resources.Load("se_07"),
            (AudioClip)Resources.Load("se_08"),
            (AudioClip)Resources.Load("se_09"),
            (AudioClip)Resources.Load("se_10"),
            (AudioClip)Resources.Load("se_11"),
            (AudioClip)Resources.Load("se_12"),
            (AudioClip)Resources.Load("se_13"),
            (AudioClip)Resources.Load("se_14"),
            (AudioClip)Resources.Load("se_15"),
            (AudioClip)Resources.Load("se_16"),
            (AudioClip)Resources.Load("se_17"),
            (AudioClip)Resources.Load("se_18"),
            (AudioClip)Resources.Load("se_19")
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
