using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMoveScript : MonoBehaviour
{
    public string direction = "left";
    public float beatTempo;

    public bool canBePressed = false;
    // Start is called before the first frame update
    void Start()
    {
        beatTempo = beatTempo / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == "left")
            transform.position += Vector3.left * beatTempo * Time.deltaTime;
        else 
            transform.position += Vector3.right * beatTempo * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = false;
        }
    }
}
