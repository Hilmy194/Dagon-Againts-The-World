using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax: MonoBehaviour
{
Material mat;
float distance;

public float speed=0.2f;

void Start()
{
mat = GetComponent<Renderer>().material;
}
void Update()
{
distance += Time.deltaTime*speed;
mat.SetTextureOffset("_MainTex", Vector2.right * distance);
}
}