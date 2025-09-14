using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UIElements;

/// <summary>
/// �G�s���^�[���\��
/// </summary>
public class TurnCounter : MonoBehaviour
{
    /// <summary>
    /// ��Ԉꗗ
    /// </summary>
    private enum State
    {
        /// <summary>�ҋ@</summary>
        Wait,
        /// <summary>�t�F�[�h�C��</summary>
        FadeIn,
        /// <summary>�\����</summary>
        Process,
        /// <summary>�t�F�[�h�A�E�g</summary>
        FadeOut,
        /// <summary>��A�N�e�B�u</summary>
        Invalid,
    };
    /// <summary>
    /// ���
    /// </summary>
    private State state;
    /// <summary>
    /// �^�[���J�E���g�̕\���ʒux���W
    /// </summary>
    public float countOffsetX = 0.5f;
    /// <summary>
    /// �^�[���J�E���g�̕\���ʒuy���W
    /// </summary>
    public float countOffsetY = 0.6f;
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
    /// �Ǐ]��̃G�l�~�[
    /// </summary>
    public Enemy parent;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;

    /// <summary>
    /// �\�����J�n����
    /// </summary>
    public void SetVisibility()
    {
        state = State.FadeIn;
        sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        sr.color = new Color(col.r, col.g, col.b, 0);
        transform.position = new Vector2(parent.transform.position.x + countOffsetX, parent.transform.position.y + countOffsetY);
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
        sr = GetComponent<SpriteRenderer>();
        FadeTime = (int)(FadeMiliSec / func.FRAMETIME);
    }

    // Update is called once per frame
    void Update()
    {
        Color col = sr.color;
        switch(state)
        {
        case State.Wait:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
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
        case State.Invalid:
            break;
        }

        sr.sprite = Font.GetFont(parent.turnCount);
    }
}
