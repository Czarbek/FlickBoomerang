using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���@�̃p���[�\��
/// </summary>
public class PowerCounter : MonoBehaviour
{
    /// <summary>
    /// �������\������ۂ̕����Ԋu
    /// </summary>
    public float gap;
    /// <summary>
    /// �v���C���[�N���X�̃I�u�W�F�N�g
    /// </summary>
    public Player player;
    /// <summary>
    /// �S������
    /// </summary>
    public int index;
    /// <summary>
    /// ����
    /// </summary>
    private int digit;
    /// <summary>
    /// ���S�ʒux���W
    /// </summary>
    private float centerX;
    /// <summary>
    /// ���S�ʒuy���W
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
