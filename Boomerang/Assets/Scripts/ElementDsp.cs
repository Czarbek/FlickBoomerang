using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ElementDsp : MonoBehaviour
{
    /// <summary>
    /// ��Ԉꗗ
    /// </summary>
    private enum State
    {
        /// <summary>�t�F�[�h�C��</summary>
        FadeIn,
        /// <summary>�\����</summary>
        Process,
        /// <summary>�t�F�[�h�A�E�g</summary>
        FadeOut,
    };
    /// <summary>
    /// ���
    /// </summary>
    private State state;
    /// <summary>
    /// �摜�̃s�N�Z���P�ʂ̉���
    /// </summary>
    private const int PxSize = 440;
    /// <summary>
    /// ���[�g���P�ʂ̉���
    /// </summary>
    public const float MetreSize = 3;
    /// <summary>
    /// 
    /// </summary>
    private float scaleExpandRate;
    /// <summary>
    /// �t�F�[�h�ɂ����鎞��(�~���b)
    /// </summary>
    public int FadeMiliSec;
    /// <summary>
    /// �t�F�[�h�ɂ����鎞��(�t���[��)
    /// </summary>
    private int FadeTime;
    /// <summary>
    /// �t�F�[�h���̎��ԃJ�E���g(�t���[��)
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
    /// ���������蓖�Ă�
    /// </summary>
    /// <param name="element">����</param>
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
    /// �g�嗦�̕ω������w�肷��
    /// </summary>
    /// <param name="rate"></param>
    public void SetExpand(float rate)
    {
        scaleExpandRate = rate;
    }
    /// <summary>
    /// ��\���ɂ���
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
