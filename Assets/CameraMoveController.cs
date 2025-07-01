using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    // キャラクターオブジェクト
    public GameObject targetObj;
    // カメラとの距離
    private Vector3 offset;
    public float smoothSpeed = 0.125f; // カメラのスムーズな移動速度

    void Start()
    {
        //playerObj = GameObject.FindGameObjectWithTag(Groval.TAG_PLAYER);
        //offset = transform.position - playerObj.transform.position;
        Debug.Log("ofset:"+offset);
        offset = new Vector3(0f,120f,-260f);
    }

    void LateUpdate()
    {
        if(targetObj){
            transform.position = targetObj.transform.position + offset;
        }
    }

}
