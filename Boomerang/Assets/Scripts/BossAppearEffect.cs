using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボス登場時エフェクト
/// </summary>
public class BossAppearEffect : MonoBehaviour
{
    /// <summary>
    /// 画像の不透明度
    /// </summary>
    private const float Alpha = 0.2f;
    /// <summary>
    /// x方向拡大率最大値
    /// </summary>
    private const float MaxScaleX = 3.0f;
    /// <summary>
    /// y方向拡大率最大値
    /// </summary>
    private const float MaxScaleY = 3.0f;
    /// <summary>
    /// 最大まで拡大するまでの時間
    /// </summary>
    private const int ExpandTime = (int)(1000.0f / func.FRAMETIME);
    /// <summary>
    /// 経過時間
    /// </summary>
    private int time;
    /// <summary>
    /// 何個目か
    /// </summary>
    private int index;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    /// <summary>
    /// 依存先のボスオブジェクト
    /// </summary>
    private GameObject parent;

    /// <summary>
    /// indexをセットする
    /// </summary>
    /// <param name="index">indexの値</param>
    /// <param name="parent">依存先のオブジェクト</param>
    public void SetIndex(int index, GameObject parent)
    {
        this.index = index;
        this.parent = parent;
    }
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("BossAppearEffect_");
        transform.localScale = new Vector2(0, 0);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Alpha);
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        transform.localScale = new Vector2(MaxScaleX * (float)time / ExpandTime, MaxScaleY * (float)time / ExpandTime);
        if(time == ExpandTime)
        {
            if(index == Enemy.WaveNum - 1)
            {
                parent.GetComponent<Enemy>().EndWave();
            }
            Destroy(gameObject);
        }
    }
}
