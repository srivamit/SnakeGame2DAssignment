using UnityEngine;

public class Mouse : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;

        float x = Mathf.Floor(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Floor(Random.Range(bounds.min.y, bounds.max.y));

        transform.position = new Vector3(x, y, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "SnakeBody")
        {
            RandomizePosition();
        }
    }
}
