using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountWindow : MonoBehaviour
{
    [SerializeField] Text Correct;      //정답
    [SerializeField] Text InCorrect;    //오답
    int c_a;
    int ic_a;
    // Start is called before the first frame update
    
    void Awake()
    {
        if(Correct)
            Correct.text = "0";
        if (InCorrect)
            InCorrect.text = "0";
        c_a = 0;
        ic_a = 0;
    }

    public void CorrectAnswer()
    {
        c_a += 1;
        Correct.text = c_a.ToString();
    }

    public void InCorrectAnswer()
    {
        ic_a += 1;
        InCorrect.text = ic_a.ToString();
    }
}
