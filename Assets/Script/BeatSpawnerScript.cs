using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSpawnerScript : MonoBehaviour
{
    public GameObject beat;
    public Transform cam;
    public float beatTempo; 
    private float timer = 0;
    private float waitTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnBeat();
        waitTime = 74f/beatTempo;
    }

    // Update is called once per frame
    void Update()
    {
        var gms = transform.root.GetComponent<GridManagerScript>();
        if (gms.gameFinished) {
            return;
        }

        if (timer < waitTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            spawnBeat();
            timer = 0;
        }
        
    }

    void spawnBeat()
    {
        var pos = transform.position;
        var newObj = Instantiate(beat, pos, transform.rotation);
        newObj.transform.SetParent(cam);

    }
}
