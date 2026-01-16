using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// ゲームクリア時テキスト
/// </summary>
public class ClearTx : MonoBehaviour
{
    /// <summary>
    /// 表示状態一覧
    /// </summary>
    private enum State
    {
        /// <summary>表示前待機</summary>
        Wait,
        /// <summary>フェードイン</summary>
        FadeIn,
        /// <summary>ジャンプ中</summary>
        Jump,
        /// <summary>表示中</summary>
        Display,
    };
    /// <summary>
    /// 現在のステージ
    /// </summary>
    private int stage;
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
    /// ジャンプ後待機時間
    /// </summary>
    private const int DisplayWaitTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// ジャンプから着地までの時間
    /// </summary>
    private const int JumpTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// 着地から次のジャンプまでの時間
    /// </summary>
    private const int JumpGapTime = (int)(250.0f / func.FRAMETIME);
    /// <summary>
    /// ジャンプ回数
    /// </summary>
    private const int JumpNum = 3;
    /// <summary>
    /// ジャンプした回数
    /// </summary>
    private int jumpNum;
    /// <summary>
    /// ジャンプの高さ
    /// </summary>
    private const float JumpHeight = 1.0f;
    /// <summary>
    /// 着地地点のx座標
    /// </summary>
    private float standardx;
    /// <summary>
    /// 着地地点のy座標
    /// </summary>
    private float standardy;
    /// <summary>
    /// ステージ番号をセットする
    /// </summary>
    /// <param name="stage">ステージ</param>
    public void SetStage(int stage)
    {
        this.stage = stage;
    }
    /// <summary>
    /// テキスト表示を開始する
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
