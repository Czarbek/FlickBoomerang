using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�X�o�ꎞ�G�t�F�N�g
/// </summary>
public class BossAppearEffect : MonoBehaviour
{
    /// <summary>
    /// �摜�̕s�����x
    /// </summary>
    private const float Alpha = 0.2f;
    /// <summary>
    /// x�����g�嗦�ő�l
    /// </summary>
    private const float MaxScaleX = 3.0f;
    /// <summary>
    /// y�����g�嗦�ő�l
    /// </summary>
    private const float MaxScaleY = 3.0f;
    /// <summary>
    /// �ő�܂Ŋg�傷��܂ł̎���
    /// </summary>
    private const int ExpandTime = (int)(1000.0f / func.FRAMETIME);
    /// <summary>
    /// �o�ߎ���
    /// </summary>
    private int time;
    /// <summary>
    /// ���ڂ�
    /// </summary>
    private int index;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    /// <summary>
    /// �ˑ���̃{�X�I�u�W�F�N�g
    /// </summary>
    private GameObject parent;

    /// <summary>
    /// index���Z�b�g����
    /// </summary>
    /// <param name="index">index�̒l</param>
    /// <param name="parent">�ˑ���̃I�u�W�F�N�g</param>
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
