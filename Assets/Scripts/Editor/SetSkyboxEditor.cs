using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class SetSkyboxEditor
{
    static SetSkyboxEditor()
    {
        EditorApplication.delayCall += ApplySkybox;
    }

    [MenuItem("Tools/Set Day Skybox")]
    static void ApplySkybox()
    {
        Material skyboxMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/ARCADE - FREE Racing Car/Skybox/Day/Day Skybox.mat");
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;
            DynamicGI.UpdateEnvironment();
            Debug.Log("Day Skybox applied successfully to the scene!");
            EditorApplication.delayCall -= ApplySkybox; // Remove callback after execution
        }
        else
        {
            Debug.LogError("Could not find Day Skybox material at path!");
        }
    }
}
