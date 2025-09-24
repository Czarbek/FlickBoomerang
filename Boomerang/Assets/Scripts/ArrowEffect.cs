using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEffect : MonoBehaviour
{
    /// <summary>
    /// プレイヤー位置に対する表示位置x
    /// </summary>
    private const float OffsetX = -0.3f;
    /// <summary>
    /// プレイヤー位置に対する表示位置y
    /// </summary>
    private const float OffsetY = 0.2f;
    /// <summary>
    /// 上移動の最大距離
    /// </summary>
    private const float MoveDistance = 0.4f;
    /// <summary>
    /// 移動にかかる時間(フレーム)
    /// </summary>
    private const int MoveTime = (int)(500.0f / func.FRAMETIME);
    /// <summary>
    /// 処理時間
    /// </summary>
    private int time;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(2, 2);
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player");

        time++;
        float standardx = player.transform.position.x + OffsetX;
        float standardy = player.transform.position.y + OffsetY;

        transform.position = new Vector2(standardx, standardy + MoveDistance * time / MoveTime);

        if(time == MoveTime)
        {
            Destroy(gameObject);
        }
    }
}
