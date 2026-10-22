using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    private bool smash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        Smashing();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            smash = true;
            rb.linearVelocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
        }

        if(rb.linearVelocity.y > 5)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x ,5, rb.linearVelocity.z);
        }
    }

    private void OnCollisionEnter(Collision target)
    {
        if (!smash)
        {
            rb.linearVelocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
        else
        {
            if(target.gameObject.tag == "enemy")
            {
                Destroy(target.transform.parent.gameObject);
            }

            if(target.gameObject.tag == "plane")
            {
                Debug.Log("Over");
            }
        }

    }

    private void OnCollisionStay(Collision target)
    {
        if(!smash || target.gameObject.tag == "Finish")
        {
            rb.linearVelocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
    }

    private void Smashing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            smash = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            smash = false;
        }
    }
}
