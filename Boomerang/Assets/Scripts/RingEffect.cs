using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEffect : MonoBehaviour
{
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
    static private SpriteRenderer sr;
    /// <summary>
    /// �v���C���[�I�u�W�F�N�g
    /// </summary>
    private GameObject player;

    /// <summary>
    /// �\�����J�n����
    /// </summary>
    /// <param name="sort">�A�C�e���̎��</param>
    /// <param name="element">�N���X�^���̑���</param>
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
