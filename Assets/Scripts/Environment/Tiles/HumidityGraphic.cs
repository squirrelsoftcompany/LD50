using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumidityGraphic : MonoBehaviour
{
    public List<float> thresholdHumidity;

    private Material materialHumidity;

    private const string shaderName = "Shader Graphs/Humidity";
    private const string propertyThresholdName = "_CurrentThreshold";

    // Start is called before the first frame update
    void Start()
    {
        List<Material> materials = new List<Material>();
        GetComponentInChildren<MeshRenderer>().GetMaterials(materials);
        foreach (var mat in materials)
        {
            if (mat.shader.name == shaderName)
                materialHumidity = mat;
        }
    }

    public void UpdateHumidity(int fireIntensity)
    {
        var humidity = fireIntensity < 0 ? Mathf.Abs(fireIntensity) : 0;

        float threshold = 0;
        if (humidity >= 0 && humidity < thresholdHumidity.Count)
            threshold = thresholdHumidity[humidity];

        materialHumidity?.SetFloat(propertyThresholdName, threshold);
    }
}
