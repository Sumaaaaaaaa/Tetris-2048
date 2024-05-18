using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CountDown : MonoBehaviour
{
    private TextMeshPro UI;
    public GameObject Mainsystem;
    private BuildUp MainScript;
    // Start is called before the first frame update
    void Start()
    {
        MainScript = Mainsystem.GetComponent<BuildUp>();
        UI = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        int value = MainScript._countDown;
        if (value == 1)
        {
            UI.text = "Next Round";
        }
        else
        { 
            UI.text = (MainScript._countDown-1).ToString();
        }
    }
}
