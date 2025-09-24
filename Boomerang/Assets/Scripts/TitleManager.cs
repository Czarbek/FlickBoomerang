using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�C�g����ʊǗ��A�X�^�[�g�{�^��
/// </summary>
public class TitleManager : MonoBehaviour
{
    /// <summary>
    /// �s�N�Z���P�ʂ̉���
    /// </summary>
    private const int ButtonPxSizeX = 640;
    /// <summary>
    /// �s�N�Z���P�ʂ̏c��
    /// </summary>
    private const int ButtonPxSizeY = 190;
    /// <summary>
    /// ����(����)
    /// </summary>
    public float BSizeX;
    /// <summary>
    /// �c��(����)
    /// </summary>
    public float BSizeY;
    /// <summary>
    /// ��Փx�{�^����y���W
    /// </summary>
    private readonly float[] DiffButtonY = new float[4];
    /// <summary>
    /// ���[�h�ꗗ
    /// </summary>
    public enum State
    {
        /// <summary>�^�C�g��</summary>
        Title,
        /// <summary>�w���v</summary>
        Help,
        /// <summary>��Փx�I��</summary>
        Select,
        /// <summary>�I��s�\��</summary>
        Caution,
    };
    /// <summary>
    /// �\�����
    /// </summary>
    public enum DspState
    {
        /// <summary>�J�n�O�ҋ@</summary>
        Wait,
        /// <summary>�t�F�[�h�C��</summary>
        FadeIn,
        /// <summary>�\����</summary>
        Process,
        /// <summary>�t�F�[�h�A�E�g</summary>
        FadeOut,
        /// <summary>�w���v�ڍs���t�F�[�h�A�E�g</summary>
        HelpFadeOut,
        /// <summary>�t�F�[�h�C��2��ڈȍ~</summary>
        FadeIn2,
        /// <summary>��\��</summary>
        Invalid,
    };
    /// <summary>
    /// ���[�h
    /// </summary>
    public State state;
    /// <summary>
    /// �\�����
    /// </summary>
    public DspState dspState;
    /// <summary>
    /// �t�F�[�h�C���ɂ����鎞��
    /// </summary>
    protected const int FadeInTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// 2��ڈȍ~�̃t�F�[�h�C���ɂ����鎞��
    /// </summary>
    protected const int FadeInTime2 = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// �t�F�[�h�A�E�g�ɂ����鎞��
    /// </summary>
    protected const int FadeOutTime = (int)(1000.0f / func.FRAMETIME);
    /// <summary>
    /// �w���v���[�h�ڍs���t�F�[�h�A�E�g�ɂ����鎞��
    /// </summary>
    protected const int HelpFadeOutTime = (int)(250.0f / func.FRAMETIME);
    /// <summary>
    /// �_�Ŏ���
    /// </summary>
    private const int BlinkTime = (int)(1500.0f / func.FRAMETIME);
    /// <summary>
    /// �k����
    /// </summary>
    protected const float ShrinkRate = 0.4f;
    /// <summary>
    /// ��������
    /// </summary>
    protected int time;
    /// <summary>
    /// 
    /// </summary>
    private bool selected;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);
        transform.position = new Vector2(0, StageInfo.ycalc(50));
        BSizeX = func.pxcalc(ButtonPxSizeX) / 2;
        BSizeY = func.pxcalc(ButtonPxSizeY) / 2;
        DiffButtonY[0] = StageInfo.ycalc(110); //2.5
        DiffButtonY[1] = StageInfo.ycalc(80); //0.625
        DiffButtonY[2] = StageInfo.ycalc(50); //-1.25
        DiffButtonY[3] = StageInfo.ycalc(17.5f);

        state = State.Title;
        dspState = DspState.Wait;
        selected = false;

        if(Initializer.GetRetry())
        {
            SetState(State.Select);
            dspState= DspState.Wait;
            selected = true;
            sr.color = new Color(1, 1, 1, 0);
            for(int i = 0; i < 4; i++)
            {
                GameObject button = (GameObject)Resources.Load("DiffButton");
                button = Instantiate(button);
                button.transform.position = new Vector2(0, DiffButtonY[i]);
                button.GetComponent<DiffButton>().SetSprite(i);
            }
        }
    }
    /// <summary>
    /// ��Ԃ��擾����
    /// </summary>
    /// <returns>���</returns>
    public State GetState()
    {
        return state;
    }
    /// <summary>
    /// ��Ԃ�ύX����
    /// </summary>
    /// <param name="state">�ύX��̏��</param>
    public void SetState(State state)
    {
        this.state = state;
    }
    /// <summary>
    /// �\����Ԃ�ύX����
    /// </summary>
    /// <param name="state">�ύX��̕\�����</param>
    public void SetDspState(DspState dspState)
    {
        time = 0;
        this.dspState = dspState;
        if(dspState == DspState.FadeIn2)
        {
            transform.localScale = new Vector2(1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        bool touchOnObj = Application.isEditor ? func.MouseCollision(transform.position, BSizeX, BSizeY, true) : func.MouseCollision(transform.position, BSizeX, BSizeY, true)||func.TouchCollision(transform.position, BSizeX, BSizeY, true);
        bool touched = Application.isEditor ? Input.GetMouseButtonDown(0) : Input.GetMouseButtonDown(0)||func.getTouch() == 1;
        */

        bool touchOnObj = func.MouseCollision(transform.position, BSizeX, BSizeY, true);
        bool touched = Input.GetMouseButtonDown(0);

        Color col = sr.color;
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(func.mouse());
        }
        switch(state)
        {
        case State.Title:
            time++;
            switch(dspState)
            {
            case DspState.Wait:
                break;
            case DspState.FadeIn:
                sr.color = new Color(col.r, col.g, col.b, (float)time / FadeInTime);
                if(time == FadeInTime)
                {
                    SetDspState(DspState.Process);
                }
                break;
            case DspState.FadeIn2:
                sr.color = new Color(col.r, col.g, col.b, (float)time / FadeInTime2);
                if(time == FadeInTime2)
                {
                    SetDspState(DspState.Process);
                }
                break;
            case DspState.Process:
                sr.color = new Color(col.r, col.g, col.b, func.abs(func.sin((float)time / BlinkTime * 180 + 90)));
                if(touchOnObj && touched && Fader.IsEnd())
                {
                    GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.Se.Button);
                    SetDspState(DspState.FadeOut);
                    GameObject.Find("TitleLogo").GetComponent<TitleLogo>().SetState(TitleLogo.State.FadeOut);
                    GameObject.Find("HelpButton").GetComponent<HelpButton>().SetDspState(DspState.FadeOut);
                }
                break;
            case DspState.FadeOut:
                sr.color = new Color(col.r, col.g, col.b, 1.0f - (float)time / FadeOutTime);
                float scale = 1.0f - (1.0f - ShrinkRate) * (float)time / FadeOutTime;
                transform.localScale = new Vector2(scale, scale);
                if(time == FadeOutTime)
                {
                    if(!selected)
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            GameObject button = (GameObject)Resources.Load("DiffButton");
                            button = Instantiate(button);
                            button.transform.position = new Vector2(0, DiffButtonY[i]);
                            button.GetComponent<DiffButton>().SetSprite(i);
                        }
                        selected = true;
                    }
                    state = State.Select;
                    SetDspState(DspState.Invalid);
                }
                break;
            case DspState.HelpFadeOut:
                sr.color = new Color(col.r, col.g, col.b, 1.0f - (float)time / HelpFadeOutTime);
                if(time == HelpFadeOutTime)
                {
                    SetDspState(DspState.Invalid);
                }
                break;
            case DspState.Invalid:
                break;
            }
            break;
        case State.Help:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        case State.Select:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        case State.Caution:
            break;
        }
    }
}
