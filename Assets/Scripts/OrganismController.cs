using UnityEngine;
using System.Collections;

public class OrganismController : MonoBehaviour
{
    public Animator animator;
    //public Animator animatorBack;

    public float bloomScale = 1.4f;
    public float scaleSpeed = 1.5f;

    private int currentCount = 0;
    private bool isFullBloom = false;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        if (animator == null)
        {
            Debug.LogWarning("Animator is not assigned.");
        }
    }

    void Update()
    {
        if (isFullBloom)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                originalScale * bloomScale,
                Time.deltaTime * scaleSpeed
            );
        }
        else
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                originalScale,
                Time.deltaTime * scaleSpeed
            );
        }
    }

    public void UpdateActiveCount(int newCount)
    {
        Debug.Log("UpdateActiveCount called with: " + newCount);

        currentCount = newCount;

        if (animator == null) return;

        // Force reset to Idle first
        animator.Play("Idle", 0, 0f);

       /* if (animatorBack != null)
        {
            animatorBack.Play("Idle", 0, 0f);
        }
*/
        StopAllCoroutines();
        StartCoroutine(PlayClipDelayed(currentCount));

        isFullBloom = currentCount >= 5;
    }

    private IEnumerator PlayClipDelayed(int count)
    {
        yield return null;

        string clipName = count switch
        {
            1 => "Clip_Rhythm",
            2 => "Clip_Bass",
            3 => "Clip_Harmony",
            4 => "Clip_Melody",
            5 => "Clip_Sample",
            _ => "Idle"
        };

        Debug.Log("Trying to play: " + clipName);

        // Check if state exists before playing
        if (HasState(animator, clipName))
        {
            animator.Play(clipName, 0, 0f);

           /* if (animatorBack != null && HasState(animatorBack, clipName))
            {
                animatorBack.Play(clipName, 0, 0f);
            }
            */
        }
        else
        {
            Debug.LogWarning("Animator state NOT FOUND: " + clipName);
        }
    }

    // Helper to check if animator has a state
    bool HasState(Animator anim, string stateName)
    {
        for (int i = 0; i < anim.layerCount; i++)
        {
            if (anim.HasState(i, Animator.StringToHash(stateName)))
            {
                return true;
            }
        }
        return false;
    }
}