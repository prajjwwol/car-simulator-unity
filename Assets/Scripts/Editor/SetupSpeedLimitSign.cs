using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SetupSpeedLimitSign
{
    [MenuItem("Tools/Setup Speed Limit Sign")]
    static void Setup()
    {
        // Find the SpeedLimitSign GameObject
        GameObject speedLimitSign = GameObject.Find("SpeedLimitSign");
        if (speedLimitSign == null)
        {
            Debug.LogError("SpeedLimitSign GameObject not found!");
            return;
        }

        // Get the Image component
        Image imageComponent = speedLimitSign.GetComponent<Image>();
        if (imageComponent == null)
        {
            Debug.LogError("Image component not found on SpeedLimitSign!");
            return;
        }

        // Load the sprite
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Speed_limit_50_sign.svg.png");
        if (sprite == null)
        {
            Debug.LogError("Could not load sprite from Assets/Images/Speed_limit_50_sign.svg.png");
            return;
        }

        // Set the sprite
        imageComponent.sprite = sprite;
        
        // Mark the scene as dirty so it can be saved
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(speedLimitSign.scene);
        
        Debug.Log("Speed limit sign sprite set successfully!");
    }
}
