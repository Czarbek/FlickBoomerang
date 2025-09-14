using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverTx : MonoBehaviour
{
    /// <summary>
    /// 同種オブジェクト管理用：GAME OVERと表示するなら0
    /// </summary>
    public int index;

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
    private float r;
    /// <summary>
    /// 文字のg値
    /// </summary>
    private float g;
    /// <summary>
    /// 文字のb値
    /// </summary>
    private float b;
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
