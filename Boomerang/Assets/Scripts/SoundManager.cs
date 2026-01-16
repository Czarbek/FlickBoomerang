using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 効果音管理
/// </summary>
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// 効果音
    /// </summary>
    public enum Se
    {
        /// <summary>ボタンタップ音</summary>
        Button,
        /// <summary>ブーメランフリック音</summary>
        Flick,
        /// <summary>リング獲得音</summary>
        Ring,
        /// <summary>ブーメランヒット音</summary>
        BoomerangHit,
        /// <summary>敵攻撃音</summary>
        EnemyAttack,
        /// <summary>敵の攻撃ヒット音</summary>
        EnemyAttackHit,
        /// <summary>炎ボス攻撃音</summary>
        BossAttackFire,
        /// <summary>フロア切り替え時音</summary>
        MoveFloor,
        /// <summary>ターン開始時音</summary>
        StartTurn,
        /// <summary>敵登場音</summary>
        EnemyAppear,
        /// <summary>ボス登場音</summary>
        BossAppear,
        /// <summary>敵撃破時音</summary>
        EnemyDefeat,
        /// <summary>クリスタル取得時音</summary>
        Crystal,
        /// <summary>黄金の果実取得時音</summary>
        Fruit,
        /// <summary>エラー音</summary>
        Error,
        /// <summary>HP回復時</summary>
        HPCure,
        /// <summary>ブーメラン回転時音</summary>
        BoomerangRotate,
        /// <summary>水ボス攻撃音</summary>
        BossAttackAqua,
        /// <summary>草ボス攻撃音</summary>
        BossAttackLeaf,
    }
    /// <summary>
    /// 効果音リストの数
    /// </summary>
    private const int SeNum = 19;
    /// <summary>
    /// 効果音リスト
    /// </summary>
    private List<AudioClip> seList;

    /// <summary>
    /// サウンドを再生する
    /// </summary>
    /// <param name="se">再生するサウンド</param>
    public void PlaySound(Se se)
    {
        Debug.Log((int)se);
        GetComponent<AudioSource>().PlayOneShot(seList[(int)se]);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        seList = new List<AudioClip>
        {
            (AudioClip)Resources.Load("se_01"),
            (AudioClip)Resources.Load("se_02"),
            (AudioClip)Resources.Load("se_03"),
            (AudioClip)Resources.Load("se_04"),
            (AudioClip)Resources.Load("se_05"),
            (AudioClip)Resources.Load("se_06"),
            (AudioClip)Resources.Load("se_07"),
            (AudioClip)Resources.Load("se_08"),
            (AudioClip)Resources.Load("se_09"),
            (AudioClip)Resources.Load("se_10"),
            (AudioClip)Resources.Load("se_11"),
            (AudioClip)Resources.Load("se_12"),
            (AudioClip)Resources.Load("se_13"),
            (AudioClip)Resources.Load("se_14"),
            (AudioClip)Resources.Load("se_15"),
            (AudioClip)Resources.Load("se_16"),
            (AudioClip)Resources.Load("se_17"),
            (AudioClip)Resources.Load("se_18"),
            (AudioClip)Resources.Load("se_19")
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
