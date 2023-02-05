using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMoveScript : MonoBehaviour
{
    public string direction = "left";
    public float speed;

    public bool canBePressed = false;
    private float prevX;
    // Start is called before the first frame update
    void Start()
    {
        speed = 1.5f;
        prevX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        var gms = transform.root.GetComponent<GridManagerScript>();
        if (gms.gameFinished) {
            return;
        }

        if (direction == "left")
            transform.position += Vector3.left * speed * Time.deltaTime;
        else 
            transform.position += Vector3.right * speed * Time.deltaTime;
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
