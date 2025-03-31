using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Inscribed")]
    public float rotationPerSecond = 0.1f;

    [Header("Dynamic")]
    public int levelShown = 0;
    
    //This non-public variable will not appear in the inspector
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;        
    }

    // Update is called once per frame
    void Update()
    {
        //Read the current shield level form the Hero Singleton
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);
        //If this is different from the level shown
        if(levelShown != currLevel){
            levelShown = currLevel;
            //Adjust the texture offset to show different shield level
            mat.mainTextureOffset = new Vector2(0.2f*levelShown, 0);
        }

        //Roate the shield a bit every frame in a time-based way
        float rZ = -(rotationPerSecond*Time.time*360) % 360f;
        transform.rotation = Quaternion.Euler(0,0,rZ);
    }
}
