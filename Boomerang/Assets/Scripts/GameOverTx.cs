using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverTx : MonoBehaviour
{
    /// <summary>
    /// ����I�u�W�F�N�g�Ǘ��p�FGAME OVER�ƕ\������Ȃ�0
    /// </summary>
    public int index;

    /// <summary>
    /// �\����Ԉꗗ
    /// </summary>
    private enum State {
        /// <summary>�\���O�ҋ@</summary>
        Wait,
        /// <summary>�t�F�[�h�C��</summary>
        FadeIn,
        /// <summary>�\����</summary>
        Display,
    };
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
    /// �t�F�[�h�C����ҋ@����
    /// </summary>
    private const int DisplayWaitTime = (int)(1000.0f / func.FRAMETIME);

    public void SetText()
    {
        state = State.FadeIn;
        time = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Wait;
        alpha = 0;
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
                state = State.Display;
            }
            break;
        case State.Display:
            if(time == DisplayWaitTime)
            {
                if(index == 0)
                {
                    GameObject.Find("ContinueTx").GetComponent<GameOverTx>().SetText();
                    GameObject continueButtonYes = Instantiate((GameObject)Resources.Load("ContinueButton"));
                    continueButtonYes.GetComponent<ContinueButton>().SetButton(ContinueButton.ButtonSort.Continue_Yes);
                    GameObject continueButtonNo = Instantiate((GameObject)Resources.Load("ContinueButton"));
                    continueButtonNo.GetComponent<ContinueButton>().SetButton(ContinueButton.ButtonSort.Continue_No);
                    continueButtonYes.GetComponent<ContinueButton>().SetPartner(continueButtonNo);
                    continueButtonNo.GetComponent<ContinueButton>().SetPartner(continueButtonYes);
                }
            }
            break;
        }
    }
}
