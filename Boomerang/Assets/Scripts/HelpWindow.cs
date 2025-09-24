using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpWindow : MonoBehaviour
{
    public enum State
    {
        Invalid,
        FadeIn,
        Process,
        FadeOut,
    };
    private State state;
    /// <summary>
    /// �t�F�[�h�C���ɂ����鎞��
    /// </summary>
    private const int FadeInTime = (int)(100.0f / func.FRAMETIME);
    /// <summary>
    /// �t�F�[�h�A�E�g�ɂ����鎞��
    /// </summary>
    private const int FadeOutTime = (int)(250.0f / func.FRAMETIME);
    /// <summary>
    /// ��������
    /// </summary>
    private int time;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;

    public void SetState(State state)
    {
        time = 0;
        this.state = state;
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);

        SetState(State.Invalid);
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        switch(state)
        {
        case State.Invalid:
            break;
        case State.FadeIn:
            sr.color = new Color(1, 1, 1, (float)time / FadeInTime);
            if(time == FadeInTime)
            {
                SetState(State.Process);
            }
            break;
        case State.Process:
            break;
        case State.FadeOut:
            sr.color = new Color(1, 1, 1, 1.0f - (float)time / FadeInTime);
            if(time == FadeOutTime)
            {
                SetState(State.Invalid);
            }
            break;
        }
    }
}
