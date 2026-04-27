using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    public float duration = 1f;
    public float maxScale = 2f;

    private float timer = 0f;
    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;

        // 放大
        float scale = Mathf.Lerp(0.1f, maxScale, t);
        transform.localScale = new Vector3(scale, scale, scale);

        // 渐隐
        if (mat != null)
        {
            Color c = mat.color;
            c.a = Mathf.Lerp(0.5f, 0f, t);
            mat.color = c;
        }

        // 自动销毁
        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }
}