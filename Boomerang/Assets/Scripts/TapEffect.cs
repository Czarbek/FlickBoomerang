using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�b�v�G�t�F�N�g
/// </summary>
public class TapEffect : MonoBehaviour
{
    /// <summary>
    /// �摜�̏����s�����x
    /// </summary>
    private const float InitialAlpha = 1.0f;
    /// <summary>
    /// x�����g�嗦�ő�l
    /// </summary>
    private const float MaxScaleX = 0.5f;
    /// <summary>
    /// y�����g�嗦�ő�l
    /// </summary>
    private const float MaxScaleY = 0.5f;
    /// <summary>
    /// �ő�܂Ŋg�傷��܂ł̎���
    /// </summary>
    private const int ExpandTime = (int)(300.0f / func.FRAMETIME);
    /// <summary>
    /// �o�ߎ���
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
