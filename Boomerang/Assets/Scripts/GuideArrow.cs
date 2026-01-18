using UnityEngine;

/// <summary>
/// チュートリアル用矢印
/// </summary>
public class GuideArrow : MonoBehaviour
{
    /// <summary>
    /// 不透明度最大値
    /// </summary>
    private const float MaxAlpha = 1.0f;
    /// <summary>
    /// 不透明度最小値
    /// </summary>
    private const float MinAlpha = 0.0f;
    /// <summary>
    /// 点滅周期
    /// </summary>
    private readonly float Frequency = 2.0f;
    /// <summary>
    /// 持続時間(ミリ秒)
    /// </summary>
    private int SustainTimeMiliSec = 5000;
    /// <summary>
    /// 持続時間(フレーム数)
    /// </summary>
    private int SustainTime;
    /// <summary>
    /// 処理時間
    /// </summary>
    private int time;
    /// <summary>
    /// SpriteRendererコンポーネント
    /// </summary>
    private SpriteRenderer sr;

    /// <summary>
    /// 角度を変更する
    /// </summary>
    /// <param name="angle">角度(度数法)</param>
    public void SetAngle(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = 0;
        sr = GetComponent<SpriteRenderer>();
        SustainTime = (int)(SustainTimeMiliSec / func.FRAMETIME);
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        sr.color = new Color(1, 1, 1, MinAlpha+(MaxAlpha-MinAlpha)*(func.sin(time*Frequency)+1.0f) / 2.0f);
        if(time == SustainTime)
        {
            Destroy(gameObject);
        }
    }
}
