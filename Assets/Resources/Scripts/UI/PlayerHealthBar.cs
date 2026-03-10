using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Canvas canvas;
    private RectTransform healthBarRect;

    void Start()
    {
        healthBarRect = GetComponent<RectTransform>();
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
        
        // 设置血条大小为合适的尺寸
        if (healthBarRect != null)
        {
            healthBarRect.sizeDelta = new Vector2(150, 25); // 增大血条尺寸
            // 设置血条位置在屏幕左下角
            healthBarRect.anchorMin = new Vector2(0, 0);
            healthBarRect.anchorMax = new Vector2(0, 0);
            healthBarRect.pivot = new Vector2(0, 0);
            healthBarRect.anchoredPosition = new Vector2(20, 20); // 距离左下角的偏移
        }
    }

    void Update()
    {
        // 血条固定在屏幕左下角，不需要跟随玩家
    }
}
