using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEffect : MonoBehaviour
{
    /// <summary>
    /// �\���ʒux���W
    /// </summary>
    private const float CenterX = 0;
    /// <summary>
    /// �\���ʒuy���W
    /// </summary>
    private const float CenterY = 0;
    /// <summary>
    /// �J�n���A���t�@�l
    /// </summary>
    private const float InitialAlpha = 0;
    /// <summary>
    /// �\������
    /// </summary>
    private const int DspTime = (int)(600.0f / func.FRAMETIME);
    /// <summary>
    /// �o�ߎ���
    /// </summary>
    static private int time;
    /// <summary>
    /// �\�������ǂ���
    /// </summary>
    static private bool dsp;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    
    /// <summary>
    /// �\�����J�n����
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
