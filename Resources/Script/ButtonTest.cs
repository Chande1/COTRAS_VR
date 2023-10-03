using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    public Text TestText;
    int clickcount=0;

    public void ButtonTestLog()
    {
        Debug.Log("button work.");
    }

    public void ButtonTestText()
    {
        Debug.Log("click test is work.");

        if (TestText!=null)
        {
            clickcount += 1;
            TestText.text = "Å¬¸¯ È½¼ö : " + clickcount.ToString();
        }
    }
}
