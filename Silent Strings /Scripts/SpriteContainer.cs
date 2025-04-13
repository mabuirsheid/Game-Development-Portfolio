using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteContainer : MonoBehaviour
{
    [SerializeField] public Sprite[] pLegs, pUnarmedWalk;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Sprite[] getPlayerLegs(){
        return pLegs;
    }
    public Sprite[] getPlayerUnarmedWalk(){
        return pUnarmedWalk;
    }
}
