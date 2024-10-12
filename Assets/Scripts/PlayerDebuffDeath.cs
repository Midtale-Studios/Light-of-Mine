using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDebuffs : MonoBehaviour
{
    public FirstPersonController firstPersonController;
    public Animator cameraAnimator;
    public CanvasGroup blackOutImage;
    public float minWaitTime;
    public float maxWaitTime;
    public AnimationCurve blink_timing;

    private bool isFadeIn = false;
    private GameObject teleportDestination;
    private float timeBetweenDeaths;
    public float currentCurveX = 0f;
    private Rigidbody rb;

    void Start()
    {
        teleportDestination = new GameObject("Teleport Dest");
        teleportDestination.transform.position = transform.position;
        teleportDestination.transform.SetParent(null);
        rb = GetComponent<Rigidbody>();

        timeBetweenDeaths = Random.Range(minWaitTime, maxWaitTime);
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
        timeBetweenDeaths = Random.Range(minWaitTime, maxWaitTime);
    }

    //During a flash back this updates the fade in
    private void UpdateFadeIn()
    {
        if (isFadeIn)
        {
            currentCurveX = currentCurveX + (Time.deltaTime / 3);
            //Debug.Log(newFade);
            blackOutImage.alpha = blink_timing.Evaluate(currentCurveX);
            if (currentCurveX >= 1)
            {
                Debug.Log("timer up");
                isFadeIn = false;
                firstPersonController.enabled = true;
                currentCurveX = 0f;
            }
        }
    }

    //Handles start of flashback
    void StartDeathFlashback(){
        transform.position = teleportDestination.transform.position;
        transform.rotation = teleportDestination.transform.rotation;
        Debug.Log("anaimtion start");
        cameraAnimator.Play("DeathMaskDebuff", 0, 0.0f);
        blackOutImage.alpha = 1f;
        isFadeIn = true;
        firstPersonController.enabled = false;
        //0 out the velocity of the rigidbody so the player does not slide
        rb.velocity = new Vector3(0, 0, 0);
    }

    //If a flashback is about to happen update the position of the destination object
    void UpdateTeleportDestination(){
        if(timeBetweenDeaths <= 7f){
            return;
        }

        if(Vector3.Distance(transform.position, teleportDestination.transform.position) > 15f ){
            Debug.Log("updating position");
            teleportDestination.transform.position = transform.position;
            teleportDestination.transform.rotation = transform.rotation;
        }
        
    }
}
