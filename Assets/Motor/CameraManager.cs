using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Controls")]
    public Vector3 speeds;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 forwardMvmt = transform.forward * Input.GetAxis("MovementZ") * speeds.z * Time.deltaTime;
        //Vector3 rightMvmt = transform.right * Input.GetAxis("MovementX") * speeds.x * Time.deltaTime;
        //Vector3 upMvmt = transform.up * Input.GetAxis("MovementY") * speeds.y * Time.deltaTime;
        float forwardMvmt = Input.GetAxis("MovementZ") * speeds.z * Time.deltaTime;
        float rightMvmt = Input.GetAxis("MovementX") * speeds.x * Time.deltaTime;
        float upMvmt = Input.GetAxis("MovementY") * speeds.y * Time.deltaTime;
        transform.position += new Vector3(rightMvmt, upMvmt, forwardMvmt);
    }
}
