using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SnakeMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction = Vector2.right;
    private bool isReversed = false;

    public Button buttonU;
    public Button buttonD;
    public Button buttonR;
    public Button buttonL;
    public Button reverseButton;

    public Text scoreText;

    private int score = 0;
    private List<Transform> segments = new List<Transform>();
    private List<Vector3> segmentPositions = new List<Vector3>();

    public GameObject gameOverPanel;

    private SpriteRenderer spriteRenderer;

    public GameObject segmentPrefab;

    public RectTransform gridArea;

    public float segmentDistance = 3f;

    void Start()
    {
        gameOverPanel.SetActive(false);
        buttonU.onClick.AddListener(MoveUp);
        buttonD.onClick.AddListener(MoveDown);
        buttonR.onClick.AddListener(MoveRight);
        buttonL.onClick.AddListener(MoveLeft);
        reverseButton.onClick.AddListener(ToggleReverse);

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.green;

        segments.Clear();
        segments.Add(transform);

        for (int i = 0; i < 4; i++)
        {
            Grow(false);
        }

        segmentPositions.Clear();
        segmentPositions.Add(transform.position);
    }

    void Update()
    {
        Move();
        CheckOutOfBounds();
    }

    void MoveUp()
    {
        direction = isReversed ? Vector2.down : Vector2.up;
    }

    void MoveDown()
    {
        direction = isReversed ? Vector2.up : Vector2.down;
    }

    void MoveRight()
    {
        direction = isReversed ? Vector2.left : Vector2.right;
    }

    void MoveLeft()
    {
        direction = isReversed ? Vector2.right : Vector2.left;
    }

    void Move()
    {
        Vector3 previousHeadPosition = transform.position;
        transform.Translate(direction * speed * Time.deltaTime);
        segmentPositions.Insert(0, transform.position);

        for (int i = 1; i < segments.Count; i++)
        {
            Vector3 segmentPosition = segmentPositions[i];
            segments[i].position = segmentPosition;
        }

        if (segmentPositions.Count > segments.Count)
        {
            segmentPositions.RemoveAt(segmentPositions.Count - 1);
        }

        for (int i = 1; i < segments.Count; i++)
        {
            Vector3 previousSegmentPosition = segments[i - 1].position;
            Vector3 currentSegmentPosition = segments[i].position;
            Vector3 directionToPrevious = (previousSegmentPosition - currentSegmentPosition).normalized;
            segments[i].position = previousSegmentPosition - directionToPrevious * segmentDistance;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("SnakeBody"))
        {
            GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            Grow();
            RandomizeMousePosition(other.gameObject);
        }
    }

    void Grow(bool addToLength = true)
    {
        Vector3 newSegmentPosition = segments.Count > 0 ? segments[segments.Count - 1].position : transform.position;
        GameObject newSegment = Instantiate(segmentPrefab, newSegmentPosition, Quaternion.identity);
        segments.Add(newSegment.transform);
        segmentPositions.Add(newSegment.transform.position);

        if (addToLength)
        {
            score++;
            scoreText.text = "Score: " + score.ToString();
        }
    }

    void RandomizeMousePosition(GameObject mouse)
    {
        if (gridArea != null)
        {
            Vector3[] worldCorners = new Vector3[4];
            gridArea.GetWorldCorners(worldCorners);
            Vector2 minPosition = worldCorners[0];
            Vector2 maxPosition = worldCorners[2];
            float x = Random.Range(minPosition.x, maxPosition.x);
            float y = Random.Range(minPosition.y, maxPosition.y);
            mouse.transform.position = new Vector3(x, y, 0f);
        }
    }

    void CheckOutOfBounds()
    {
        if (gridArea != null)
        {
            Vector3[] worldCorners = new Vector3[4];
            gridArea.GetWorldCorners(worldCorners);
            Vector2 minPosition = worldCorners[0];
            Vector2 maxPosition = worldCorners[2];

            if (transform.position.x < minPosition.x || transform.position.x > maxPosition.x ||
                transform.position.y < minPosition.y || transform.position.y > maxPosition.y)
            {
                GameOver();
            }
        }
    }

    void ToggleReverse()
    {
        isReversed = !isReversed;
        spriteRenderer.color = isReversed ? Color.red : Color.green;
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
