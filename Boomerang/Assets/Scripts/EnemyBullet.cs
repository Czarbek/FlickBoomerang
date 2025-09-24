using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�e
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    /// <summary>
    /// �����蔻��̔��a
    /// </summary>
    public float Collisionr = 0.25f;
    /// <summary>
    /// ���e�܂ł̎���(�t���[����)
    /// </summary>
    public int flyingTime = 60;
    /// <summary>
    /// ���ˌ��̓G�I�u�W�F�N�g
    /// </summary>
    public GameObject parent;
    /// <summary>
    /// �v���C���[�Q�[�W�̃I�u�W�F�N�g
    /// </summary>
    private GameObject gauge;
    /// <summary>
    /// �Q�[�W�̒��S�ʒu
    /// </summary>
    private Vector2 targetPoint;
    /// <summary>
    /// �^���Ɋւ���\����
    /// </summary>
    private func.accel spd;
    /// <summary>
    /// �o�ߎ���
    /// </summary>
    private int time;
    /// <summary>
    /// �U����
    /// </summary>
    public int atk;
    /// <summary>
    /// �{�X�̍U�����ǂ���
    /// </summary>
    public bool isBoss;
    // Start is called before the first frame update
    void Start()
    {
        gauge = GameObject.Find("PlayerGauge");
        targetPoint = new Vector2(PlayerGauge.DefaultX, PlayerGauge.DefaultY);
        float sx = transform.position.x;
        float sy = transform.position.y;
        float dx = targetPoint.x;
        float dy = targetPoint.y;
        spd = func.getDecelerationVector(sx, sy, dx, dy, flyingTime);
        isBoss = parent.GetComponent<Enemy>().boss;
        transform.localScale = new Vector2(0.3f, 0.3f) * (isBoss ? 2 : 1);
        transform.rotation = Quaternion.Euler(0, 0, spd.angle);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        switch(parent.GetComponent<Enemy>().element)
        {
        case Enemy.Element.Fire:
            sr.sprite = Resources.Load<Sprite>(isBoss ? "Boss_attack_fire" : "attack_fire");
            break;
        case Enemy.Element.Aqua:
            sr.sprite = Resources.Load<Sprite>(isBoss ? "Boss_attack_aqua" : "attack_aqua");
            break;
        case Enemy.Element.Leaf:
            sr.sprite = Resources.Load<Sprite>(isBoss ? "Boss_attack_leaf" : "attack_leaf");
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float speed = spd.firstspd + spd.delta * time;
        x += speed*func.cos(spd.angle);
        y += speed*func.sin(spd.angle);

        time++;

        transform.position = new Vector2(x, y);

        if(func.CircleCollision(transform.position, Collisionr, targetPoint, PlayerGauge.Collisionr))
        {
            gauge.GetComponent<PlayerGauge>().Hit(atk);
            parent.GetComponent<Enemy>().SetChange();
            parent.GetComponent<Enemy>().ResetTurn();
            Destroy(gameObject);
        }
    }
}
