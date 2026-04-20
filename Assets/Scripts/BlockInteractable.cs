using UnityEngine;

public class BlockInteractable : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip[] variants = new AudioClip[3];
    public AudioSource audioSource;

    [Header("State")]
    public int currentVariant = 0;
    public bool isPlaced = false;
    public bool isHeld = false;
    public bool isPaused = false;

    [Header("Visual")]
    public Renderer domeRenderer;

    // 每个变体的颜色，也作为base颜色
    private Color[] variantColors = new Color[]
    {
        new Color(2f, 0.6f, 2f),    // 变体0 粉紫
        new Color(0.6f, 2f, 2f),    // 变体1 青蓝
        new Color(2f, 1.5f, 0.3f)   // 变体2 金黄
    };

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.volume = 0f;
            if (variants[0] != null)
                audioSource.clip = variants[0];
            audioSource.Play();
        }

        // 设置初始颜色为变体0的颜色
        if (domeRenderer != null)
            domeRenderer.material.SetColor("_EmissionColor", variantColors[0] * 0.5f);
    }

    public void OnGrab()
    {
        isHeld = true;
        currentVariant = (currentVariant + 1) % variants.Length;

        if (audioSource != null && variants[currentVariant] != null)
        {
            audioSource.clip = variants[currentVariant];
            audioSource.volume = 1f;
            audioSource.Play();
        }

        // 切换颜色反馈
        if (domeRenderer != null)
            domeRenderer.material.SetColor("_EmissionColor", variantColors[currentVariant]);
    }

    public void OnRelease()
    {
        isHeld = false;
    }

    public void OnTap()
    {
        currentVariant = (currentVariant + 1) % variants.Length;
        if (audioSource != null && variants[currentVariant] != null)
        {
            audioSource.clip = variants[currentVariant];
            audioSource.Play();
        }
        if (domeRenderer != null)
            domeRenderer.material.SetColor("_EmissionColor", variantColors[currentVariant]);
    }

    public void OnPlaced()
    {
        isPlaced = true;
        if (audioSource != null)
            audioSource.volume = 1f;
        if (domeRenderer != null)
            domeRenderer.material.SetColor("_EmissionColor",
                variantColors[currentVariant] * 2f);
    }

    public void OnRemoved()
    {
        isPlaced = false;
        if (audioSource != null)
            audioSource.volume = 0f;
        if (domeRenderer != null)
            domeRenderer.material.SetColor("_EmissionColor",
                variantColors[currentVariant] * 0.5f);
    }

    public void OnTogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            audioSource?.Pause();
            if (domeRenderer != null)
                domeRenderer.material.SetColor("_EmissionColor",
                    variantColors[currentVariant] * 0.2f);
        }
        else
        {
            audioSource?.UnPause();
            if (domeRenderer != null)
                domeRenderer.material.SetColor("_EmissionColor",
                    variantColors[currentVariant] * 2f);
        }
    }
}