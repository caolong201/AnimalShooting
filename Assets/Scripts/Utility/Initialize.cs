using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour
{

    private void Awake()
    {
        Initialize_SDK();
    }

    public void Initialize_SDK()
    {
        //AdTracking
        TrackingUtil.Init(); //adjust
        AdUtil.Init(); //MAX
        //Debug.Log("計測開始");
    }
}
