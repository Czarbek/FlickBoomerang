using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectTx : MonoBehaviour
{
    private TextMeshProUGUI tmpro;
    private GameObject titleManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(StageInfo.xcalc(-10), StageInfo.ycalc(140));

        tmpro = GetComponent<TextMeshProUGUI>();
        tmpro.color = new Color(1, 1, 1, 0);

        titleManager = GameObject.Find("TitleManager");

        if(Initializer.GetRetry())
        {
            tmpro.color = new Color(1, 1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(titleManager.GetComponent<TitleManager>().state)
        {
        case TitleManager.State.Title:
            tmpro.color = new Color(1, 1, 1, 0);
            break;
        case TitleManager.State.Help:
            tmpro.color = new Color(1, 1, 1, 0);
            break;
        case TitleManager.State.Select:
            tmpro.color = new Color(1, 1, 1, 1);
            break;
        }
    }
}
