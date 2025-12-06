using UnityEngine;
using UnityEngine.UI;

public class SetSpeedLimitSprite : MonoBehaviour
{
    void Start()
    {
        // Load the sprite from Resources or use the asset path
        Sprite speedLimitSprite = Resources.Load<Sprite>("Images/Speed_limit_50_sign.svg");
        
        // If that doesn't work, try direct asset loading
        if (speedLimitSprite == null)
        {
            #if UNITY_EDITOR
            speedLimitSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Speed_limit_50_sign.svg.png");
            #endif
        }
        
        // Set the sprite on the Image component
        Image imageComponent = GetComponent<Image>();
        if (imageComponent != null && speedLimitSprite != null)
        {
            imageComponent.sprite = speedLimitSprite;
        }
        else
        {
            Debug.LogWarning("Could not load speed limit sprite or Image component not found.");
        }
    }
}
