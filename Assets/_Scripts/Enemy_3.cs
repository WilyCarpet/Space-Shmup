using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("ENemy_3 Inscribed Fields")]
    public float lifeTime = 5;
    public Vector2 midPointYRange = new Vector2(1.5f,3);
    [Tooltip("If true, the bezier poitns & path are drawn in the Scene pane.")]
    public bool drawDebugInfo = true;
    [Header("Enemy_3 Private Fields")]
    [SerializeField]
    private Vector3[] points;
    [SerializeField]
    private float birthTime;
    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[3];

        points[0] = pos;

        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        //Pick a random middle position in the bottom half of the screen
        points[1] = Vector3.zero;
        points[1].x = Random.Range(xMin,xMax);
        float midYMult = Random.Range(midPointYRange[0],midPointYRange[1]);
        points[1].y = -bndCheck.camHeight * midYMult;

        //Pick a random final position above the top of the screen
        points[2] = Vector3.zero;
        points[2].y = pos.y;
        points[2].x = Random.Range(xMin, xMax);

        //set brithTime to current time
        birthTime = Time.time;

        if(drawDebugInfo) DrawDebug();
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if(u > 1){
            //The Enemy_3 has finished its life
            Destroy(this.gameObject);
            return;
        }

        transform.rotation = Quaternion.Euler(u * 180, 0, 0);

        //Interpolate the three Bezier curve points
        u = u - 0.1f *Mathf.Sin(u * Mathf.PI * 2);
        pos = Utils.Bezier(u,points);
    }

    void DrawDebug(){
        float numSections = 20;
        Vector3 prevPoint = points[0];
        Color col;
        Vector3 pt;
        for(int i = 1; i < numSections; i++){
            float u = i / numSections;
            pt = Utils.Bezier(u, points);
            col = Color.Lerp(Color.cyan, Color.yellow, u);
            Debug.DrawLine(prevPoint, pt, col, lifeTime);
            prevPoint = pt;
        }
    }
}

