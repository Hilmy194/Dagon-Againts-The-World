using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f; // Kecepatan fireball
    public float lifeTime = 2f; // Waktu hidup fireball
    public float explosionDuration = 0.5f; // Durasi animasi ledakan

    private bool isExploding = false; // Mencegah multiple trigger

    private void Start()
    {
        Destroy(gameObject, lifeTime); // Hancurkan jika waktu habis
    }

    private void Update()
    {
        if (!isExploding)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime); // Gerakkan fireball
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isExploding)
        {
            isExploding = true; // Hindari trigger ganda

            // Trigger animasi ledakan
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Explode");
            }

            // Hancurkan fireball setelah animasi selesai
            Destroy(gameObject, explosionDuration);

            // Hancurkan obstacle
            Destroy(collision.gameObject);
        }
    }
}
