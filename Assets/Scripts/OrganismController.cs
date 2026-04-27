using UnityEngine;


public class OrganismController : MonoBehaviour
{
    // 在Inspector里拖入Animator组件
    public Animator animator;
    public Animator animatorBack;    // 背面，新加
    
    // Full Bloom时的目标Scale大小
    public float bloomScale = 1.4f;
    
    // Scale变化的速度
    public float scaleSpeed = 1.5f;
    
    // 内部记录当前激活数量
    private int currentCount = 0;
    
    // 是否正在Bloom
    private bool isFullBloom = false;
    
    // 原始Scale大小
    private Vector3 originalScale;

    void Start()
    {
        // 记录模型初始大小
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 如果是Full Bloom状态，持续Lerp放大
       // if (isFullBloom)
       // {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                originalScale * bloomScale,
                Time.deltaTime * scaleSpeed
            );
      //  }
       // else
       // {
            // 没有Full Bloom就保持原始大小
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                originalScale,
                Time.deltaTime * scaleSpeed
            );
       // }

   

    }

    // 这个函数由StateManager调用
    // 每次有track激活或关闭时传入新的count值
   public void UpdateActiveCount(int newCount)
{   
     Debug.Log("UpdateActiveCount called with: " + newCount);
    currentCount = newCount;

    currentCount = newCount;
    
    // 先回到Idle状态，强制动画重新播放
    animator.Play("Idle");
     if (animatorBack != null) animatorBack.Play("Idle"); // 新加
    // 等一帧再切换到目标状态
    StartCoroutine(PlayClipDelayed(currentCount));
    
    if (currentCount >= 5)
    {
        isFullBloom = true;
    }
    else
    {
        isFullBloom = false;
    }
}


private System.Collections.IEnumerator PlayClipDelayed(int count)
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
    Debug.Log("Playing clip: " + clipName + " on animator: " + animator);
    animator.Play(clipName);
    if (animatorBack != null) animatorBack.Play(clipName);
}
}