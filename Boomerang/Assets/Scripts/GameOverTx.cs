using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverTx : MonoBehaviour
{
    /// <summary>
    /// 表示状態一覧
    /// </summary>
    private enum State {
        /// <summary>表示前待機</summary>
        Wait,
        /// <summary>フェードイン</summary>
        FadeIn,
        /// <summary>表示中</summary>
        Display,
    };
    /// <summary>
    /// 文字のr値
    /// </summary>
    private const float Red = 1.0f;
    /// <summary>
    /// 文字のg値
    /// </summary>
    private const float Green = 0.0f;
    /// <summary>
    /// 文字のb値
    /// </summary>
    private const float Blue = 0.0f;
    /// <summary>
    /// 文字のalpha値
    /// </summary>
    private float alpha;
    /// <summary>
    /// 表示状態
    /// </summary>
    private State state;
    /// <summary>
    /// 時間
    /// </summary>
    private int time;
    /// <summary>
    /// フェードインにかかる時間
    /// </summary>
    private const int FadeInTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// フェードイン後待機時間
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
