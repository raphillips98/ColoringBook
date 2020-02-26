using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadsplash : MonoBehaviour
{

    bool fadeBool = false;
    float timer = 0f;
    float fadeDelay = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < fadeDelay)
        {
            timer += Time.deltaTime;
        }
        else
            SceneManager.LoadScene("FourEyedTurtle");

    }
}
