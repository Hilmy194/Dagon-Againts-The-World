using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Kecepatan gerakan

    private void Update()
    {
        // Gerakkan enemy ke kiri
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }
}
