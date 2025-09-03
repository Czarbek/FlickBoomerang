using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�C�g����ʊǗ��A�X�^�[�g�{�^��
/// </summary>
public class TitleManager : MonoBehaviour
{
    /// <summary>
    /// �s�N�Z���P�ʂ̉���
    /// </summary>
    private const int ButtonPxSizeX = 640;
    /// <summary>
    /// �s�N�Z���P�ʂ̏c��
    /// </summary>
    private const int ButtonPxSizeY = 190;
    /// <summary>
    /// ����
    /// </summary>
    public float BSizeX;
    /// <summary>
    /// �c��
    /// </summary>
    public float BSizeY;
    /// <summary>
    /// ��Փx�{�^����y���W
    /// </summary>
    private readonly float[] DiffButtonY = new float[4];
    /// <summary>
    /// �\����Ԉꗗ
    /// </summary>
    public enum State
    {
        /// <summary>�^�C�g��</summary>
        Title,
        /// <summary>�w���v</summary>
        Help,
        /// <summary>��Փx�I��</summary>
        Select,
    };
    /// <summary>
    /// �\�����
    /// </summary>
    public State state;
    /// <summary>
    /// 
    /// </summary>
    private bool selected;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = func.FRAMERATE;

        BSizeX = (float)ButtonPxSizeX / func.SCW * func.camWidth * 4 * transform.localScale.x;
        BSizeY = (float)ButtonPxSizeY / func.SCH * func.camHeight * 4 * transform.localScale.y;
        DiffButtonY[0] = 2.0f;
        DiffButtonY[1] = 0.5f;
        DiffButtonY[2] = -1.0f;
        DiffButtonY[3] = -2.5f;

        state = State.Title;
        selected = false;
    }
    /// <summary>
    /// ��Ԃ��擾����
    /// </summary>
    /// <returns>���</returns>
    public State GetState()
    {
        return state;
    }
    /// <summary>
    /// ��Ԃ�ύX����
    /// </summary>
    /// <param name="state">�ύX��̏��</param>
    public void SetState(State state)
    {
        this.state = state;
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(func.mouse());
        }
        switch(state)
        {
        case State.Title:
            sr.color = new Color(col.r, col.g, col.b, 1);
            if(func.MouseCollision(transform.position, BSizeX, BSizeY, true) && Input.GetMouseButtonDown(0) && Fader.IsEnd())
            {
                state = State.Select;
                if(!selected)
                {
                    for(int i = 0; i < 4; i++)
                    {
                        GameObject button = (GameObject)Resources.Load("DiffButton");
                        button = Instantiate(button);
                        button.transform.position = new Vector2(0, DiffButtonY[i]);
                        button.GetComponent<DiffButton>().SetSprite(i);
                    }
                    selected = true;
                }
            }
            break;
        case State.Help:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        case State.Select:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        }
    }
}
