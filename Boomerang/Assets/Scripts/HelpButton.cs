using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �w���v�\���{�^��
/// </summary>
public class HelpButton : TitleManager
{
    /// <summary>
    /// �s�N�Z���P�ʂ̉���
    /// </summary>
    private const int ButtonPxSizeX = 128;
    /// <summary>
    /// �s�N�Z���P�ʂ̏c��
    /// </summary>
    private const int ButtonPxSizeY = 128;
    /// <summary>
    /// TitleManager�I�u�W�F�N�g(��ԓ����p)
    /// </summary>
    private GameObject manager;
    /// <summary>
    /// �w���v�{�^���̃X�v���C�g
    /// </summary>
    private Sprite sp_help;
    /// <summary>
    /// ����{�^���̃X�v���C�g
    /// </summary>
    private Sprite sp_close;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("TitleManager");
        sp_help = Resources.Load<Sprite>("button_help");
        sp_close = Resources.Load<Sprite>("button_cross");

        BSizeX = func.pxcalc(ButtonPxSizeX) / 2;
        BSizeY = func.pxcalc(ButtonPxSizeY) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        sr.sprite = sp_help;
        state = manager.GetComponent<TitleManager>().GetState();
        switch(state)
        {
        case State.Title:
            sr.color = new Color(col.r, col.g, col.b, 1);
            sr.sprite = sp_help;
            if(func.MouseCollision(transform.position, BSizeX, BSizeY, true) && Input.GetMouseButtonDown(0) && Fader.IsEnd())
            {
                manager.GetComponent<TitleManager>().SetState(State.Help);
            }
            break;
        case State.Help:
            sr.color = new Color(col.r, col.g, col.b, 1);
            sr.sprite = sp_close;
            if(func.MouseCollision(transform.position, BSizeX, BSizeY, true) && Input.GetMouseButtonDown(0) && Fader.IsEnd())
            {
                manager.GetComponent<TitleManager>().SetState(State.Title);
            }
            break;
        case State.Select:
            sr.color = new Color(col.r, col.g, col.b, 0);
            break;
        }
    }
}
