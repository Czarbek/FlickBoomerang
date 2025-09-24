using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム初期設定
/// </summary>
public class Initializer : MonoBehaviour
{
    /// <summary>
    /// 初期化済みのときtrue
    /// </summary>
    static private bool initialized;

    /// <summary>
    /// リトライ、次に進むを選んだ場合trueにする
    /// </summary>
    static private bool retry;


    /// <summary>
    /// リングの画像リスト
    /// </summary>
    static private List<Sprite> ringList;

    /// <summary>
    /// リングの画像を取得する
    /// </summary>
    /// <param name="power">リングのパワー</param>
    /// <returns>リングの画像(Sprite)</returns>
    static public Sprite GetRingImg(int power)
    {
        return ringList[power - 1];
    }
    /// <summary>
    /// リトライ判定を更新する
    /// </summary>
    static public void SetRetry(bool goRetry)
    {
        retry = goRetry;
    }
    /// <summary>
    /// リトライ判定を取得する
    /// </summary>
    static public bool GetRetry()
    {
        return retry;
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = func.FRAMERATE;
        ClearData.Initialize();
        initialized = false;

        ringList = new List<Sprite>
        {
            Resources.Load<Sprite>("ring_01"),
            Resources.Load<Sprite>("ring_02"),
            Resources.Load<Sprite>("ring_03"),
            Resources.Load<Sprite>("ring_04"),
            Resources.Load<Sprite>("ring_05"),
            Resources.Load<Sprite>("ring_06"),
            Resources.Load<Sprite>("ring_07"),
            Resources.Load<Sprite>("ring_08"),
            Resources.Load<Sprite>("ring_09"),
            Resources.Load<Sprite>("ring_10")
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(!initialized)
        {
            Fader.SetFader(0, true, "Title");
            initialized = true;
        }
        
    }
}
