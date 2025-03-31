using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(BoundsCheck))]
public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f; //The movenment speed is 10m/s
    public float fireRate = 0.3f; //Sceonds/shots
    public float health = 10; //Damage needed to destooy this enemy
    public int score = 100; // Points earned for destroying this
    public float powerUpDropChance = 1f;

    protected bool calledShipDestroyed = false;
    protected BoundsCheck bndCheck;

    void Awake(){
        bndCheck = GetComponent<BoundsCheck>();
    }

    //This is property: a method that acts like a field
    public Vector3 pos{
        get{
            return this.transform.position;
        }

        set{
            this.transform.position = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        //Check whether this enemy has gone off the bottom of the screen

        if(bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown)) {
            Destroy(gameObject);
        }
        // if(!bndCheck.isOnScreen){
        //     if(pos.y < bndCheck.camHeight - bndCheck.radius){
        //         //We're off the bottom, so destroy this GameObject
        //         Destroy(gameObject);
        //     }
        // }
    }

    public virtual void Move(){
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    // void OnCollisionEnter(Collision coll)
    // {
    //     GameObject otherGO = coll.gameObject;
    //     if(otherGO.GetComponent<ProjectileHero>() != null){
    //         Destroy(otherGO);
    //         Destroy(gameObject);
    //     } else {
    //         Debug.Log("Enemy hit by non-projectileHero: " + otherGO.name);
    //     }
    // }

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        //Check for collisions with ProjectileHero
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if(p != null){
            if(bndCheck.isOnScreen){
                health -= Main.GET_WEAPON_DEFINITION(p.type).damageOnHit;
                if(health <= 0){
                    if(!calledShipDestroyed){
                        calledShipDestroyed = true;
                        Main.SHIP_DESTROYED(this);
                    }
                    Destroy(this.gameObject);
                }
            }
            Destroy(otherGO);
        } else {
            print("Enemy hit by non-projectileHeero: " + otherGO.name);
        }

    }
}
