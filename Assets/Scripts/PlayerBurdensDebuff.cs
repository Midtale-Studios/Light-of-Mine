using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBurdensDebuff : MonoBehaviour
{


    public FirstPersonController firstPersonController; 

    void OnEnable(){
        firstPersonController.enableSprint = false;
        firstPersonController.walkSpeed = 2f;
    }

    void OnDisable(){
        firstPersonController.enableSprint = true;
        firstPersonController.walkSpeed = 3.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
