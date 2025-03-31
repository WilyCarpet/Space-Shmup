using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    /// <summary>
    /// Keeps a Gameobject on scren.
    /// Note that this only works fron an orthographic main camera
    /// </summary>
public class BoundsCheck : MonoBehaviour
{
    [System.Flags]
    public enum eScreenLocs{
        onScreen = 0, // 0000 is binary
        offRight = 1,
        offLeft = 2,
        offUp = 4,
        offDown = 8
    }
    public enum eType{center, inset, outset};
    [Header("Inscribed")]
    public eType boundsType = eType.center;
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Dyanmic")]
    public eScreenLocs screenLocs = eScreenLocs.onScreen;
    // public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;

    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        //Find the checkRadius that will enable center, inset, or outset
        float checkRadius = 0;
        if(boundsType == eType.inset) checkRadius = -radius;
        if(boundsType == eType.outset) checkRadius = radius;

        Vector3 pos = transform.position;
        screenLocs = eScreenLocs.onScreen;
        //Restrict the x position camwidth

        if(pos.x > camWidth + checkRadius){
            pos.x = camWidth + checkRadius;
            screenLocs |= eScreenLocs.offRight;
        }
        if(pos.x < -camWidth - checkRadius){
            pos.x = -camWidth - checkRadius;
            screenLocs |= eScreenLocs.offLeft;
        }

        //Restrict the y position to camHeight
        if(pos.y > camHeight + checkRadius){
            pos.y = camHeight + checkRadius;
            screenLocs |= eScreenLocs.offUp;
        }
        if(pos.y < -camHeight - checkRadius){
            pos.y = -camHeight - checkRadius;
            screenLocs |= eScreenLocs.offDown;
        }

        if(keepOnScreen && !isOnScreen){
            transform.position = pos;
            screenLocs |= eScreenLocs.onScreen;
        }

        transform.position = pos;
    }

    public bool isOnScreen{
        get{
            return(screenLocs == eScreenLocs.onScreen);
        }
    }

    public bool LocIs(eScreenLocs checkLoc){
        if(checkLoc == eScreenLocs.onScreen) return isOnScreen;
        return((screenLocs & checkLoc) == checkLoc);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
