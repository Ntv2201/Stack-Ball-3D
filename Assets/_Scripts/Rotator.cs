using UnityEditor.Rendering;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 100;

    private void Update()
    {
        Spin();
    }

    private void Spin()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }

}
