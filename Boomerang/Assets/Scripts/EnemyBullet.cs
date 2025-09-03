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
    // Start is called before the first frame update
    void Start()
    {
        gauge = GameObject.Find("PlayerGauge");
        targetPoint = new Vector2(gauge.GetComponent<PlayerGauge>().defaultx, gauge.GetComponent<PlayerGauge>().defaulty);
        float sx = transform.position.x;
        float sy = transform.position.y;
        float dx = targetPoint.x;
        float dy = targetPoint.y;
        spd = func.getDecelerationVector(sx, sy, dx, dy, flyingTime);
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
            gauge.GetComponent<PlayerGauge>().hit(atk);
            parent.GetComponent<Enemy>().SetChange();
            parent.GetComponent<Enemy>().ResetTurn();
            Destroy(gameObject);
        }
    }
}
