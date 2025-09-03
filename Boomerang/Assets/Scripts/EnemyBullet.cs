using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵弾
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    /// <summary>
    /// 当たり判定の半径
    /// </summary>
    public float Collisionr = 0.25f;
    /// <summary>
    /// 着弾までの時間(フレーム数)
    /// </summary>
    public int flyingTime = 60;
    /// <summary>
    /// 発射元の敵オブジェクト
    /// </summary>
    public GameObject parent;
    /// <summary>
    /// プレイヤーゲージのオブジェクト
    /// </summary>
    private GameObject gauge;
    /// <summary>
    /// ゲージの中心位置
    /// </summary>
    private Vector2 targetPoint;
    /// <summary>
    /// 運動に関する構造体
    /// </summary>
    private func.accel spd;
    /// <summary>
    /// 経過時間
    /// </summary>
    private int time;
    /// <summary>
    /// 攻撃力
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
