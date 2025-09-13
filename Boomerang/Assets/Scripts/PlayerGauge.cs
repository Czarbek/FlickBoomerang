using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機のHPゲージ
/// </summary>
public class PlayerGauge : MonoBehaviour
{
    /// <summary>
    /// 当たり判定の半径
    /// </summary>
    static public float Collisionr = 0.1f;
    /// <summary>
    /// max時のx座標
    /// </summary>
    public float defaultx = 1.65f;
    /// <summary>
    /// y座標
    /// </summary>
    public float defaulty = -4.5f;
    /// <summary>
    /// 横幅
    /// </summary>
    public float scaleX = func.metrecalc(25);
    /// <summary>
    /// 縦幅
    /// </summary>
    public float scaleY = 0.25f;
    /// <summary>
    /// 枠の横幅
    /// </summary>
    public float frameScaleX = 2.1f;
    /// <summary>
    /// 枠の縦幅
    /// </summary>
    public float frameScaleY = 0.35f;
    /// <summary>
    /// ヒットポイント最大値
    /// </summary>
    private int maxHP = 100;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject player;
    /// <summary>
    /// ヒットポイント
    /// </summary>
    private int hp;
    /// <summary>
    /// 敵弾がヒットした場合、プレイヤーのHPを減らす
    /// </summary>
    /// <param name="atk">敵の攻撃力</param>
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
