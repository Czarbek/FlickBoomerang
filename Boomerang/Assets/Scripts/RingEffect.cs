using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEffect : MonoBehaviour
{
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
    static private SpriteRenderer sr;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject player;

    /// <summary>
    /// 表示を開始する
    /// </summary>
    /// <param name="sort">アイテムの種類</param>
    /// <param name="element">クリスタルの属性</param>
    static public void SetDsp()
    {
        sr.sprite = Resources.Load<Sprite>("RingEffect_");
        time = 0;
        dsp = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        sr.color = new Color(c.r, c.g, c.b, InitialAlpha);

        transform.position = player.transform.position;
        time = 0;
        dsp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(dsp)
        {
            time++;
            transform.position = player.transform.position;
            Color c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 1.0f - ((float)time / DspTime));
            if(time == DspTime)
            {
                dsp = false;
            }
        }
    }
}
