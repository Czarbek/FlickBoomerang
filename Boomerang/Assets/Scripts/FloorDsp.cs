using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FloorDsp : MonoBehaviour
{
    TextMeshProUGUI tmpro;
    public void SetText(int currentFloorNumber, int lastFloorNumber)
    {
        tmpro.text = currentFloorNumber + " / " + lastFloorNumber;
    }
    public void DeleteText()
    {
        tmpro.text = "";
    }
    // Start is called before the first frame update
    void Start()
    {
        tmpro = GetComponent<TextMeshProUGUI>();
        tmpro.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
