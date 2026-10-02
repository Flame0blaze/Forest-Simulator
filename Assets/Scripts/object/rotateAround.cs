using UnityEngine;

public class rotateAround : MonoBehaviour
{
    public float rotateSpeed = 50;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.World);
    }
}