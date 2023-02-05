using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Sprite defaultSprite;
    public Sprite successSprite;
    public Sprite missedSprite;

    private bool isDefaultSprite;
    private float timer;


    private bool missed = true;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>(); 
        defaultSprite = spriteRenderer.sprite;
        isDefaultSprite = true;
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


        if (!isDefaultSprite)
        {
            if (timer < 0.3) {
                timer += Time.deltaTime;
            } else {
                timer = 0;
                isDefaultSprite = true;
                spriteRenderer.sprite = defaultSprite;
            }
        }

    }

    void buttonPressed()
    {
        if (missed) spriteRenderer.sprite = missedSprite;
        else spriteRenderer.sprite = successSprite;

        isDefaultSprite = false;
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
            missed = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Beat")
        {
            var gms = transform.root.GetComponent<GridManagerScript>();
            gms.allowedToMove = false;

            Destroy(other.gameObject);
            missed = true;
        }
        else if (other.tag == "BeatLeft")
        {
            Destroy(other.gameObject);

        }
    }
}
