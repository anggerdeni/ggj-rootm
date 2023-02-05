using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Sprite defaultSprite;
    public Sprite pressedSprite;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>(); 
        defaultSprite = spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            buttonPressed();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            buttonPressed();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            buttonPressed();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            buttonPressed();
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            buttonReleased();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            buttonReleased();
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            buttonReleased();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            buttonReleased();
        }
    }

    void buttonPressed()
    {
        spriteRenderer.sprite = pressedSprite;
    }

    void buttonReleased()
    {
        spriteRenderer.sprite = defaultSprite;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Beat")
        {
            var gms = transform.root.GetComponent<GridManagerScript>();
            gms.allowedToMove = true;
            gms.waterEnergy -= 6;
            if (gms.waterEnergy < 0) gms.waterEnergy = 0;
            gms.UpdateWaterText();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Beat")
        {
            var gms = transform.root.GetComponent<GridManagerScript>();
            if (gms.allowedToMove && !gms.isPaused)
            {
                gms.allowedToMove = false;
                gms.missStreak += 1;
            }

            Destroy(other.gameObject);
        }
        else if (other.tag == "BeatLeft")
        {
            Destroy(other.gameObject);

        }
    }
}
