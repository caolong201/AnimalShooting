[System.Serializable]
public class SaveData {
    //-----------
    //const
    //-----------
    //チュートリアルステータス
    //動物仲間追加
    public bool tutorialStepFirst = false;
    //初めてShowmoreクリック （showmoreをクリックしたことあれば無視）
    public bool tutorialStepSecond = false;
    //初めてエリアを強化
    public bool tutorialStepThird = false;
    //完了
    public bool tutorialStatus = false;
    //所持マネー
    public float money = 0;
    //所持ハート
    public float heart = 0;
    //所持エナジー
    public int energy = 0;
    //動物の数
    public int animal = 0;
    //Autoマネー
    public float autoMoney = 0;
    //Area
    public int[] areaMoneyLevel = new int[]{1,1,1,1,1,1,1,1,1,1};
    public int[] areaHabitatLevel = new int[]{1,1,1,1,1,1,1,1,1,1};
    public int areaRewordLevel = 0;
    public string[] areaMoneyPrice = new string[]
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
    public string[] areaHabitatPrice = new string[]
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
    public int[] animalCount = new int[]{0,0,0,0,0,0,0,0,0,0};
    public string[] animalPrice = new string[]
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
    public float[] areaBaseMoneyPrice = new float[]
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
    public float[] areaBaseMoneyPer = new float[]{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f};
    public float[] areaNowMoneyPrice = new float[]{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f};
    
    //1stフィールドクリア状況
    public bool[] field1stStatus = new bool[]{false,false,false,false,false,false,false,false,false,false};
    //フィールドアンロック状況
    public bool[] fieldUnlockStatus = new bool[]{false,false,false};

}