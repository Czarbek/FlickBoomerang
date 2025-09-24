using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ClearTx : MonoBehaviour
{
    /// <summary>
    /// �\����Ԉꗗ
    /// </summary>
    private enum State
    {
        /// <summary>�\���O�ҋ@</summary>
        Wait,
        /// <summary>�t�F�[�h�C��</summary>
        FadeIn,
        /// <summary>�W�����v��</summary>
        Jump,
        /// <summary>�\����</summary>
        Display,
    };
    /// <summary>
    /// ���݂̃X�e�[�W
    /// </summary>
    private int stage;
    /// <summary>
    /// ������r�l
    /// </summary>
    private float r;
    /// <summary>
    /// ������g�l
    /// </summary>
    private float g;
    /// <summary>
    /// ������b�l
    /// </summary>
    private float b;
    /// <summary>
    /// ������alpha�l
    /// </summary>
    private float alpha;
    /// <summary>
    /// �\�����
    /// </summary>
    private State state;
    /// <summary>
    /// ����
    /// </summary>
    private int time;
    /// <summary>
    /// �t�F�[�h�C���ɂ����鎞��
    /// </summary>
    private const int FadeInTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// �W�����v��ҋ@����
    /// </summary>
    private const int DisplayWaitTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// �W�����v���璅�n�܂ł̎���
    /// </summary>
    private const int JumpTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// ���n���玟�̃W�����v�܂ł̎���
    /// </summary>
    private const int JumpGapTime = (int)(250.0f / func.FRAMETIME);
    /// <summary>
    /// �W�����v��
    /// </summary>
    private const int JumpNum = 3;
    /// <summary>
    /// �W�����v������
    /// </summary>
    private int jumpNum;
    /// <summary>
    /// �W�����v�̍���
    /// </summary>
    private const float JumpHeight = 1.0f;
    /// <summary>
    /// ���n�n�_��x���W
    /// </summary>
    private float standardx;
    /// <summary>
    /// ���n�n�_��y���W
    /// </summary>
    private float standardy;
    /// <summary>
    /// �X�e�[�W�ԍ����Z�b�g����
    /// </summary>
    /// <param name="stage">�X�e�[�W</param>
    public void SetStage(int stage)
    {
        this.stage = stage;
    }
    /// <summary>
    /// �e�L�X�g�\�����J�n����
    /// </summary>
    public void SetText()
    {
        time = 0;
        state = State.FadeIn;
        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusic(MusicManager.BGM.GameClear);
    }
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        jumpNum = 0;
        state = State.Wait;
        alpha = 0;
        standardx = transform.position.x;
        standardy = transform.position.y;
        TextMeshProUGUI tmpro = GetComponent<TextMeshProUGUI>();
        Color col = tmpro.color;
        r = col.r;
        g = col.g;
        b = col.b;
        tmpro.color = new Color(r, g, b, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        switch(state)
        {
        case State.Wait:
            break;
        case State.FadeIn:
            alpha = 1.0f * time / FadeInTime;
            GetComponent<TextMeshProUGUI>().color = new Color(r, g, b, alpha);
            if(time == FadeInTime)
            {
                GetComponent<TextMeshProUGUI>().color = new Color(r, g, b, 1.0f);
                time = 0;
                state = State.Jump;
            }
            break;
        case State.Jump:
            if(time <= JumpTime)
            {
                float y = standardy + func.sin((float)time / JumpTime * 180.0f) * JumpHeight;
                transform.position = new Vector2(standardx, y);
            }
            else if(time == JumpTime + JumpGapTime)
            {
                jumpNum++;
                time = 0;
                if(jumpNum == JumpNum)
                {
                    ClearData.SetClear(stage);
                    state = State.Display;
                }
            }
            break;
        case State.Display:
            if(time == DisplayWaitTime)
            {
                GameObject continueButtonNext = Instantiate((GameObject)Resources.Load("ContinueButton"));
                continueButtonNext.GetComponent<ContinueButton>().SetButton(ContinueButton.ButtonSort.Clear_Next);
                GameObject continueButtonQuit = Instantiate((GameObject)Resources.Load("ContinueButton"));
                continueButtonQuit.GetComponent<ContinueButton>().SetButton(ContinueButton.ButtonSort.Clear_Quit);
                continueButtonNext.GetComponent<ContinueButton>().SetPartner(continueButtonQuit);
                continueButtonQuit.GetComponent<ContinueButton>().SetPartner(continueButtonNext);
            }
            break;
        }
    }
}
