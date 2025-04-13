using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections;
using UnityEngine;

public class CameraRotationEffect : MonoBehaviour{
    PlayerMovement pm;
    public float mod=0.005f;
    //public float modUpdate;
    float zVal = 0.0f;
    // Start is called before the first frame update
    void Start(){
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();       
    }

    // Update is called once per frame
    void Update(){
        if (pm.moving==true){
            Vector3 rot = new Vector3(0,0,zVal);
            this.transform.eulerAngles = rot;
            zVal+=mod;
        }
        if (transform.eulerAngles.z >= 1.5f && transform.eulerAngles.z < 10.0f){
            mod = -0.005f;
        }else if (transform.eulerAngles.z <= 358.5f && transform.eulerAngles.z > 350.0f){
            mod = 0.005f;
        }
        // else if (transform.eulerAngles.z >= 0.2f && transform.eulerAngles.z<10.0f){
        //     mod = -0.01f;
        // }
        // else if (transform.eulerAngles.z <= 358.0f && transform.eulerAngles.z>350.0f){
        //     mod = 0.01f;
        // }
    }
}
