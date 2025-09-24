using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タップエフェクト
/// </summary>
public class TapEffect : MonoBehaviour
{
    /// <summary>
    /// 画像の初期不透明度
    /// </summary>
    private const float InitialAlpha = 1.0f;
    /// <summary>
    /// x方向拡大率最大値
    /// </summary>
    private const float MaxScaleX = 0.5f;
    /// <summary>
    /// y方向拡大率最大値
    /// </summary>
    private const float MaxScaleY = 0.5f;
    /// <summary>
    /// 最大まで拡大するまでの時間
    /// </summary>
    private const int ExpandTime = (int)(300.0f / func.FRAMETIME);
    /// <summary>
    /// 経過時間
    /// </summary>
    private int time;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("tapEffect");
        transform.localScale = new Vector2(0, 0);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, InitialAlpha);
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, InitialAlpha * (1 - (float)time / ExpandTime));
        transform.localScale = new Vector2(MaxScaleX * (float)time / ExpandTime, MaxScaleY * (float)time / ExpandTime);
        if(time == ExpandTime)
        {
            Destroy(gameObject);
        }
    }
}
