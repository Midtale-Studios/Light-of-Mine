using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDebuffs : MonoBehaviour
{
    public FirstPersonController firstPersonController;
    public Animator cameraAnimator;
    public CanvasGroup blackOutImage;

    private bool isFadeIn = false;
    private GameObject teleportDestination;
    private float timeBetweenDeaths;

    void Start()
    {
        teleportDestination = new GameObject("Teleport Dest");
        teleportDestination.transform.position = transform.position;
        teleportDestination.transform.SetParent(null);

        timeBetweenDeaths = Random.Range(8, 18);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFadeIn();
        UpdateTeleportDestination();
        CheckForFlashback();
    }

    //Checks to see if it's time for a flashback
    private void CheckForFlashback()
    {
        timeBetweenDeaths -= Time.deltaTime;
        //Debug.Log("Time Left: " + timeBetweenDeaths.ToString());
        if (timeBetweenDeaths > 0) { return; }
        
        StartDeathFlashback();
        timeBetweenDeaths = Random.Range(8, 18);
    }

    //During a flash back this updates the fade in
    private void UpdateFadeIn()
    {
        if (isFadeIn)
        {
            float newFade = blackOutImage.alpha - (Time.deltaTime / 2);
            Debug.Log(newFade);
            blackOutImage.alpha = newFade;
            if (blackOutImage.alpha == 0)
            {
                isFadeIn = false;
                firstPersonController.enabled = true;
            }
        }
    }

    //Handles start of flashback
    void StartDeathFlashback(){
        transform.position = teleportDestination.transform.position;
        Debug.Log("anaimtion start");
        cameraAnimator.Play("DeathMaskDebuff", 0, 0.0f);
        blackOutImage.alpha = 1f;
        isFadeIn = true;
        firstPersonController.enabled = false;
    }

    //If a flashback is about to happen update the position of the destination object
    void UpdateTeleportDestination(){
        if(timeBetweenDeaths >= 7f){
            return;
        }

        if(Vector3.Distance(transform.position, teleportDestination.transform.position) > 15f ){
            Debug.Log("updating position");
            teleportDestination.transform.position = transform.position;
        }
        
    }
}
