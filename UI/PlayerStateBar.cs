using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    private Character currentCharacter;
   public Image healthImage;
   public Image healthDelayImage;
   public Image powerImage;
    private bool isRecovering;
    private void Update() 
    {   
        //通过时间修正实现渐变
        if(healthDelayImage.fillAmount>healthImage.fillAmount)
        {
            healthDelayImage.fillAmount-=Time.deltaTime;
        }
        if(isRecovering)
        {
            float persentage = currentCharacter.currentPower / currentCharacter.maxPower;
            powerImage.fillAmount = persentage;
            if(persentage>=1)
            {
                isRecovering = false;
                return;
            }
        }
    }
/// <summary>
/// 接受Health的百分比更改
/// </summary>
/// <param name="persentage">百分比 ： current /max</param>
   public void OnHealthChange(float persentage)
   {
        healthImage.fillAmount = persentage;
   }
   public void OnPowerChange(Character character)
   {
        isRecovering = true ;
        currentCharacter = character;
   }
}
