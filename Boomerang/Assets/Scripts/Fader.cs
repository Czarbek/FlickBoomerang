using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �t�F�[�h�Ǘ�
/// </summary>
public class Fader : MonoBehaviour
{
    /// <summary>
    /// �X�v���C�g��x�����̊g�嗦
    /// </summary>
    private const float Sizex = func.camWidth * 4;
    /// <summary>
    /// �X�v���C�g��y�����̊g�嗦
    /// </summary>
    private const float Sizey = func.camHeight * 4;
    /// <summary>
    /// ��Ԉꗗ
    /// </summary>
    private enum State
    {
        Wait,
        FadeOut,
        FadeWait,
        FadeIn,
    };
    /// <summary>
    /// ���
    /// </summary>
    static private State state;
    /// <summary>
    /// ��`�̐F��R�l
    /// </summary>
    private float r;
    /// <summary>
    /// ��`�̐F��G�l
    /// </summary>
    private float g;
    /// <summary>
    /// ��`�̐F��B�l
    /// </summary>
    private float b;
    /// <summary>
    /// ��`�̕s�����x
    /// </summary>
    private float alpha;
    /// <summary>
    /// ��Ԃ��ω����Ă���̌o�ߎ���(�t���[����)
    /// </summary>
    static private int time;
    /// <summary>
    /// �t�F�[�h�A�E�g�ォ��t�F�[�h�C���Ɉڍs����܂ł̑ҋ@����(�t���[����)
    /// </summary>
    static private int waitTime;
    /// <summary>
    /// �J�ڐ�̃V�[����
    /// </summary>
    static private string destination;

    /// <summary>
    /// �t�F�[�h�ɂ����鎞��(�t���[����)
    /// </summary>
    public int FadeTime = 45;
    /// <summary>
    /// �t�F�[�h�A�E�g���Ă���t�F�[�h�C���J�n�܂ł̃f�t�H���g�ҋ@����
    /// </summary>
    static public int FadeWaitTime = 30;

    // Start is called before the first frame update
    void Start()
    {
        
        DontDestroyOnLoad(this);
        state = State.Wait;
        r = 0.0f;
        g = 0.0f;
        b = 0.0f;
        alpha = 0.0f;
        time = 0;
        transform.position = new Vector2(func.SCCX, func.SCCY);
        transform.localScale = new Vector3(Sizex, Sizey, 1.0f);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(r, g, b, alpha);
    }
    /// <summary>
    /// �t�F�[�h�̏�Ԃ�ω�������
    /// </summary>
    /// <param name="waitTime_">�t�F�[�h�A�E�g��̑ҋ@����</param>
    /// <param name="isFadeOut">�t�F�[�h�A�E�g�Ɉڍs���邩�ǂ���</param>
    /// <param name="destinatedScene">�J�ڐ�̃V�[����</param>
    /// <return>�Ȃ�</return>
    static public void SetFader(int waitTime_, bool isFadeOut = true, string destinatedScene = null)
    {
        time = 0;
        waitTime = waitTime_;
        destination = destinatedScene;
        state = isFadeOut ? State.FadeOut : State.FadeIn;
    }
    /// <summary>
    /// �t�F�[�h�������łȂ����ǂ������肷��
    /// </summary>
    /// <returns>�t�F�[�h�������łȂ��Ȃ�true��Ԃ�</returns>
    static public bool IsEnd()
    {
        return state == State.Wait;
    }
    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        time++;
        switch(state)
        {
        case State.Wait:
            break;
        case State.FadeOut:
            alpha = 1.0f / FadeTime * time;
            if(time >= FadeTime)
            {
                time = 0;
                alpha = 1.0f;
                state = State.FadeWait;
            }
            break;
        case State.FadeWait:
            if(time >= waitTime)
            {
                time = 0;
                if(destination != null)
                {
                    SceneManager.LoadScene(destination);
                }
                state = State.FadeIn;
            }
            break;
        case State.FadeIn:
            alpha = 1.0f - 1.0f / FadeTime * time;
            if(time >= FadeTime)
            {
                time = 0;
                waitTime = 0;
                destination = null;
                alpha = 0.0f;
                state = State.Wait;
            }
            break;
        }
        sr.color = new Color(r, g, b, alpha);
    }
}
