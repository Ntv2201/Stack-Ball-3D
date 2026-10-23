using System.Collections;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private StackPartController[] stackPartControlls = null;

    public void ShatterAllPart()
    {
        if(transform.parent != null)
        {
            transform.parent = null;
            FindAnyObjectByType<Ball>().IncreaseBrokenStack();
        }

        foreach(StackPartController stack in stackPartControlls)
        {
            stack.Shatter();
        }
        StartCoroutine(RemoveAllPartRoutine());
    }

    private IEnumerator RemoveAllPartRoutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}
