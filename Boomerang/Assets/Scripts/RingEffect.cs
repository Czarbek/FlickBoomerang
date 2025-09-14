using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEffect : MonoBehaviour
{
    /// <summary>
    /// 表示位置x座標
    /// </summary>
    private const float CenterX = 0;
    /// <summary>
    /// 表示位置y座標
    /// </summary>
    private const float CenterY = 0;
    /// <summary>
    /// 開始時アルファ値
    /// </summary>
    private const float InitialAlpha = 0;
    /// <summary>
    /// 表示時間
    /// </summary>
    private const int DspTime = (int)(600.0f / func.FRAMETIME);
    /// <summary>
    /// 経過時間
    /// </summary>
    static private int time;
    /// <summary>
    /// 表示中かどうか
    /// </summary>
    static private bool dsp;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    
    /// <summary>
    /// 表示を開始する
    /// </summary>
    static public void SetDsp()
    {
        time = 0;
        dsp = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(CenterX, CenterY);
        sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        sr.color = new Color(c.r, c.g, c.b, InitialAlpha);

        time = 0;
        dsp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(dsp)
        {
            time++;
            Color c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 1.0f - ((float)time / DspTime));
            if(time == DspTime)
            {
                dsp = false;
            }
        }
    }
}
