using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�X��HP�Q�[�W
/// </summary>
public class BossGauge : MonoBehaviour
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
    };
    /// <summary>
    /// ���
    /// </summary>
    private State state;
    /// <summary>
    /// ����
    /// </summary>
    public float MaxScaleX = 4.2f;
    /// <summary>
    /// �c��
    /// </summary>
    public float MaxScaleY = 0.3f;
    /// <summary>
    /// �g�̉���
    /// </summary>
    public float frameScaleX = 4.6f;
    /// <summary>
    /// �g�̏c��
    /// </summary>
    public float frameScaleY = 0.6f;
    /// <summary>
    /// �e�F�̒l
    /// </summary>
    public float BlueColr;
    public float BlueColg;
    public float BlueColb;
    public float GreenColr;
    public float GreenColg;
    public float GreenColb;
    public float YellowColr;
    public float YellowColg;
    public float YellowColb;
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
    /// �Q�[�W��
    /// </summary>
    private float scaleX;
    private float scaleY;
    /// <summary>
    /// ��]�p�x
    /// </summary>
    private float angle;
    /// <summary>
    /// �Q�[�W�̖{��
    /// </summary>
    private int gaugeNum;
    /// <summary>
    /// �t�F�[�h�C���ɂ����鎞��(�~���b)
    /// </summary>
    public int FadeMiliSec;
    /// <summary>
    /// �t�F�[�h�C���ɂ����鎞��(�t���[��)
    /// </summary>
    private int FadeTime;
    /// <summary>
    /// �t�F�[�h���̎��ԃJ�E���g(�t���[��)
    /// </summary>
    private int time;
    /// <summary>
    /// �g�I�u�W�F�N�g
    /// </summary>
    GameObject frame;
    /// <summary>
    /// �Q�[�W�̖{����ݒ肷��
    /// </summary>
    /// <param name="num">�{��</param>
    public void SetGaugeNum(int num)
    {
        gaugeNum = num;
    }
    /// <summary>
    /// �Q�[�W�̕\�����J�n����
    /// </summary>
    public void SetVisibility()
    {
        state = State.FadeIn;
        transform.position = new Vector2(parent.transform.position.x, parent.transform.position.y);
        centerX = transform.position.x;
        centerY = transform.position.y;
        scaleX = 0;
        scaleY = 0;
        angle = 0;
        transform.localScale = new Vector2(scaleX, scaleY);
        frame = (GameObject)Resources.Load("GaugeFrame");
        frame = Instantiate(frame);
        frame.transform.position = transform.position;
        frame.transform.localScale = new Vector2(0, 0);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        sr.color = new Color(col.r, col.g, col.b, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.Wait;

        time = 0;
        FadeTime = (int)(FadeMiliSec / func.FRAMETIME);
        Debug.Log(FadeTime);
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        switch(state)
        {
        case State.Wait:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        case State.FadeIn:
            time++;
            angle = (float)time / FadeTime * 360.0f;
            sr.color = new Color(col.r, col.g, col.b, (float)time / FadeTime);
            Color framecol = frame.GetComponent<SpriteRenderer>().color;
            frame.GetComponent<SpriteRenderer>().color = new Color(framecol.r, framecol.g, framecol.b, (float)time / FadeTime);
            if(time == FadeTime)
            {
                scaleX = MaxScaleX;
                scaleY = MaxScaleY;
                angle = 0;
                state = State.Process;
            }
            break;
        case State.Process:
            sr.color = new Color(col.r, col.g, col.b, 1);
            break;
        }
        hp = parent.hp - maxhp * (gaugeNum - 1);
        while(hp < 0)
        {
            gaugeNum--;
            if(gaugeNum < 0)
            {
                hp = 0;
                break;
            }
            hp = parent.hp - maxhp * (gaugeNum - 1);
        }
        float posx = (float)hp / maxhp * MaxScaleX / 2 + (centerX - MaxScaleX / 2);
        float posy = centerY;
        float scalex = (float)hp / maxhp * MaxScaleX;

        transform.position = new Vector2(posx, posy);
        transform.localScale = new Vector2(scalex, scaleY);
    }
}
