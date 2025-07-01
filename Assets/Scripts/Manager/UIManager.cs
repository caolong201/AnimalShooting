using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using IE.RSB;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //時間を表示するText型の変数
    public TextMeshProUGUI timeText;

    public GameObject PopupTimeUp;

    [SerializeField]
    private GameObject LoadingScene = null;

    [SerializeField] private GameObject InGameUI = null;
    [SerializeField] private CanvasGroup HUD = null;
    [SerializeField] private GameObject FaildUI = null;
    [SerializeField] private GameObject ClearUI = null;

    [SerializeField] private RectTransform ScopeUnmask = null;
    [SerializeField] private TextMeshProUGUI bulletText = null;
    [SerializeField] private PlayerWeaponController m_weaponController = null;

    private float tallScreenY = 80f; // 縦長画面の時のY座標
    private float wideScreenY = 40f; // 横長画面の時のY座標
    private float aspectThreshold = 1.8f;

    [SerializeField]public List<GameObject> TargetUI;

    //Getcomponent<Image>.color = new Color32 (242, 108, 216, 255);
    [SerializeField] private List<GameObject> ClearStar = new List<GameObject>();
    [SerializeField] private List<GameObject> ClearCheck = new List<GameObject>();
    [SerializeField] private List<TextMeshProUGUI> ClearCheckText = new List<TextMeshProUGUI>();

    void Awake(){
        for(int i=0;i<ClearStar.Count;i++){
            ClearStar[i].SetActive(false);
            ClearCheck[i].GetComponent<Image>().color = new Color32 (255, 255, 255, 0);
            ClearCheckText[i].text = "";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AdjustPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TimeCountDown(float time){
        if(time > 5){
            timeText.text = time.ToString("f1");
        }else{
            timeText.text = "<color=red>"+time.ToString("f1")+"</color>";
        }

        //0の場合TimeUp
        if(time < 0.8f){
            if(!PopupTimeUp.activeSelf) PopupTimeUp.SetActive(true);
        }
    }

    public void LoadingComplete(){
        LoadingScene.SetActive(false);
        InGameUI.SetActive(true);
    }

    public void LoadingStart(){
        InGameUI.SetActive(false);
        LoadingScene.SetActive(true);
    }

    void AdjustPosition()
    {
        if (ScopeUnmask == null) return;

        // 現在のアスペクト比（画面の幅 ÷ 高さ）
        float aspectRatio =  Screen.height /(float)Screen.width;

        // 縦長か横長か判定
        float newY = (aspectRatio > aspectThreshold) ? tallScreenY : wideScreenY;

        // y軸の変更（x, y, z のうち y だけ変更）
        SetStretchedRectOffset(ScopeUnmask,55,newY,55,newY);
    }

    public static void SetStretchedRectOffset(RectTransform rectT, float left, float top, float right, float bottom) {
        rectT.offsetMin = new Vector2(left, bottom);
        rectT.offsetMax = new Vector2(-right, -top);
    }

    public void SetBulletCount(int bulletCount){
        m_weaponController.m_ammoInMagazine = bulletCount;
        m_weaponController.m_availableAmmoNow = bulletCount;
        bulletText.text = bulletCount.ToString("000"); 
    }

    public void SetStageName(string name){
        timeText.text = name;
    }

    public void FaildUIChange(){
        FaildUI.SetActive(!FaildUI.activeSelf);
        HUDChange();
    }

    public void ClearUIChange(){
        ClearUI.SetActive(!ClearUI.activeSelf);
        HUDChange();
    }
    public void ClearUIClose(){
        ClearUI.SetActive(false);
        HUDChange();
    }

    public void ClearStatusUIChange(int no){
        if(no > 3) return; 
        ClearStar[no].SetActive(true);
        ClearCheck[no].GetComponent<Image>().color = new Color32 (0, 255, 0, 255);
    }

    public void ClearText(int no,int targetNo){
        ClearCheckText[no].text = targetNo.ToString()+" or more bullets remaining";
    }

    public void HUDChange(){
        HUD.alpha = 0;
    }
    public void HUDView(){
        HUD.alpha = 1;
    }

    public void TargetAnimalComplete(int No){
        //達成の演出

        //
        TargetUI[No].transform.Find("Content/Label_KilledUser").gameObject.SetActive(false);
        TargetUI[No].transform.Find("Content/Label_UserName").gameObject.SetActive(false);
        TargetUI[No].transform.Find("Content/ICON/Check").gameObject.SetActive(true);
    }

    public void TargetAnimalReset(int No){
        //達成の演出

        //
        TargetUI[No].transform.Find("Content/Label_KilledUser").gameObject.SetActive(true);
        TargetUI[No].transform.Find("Content/Label_UserName").gameObject.SetActive(true);
        TargetUI[No].transform.Find("Content/ICON/Check").gameObject.SetActive(false);
    }
}

