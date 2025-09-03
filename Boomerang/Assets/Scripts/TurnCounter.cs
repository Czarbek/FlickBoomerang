using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�s���^�[���\��
/// </summary>
public class TurnCounter : MonoBehaviour
{
    /// <summary>
    /// �^�[���J�E���g�̕\���ʒux���W
    /// </summary>
    public float countOffsetX = 0.5f;
    /// <summary>
    /// �^�[���J�E���g�̕\���ʒuy���W
    /// </summary>
    public float countOffsetY = 0.6f;
    /// <summary>
    /// �Ǐ]��̃G�l�~�[
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
