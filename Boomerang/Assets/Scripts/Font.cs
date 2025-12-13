using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フォント管理
/// </summary>
public class Font : MonoBehaviour
{
    /// <summary>
    /// 取り扱う文字数
    /// </summary>
    private const int FontNum = 10;
    /// <summary>
    /// 黒塗り数字画像リスト
    /// </summary>
    static Sprite[] font = new Sprite[FontNum];
    /// <summary>
    /// 白抜き数字画像リスト
    /// </summary>
    static Sprite[] fontW = new Sprite[FontNum];
    /// <summary>
    /// 数字画像を取得する
    /// </summary>
    /// <param name="index">数字</param>
    /// <returns>Sprite</returns>
    static public Sprite GetFont(int index)
    {
        return font[index];
    }
    /// <summary>
    /// 白抜き数字画像を取得する
    /// </summary>
    /// <param name="index">数字</param>
    /// <returns>Sprite</returns>
    static public Sprite GetFontW(int index)
    {
        return fontW[index];
    }
    // Start is called before the first frame update
    void Start()
    {
        font[0] = Resources.Load<Sprite>("Font/n_0");
        font[1] = Resources.Load<Sprite>("Font/n_1");
        font[2] = Resources.Load<Sprite>("Font/n_2");
        font[3] = Resources.Load<Sprite>("Font/n_3");
        font[4] = Resources.Load<Sprite>("Font/n_4");
        font[5] = Resources.Load<Sprite>("Font/n_5");
        font[6] = Resources.Load<Sprite>("Font/n_6");
        font[7] = Resources.Load<Sprite>("Font/n_7");
        font[8] = Resources.Load<Sprite>("Font/n_8");
        font[9] = Resources.Load<Sprite>("Font/n_9");

        fontW[0] = Resources.Load<Sprite>("Font/nwhite_0");
        fontW[1] = Resources.Load<Sprite>("Font/nwhite_1");
        fontW[2] = Resources.Load<Sprite>("Font/nwhite_2");
        fontW[3] = Resources.Load<Sprite>("Font/nwhite_3");
        fontW[4] = Resources.Load<Sprite>("Font/nwhite_4");
        fontW[5] = Resources.Load<Sprite>("Font/nwhite_5");
        fontW[6] = Resources.Load<Sprite>("Font/nwhite_6");
        fontW[7] = Resources.Load<Sprite>("Font/nwhite_7");
        fontW[8] = Resources.Load<Sprite>("Font/nwhite_8");
        fontW[9] = Resources.Load<Sprite>("Font/nwhite_9");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
