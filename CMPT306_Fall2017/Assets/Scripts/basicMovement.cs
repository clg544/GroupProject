using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicMovement : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame 
    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 15.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 15f;

        transform.Translate(x, z, 0);
        Vector3 MouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        transform.LookAt(MouseWorldPosition);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z));
    }
}
