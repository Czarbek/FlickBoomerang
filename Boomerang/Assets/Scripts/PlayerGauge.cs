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
    public float scaleX = func.metrecalc(25);
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
    private int maxHP = 100;
    /// <summary>
    /// �v���C���[�I�u�W�F�N�g
    /// </summary>
    private GameObject player;
    /// <summary>
    /// �q�b�g�|�C���g
    /// </summary>
    private int hp;
    /// <summary>
    /// �G�e���q�b�g�����ꍇ�A�v���C���[��HP�����炷
    /// </summary>
    /// <param name="atk">�G�̍U����</param>
    public void Hit(int atk)
    {
        player.GetComponent<Player>().Hit(atk);
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(defaultx, defaulty);
        transform.localScale = new Vector2(scaleX, scaleY);
        player = GameObject.Find("Player");
        maxHP = player.GetComponent<Player>().MaxHP;

        GameObject frame = (GameObject)Resources.Load("GaugeFrame");
        frame = Instantiate(frame);
        frame.transform.position = transform.position;
        frame.transform.localScale = new Vector2(frameScaleX, frameScaleY);
    }

    // Update is called once per frame
    void Update()
    {
        hp = player.GetComponent<Player>().GetHP();
        float posx = (float)hp / maxHP * scaleX / 2 + (defaultx - scaleX / 2);
        float posy = defaulty;
        float scalex = (float)hp / maxHP * scaleX;


        transform.position = new Vector2(posx, posy);
        transform.localScale = new Vector2(scalex, scaleY);
    }
}
