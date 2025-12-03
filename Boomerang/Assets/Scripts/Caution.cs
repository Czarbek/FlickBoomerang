using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caution : MonoBehaviour
{
    /// <summary>
    /// ピクセル単位の横幅
    /// </summary>
    private const int PxSizeX = 820;
    /// <summary>
    /// ピクセル単位の縦幅
    /// </summary>
    private const int PxSizeY = 990;
    /// <summary>
    /// 横幅(半分)
    /// </summary>
    private float WSizeX;
    /// <summary>
    /// 縦幅(半分)
    /// </summary>
    private float WSizeY;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    static private SpriteRenderer sr;
    /// <summary>
    /// 画像リスト
    /// </summary>
    static private Sprite[] spriteList = new Sprite[2];
    /// <summary>
    /// 表示中かどうか
    /// </summary>
    static private bool dsp;

    /// <summary>
    /// 表示する
    /// </summary>
    /// <param name="index">画像リストのインデックス</param>
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