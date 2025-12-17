using UnityEngine;
using UnityEngine.UI;

public class NextFloor : MonoBehaviour
{
    private const float Width = 0.6f;
    private const float Height = 0.3f;

    public bool toNext;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(func.DEBUG)
        {

        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool touched = Application.isEditor ? Input.GetMouseButtonDown(0) : Input.GetMouseButtonDown(0) || func.getTouch() == 1;
        if(touched && func.MouseCollision(transform.position, Width, Height, true))
        {
            GameObject.Find("BattleManager").GetComponent<BattleManager>().MoveFloor_debug(toNext);
        }
    }
}
