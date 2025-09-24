using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ʏ�G��HP�Q�[�W
/// </summary>
public class EnemyGauge : MonoBehaviour
{
    /// <summary>
    /// ��Ԉꗗ
    /// </summary>
    private enum State
    {
        /// <summary>�ҋ@</summary>
        Wait,
        /// <summary>�t�F�[�h�C��</summary>
        FadeIn,
        /// <summary>�\����</summary>
        Process,
        /// <summary>������</summary>
        Decrease,
        /// <summary>�t�F�[�h�A�E�g</summary>
        FadeOut,
        /// <summary>��A�N�e�B�u</summary>
        Invalid,
    };
    /// <summary>
    /// ���
    /// </summary>
    private State state;
    /// <summary>
    /// ����
    /// </summary>
    public float ScaleX = 1.6f;
    /// <summary>
    /// �c��
    /// </summary>
    public float ScaleY = 0.15f;
    /// <summary>
    /// �g�̉���
    /// </summary>
    public float frameScaleX = 1.7f;
    /// <summary>
    /// �g�̏c��
    /// </summary>
    public float frameScaleY = 0.25f;
    /// <summary>
    /// �ˑ���
    /// </summary>
    public Enemy parent;
    /// <summary>
    /// �q�b�g�|�C���g
    /// </summary>
    public int hp;
    /// <summary>
    /// �ő�q�b�g�|�C���g
    /// </summary>
    public int maxhp;
    /// <summary>
    /// �Q�[�W�̒��S�ʒu
    /// </summary>
    private float centerX;
    private float centerY;
    /// <summary>
    /// �t�F�[�h�ɂ����鎞��(�~���b)
    /// </summary>
    public int FadeMiliSec;
    /// <summary>
    /// �t�F�[�h�ɂ����鎞��(�t���[��)
    /// </summary>
    private int FadeTime;
    /// <summary>
    /// �t�F�[�h���̎��ԃJ�E���g(�t���[��)
    /// </summary>
    private int time;
    /// <summary>
    /// �Q�[�W�����ɂ����鎞��(�~���b)
    /// </summary>
    public int DecMiliSec;
    /// <summary>
    /// �Q�[�W�����ɂ����鎞��(�t���[��)
    /// </summary>
    private int DecTime;
    /// <summary>
    /// �Q�[�W�\������HP���l
    /// </summary>
    private float dspHP;
    /// <summary>
    /// �Q�[�W�����O��HP���l
    /// </summary>
    private int dspMaxHP;
    /// <summary>
    /// �g�I�u�W�F�N�g
    /// </summary>
    GameObject frame;
    /// <summary>
    /// �����\���I�u�W�F�N�g
    /// </summary>
    GameObject elem;
    /// <summary>
    /// �������I�u�W�F�N�g
    /// </summary>
    GameObject gaugeLine;
    /// <summary>
    /// �\�����J�n����
    /// </summary>
    public void SetVisibility()
    {
        state = State.FadeIn;
        transform.position = new Vector2(parent.transform.position.x, parent.transform.position.y + parent.GetComponent<Enemy>().gaugeOffsetY);
        centerX = transform.position.x;
        centerY = transform.position.y;
        transform.localScale = new Vector2(ScaleX, ScaleY);
        frame = (GameObject)Resources.Load("GaugeFrame");
        frame = Instantiate(frame);
        frame.transform.position = transform.position;
        frame.transform.localScale = new Vector2(frameScaleX, frameScaleY);
        elem = (GameObject)Resources.Load("ElementDsp");
        elem = Instantiate(elem);
        elem.transform.position = new Vector2(centerX - frameScaleX / 2 - func.metrecalc(ElementDsp.MetreSize), centerY);
        elem.GetComponent<ElementDsp>().SetElement(parent.element);
        elem.GetComponent<ElementDsp>().SetExpand(1);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        sr.color = new Color(col.r, col.g, col.b, 0);
    }
    /// <summary>
    /// �Q�[�W������Ԃɂ���
    /// </summary>
    /// <param name="damage">�_���[�W�l</param>
    public void SetDecrease(int damage)
    {
        state = State.Decrease;
        dspMaxHP = hp;
        int dspMinHP = hp - damage;
        if(dspMinHP < 0)
        {
            dspMinHP = 0;
        }
        time = 0;

        gaugeLine = Instantiate((GameObject)Resources.Load("GaugeLine"));
        gaugeLine.transform.position = new Vector2((float)dspMinHP / maxhp * ScaleX + (centerX - ScaleX / 2), centerY);
    }
    /// <summary>
    /// ��\���ɂ���
    /// </summary>
    public void Die()
    {
        state = State.FadeOut;
        elem.GetComponent<ElementDsp>().Die();
        time = 0;
    }
    /// <summary>
    /// ���o���łȂ����𔻒肷��
    /// </summary>
    /// <returns>���o���łȂ��Ȃ�true</returns>
    public bool IsProcessing()
    {
        return state == State.Process;
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.Wait;

        time = 0;
        FadeTime = (int)(FadeMiliSec / func.FRAMETIME);
        DecTime = (int)(DecMiliSec / func.FRAMETIME);
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;

        //HP����
        hp = parent.hp;
        if(state != State.Decrease) dspHP = hp;

        switch(state)
        {
        case State.Wait:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        case State.FadeIn:
            time++;
            sr.color = new Color(col.r, col.g, col.b, (float)time / FadeTime);
            Color framecol = frame.GetComponent<SpriteRenderer>().color;
            frame.GetComponent<SpriteRenderer>().color = new Color(framecol.r, framecol.g, framecol.b, (float)time / FadeTime);
            if(time == FadeTime)
            {
                state = State.Process;
            }
            break;
        case State.Process:
            sr.color = new Color(col.r, col.g, col.b, 1);
            break;
        case State.Decrease:
            dspHP = hp + (float)(dspMaxHP - hp) / DecTime * (DecTime - time);
            time++;
            if(time == DecTime)
            {
                Destroy(gaugeLine);
                state = State.Process;
            }
            break;
        case State.FadeOut:
            time++;
            sr.color = new Color(col.r, col.g, col.b, 1.0f - (float)time / FadeTime);
            framecol = frame.GetComponent<SpriteRenderer>().color;
            frame.GetComponent<SpriteRenderer>().color = new Color(framecol.r, framecol.g, framecol.b, 1.0f - (float)time / FadeTime);
            if(time == FadeTime)
            {
                Destroy(frame);
                Destroy(gameObject);
            }
            break;
        case State.Invalid:
            break;
        }

        float posx = dspHP / maxhp * ScaleX / 2 + (centerX - ScaleX / 2);
        float posy = centerY;
        float scalex = dspHP / maxhp * ScaleX;

        transform.position = new Vector2(posx, posy);
        transform.localScale = new Vector2(scalex, ScaleY);
    }
}
