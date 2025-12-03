using UnityEngine;

/// <summary>
/// 当たり判定可視化(デバッグ用)
/// </summary>
public class CollisionCircle : MonoBehaviour
{
    /// <summary>
    /// 依存先のgameObject
    /// </summary>
    private GameObject parent;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="scale"></param>
    /// <param name="parent"></param>
    public void Init(float scale, GameObject parent)
    {
        transform.localScale = new Vector3(scale*2, scale*2, scale*2);
        this.parent = parent;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(parent == null)
        {
            Destroy(gameObject);
        }
        else
        {
            this.transform.position = parent.transform.position;
        }
    }
}
