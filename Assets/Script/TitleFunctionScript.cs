using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleFunctionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // AudioManager.I.Play("Title");


        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S)) SceneManager.LoadScene(1);
        
    }
}
