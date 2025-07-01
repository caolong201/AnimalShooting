using System.IO;
using UnityEngine;
using LitJson;

public class DataManager : MonoBehaviour 
{
    [HideInInspector] public SaveData data;     // json変換するデータのクラス
    string filepath;                            // jsonファイルのパス
    string folder;  
    string fileName = "SaveData.json";              // jsonファイル名

    //-------------------------------------------------------------------
    // 開始時にファイルチェック、読み込み
    void Awake()
    {
        // パス名取得
        //#if UNITY_EDITOR
        //    filepath = Application.dataPath + "/Save/" + fileName;
        //#elif UNITY_ANDROID
            folder = Path.Combine(Application.persistentDataPath,"Save");
            filepath = Path.Combine(Application.persistentDataPath,"Save",fileName);  
        //#endif

        // ファイルがないとき、ファイル作成
        if (!File.Exists(filepath)) {
            //dataの初期化
            dataInit();
            Directory.CreateDirectory(folder);
            Save(data);
        }

        // ファイルを読み込んでdataに格納
        data = Load(filepath);          
    }

    //-------------------------------------------------------------------
    // jsonとしてデータを保存
    void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);                 // jsonとして変換
        StreamWriter wr = new StreamWriter(filepath, false);    // ファイル書き込み指定
        wr.WriteLine(json);                                     // json変換した情報を書き込み
        wr.Flush();                                  
        wr.Close();                                             // ファイル閉じる
    }

    // jsonファイル読み込み
    SaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);               // ファイル読み込み指定
        string json = rd.ReadToEnd();                           // ファイル内容全て読み込む
        rd.Close();                                             // ファイル閉じる
                                                                
        return JsonUtility.FromJson<SaveData>(json);            // jsonファイルを型に戻して返す
    }

    //-------------------------------------------------------------------
    // ゲーム終了時に保存
    void OnDestroy()
    {
        Save(data);
    }
    void onPause()
    {
        Save(data);
    }


    void dataInit(){
        data.tutorialStepFirst = false;
        data.tutorialStepSecond = false;
        data.tutorialStepThird = false;
        //チュートリアルステータス
        data.tutorialStatus = false;
        //所持マネー
        data.money = 0;
        //所持ハート
        data.heart = 0;
        //所持エナジー
        data.energy = 0;
        //動物の数
        data.animal = 0;
        //Autoマネー
        data.autoMoney = 0;
        //エリアレベル
        data.areaMoneyLevel = new int[]{1,1,1,1,1,1,1,1,1,1};
        data.areaHabitatLevel = new int[]{1,1,1,1,1,1,1,1,1,1};
        data.areaRewordLevel = 0;
        data.areaMoneyPrice = new string[]
            {
                //うさぎ
                "10.2a",
                //きつね
                "40.17a",
                //くま
                "431.75a",
                //狼
                "1.29b",
                //りす
                "6.47b",
                //ひつじ
                "32.38b",
                //カモメ
                "285.93b",
                //ハゲタカ
                "857.79b",
                //しか
                "2.57c",
                //アライグマ
                "7.72c"
            };
        data.areaHabitatPrice = new string[]
            {
                //うさぎ
                "30.8a",
                //きつね
                "800.5a",
                //くま
                "1.6b",
                //狼
                "41.6b",
                //りす
                "83.2b",
                //ひつじ
                "166.4b",
                //カモメ
                "4.32c",
                //ハゲタカ
                "8.65c",
                //しか
                "224.97c",
                //アライグマ
                "449.94c"
            };
        data.animalCount = new int[]{0,0,0,0,0,0,0,0,0,0};
        data.animalPrice = new string[]
            {
                //うさぎ
                "50.78a",
                //きつね
                "240.6a",
                //くま
                "8.56b",
                //狼
                "461b",
                //りす
                "26.46c",
                //ひつじ
                "1.51b",
                //カモメ
                "87.19d",
                //ハゲタカ
                "5e",
                //しか
                "287e",
                //アライグマ
                "16.49f"
            };
        data.areaBaseMoneyPrice = new float[]
            {
                //うさぎ
                1.2f,
                //きつね
                14.5f,
                //くま
                35.3f,
                //狼
                105.9f,
                //りす
                158.85f,
                //ひつじ
                238.275f,
                //カモメ
                714.825f,
                //ハゲタカ
                1072.23f,
                //しか
                3216.71f,
                //アライグマ
                4825.06f
            };
        data.areaBaseMoneyPer = new float[]{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f};
        data.areaNowMoneyPrice = new float[]{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f};

        data.field1stStatus = new bool[]{false,false,false,false,false,false,false,false,false,false};

        data.fieldUnlockStatus = new bool[]{false,false,false};
    }
}