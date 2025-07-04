using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using IE.RSB;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float countDown = 20.0f;
    public UIManager uIManager;
    
    //クリックされたかどうか
    private bool inGameBool = false;
    [SerializeField]private TouchCameraRotation m_touchCameraRotation = null;
    [SerializeField] private PlayerWeaponController m_weaponController = null;
    [SerializeField] private MotionController m_weaponMotionController = null;

    [SerializeField]private StageSetting m_stageSetting;
    [SerializeField]private GameObject player;
    [SerializeField]private Transform enemyParent;
    [SerializeField]private List<GameObject> animals = new List<GameObject>();
    [SerializeField]private Button nextBtn;

    private int stageNo = 1;
    private int stageNoSub = 1;
    private string stageName = "";

    public int targetAnimal = 0; 
    public List<int> AnimalCount = new List<int>();
    
    private bool clearStatus= false;
    [SerializeField] private ArrowIndicatorManager arrowManager;
    //[SerializeField] private List<Transform> allEnemies; // Gán 4 enemy ở đây
    void Awake(){
    }

    void Start()
    {
        //arrowManager.SetEnemies(allEnemies);

        //ステージ生成
        StageGenerate();
    }

    void Update()
    {
        if(!inGameBool) return;

        //時間をカウントダウンする
        countDown -= Time.deltaTime;

        if(countDown < 0){
            countDown = 0f;
            inGameBool = false;
        }
    }

    void LateUpdate(){
        if (SniperAndBallisticsSystem.instance.BulletTimeRunning) return;
        if(m_weaponController.ammoZeroFlg){
            m_weaponController.ammoZeroFlg = false;
            if(targetAnimal > 0){
                //Debug.Log("ここまできたTest1");
                Faild();
            }
        }

        if(clearStatus){
            clearStatus = false;
            StageClear();
        }
    }

    void StageGenerate(){
        //Debug.Log("StartNo:"+stageNo);
        stageNo = PlayerPrefs.GetInt("StageNo", 1);
        //ステージループ用
        if(stageNo > 3){
            stageName = "";
            stageNo = 1;
            PlayerPrefs.DeleteKey("StageName");
            PlayerPrefs.DeleteKey("StageNo");
        }

        //現在のステージ名を取得（最初の場合はStage1）
        stageName = PlayerPrefs.GetString("StageName", "Stage1");
        stageNoSub = PlayerPrefs.GetInt("StageNoSub", 1);


        // Debug.Log("stageName:"+stageName);
        // Debug.Log("stageNo:"+stageNo);
        // Debug.Log("stageNoSub:"+stageNoSub);
        // Debug.Log("targetAnimal:"+targetAnimal);
        


        //StageNameセット
        uIManager.SetStageName(("Stage"+stageNoSub.ToString()));

        var stageData = m_stageSetting.DataList.FirstOrDefault(stage => stage.StageName == stageName);
        
        //弾数をセット
        uIManager.SetBulletCount(stageData.BulletCount);

        //プレイヤーのセッティング
        Vector3 playerPos = stageData.PlayerPosition;
        //Debug.Log("PlayPos:"+playerPos);
        player.transform.position = playerPos;
        player.transform.rotation =Quaternion.Euler(stageData.PlayerRoatation);
        Camera.main.transform.localRotation = Quaternion.identity;
        Camera.main.gameObject.GetComponent<TouchCameraRotation>().ResetRotation();

        //ターゲットアニマル生成
        foreach (Transform child in enemyParent)
        {
            Destroy(child.gameObject);
        }
        for(int i=0;i<stageData.GenerateAnimalNo.Count;i++){
            int no = stageData.GenerateAnimalNo[i] -1;

            Vector3 spawnPosition = stageData.GenerateAnimalPosition[i];
            Quaternion spawnRotation = Quaternion.Euler(stageData.GenerateAnimalRoatation[i]);

            GameObject newObject = Instantiate(animals[no], spawnPosition, spawnRotation, enemyParent);
            if(stageData.GenerateAnimalAnimation[i] > 0){
                newObject.GetComponent<Enemy>().DoMoveAnimal(stageData.GenerateAnimalAnimation[i]);
            }
        }

        ////ll
        //List<Transform> enemyList = new List<Transform>();
        //foreach (Transform child in enemyParent)
        //{
        //    enemyList.Add(child);
        //}

        //if (arrowManager != null)
        //{
        //    arrowManager.SetEnemies(enemyList);
        //}

        List<Enemy> enemyList = new List<Enemy>();
        foreach (Transform child in enemyParent)
        {
            Enemy enemy = child.GetComponent<Enemy>();
            if (enemy != null) enemyList.Add(enemy);
        }
        arrowManager.SetEnemies(enemyList);



        targetAnimal = 0;
        AnimalCount = new List<int>();
        //ターゲット情報のUI更新
        for(int i=0;i<stageData.Animal.Count;i++){
            targetAnimal += stageData.Animal[i];
            AnimalCount.Add(stageData.Animal[i]);
            //Bear
            if(stageData.Animal[i] > 0){
                //テキスト更新
                uIManager.TargetUI[i].transform.Find("Content/Label_KilledUser").gameObject.GetComponent<TextMeshProUGUI>().text = "<color=#EC6161>"+stageData.Animal[i].ToString();
                //テキスト表示
                uIManager.TargetUI[i].SetActive(true);
            }else{
                if(uIManager.TargetUI[i].activeSelf) uIManager.TargetUI[i].SetActive(false);
            }
            uIManager.TargetAnimalReset(i);
        }

        //UIリセット
        uIManager.HUDView();
        
        //クリアテキスト
        for(int i=0;i<stageData.StageStarConditions.Count;i++){
            uIManager.ClearText(i,stageData.StageStarConditions[i]);
        }

        StartCoroutine(Complete());
    }

    public void EnemyDown(int No){
        No -= 1;
        targetAnimal -= 1;
        AnimalCount[No] -= 1;

        uIManager.TargetUI[No].transform.Find("Content/Label_KilledUser").gameObject.GetComponent<TextMeshProUGUI>().text = "<color=#EC6161>"+AnimalCount[No].ToString();
        if(AnimalCount[No] <= 0){
            EnemyCountZero(No);
        }

        //全ての動物がいなくなったか？チェック
        if(targetAnimal <=0){
            clearStatus = !clearStatus;
        }
    }
    //対象動物の数が0になった際の処理
    void EnemyCountZero(int No){
        uIManager.TargetAnimalComplete(No);
    }

    IEnumerator Complete()
    {
        yield return new WaitForSeconds(3); 
        uIManager.LoadingComplete();
        yield return new WaitForSeconds(1); 
        inGameBool = true;
        m_touchCameraRotation.touchFlg = true;
    }

    void StageClear(){
        if (SniperAndBallisticsSystem.instance.BulletTimeRunning) return;
        m_touchCameraRotation.touchFlg = false;
        //NextButtonにListenerを紐づける
        int nextStage = stageNo+1;
        PlayerPrefs.SetInt("StageNo", nextStage);
        PlayerPrefs.SetInt("StageNoSub", nextStage);
        PlayerPrefs.SetString("StageName", ("Stage"+nextStage));
        nextBtn.onClick.AddListener(() => OnClickStageNext(nextStage));
        StartCoroutine(ClearEchoes());
    }

    //クリア演出の余韻が欲しい
    IEnumerator ClearEchoes(){
        //クリア状態
        //残り玉数と条件の比較
        int bullet = m_weaponController.m_availableAmmoNow;
        var stageData = m_stageSetting.DataList.FirstOrDefault(stage => stage.StageName == stageName);

        for(int i=0;i<stageData.StageStarConditions.Count;i++){
            if(bullet >= stageData.StageStarConditions[i]){
                //Debug.Log("Clear"+i);
                uIManager.ClearStatusUIChange(i);
            }
        }
        yield return new WaitForSeconds(1); 
        uIManager.ClearUIChange();
    }

    void Faild(){
        if (SniperAndBallisticsSystem.instance.BulletTimeRunning) return;
        m_touchCameraRotation.touchFlg = false;
        uIManager.FaildUIChange();
    }

    public void OnClickStageRetry(bool clearStatus){
        //Debug.Log("リスタート！");
        uIManager.LoadingStart();
        if(clearStatus){
            uIManager.ClearUIChange();
            PlayerPrefs.SetInt("StageNo", stageNo);
            PlayerPrefs.SetInt("StageNoSub", stageNoSub);
            PlayerPrefs.SetString("StageName", ("Stage"+stageNo));
        }else{
            uIManager.FaildUIChange();
        }
        StageGenerate();
    }

    public void OnClickStageNext(int nextStage){
        //Debug.Log("次のステージ");
        uIManager.LoadingStart();
        uIManager.ClearUIClose();
        //セーブ
        StageGenerate();
    }

    // ゲーム終了時に保存
    void OnDestroy()
    {
        
    }
    void onPause()
    {
    }
}
