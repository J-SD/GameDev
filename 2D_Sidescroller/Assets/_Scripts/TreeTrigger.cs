using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TreeTrigger : MonoBehaviour
{
    bool isTree = false;
    public Transform treeCameraTarget;
    public LayerMask playerMask;

    public CinemachineVirtualCamera treeCam;

    private void Start()
    {
        treeCam.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Player.Instance)
        {
            if (Player.Instance.transform.position.x > transform.position.x) Tree();
            else NotTree();
        }

    }

    void Tree()
    {
        if (isTree) return;
        isTree = true;
        MainCamera.Instance.ChangeCam(treeCam);
        
        //MainCamera.Instance.SetTarget(treeCameraTarget);
        //MainCamera.Instance.SetSmoothSpeed(.01f);
        //MainCamera.Instance.ChangeSize(13f);
        Player.Instance.canDie = false;
    }
    void NotTree()
    {
        if (!isTree) return;
        isTree = false;
        MainCamera.Instance.ResetCam();

        //MainCamera.Instance.SetTarget(Player.Instance.transform);
        //MainCamera.Instance.ChangeSize(8);
        //MainCamera.Instance.SetSmoothSpeed(.125f);

        Player.Instance.canDie = true;


    }

}
