using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using System.Runtime.InteropServices;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    private float currentTime;
    private bool smash, invicible;
    private int currentBrokenStack, totalStacks;

    public GameObject invicibleObj;
    public GameObject fireEffect;
    public Image invicibleFill;
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
        currentBrokenStack = 0; 
    }

    private void Start()
    {
        totalStacks = FindObjectsByType<StackController>().Length;
    }

    private void Update()
    {

        if(ballState == BallState.Playing)
        {
            Smashing();
            OnInvicible();
        }

        if(ballState == BallState.Prepare)
        {
            if (Input.GetMouseButton(0))
            {
                ballState = BallState.Playing;
            }

            if(ballState == BallState.Finish)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    FindAnyObjectByType<LevelSpawner>().NextLevel();
                }
            }
        }

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
            rb.linearVelocity = new Vector3(rb.linearVelocity.x , 5, rb.linearVelocity.z);
        }
    }

    public void IncreaseBrokenStack()
    {
        currentBrokenStack++;

        if (!invicible)
        {
            ScoreManager.instance.AddScore(1);
            SoundManager.instance.PlaySoundFX(destroyClip, 0.5f);
        }
        else
        {
            ScoreManager.instance.AddScore(2);
            SoundManager.instance.PlaySoundFX(iDestroyClip, 0.5f);
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

        FindAnyObjectByType<GameUI>().LevelSliderFill(currentBrokenStack / (float)totalStacks);

        if(target.gameObject.tag == "Finish" && ballState == BallState.Playing)
        {
            ballState = BallState.Finish;
            SoundManager.instance.PlaySoundFX(winClip, .8f);
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
            
            // set fire FX
            if (!fireEffect.activeInHierarchy)
            {
                fireEffect.SetActive(true);
            }
        }
        else
        {   
            // unset fire FX
            if (fireEffect.activeInHierarchy)
            {
                fireEffect.SetActive(false);
            }
            
            if (smash)
            {
                currentTime += Time.deltaTime * .8f;
            }
            else
            {
                currentTime -= Time.deltaTime * .5f;
            }
        }

        if(currentTime >= .35f || invicibleFill.color == Color.red)
        {
            invicibleObj.SetActive(true);
        }
        else
        {
            invicibleObj.SetActive(false);
        }

        if(currentTime >= 1)
        {
            currentTime = 1;
            invicible = true;
            invicibleFill.color = Color.red;
        }
        else if(currentTime <= 0)
        {
            currentTime = 0;
            invicible = false;
            invicibleFill.color = Color.white;
        }

        if (invicibleObj.activeInHierarchy)
        {
            invicibleFill.fillAmount = currentTime / 1;
        }
    }
}
