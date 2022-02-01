using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{

    [SerializeField] GameObject ballPrefab;
    [SerializeField] Rigidbody2D pivot;
    [SerializeField] private float respawnDelay;
    [SerializeField] private float attachDuration;


    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentPivot;
    private Camera mainCamera;
    private bool isDragging;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        SpawnNewBall();

    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidbody == null)
        {
            return;
        }

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }

            isDragging = false;

            return;
        }

        isDragging = true;
        currentBallRigidbody.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidbody.position = worldPosition;

    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        Invoke(nameof(DetachBall), attachDuration);
    }

    private void DetachBall()
    {
        currentPivot.enabled = false;
        currentPivot = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }

    private void SpawnNewBall()
    {
        GameObject newBall = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRigidbody = newBall.GetComponent<Rigidbody2D>();
        currentPivot = newBall.GetComponent<SpringJoint2D>();

        currentPivot.connectedBody = pivot;

    }
}
