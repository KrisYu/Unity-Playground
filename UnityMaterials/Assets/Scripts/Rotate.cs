using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
 
    float degreesPerSecond = 60;

    private void Update()
    {
        transform.Rotate(new Vector3(degreesPerSecond, 0, 0) * Time.deltaTime);
    }

}
