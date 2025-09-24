using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ElementDsp : MonoBehaviour
{
    /// <summary>
    /// 状態一覧
    /// </summary>
    private enum State
    {
        /// <summary>フェードイン</summary>
        FadeIn,
        /// <summary>表示中</summary>
        Process,
        /// <summary>フェードアウト</summary>
        FadeOut,
    };
    /// <summary>
    /// 状態
    /// </summary>
    private State state;
    /// <summary>
    /// 画像のピクセル単位の横幅
    /// </summary>
    private const int PxSize = 440;
    /// <summary>
    /// メートル単位の横幅
    /// </summary>
    public const float MetreSize = 3;
    /// <summary>
    /// 
    /// </summary>
    private float scaleExpandRate;
    /// <summary>
    /// フェードにかかる時間(ミリ秒)
    /// </summary>
    public int FadeMiliSec;
    /// <summary>
    /// フェードにかかる時間(フレーム)
    /// </summary>
    private int FadeTime;
    /// <summary>
    /// フェード時の時間カウント(フレーム)
    /// </summary>
    private int time;
    /// <summary>
    /// 
    /// </summary>
    public GameObject parent;
    /// <summary>
    /// SpriteRederer
    /// </summary>
    private SpriteRenderer sr;

    /// <summary>
    /// 属性を割り当てる
    /// </summary>
    /// <param name="element">属性</param>
    public void SetElement(Enemy.Element element)
    {
        sr = GetComponent<SpriteRenderer>();
        switch(element)
        {
        case Enemy.Element.Fire:
            sr.sprite = Resources.Load<Sprite>("icon_fire");
            break;
        case Enemy.Element.Aqua:
            sr.sprite = Resources.Load<Sprite>("icon_aqua");
            break;
        case Enemy.Element.Leaf:
            sr.sprite = Resources.Load<Sprite>("icon_leaf");
            break;
        }
    }
    /// <summary>
    /// 拡大率の変化率を指定する
    /// </summary>
    /// <param name="rate"></param>
    public void SetExpand(float rate)
    {
        scaleExpandRate = rate;
    }
    /// <summary>
    /// 非表示にする
    /// </summary>
    public void Die()
    {
        state = State.FadeOut;
        time = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.FadeIn;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        transform.localScale = func.scalecalc(MetreSize, PxSize) * scaleExpandRate;

        FadeTime = (int)(FadeMiliSec / func.FRAMETIME);
    }

    // Update is called once per frame
    void Update()
    {
        Color col = sr.color;

        switch(state)
        {
        case State.FadeIn:
            time++;
            sr.color = new Color(col.r, col.g, col.b, (float)time / FadeTime);
            if(time == FadeTime)
            {
                state = State.Process;
            }
            break;
        case State.Process:
            sr.color = new Color(col.r, col.g, col.b, 1);
            break;
        case State.FadeOut:
            time++;
            sr.color = new Color(col.r, col.g, col.b, 1.0f - (float)time / FadeTime);
            if(time == FadeTime)
            {
                Destroy(gameObject);
            }
            break;
        }
    }
}
