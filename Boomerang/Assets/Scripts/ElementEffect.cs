using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementEffect : MonoBehaviour
{
    /// <summary>
    /// フェードインにかかる時間
    /// </summary>
    private const int FadeInTime = (int)(300.0f / func.FRAMETIME);
    /// <summary>
    /// 処理時間
    /// </summary>
    static private int time;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    static private SpriteRenderer sr;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    public GameObject player;

    /// <summary>
    /// 属性を変更する
    /// </summary>
    /// <param name="element">変更先の属性</param>
    static public void SetElement(Enemy.Element element)
    {
        time = 0;
        if(element == Enemy.Element.Fire)
        {
            sr.sprite = Resources.Load<Sprite>("CrystalEffect_fire");
            sr.color = new Color(1, 1, 1, 1);
        }
        else if(element == Enemy.Element.Aqua)
        {
            sr.sprite = Resources.Load<Sprite>("CrystalEffect_aqua");
            sr.color = new Color(1, 1, 1, 1);
        }
        else if(element == Enemy.Element.Leaf)
        {
            sr.sprite = Resources.Load<Sprite>("CrystalEffect_leaf");
            sr.color = new Color(1, 1, 1, 1);
        }
        else if(element == Enemy.Element.None)
        {
            sr.sprite = null;
            sr.color = new Color(1, 1, 1, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        sr = GetComponent<SpriteRenderer>();
        SetElement(Enemy.Element.None);
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        transform.position = player.transform.position;
        if(time <= FadeInTime && sr.sprite != null)
        {
            sr.color = new Color(1, 1, 1, (float)time / FadeInTime);
        }
    }
}