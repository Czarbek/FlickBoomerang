using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵消失時のマスク
/// </summary>
public class EnemyMask : MonoBehaviour
{
    /// <summary>
    /// 移動速度
    /// </summary>
    const float spd = 0.05f;
    /// <summary>
    /// 移動方向
    /// </summary>
    private int direction;
    /// <summary>
    /// 移動距離
    /// </summary>
    private float distance;
    /// <summary>
    /// 消失の対象
    /// </summary>
    public GameObject parent;
    /// <summary>
    /// 方向を設定する
    /// </summary>
    /// <param name="n">方向 1か-1</param>
    public void SetDirection(int n)
    {
        direction = n;
    }
    // Start is called before the first frame update
    void Start()
    {
        distance = func.abs(transform.position.x - parent.transform.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        if(distance > 0)
        {
            distance -= spd;
            transform.position = new Vector2(transform.position.x - spd * direction, transform.position.y);
        }
        if(distance <= 0)
        {
            if(direction == 1)
            {
                if(parent != null)
                {
                    parent.GetComponent<Enemy>().Inactivate();
                }
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
