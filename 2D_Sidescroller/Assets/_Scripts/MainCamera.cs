using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera Instance { get; private set; }

    public float progress = 0f;

    private Vector3 pathTargetPosition;
    private Vector3 targetPosition;
    public float smoothSpeed = 0.125f;
    private float origSmoothSpeed = 0.125f;
    private float pathSpeed = 5f;
    Vector3[] path;
    bool followingPath = false;
    private bool smoothing = true;
    int currentPathPosition = 0;
    LevelManager levelManager;

    public Camera mainCam;
    public CinemachineVirtualCamera vcam;
    public CinemachineBrain brain;
    public Camera backgroundCam;

    private float size;
    bool catchingUp;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.Instance;
        //backgroundCam = GetComponentInChildren<Camera>();
        //LineRenderer lineRenderer;
        //if (transform.parent)
        //{
        //    lineRenderer = transform.parent.GetComponent<LineRenderer>();
        //    int posCount = lineRenderer.positionCount;
        //    path = new Vector3[posCount];
        //    lineRenderer.GetPositions(path);
        //}
        //origSmoothSpeed = smoothSpeed;
        size = vcam.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //followingPath = levelManager.cameraFollowsPath;
        //if (catchingUp) pathSpeed = levelManager.cameraPathSpeed * 8;
        //else pathSpeed = levelManager.cameraPathSpeed;

        //if (followingPath)
        //{
        //    FollowPath();
        //}
        //else {
        //    targetPosition = playerCamTar.position + Vector3.back + Vector3.up * 2f;
        //    Vector3 desiredPosition;
        //    if (smoothing) desiredPosition = Vector3.Slerp(transform.position, targetPosition + Vector3.back, smoothSpeed);
        //    else desiredPosition = Vector3.Slerp(transform.position, targetPosition + Vector3.back, smoothSpeed/2);
        //    transform.position = desiredPosition;
        //}
        transform.position = mainCam.transform.position;
        if (mainCam.orthographicSize != size)
        {
            //cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, size, smoothSpeed);
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(mainCam.orthographicSize, size, smoothSpeed);
        }
        // find the player if we have nothing to follow on our follow cam
        if (Player.Instance && vcam.Follow == null) vcam.Follow = Player.Instance.camTarget;

    }

    void FollowPath()
    {
        //Vector3 playerCamPosition = cam.GetComponent<Camera>().WorldToViewportPoint(
        //   new Vector3(playerCamTar.transform.position.x-1, playerCamTar.transform.position.y, 0f)
        //   );
        //float playerSpeed = playerCamTar.root.GetComponent<Rigidbody2D>().velocity.x;
        //if (playerCamPosition.x > .6f && playerSpeed >= .5f) pathSpeed =  playerSpeed + 1;


        //pathTargetPosition = path[currentPathPosition];
        //if (equal(transform.position, pathTargetPosition)) // if we reached the next point
        //{
        //    currentPathPosition++; // set next target
        //    if (currentPathPosition == path.Length) currentPathPosition = path.Length - 1; 
        //}
        //Vector3 movePosition = Vector3.MoveTowards(transform.position, pathTargetPosition + Vector3.back, Time.deltaTime * pathSpeed);
        //transform.position = movePosition;

        //progress = ((float)currentPathPosition / (float)path.Length);

    }

    bool equal(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) < .03 && Mathf.Abs(a.y - b.y) < .03;
    }

    public void ToggleSmoothPlayerFollow(bool s)
    {
        smoothing = s;
    }

    public void ChangeSize(float s)
    {
        size = s;
        CinemachineFramingTransposer composer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        composer.m_DeadZoneWidth = s / 40;
        composer.m_DeadZoneHeight = s / 80;
        composer.m_LookaheadTime = s / 16;

    }

    public bool isFollowingPath()
    {
        return followingPath;
    }



    public void CatchUp()
    {
        if (catchingUp) return;
        //catchingUp = true;

    }

    public void StopCatchUp()
    {
        if (!catchingUp) return;
        //catchingUp = false;
    }

    public void Shake()
    {
        mainCam.backgroundColor = Color.red;
    }

    public void SetSmoothSpeed(float s)
    {
        smoothSpeed = s;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = s;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = s;

    }

    public void RevertSmoothSpeed()
    {
        smoothSpeed = origSmoothSpeed;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = origSmoothSpeed;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = origSmoothSpeed;
    }

    public void SetTarget(Transform tar)
    {
        vcam.Follow = tar;
    }

    public void ChangeCam(CinemachineVirtualCamera c)
    {
        c.gameObject.SetActive(true);
        vcam.gameObject.SetActive(false);
    }

    public void ResetCam()
    {
        vcam.gameObject.SetActive(true);
    }
}
