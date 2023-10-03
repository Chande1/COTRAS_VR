using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] Text SA_rage;
    [SerializeField] Text SA_score;
    [SerializeField] Text SK_rage;
    [SerializeField] Text SL_rage;
    [SerializeField] Text SW_rage;
    [SerializeField] Text SM_rage;

    public void ResetUI(int sa_r,int sa_s,int sk_r,int sl_r,int sw_r,int sm_r)
    {
        Color beforecolor= new Color(100 / 255f, 100 / 255f, 100 / 255f);
        Color aftercolor= new Color(50 / 255f, 50 / 255f, 50 / 255f);

        if (sa_r == 0)
        {
            SA_rage.color = beforecolor;
            SA_rage.text = "소독전";
        } 
        else
        {
            SA_rage.color = aftercolor;
            SA_rage.text = sa_r.ToString() + "%";
        }
            
        SA_score.text = sa_s.ToString() + "점";
        
        if (sk_r == 0)
        {
            SK_rage.color = beforecolor;
            SK_rage.text = "소독전";
        }
        else
        {
            SK_rage.color = aftercolor;
            SK_rage.text = sk_r.ToString() + "%";
        }
            
        if (sl_r == 0)
        {
            SL_rage.color = beforecolor;
            SL_rage.text = "소독전";
        }  
        else
        {
            SL_rage.color = aftercolor;
            SL_rage.text = sl_r.ToString() + "%";
        }
           
        
        if (sw_r == 0)
        {
            SW_rage.color = beforecolor;
            SW_rage.text = "소독전";
        }  
        else
        {
            SW_rage.color = aftercolor;
            SW_rage.text = sw_r.ToString() + "%";
        }
            
        
        if (sm_r == 0)
        {
            SM_rage.color = beforecolor;
            SM_rage.text = "소독전";
        }   
        else
        {
            SM_rage.color = aftercolor;
            SM_rage.text = sm_r.ToString() + "%";
        }
            

    }
}
