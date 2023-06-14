using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialRed : MonoBehaviour
{
    new Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.material.color = Color.red;

    }

}
