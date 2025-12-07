using UnityEngine;
using UnityEngine.UI;

public class DefilementTexte : MonoBehaviour
{
    public float speed = 100f; // pixels par seconde
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.right * speed * Time.deltaTime;
    }
}
