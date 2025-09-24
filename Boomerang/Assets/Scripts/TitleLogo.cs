using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogo : MonoBehaviour
{
    /// <summary>
    /// ��Ԉꗗ
    /// </summary>
    public enum State
    {
        /// <summary>�J�n�O�ҋ@</summary>
        Wait,
        /// <summary>�o�ꉉ�o</summary>
        JumpIn,
        /// <summary>�\����</summary>
        Process,
        /// <summary>�t�F�[�h�A�E�g</summary>
        FadeOut,
        /// <summary>�t�F�[�h�C��</summary>
        FadeIn,
        /// <summary>��\��</summary>
        Invalid,
    };
    /// <summary>
    /// ���
    /// </summary>
    private State state;
    /// <summary>
    /// �����ʒux���W
    /// </summary>
    private const float StartX = -func.camWidth * 4;
    /// <summary>
    /// x���W
    /// </summary>
    private readonly float CenterX = 0;
    /// <summary>
    /// y���W
    /// </summary>
    private readonly float CenterY = StageInfo.ycalc(120);
    /// <summary>
    /// �o���ɂ����鎞��
    /// </summary>
    private const int JumpInTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// ��]����
    /// </summary>
    private const int RotateTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// �t�F�[�h�A�E�g�ɂ����鎞��
    /// </summary>
    private const int FadeOutTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// �t�F�[�h�C���ɂ����鎞��
    /// </summary>
    private const int FadeInTime = (int)(150.0f / func.FRAMETIME);
    /// <summary>
    /// ��������
    /// </summary>
    private int time;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;

    /// <summary>
    /// ��Ԃ�ύX����
    /// </summary>
    /// <param name="state">�ύX��̏��</param>
    public void SetState(State state)
    {
        time = 0;
        this.state = state;
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("testLogo");
        transform.position = new Vector2(StartX, CenterY);

        SetState(State.Wait);

        if(Initializer.GetRetry())
        {
            SetState(State.Invalid);
            sr.color = new Color(1, 1, 1, 0);
            transform.position = new Vector2(CenterX, CenterY);
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusic(MusicManager.BGM.Opening);
        }

    }

    // Update is called once per frame
    void Update()
    {
        time++;
        switch(state)
        {
        case State.Wait:
            if(Fader.IsEnd())
            {
                GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusic(MusicManager.BGM.Opening);
                SetState(State.JumpIn);
            }
            break;
        case State.JumpIn:
            if(time <= JumpInTime)
            {
                transform.position = new Vector2(StartX + (CenterX - StartX) * func.sin((float)time / JumpInTime * 90.0f), CenterY);
            }
            transform.rotation = Quaternion.Euler(0, 0, func.sin((float)time / RotateTime * 90.0f) * 360.0f);
            if(time == RotateTime)
            {
                SetState(State.Process);
                GameObject.Find("TitleManager").GetComponent<TitleManager>().SetDspState(TitleManager.DspState.FadeIn);
                GameObject.Find("HelpButton").GetComponent<TitleManager>().SetDspState(TitleManager.DspState.FadeIn);
            }
            break;
        case State.Process:
            break;
        case State.FadeOut:
            sr.color = new Color(1, 1, 1, 1.0f - (float)time / FadeOutTime);
            if(time == FadeOutTime)
            {
                SetState(State.Invalid);
            }
            break;
        case State.FadeIn:
            sr.color = new Color(1, 1, 1, (float)time / FadeInTime);
            if(time == FadeInTime)
            {
                SetState(State.Process);
            }
            break;
        case State.Invalid:
            break;
        }
    }
}
