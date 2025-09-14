using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�������̃}�X�N
/// </summary>
public class EnemyMask : MonoBehaviour
{
    /// <summary>
    /// �ړ����x
    /// </summary>
    const float spd = 0.05f;
    /// <summary>
    /// �ړ�����
    /// </summary>
    private int direction;
    /// <summary>
    /// �ړ�����
    /// </summary>
    private float distance;
    /// <summary>
    /// �����̑Ώ�
    /// </summary>
    public GameObject parent;
    /// <summary>
    /// ������ݒ肷��
    /// </summary>
    /// <param name="n">���� 1��-1</param>
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
