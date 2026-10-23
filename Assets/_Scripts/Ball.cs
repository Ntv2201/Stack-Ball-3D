using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    private float currentTime;
    private bool smash, invicible;

    public enum BallState
    {
        Prepare, Playing, Died, Finish
    }
    [HideInInspector]
    public BallState ballState = BallState.Prepare;
    public AudioClip boundOffClip, deadClip, winClip, destroyClip, iDestroyClip;

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
        OnInvicible();
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

    public void IncreaseBrokenStack()
    {
        if (!invicible)
        {
            ScoreManager.instance.AddScore(1);
            SoundManager.instance.PlaySoundFX(destroyClip, 0.5f);
        }
        else
        {
            ScoreManager.instance.AddScore(2);
            SoundManager.instance.PlaySoundFX(destroyClip, 0.5f);
        }
    }

    private void OnCollisionEnter(Collision target)
    {
        if (!smash)
        {
            rb.linearVelocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            SoundManager.instance.PlaySoundFX(boundOffClip, 0.5f);
        }
        else
        {
            if (invicible)
            {
                if(target.gameObject.tag == "enemy" || target.gameObject.tag == "plane")
                {
                    target.transform.parent.GetComponent<StackController>().ShatterAllPart();
                }
            }
            else
            {
                if(target.gameObject.tag == "enemy")
                {
                    target.transform.parent.GetComponent<StackController>().ShatterAllPart();
                }
                if(target.gameObject.tag == "plane")
                {
                    print("Over");
                    ScoreManager.instance.ResetScore();
                    SoundManager.instance.PlaySoundFX(deadClip, 0.5f);
                }
            }
        }

        if(target.gameObject.tag == "Finish" && ballState == BallState.Playing)
        {
            ballState = BallState.Finish;
            SoundManager.instance.PlaySoundFX(winClip, .7f);
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

    private void OnInvicible()
    {
        if (invicible)
        {
            currentTime -= Time.deltaTime * .35f;
        }
        else
        {
            if (smash)
            {
                currentTime += Time.deltaTime * .8f;
            }
            else
            {
                currentTime -= Time.deltaTime * .5f;
            }
        }

        if(currentTime >= 1)
        {
            currentTime = 1;
            invicible = true;
        }
        else if(currentTime <= 0)
        {
            currentTime = 0;
            invicible = false;
        }
        print(invicible);
    }
}
