using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵行動ターン表示
/// </summary>
public class TurnCounter : MonoBehaviour
{
    /// <summary>
    /// ターンカウントの表示位置x座標
    /// </summary>
    public float countOffsetX = 0.5f;
    /// <summary>
    /// ターンカウントの表示位置y座標
    /// </summary>
    public float countOffsetY = 0.6f;
    /// <summary>
    /// 追従先のエネミー
    /// </summary>
    public Enemy parent;
    /// <summary>
    /// SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.sprite = Font.GetFont(parent.turnCount);
        transform.position = new Vector2(parent.transform.position.x + countOffsetX, parent.transform.position.y + countOffsetY);
    }
}
