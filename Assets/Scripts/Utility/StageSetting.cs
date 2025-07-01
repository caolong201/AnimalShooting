using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageData
{
    public int StageId;
    public string StageName;
    public int BulletCount;
    // public int Bear; 1
    // public int Deer; 2
    // public int Boar; 3
    // public int Wolf; 4
    public List<int> Animal;
    public Vector3 PlayerPosition;
    public Vector3 PlayerRoatation;

    public List<int> GenerateAnimalNo;
    public List<Vector3> GenerateAnimalPosition;
    public List<Vector3> GenerateAnimalRoatation;
    public List<int> GenerateAnimalAnimation;

    public List<int> StageStarConditions;
}

[CreateAssetMenu(menuName = "ScriptableObject/Stage Setting", fileName = "StageSetting")]
public class StageSetting : ScriptableObject
{
    public List<StageData> DataList;
}
