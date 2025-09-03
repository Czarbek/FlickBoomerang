using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

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
    public float scaleX = 1.6f;
    /// <summary>
    /// �c��
    /// </summary>
    public float scaleY = 0.15f;
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
    /// �\�����J�n����
    /// </summary>
    public void SetVisibility()
    {
        state = State.FadeIn;
        transform.position = new Vector2(parent.transform.position.x, parent.transform.position.y + parent.GetComponent<Enemy>().gaugeOffsetY);
        centerX = transform.position.x;
        centerY = transform.position.y;
        transform.localScale = new Vector2(scaleX, scaleY);
        frame = (GameObject)Resources.Load("GaugeFrame");
        frame = Instantiate(frame);
        frame.transform.position = transform.position;
        frame.transform.localScale = new Vector2(frameScaleX, frameScaleY);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        sr.color = new Color(col.r, col.g, col.b, 0);
    }
    /// <summary>
    /// ��\���ɂ���
    /// </summary>
    public void Die()
    {
        state = State.FadeOut;
        time = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.Wait;

        time = 0;
        FadeTime = (int)(FadeMiliSec / func.FRAMETIME);
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
        hp = parent.hp;
        float posx = (float)hp / maxhp * scaleX / 2 + (centerX - scaleX / 2);
        float posy = centerY;
        float scalex = (float)hp / maxhp * scaleX;

        transform.position = new Vector2(posx, posy);
        transform.localScale = new Vector2(scalex, scaleY);
    }
}
