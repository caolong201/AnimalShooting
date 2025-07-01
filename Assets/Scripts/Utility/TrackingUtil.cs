using UnityEngine;
using com.adjust.sdk;
using GameAnalyticsSDK;

/// <summary>
/// トラッキングツールを操作するためのクラス
/// </summary>
public static class TrackingUtil
{
    /// <summary>
    /// Adjustアプリトークン
    /// </summary>
    private static readonly string ADJUST_AP_TOKEN = "a7y0grjw5xxc";
    
    /// <summary>
    /// 初期化処理
    /// </summary>
    public static void Init()
    {
        // adjust
        InitializeAdjust();
        Debug.Log("Adjust計測");
        InitializeGameAnalytics();
        Debug.Log("GA計測");
    }

    // public static void StartEvent(){
    //     AdjustEvent starEvent = new AdjustEvent("5xtbey");
    //     Adjust.trackEvent(starEvent);
    // }

    // public static void EndEvent(){
    //     AdjustEvent endEvent = new AdjustEvent("xn6ao1");
    //     Adjust.trackEvent(endEvent);
    // }

    #region Adjust

    /// <summary>
    /// Adjustの初期化処理
    /// </summary>
    private static void InitializeAdjust()
    {
        
        AdjustConfig adjustConfig = new AdjustConfig(
            ADJUST_AP_TOKEN,
            AdjustEnvironment.Production,
            true
        );
        
        adjustConfig.setLogLevel(AdjustLogLevel.Info);
        adjustConfig.setSendInBackground(true);
        
        new GameObject("Adjust").AddComponent<Adjust>();

        Adjust.start(adjustConfig);
    }

    #endregion

    #region GameAnalytics
    /// <summary>
    /// GameAnalyticsの初期化処理
    /// Androidのみの実装
    /// </summary>
    private static void InitializeGameAnalytics()
    {
        GameAnalytics.Initialize();
    }
    
    /// <summary>
    /// イベントを送信する
    /// </summary>
    /// <param name="eventName"></param>
    public static void GameAnalyticsNewEvent(string eventName)
    {
        GameAnalytics.NewDesignEvent(eventName);
    }
    
    /// <summary>
    /// パラメータ付きのイベントを送信する
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="value"></param>
    public static void GameAnalyticsNewEventWithValue(string eventName, int value)
    {
        GameAnalytics.NewDesignEvent(eventName, value);
    }
    #endregion
}
