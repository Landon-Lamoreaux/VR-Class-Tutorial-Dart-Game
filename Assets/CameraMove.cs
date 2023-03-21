using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    private float thetaY = 0;
    private float thetaX = 0;
    [SerializeField]
    private float hoizontalArc = 50;
    [SerializeField]
    private float verticalArc = 30;
    [SerializeField]
    private float speed = 0.2f;

    public void move(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();
        thetaY += delta.x * speed;
        thetaX -= delta.y * speed;
        thetaY = Mathf.Clamp(thetaY, -hoizontalArc, hoizontalArc);
        thetaX = Mathf.Clamp(thetaX, -verticalArc, verticalArc);
        transform.localRotation = Quaternion.Euler(thetaX, thetaY, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
