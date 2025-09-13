using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    /// <summary>
    /// ボタンの種類一覧
    /// </summary>
    public enum ButtonSort
    {
        /// <summary>コンティニューする</summary>
        Continue_Yes,
        /// <summary>コンティニューしない</summary>
        Continue_No,
        /// <summary>次に進む</summary>
        Clear_Next,
        /// <summary>タイトルに戻る</summary>
        Clear_Quit,
    };
    /// <summary>
    /// ボタンの状態一覧
    /// </summary>
    public enum State
    {
        /// <summary>非表示</summary>
        Invalid,
        /// <summary>フェードイン</summary>
        FadeIn,
        /// <summary>押された</summary>
        Pushed,
        /// <summary>押されなかった</summary>
        NotPushed,
    };
    /// <summary>
    /// ボタンの種類
    /// </summary>
    ButtonSort buttonSort;
    /// <summary>
    /// Component<SpriteRenderer>
    /// </summary>
    SpriteRenderer sp;
    /// <summary>
    /// 画像のr値
    /// </summary>
    private float r;
    /// <summary>
    /// 画像のg値
    /// </summary>
    private float g;
    /// <summary>
    /// 画像のb値
    /// </summary>
    private float b;
    /// <summary>
    /// 画像のalpha値
    /// </summary>
    private int alpha;
    public void SetButton(ButtonSort buttonSort)
    {
        this.buttonSort = buttonSort;
    }
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        r = sp.color.r;
        g = sp.color.g;
        b = sp.color.b;
        alpha = 0;
        sp.color = new Color(r, g, b, alpha);

        switch(buttonSort)
        {
        case ButtonSort.Continue_Yes:
            sp.sprite = (Sprite)Resources.Load("button_yes");
            break;
        case ButtonSort.Continue_No:
            sp.sprite = (Sprite)Resources.Load("button_no");
            break;
        case ButtonSort.Clear_Next:
            sp.sprite = (Sprite)Resources.Load("button_next");
            break;
        case ButtonSort.Clear_Quit:
            sp.sprite = (Sprite)Resources.Load("button_quit");
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(buttonSort)
        {
        case ButtonSort.Continue_Yes:
            break;
        case ButtonSort.Continue_No:
            break;
        case ButtonSort.Clear_Next:
            break;
        case ButtonSort.Clear_Quit:
            break;
        }
    }
}
