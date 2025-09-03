using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル画面管理、スタートボタン
/// </summary>
public class TitleManager : MonoBehaviour
{
    /// <summary>
    /// ピクセル単位の横幅
    /// </summary>
    private const int ButtonPxSizeX = 640;
    /// <summary>
    /// ピクセル単位の縦幅
    /// </summary>
    private const int ButtonPxSizeY = 190;
    /// <summary>
    /// 横幅
    /// </summary>
    public float BSizeX;
    /// <summary>
    /// 縦幅
    /// </summary>
    public float BSizeY;
    /// <summary>
    /// 難易度ボタンのy座標
    /// </summary>
    private readonly float[] DiffButtonY = new float[4];
    /// <summary>
    /// 表示状態一覧
    /// </summary>
    public enum State
    {
        /// <summary>タイトル</summary>
        Title,
        /// <summary>ヘルプ</summary>
        Help,
        /// <summary>難易度選択</summary>
        Select,
    };
    /// <summary>
    /// 表示状態
    /// </summary>
    public State state;
    /// <summary>
    /// 
    /// </summary>
    private bool selected;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = func.FRAMERATE;

        BSizeX = (float)ButtonPxSizeX / func.SCW * func.camWidth * 4 * transform.localScale.x;
        BSizeY = (float)ButtonPxSizeY / func.SCH * func.camHeight * 4 * transform.localScale.y;
        DiffButtonY[0] = 2.0f;
        DiffButtonY[1] = 0.5f;
        DiffButtonY[2] = -1.0f;
        DiffButtonY[3] = -2.5f;

        state = State.Title;
        selected = false;
    }
    /// <summary>
    /// 状態を取得する
    /// </summary>
    /// <returns>状態</returns>
    public State GetState()
    {
        return state;
    }
    /// <summary>
    /// 状態を変更する
    /// </summary>
    /// <param name="state">変更先の状態</param>
    public void SetState(State state)
    {
        this.state = state;
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(func.mouse());
        }
        switch(state)
        {
        case State.Title:
            sr.color = new Color(col.r, col.g, col.b, 1);
            if(func.MouseCollision(transform.position, BSizeX, BSizeY, true) && Input.GetMouseButtonDown(0) && Fader.IsEnd())
            {
                state = State.Select;
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
            }
            break;
        case State.Help:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        case State.Select:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        }
    }
}
