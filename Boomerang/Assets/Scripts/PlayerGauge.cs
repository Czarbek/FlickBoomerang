using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using System;

/// <summary>
/// ���@��HP�Q�[�W
/// </summary>
public class PlayerGauge : MonoBehaviour
{
    /// <summary>
    /// ��Ԉꗗ
    /// </summary>
    private enum State
    {
        /// <summary>�\����</summary>
        Process,
        /// <summary>������</summary>
        Decrease,
        /// <summary>������</summary>
        Increase,
    };
    /// <summary>
    /// ���
    /// </summary>
    private State state;
    /// <summary>
    /// �����蔻��̔��a
    /// </summary>
    static public float Collisionr = 0.1f;
    /// <summary>
    /// max����x���W
    /// </summary>
    public const float DefaultX = 1.65f;
    /// <summary>
    /// y���W
    /// </summary>
    public const float DefaultY = -4.5f;
    /// <summary>
    /// ����
    /// </summary>
    private readonly float ScaleX = func.metrecalc(25);
    /// <summary>
    /// �c��
    /// </summary>
    private readonly float ScaleY = 0.25f;
    /// <summary>
    /// �g�̉���
    /// </summary>
    private readonly float frameScaleX = func.metrecalc(25+2);
    /// <summary>
    /// �g�̏c��
    /// </summary>
    private readonly float frameScaleY = 0.35f;
    /// <summary>
    /// �q�b�g�|�C���g�ő�l
    /// </summary>
    private int maxHP = 100;
    /// <summary>
    /// �v���C���[�I�u�W�F�N�g
    /// </summary>
    private GameObject player;
    /// <summary>
    /// �q�b�g�|�C���g
    /// </summary>
    private int hp;
    /// <summary>
    /// �Q�[�W�����ɂ����鎞��(�t���[��)
    /// </summary>
    private const int DecTime = (int)(1000.0f / func.FRAMETIME);
    /// <summary>
    /// �Q�[�W�\������HP���l
    /// </summary>
    private float dspHP;
    /// <summary>
    /// �Q�[�W�����O��HP���l
    /// </summary>
    private int dspMaxHP;
    /// <summary>
    /// ��������
    /// </summary>
    private int time;
    /// <summary>
    /// �������I�u�W�F�N�g
    /// </summary>
    GameObject gaugeLine;

    /// <summary>
    /// �G�e���q�b�g�����ꍇ�A�v���C���[��HP�����炷
    /// </summary>
    /// <param name="atk">�G�̍U����</param>
    public void Hit(int atk)
    {
        SetDecrease(atk);
        player.GetComponent<Player>().DamageFromEnemy(atk);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.EnemyAttackHit);
    }
    /// <summary>
    /// �Q�[�W������Ԃɂ���
    /// </summary>
    /// <param name="damage">�_���[�W�l</param>
    public void SetDecrease(int damage)
    {
        if(gaugeLine != null)
        {
            Destroy(gaugeLine);
        }
        state = State.Decrease;
        dspMaxHP = hp;
        int dspMinHP = hp - damage;
        if(dspMinHP < 0)
        {
            dspMinHP = 0;
        }
        time = 0;

        gaugeLine = Instantiate((GameObject)Resources.Load("GaugeLine"));
        gaugeLine.transform.position = new Vector2((float)dspMinHP / maxHP * ScaleX + gaugeLine.transform.localScale.x / 2 + (DefaultX - ScaleX / 2), DefaultY);
    }
    /// <summary>
    /// �Q�[�W������Ԃɂ���
    /// </summary>
    /// <param name="damage">�_���[�W�l</param>
    public void SetIncrease(int cure)
    {
        if(gaugeLine != null)
        {
            Destroy(gaugeLine);
        }
        state = State.Increase;
        dspMaxHP = hp;
        int dspMinHP = hp + cure;
        if(dspMinHP > maxHP)
        {
            dspMinHP = maxHP;
        }
        time = 0;

        gaugeLine = Instantiate((GameObject)Resources.Load("GaugeLine"));
        gaugeLine.transform.position = new Vector2((float)dspMinHP / maxHP * ScaleX - gaugeLine.transform.localScale.x / 2 + (DefaultX - ScaleX / 2), DefaultY);
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.Process;
        time = 0;

        transform.position = new Vector2(DefaultX, DefaultY);
        transform.localScale = new Vector2(ScaleX, ScaleY);
        player = GameObject.Find("Player");
        maxHP = player.GetComponent<Player>().MaxHP;

        GameObject frame = (GameObject)Resources.Load("GaugeFrame");
        frame = Instantiate(frame);
        frame.transform.position = transform.position;
        frame.transform.localScale = new Vector2(frameScaleX, frameScaleY);
    }

    // Update is called once per frame
    void Update()
    {
        //HP����
        hp = player.GetComponent<Player>().GetHP();
        if(state == State.Process) dspHP = hp;

        switch(state)
        {
        case State.Process:
            break;
        case State.Decrease:
            time++;
            dspHP = hp + (float)(dspMaxHP - hp) / DecTime * (DecTime - time);
            if(time == DecTime)
            {
                Destroy(gaugeLine);
                state = State.Process;
            }
            break;
        case State.Increase:
            time++;
            dspHP = hp + (float)(hp - dspMaxHP) / DecTime * (DecTime - time);
            if(time == DecTime)
            {
                Destroy(gaugeLine);
                state = State.Process;
            }
            break;
        }

        float posx = dspHP / maxHP * ScaleX / 2 + (DefaultX - ScaleX / 2);
        float posy = DefaultY;
        float scalex = dspHP / maxHP * ScaleX;

        transform.position = new Vector2(posx, posy);
        transform.localScale = new Vector2(scalex, ScaleY);
    }
}
