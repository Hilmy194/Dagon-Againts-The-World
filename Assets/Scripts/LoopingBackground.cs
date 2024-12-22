using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    public float backgroundSpeed; // Kecepatan pergerakan background
    public Renderer backgroundRenderer; // Renderer untuk background

    // Update dipanggil sekali setiap frame
    void Update()
    {
        backgroundRenderer.material.mainTextureOffset += new Vector2(backgroundSpeed * Time.deltaTime, 0f); 
        // Menggeser tekstur secara horizontal
    }
}
