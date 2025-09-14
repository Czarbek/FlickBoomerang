using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    /// <summary>
    /// �{�^���̎�ވꗗ
    /// </summary>
    public enum ButtonSort
    {
        /// <summary>�R���e�B�j���[����</summary>
        Continue_Yes,
        /// <summary>�R���e�B�j���[���Ȃ�</summary>
        Continue_No,
        /// <summary>���ɐi��</summary>
        Clear_Next,
        /// <summary>�^�C�g���ɖ߂�</summary>
        Clear_Quit,
    };
    /// <summary>
    /// �{�^���̏�Ԉꗗ
    /// </summary>
    public enum State
    {
        /// <summary>��\��</summary>
        Invalid,
        /// <summary>�t�F�[�h�C��</summary>
        FadeIn,
        /// <summary>�\����</summary>
        Process,
        /// <summary>�����ꂽ</summary>
        Pushed,
        /// <summary>������Ȃ�����</summary>
        NotPushed,
    };
    /// <summary>
    /// �{�^���̕\���ʒux
    /// </summary>
    private const float ButtonX = 0;
    /// <summary>
    /// ��{�^���̕\���ʒuy
    /// </summary>
    private const float ButtonY_1 = -1.0f;
    /// <summary>
    /// ���{�^���̕\���ʒuy
    /// </summary>
    private const float ButtonY_2 = -2.5f;
    /// <summary>
    /// �s�N�Z���P�ʂ̉���
    /// </summary>
    private const int ButtonPxSizeX = 600;
    /// <summary>
    /// �s�N�Z���P�ʂ̏c��
    /// </summary>
    private const int ButtonPxSizeY = 240;
    /// <summary>
    /// ����(����)
    /// </summary>
    private readonly float BSizeX = func.pxcalc(ButtonPxSizeX) / 2;
    /// <summary>
    /// �c��(����)
    /// </summary>
    private readonly float BSizeY = func.pxcalc(ButtonPxSizeY) / 2;
    /// <summary>
    /// �{�^���̏��
    /// </summary>
    State state;
    /// <summary>
    /// �{�^���̎��
    /// </summary>
    ButtonSort buttonSort;
    /// <summary>
    /// Component<SpriteRenderer>
    /// </summary>
    SpriteRenderer sr;
    /// <summary>
    /// �摜��r�l
    /// </summary>
    private float r;
    /// <summary>
    /// �摜��g�l
    /// </summary>
    private float g;
    /// <summary>
    /// �摜��b�l
    /// </summary>
    private float b;
    /// <summary>
    /// �摜��alpha�l
    /// </summary>
    private float alpha;
    /// <summary>
    /// ����
    /// </summary>
    private int time;
    /// <summary>
    /// �t�F�[�h�C���ɂ����鎞��
    /// </summary>
    private const int FadeInTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// ��������̃{�^���̃I�u�W�F�N�g
    /// </summary>
    private GameObject partner;
    /// <summary>
    /// �{�^���̎�ނ��Z�b�g����
    /// </summary>
    /// <param name="buttonSort"></param>
    public void SetButton(ButtonSort buttonSort)
    {
        this.buttonSort = buttonSort;
    }
    /// <summary>
    /// �������Z�b�g����
    /// </summary>
    /// <param name="partner"></param>
    public void SetPartner(GameObject partner)
    {
        this.partner = partner;
    }
    /// <summary>
    /// ��Ԃ�����������
    /// </summary>
    /// <param name="state">���</param>
    public void SetState(State state) {
        this.state = state;
    }
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        r = sr.color.r;
        g = sr.color.g;
        b = sr.color.b;
        alpha = 0;
        sr.color = new Color(r, g, b, alpha);
        float x = ButtonX;
        float y = ButtonY_1;

        switch(buttonSort)
        {
        case ButtonSort.Continue_Yes:
            sr.sprite = Resources.Load<Sprite>("button_yes");
            y = ButtonY_1;
            break;
        case ButtonSort.Continue_No:
            sr.sprite = Resources.Load<Sprite>("button_no");
            y = ButtonY_2;
            break;
        case ButtonSort.Clear_Next:
            sr.sprite = Resources.Load<Sprite>("button_next");
            y = ButtonY_1;
            break;
        case ButtonSort.Clear_Quit:
            sr.sprite = Resources.Load<Sprite>("button_quit");
            y = ButtonY_2;
            break;
        }

        transform.position = new Vector2(x, y);

        state = State.FadeIn;
    }

    // Update is called once per frame
    void Update()
    {
        bool touchOnObj = Application.isEditor ? func.MouseCollision(transform.position, BSizeX, BSizeY, true) : func.MouseCollision(transform.position, BSizeX, BSizeY, true)||func.TouchCollision(transform.position, BSizeX, BSizeY, true);
        bool touched = Application.isEditor ? Input.GetMouseButtonDown(0) : Input.GetMouseButtonDown(0) || func.getTouch() == 1;
        switch(state)
        {
        case State.Invalid:
            break;
        case State.FadeIn:
            time++;
            alpha = 1.0f * time / FadeInTime;
            sr.color = new Color(r, g, b, alpha);
            if(time == FadeInTime)
            {
                state = State.Process;
            }
            break;
        case State.Process:
            if(touchOnObj)
            {
                if(touched && Fader.IsEnd())
                {
                    partner.GetComponent<ContinueButton>().SetState(State.NotPushed);
                    state = State.Pushed;
                    switch(buttonSort)
                    {
                    case ButtonSort.Continue_Yes:
                        Fader.SetFader(Fader.FadeWaitTime, true, "Title");
                        break;
                    case ButtonSort.Continue_No:
                        Fader.SetFader(Fader.FadeWaitTime, true, "Title");
                        break;
                    case ButtonSort.Clear_Next:
                        Fader.SetFader(Fader.FadeWaitTime, true, "Title");
                        break;
                    case ButtonSort.Clear_Quit:
                        Fader.SetFader(Fader.FadeWaitTime, true, "Title");
                        break;
                    }
                }
            }
            break;
        case State.Pushed:
            break;
        case State.NotPushed:
            break;
        }
        
    }
}
