using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AscendTx : MonoBehaviour
{
    /// <summary>
    /// 表示状態一覧
    /// </summary>
    private enum State
    {
        Wait,
        FadeIn,
        Process,
        FadeOut,
    }
    /// <summary>
    /// 表示状態
    /// </summary>
    private State state;
    /// <summary>
    /// 時間
    /// </summary>
    private int time;
    /// <summary>
    /// フェードにかかる時間
    /// </summary>
    private const int FadeTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// 表示時間
    /// </summary>
    private const int DspTime = (int)(2000.0f / func.FRAMETIME);
    /// <summary>
    /// 
    /// </summary>
    private TextMeshProUGUI tmpro;

    /// <summary>
    /// テキスト表示を開始する
    /// </summary>
    public void SetText()
    {
        time = 0;
        state = State.FadeIn;
    }
    /// <summary>
    /// テキスト表示中でないかどうかを判定する
    /// </summary>
    /// <returns></returns>
    public bool EndText()
    {
        return state == State.Wait;
    }

    // Start is called before the first frame update
    void Start()
    {
        tmpro = GetComponent<TextMeshProUGUI>();

        time = 0;
        state = State.Wait;
        tmpro.color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
        case State.Wait:
            break;
        case State.FadeIn:
            time++;
            tmpro.color = new Color(0, 0, 0, (float)time / FadeTime);
            if(time == FadeTime)
            {
                time = 0;
                state = State.Process;
            }
            break;
        case State.Process:
            time++;
            if(time == DspTime)
            {
                time = 0;
                state = State.FadeOut;
            }
            break;
        case State.FadeOut:
            time++;
            tmpro.color = new Color(0, 0, 0, 1.0f - (float)time / FadeTime);
            if(time == FadeTime)
            {
                time = 0;
                tmpro.color = new Color(0, 0, 0, 0);
                state = State.Wait;
            }
            break;
        }
    }
}
