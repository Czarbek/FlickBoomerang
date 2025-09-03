using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機のパワー表示
/// </summary>
public class PowerCounter : MonoBehaviour
{
    /// <summary>
    /// 複数桁表示する際の文字間隔
    /// </summary>
    public float gap;
    /// <summary>
    /// プレイヤークラスのオブジェクト
    /// </summary>
    public Player player;
    /// <summary>
    /// 担当桁数
    /// </summary>
    public int index;
    /// <summary>
    /// 桁数
    /// </summary>
    private int digit;
    /// <summary>
    /// 中心位置x座標
    /// </summary>
    private float centerX;
    /// <summary>
    /// 中心位置y座標
    /// </summary>
    private float centerY;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        digit = 0;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color col = sr.color;
        int power = player.power;
        int dspPower = 0;
        digit = 0;
        while(power > 0)
        {
            if(digit == index) dspPower = power % 10;
            digit++;
            power /= 10;
        }
        if(digit <= index)
        {
            sr.color = new Color(col.r, col.g, col.b, 0);
        }
        else
        {
            sr.color = new Color(col.r, col.g, col.b, 1);
        }
        switch(digit)
        {
        case 1:
            centerX = player.transform.position.x;
            break;
        case 2:
            centerX = player.transform.position.x + gap / 2 - gap * index;
            break;
        case 3:
            centerX = player.transform.position.x + gap - gap * index;
            break;
        case 4:
            centerX = player.transform.position.x + gap * 3 / 2 - gap * index;
            break;
        default:
            break;
        }
        sr.sprite = Font.GetFontW(dspPower);
        centerY = player.transform.position.y;
        transform.position = new Vector2(centerX, centerY);
    }
}
