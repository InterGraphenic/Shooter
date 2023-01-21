using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float maxdistance = 5f;
    public float yRotation = 0f;
    // Start is called before the first frame update
    void Start()    
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(target == null){return;}
        yRotation += Mouse.current.delta.ReadValue().y * Time.deltaTime * -30f;
        yRotation = Mathf.Clamp(yRotation, -87f, 87f);
        transform.rotation = target.rotation;
        transform.Rotate(Vector3.right, yRotation); 
        RaycastHit hit;
        float distance = maxdistance;
        if (Physics.Raycast(target.position + offset, transform.forward * -distance, out hit, maxdistance))
        {
            distance = hit.distance - 0.05f;
        }
        transform.position = target.position + offset + transform.forward * -distance;
    }
}
