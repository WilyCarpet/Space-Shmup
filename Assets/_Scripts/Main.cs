using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static private Main S;
    static private Dictionary<eWeaponType,WeaponDefinition> WEAP_DICT;
    [Header("Inscribed")]
    public bool spawnEnemies = true;
    public GameObject[] prefabEnemies; //Array of enemy prefabs
    public float enemySpawnPerSecond = 0.5f;
    public float enemyInsetDefault = 1.5f;
    public float gamesRestartDelay = 2;
    public GameObject prefabPowerUp;
    public WeaponDefinition[] weaponDefinitions;
    public eWeaponType[] powerUpFrequency = new eWeaponType[]{
        eWeaponType.blaster, eWeaponType.blaster, 
        eWeaponType.spread, eWeaponType.shield };

    BoundsCheck bndCheck;

    void Awake()
    {
        S = this;
        //Set bnd check to reference the BoundsCheck compoenent on this GameObject
        bndCheck = GetComponent<BoundsCheck>();

        //Invoke spawnEnemy() once everySpawnPerSecond
        Invoke(nameof(SpawnEnemy), 1f/enemySpawnPerSecond);

        //A generic dictionary with eweapontype as the key
        WEAP_DICT = new Dictionary<eWeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions){
            WEAP_DICT[def.type] = def;
        }

    }

    public void SpawnEnemy(){
        //If spawnenemies is false skip to the next invoke of spawn enemy
        if(!spawnEnemies){
            Invoke(nameof(SpawnEnemy), 1f/enemySpawnPerSecond);
            return;
        }
        //Pick a random Enemy prefab to instanitate
        int ndx = UnityEngine.Random.Range(0,prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        //Position the enemy above the scren with a random x position
        float enemyInset = enemyInsetDefault;
        if(go.GetComponent<BoundsCheck>() != null){
            enemyInset = MathF.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //Set the initial position for the spawned enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyInset;
        float xMax = bndCheck.camWidth - enemyInset;
        pos.x = UnityEngine.Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyInset;
        go.transform.position = pos;

        //Invoke spawnEnemy again
        Invoke(nameof(SpawnEnemy), 1f/enemySpawnPerSecond);
    }
    void DelayedRestart() {
        //Invoke the restart() method in gameRestartDelay seconds
        Invoke(nameof (Restart), gamesRestartDelay);
    }

    void Restart() {
        //Reload _Scene_0 to restart the game
        SceneManager.LoadScene("__Scene_0");
    }

    static public void HERO_DIED(){
        S.DelayedRestart();
    }

    static public WeaponDefinition GET_WEAPON_DEFINITION(eWeaponType wt){
        if(WEAP_DICT.ContainsKey(wt)){
            return(WEAP_DICT[wt]);
        }

        //If no entery of the correct type exits in weap_dict, return a new wepaondefintion withe  a type of eWeaponType.noe
        return(new WeaponDefinition());
    }

    static public void SHIP_DESTROYED(Enemy e){
        if(UnityEngine.Random.value <= e.powerUpDropChance){
            int ndx = UnityEngine.Random.Range(0,S.powerUpFrequency.Length);
            eWeaponType pUpType = S.powerUpFrequency[ndx];
        

        GameObject go = Instantiate<GameObject>(S.prefabPowerUp);
        powerUp pUp = go.GetComponent<powerUp>();
        pUp.SetType(pUpType);

        pUp.transform.position = e.transform.position;
        }
    }

}
