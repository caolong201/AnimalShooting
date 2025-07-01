using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AdUtil
{
    // <summary>
    // MAXのSDKキー
    // Applovinのアカウントと紐付くもの。アプリごとに変更する必要なし
    // 20241101更新
    // </summary>
    private static readonly string kMaxSdkKey = "uoUeqafHkGFIAZiF5MuQ6TtTnKkEDFSF4cT9m2fwJAMddQmxlzaXDThNlT2i40sxDBoZ8Cuz3nu5UWSlG39d9u";
    
    // <summary>
    /// 初期化済みか
    // </summary>
    private static bool _isInitialized = false;

    // <summary>
    /// 初期化処理
    // </summary>
    public static void Init()
    {
        if (_isInitialized)
        {
            return;
        }
        
        _isInitialized = true;

        InitializeMax();
    }

    #region MAX SDK

    private static void InitializeMax()
    {
        MaxSdk.SetVerboseLogging(true);
#pragma warning disable 618
        MaxSdk.SetSdkKey(kMaxSdkKey);
#pragma warning restore 618
        MaxSdk.InitializeSdk();
        Debug.Log("MAX計測");
    }
    #endregion
}