using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    public void MoveToPosition(Vector3 newPosition)
    {
        targetPosition = newPosition + Vector3.up * 0.5f; // 살짝 위로 띄우기
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }
}
