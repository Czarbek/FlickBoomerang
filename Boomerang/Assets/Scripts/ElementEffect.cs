using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementEffect : MonoBehaviour
{
    /// <summary>
    /// �t�F�[�h�C���ɂ����鎞��
    /// </summary>
    private const int FadeInTime = (int)(300.0f / func.FRAMETIME);
    /// <summary>
    /// ��������
    /// </summary>
    static private int time;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    static private SpriteRenderer sr;
    /// <summary>
    /// �v���C���[�I�u�W�F�N�g
    /// </summary>
    public GameObject player;

    /// <summary>
    /// ������ύX����
    /// </summary>
    /// <param name="element">�ύX��̑���</param>
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