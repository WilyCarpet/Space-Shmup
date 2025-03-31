using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Inscribed")]
    public Transform playerTrans;
    public Transform[] panels;
    public float scrollSpeed = -30f;
    public float motionMult = -30f;

    private float panelHT;
    private float depth;


    // Start is called before the first frame update
    void Start()
    {
        panelHT = panels[0].localScale.y;
        depth = panels[0].position.z;

        panels[0].position = new Vector3(0,0,depth);
        panels[1].position = new Vector3(0,panelHT,depth);
    }

    // Update is called once per frame
    void Update()
    {
        float ty, tx = 0;
        ty = Time.time * scrollSpeed % panelHT + (panelHT * 0.5f);

        if(playerTrans != null){
            tx = -playerTrans.transform.position.x * motionMult;
        }

        panels[0].position = new Vector3(tx, ty, depth);
        if(ty >= 0) {
            panels[1].position = new Vector3(tx, ty -panelHT, depth);
        }else {
            panels[1].position = new Vector3(tx, ty + panelHT, depth);
        }
    }
}
