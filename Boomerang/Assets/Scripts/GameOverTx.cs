using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverTx : MonoBehaviour
{
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
    private const float Red = 1.0f;
    /// <summary>
    /// ������g�l
    /// </summary>
    private const float Green = 0.0f;
    /// <summary>
    /// ������b�l
    /// </summary>
    private const float Blue = 0.0f;
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
        GetComponent<TextMeshProUGUI>().color = new Color(Red, Green, Blue, alpha);
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
            GetComponent<TextMeshProUGUI>().color = new Color(Red, Green, Blue, alpha);
            if(time == FadeInTime)
            {
                GetComponent<TextMeshProUGUI>().color = new Color(Red, Green, Blue, 1.0f);
                time = 0;
                state = State.Display;
            }
            break;
        case State.Display:
            if(time == DisplayWaitTime)
            {

            }
            break;
        }
    }
}
