using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDebuffs : MonoBehaviour
{
    public bool startDeathDebuff = false;
    public Animator cameraAnimator;
    public CanvasGroup blackOutImage;
    // Start is called before the first frame update

    private bool isFadeIn = false;
    private Color newColor;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isFadeIn){
            float newFade = blackOutImage.alpha - Time.deltaTime;
            //Debug.Log(newFade);
            blackOutImage.alpha = newFade;
            if(blackOutImage.alpha == 0f){
                isFadeIn = false;
            }
        }

        if(startDeathDebuff){
            startDeathDebuff = false;
            StartDeathFlashback();
        }
    }


    void StartDeathFlashback(){
        cameraAnimator.Play("DeathMaskDebuff");
        blackOutImage.alpha = 1f;
        isFadeIn = true;
    }
}
