using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private Renderer bgRenderer;

    void Update()
    {
        float x = Time.time * speed;
        bgRenderer.material.mainTextureOffset = new Vector2(x, 0f);
    }
}
