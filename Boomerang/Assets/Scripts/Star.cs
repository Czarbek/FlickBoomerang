using UnityEngine;

public class Star : MonoBehaviour
{
    private const int SustainTime = 90;
    private const float FirstSpd = 0.3f;
    private const int RotateSpeed = 12;

    private float x;
    private float y;
    private float xspd;
    private int time;
    private float alpha;
    private float angle;
    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
        time = 0;
        alpha = 1;
        angle = 0;
        sr = GetComponent<SpriteRenderer>();

        xspd = Random.Range(-2.0f, 2.0f) / SustainTime;
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        if(time > SustainTime - 30)
        {
            alpha = 1.0f - (float)(time - (SustainTime - 30)) / 30;
        }
        angle += RotateSpeed;
        x += xspd;
        y += FirstSpd * (1.0f-(float)time / (SustainTime-30));

        transform.position = new Vector2(x, y);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        sr.color = new Color(1, 1, 1, alpha);

        if(time == SustainTime)
        {
            Destroy(gameObject);
        }
    }
}
