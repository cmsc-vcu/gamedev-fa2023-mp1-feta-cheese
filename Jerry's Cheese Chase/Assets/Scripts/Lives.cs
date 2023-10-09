using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lives : MonoBehaviour
{
    public static int heart = 3;
    public TMP_Text life;
    // Start is called before the first frame update
    void Start()
    {
        life.text = "Lives:";
    }

    // Update is called once per frame
    void Update()
    {
        life.text = "Lives:";
        for(int i = 0; i<heart; i++)
        {
            life.text += "<sprite name=jerry sprite 1>";
        }
    }
}
