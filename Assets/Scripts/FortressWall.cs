using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class FortressWall : MonoBehaviour
{

    private List<Transform> blocks = new List<Transform>();
    public float timer = 2f;
    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        GetWall();
    }

    // Update is called once per frame
    void Update()
    {
        BiuldWall();
    }

    void GetWall(){
        
        foreach (Transform child in transform){
            blocks.Add(child);
            //Debug.Log(child.name);
        } 
    }


    void BiuldWall(){
        if(blocks.Count == 0) {return;}

        currentTime += Time.deltaTime;
        //Debug.Log(currentTime);

        if (currentTime >= timer){
            currentTime = 0f;
            timer -= Mathf.Clamp(timer / blocks.Count, 0.001f, 0.2f);
            

            blocks[0].gameObject.SetActive(true);
            blocks.RemoveAt(0);
        }
    }
}
