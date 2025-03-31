using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Enemy_2 Inscribed Fields")]
    public float lifeTime = 10;
    //ENemy_2 uses a Sin3e wave to modify a 2-point linear interpolation
    [Tooltip("Determine how much the Sine wave will ease the interpolation")]
    public float sinEccentricity = 0.6f;
    public AnimationCurve rotCurve;
    [Header("Enemy_2 Private Fields")]
    [SerializeField] private float birthTime;
    [SerializeField] private Vector3 p0, p1;
    private Quaternion baseRotation;

    // Start is called before the first frame update
    void Start()
    {
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //Pick any point on the right side of the screen
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //possibly swap sides
        if(Random.value > 0.5f){
            p0.x *= -1;
            p1.x *= -1;
        }

        //Set the brithTime to the current time
        birthTime =Time.time;

        //Set up the initial ship rotation
        transform.position = p0;
        transform.LookAt(p1,Vector3.back);
        baseRotation = transform.rotation;
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if(u > 1){
            Destroy(this.gameObject);
            return;
        }

        //using the animation cureve to set the rotation about y
        float shipRot = rotCurve.Evaluate(u) * 360;
        // if(p0.x > p1.x) shipRot = -shipRot;
        // transform.rotation = Quaternion.Euler(0,shipRot,0);

        transform.rotation = baseRotation * Quaternion.Euler(-shipRot, 0, 0);

        u = u + sinEccentricity*(Mathf.Sin(u*Mathf.PI*2));

        pos = (1-u)*p0 + u*p1;
    }

}
