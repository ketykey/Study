using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMusic : MonoBehaviour
{
    public GameObject objPrefabInstantSource;//音乐预知物体 
    private GameObject musicInstant = null;//场景中是否有这个物体  
    // Use this for initialization  
    // Start is called before the first frame update
    void Start()
    {
        musicInstant = GameObject.FindGameObjectWithTag("sound");
        if (musicInstant == null) { }        
            musicInstant = (GameObject)Instantiate(objPrefabInstantSource);
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
