using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caution : MonoBehaviour
{
    /// <summary>
    /// �s�N�Z���P�ʂ̉���
    /// </summary>
    private const int PxSizeX = 820;
    /// <summary>
    /// �s�N�Z���P�ʂ̏c��
    /// </summary>
    private const int PxSizeY = 990;
    /// <summary>
    /// ����(����)
    /// </summary>
    private float WSizeX;
    /// <summary>
    /// �c��(����)
    /// </summary>
    private float WSizeY;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    static private SpriteRenderer sr;
    /// <summary>
    /// �摜���X�g
    /// </summary>
    static private Sprite[] spriteList = new Sprite[2];
    /// <summary>
    /// �\�������ǂ���
    /// </summary>
    static private bool dsp;

    /// <summary>
    /// �\������
    /// </summary>
    /// <param name="index">�摜���X�g�̃C���f�b�N�X</param>
    static public void SetVisibility(int index)
    {
        sr.color = new Color(1, 1, 1, 1);
        sr.sprite = spriteList[index];
        dsp = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);
        spriteList[0] = Resources.Load<Sprite>("caution_2");
        spriteList[1] = Resources.Load<Sprite>("caution_3");
        WSizeX = func.pxcalc(PxSizeX) / 2;
        WSizeY = func.pxcalc(PxSizeY) / 2;
    }

    // Update is called once per frame
    void Update()
    {

        if(dsp)
        {
            /*
            bool touchOnObj = Application.isEditor ? func.MouseCollision(transform.position, BSizeX, BSizeY, true) : func.MouseCollision(transform.position, BSizeX, BSizeY, true)||func.TouchCollision(transform.position, BSizeX, BSizeY, true);
            bool touched = Application.isEditor ? Input.GetMouseButtonDown(0) : Input.GetMouseButtonDown(0)||func.getTouch() == 1;
            */

            bool touchOnObj = func.MouseCollision(transform.position, WSizeX, WSizeY, true);
            bool touched = Input.GetMouseButtonDown(0);

            if(!touchOnObj && touched)
            {
                GameObject.Find("TitleManager").GetComponent<TitleManager>().SetState(TitleManager.State.Select);
                sr.color = new Color(1, 1, 1, 0);
                dsp = false;
            }
        }
    }
}