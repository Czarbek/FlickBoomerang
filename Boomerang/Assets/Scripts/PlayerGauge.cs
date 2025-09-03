using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���@��HP�Q�[�W
/// </summary>
public class PlayerGauge : MonoBehaviour
{
    /// <summary>
    /// �����蔻��̔��a
    /// </summary>
    static public float Collisionr = 0.1f;
    /// <summary>
    /// max����x���W
    /// </summary>
    public float defaultx = 1.65f;
    /// <summary>
    /// y���W
    /// </summary>
    public float defaulty = -4.5f;
    /// <summary>
    /// ����
    /// </summary>
    public float scaleX = 2.0f;
    /// <summary>
    /// �c��
    /// </summary>
    public float scaleY = 0.25f;
    /// <summary>
    /// �g�̉���
    /// </summary>
    public float frameScaleX = 2.1f;
    /// <summary>
    /// �g�̏c��
    /// </summary>
    public float frameScaleY = 0.35f;
    /// <summary>
    /// �q�b�g�|�C���g�ő�l
    /// </summary>
    public int MaxHP = 100;
    /// <summary>
    /// �v���C���[�I�u�W�F�N�g
    /// </summary>
    private GameObject player;
    /// <summary>
    /// �q�b�g�|�C���g
    /// </summary>
    private int hp;
    /// <summary>
    /// �G�̍U���������������̏���
    /// </summary>
    /// <param name="atk">�G�̍U����</param>
    public void hit(int atk)
    {
        hp -= atk;
        if(hp < 0) hp = 0;
    }
    /// <summary>
    /// HP���񕜂���
    /// </summary>
    /// <param name="rate">�ő�HP�ɑ΂��銄��</param>
    public void HPCure(float rate)
    {
        hp += (int)(MaxHP * rate);
        if(hp > MaxHP) hp = MaxHP;
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(defaultx, defaulty);
        transform.localScale = new Vector2(scaleX, scaleY);
        player = GameObject.Find("Player");

        GameObject frame = (GameObject)Resources.Load("GaugeFrame");
        frame = Instantiate(frame);
        frame.transform.position = transform.position;
        frame.transform.localScale = new Vector2(frameScaleX, frameScaleY);

        hp = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        float posx = (float)hp / MaxHP * scaleX / 2 + (defaultx - scaleX / 2);
        float posy = defaulty;
        float scalex = (float)hp / MaxHP * scaleX;


        transform.position = new Vector2(posx, posy);
        transform.localScale = new Vector2(scalex, scaleY);
    }
}
