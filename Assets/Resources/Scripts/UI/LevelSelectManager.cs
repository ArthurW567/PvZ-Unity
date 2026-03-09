using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform levelButtonContainer;
    public Text titleText;
    public Button backButton;

    private List<LevelData> levelDatas;

    private void Start()
    {
        titleText.text = "选择关卡";
        backButton.onClick.AddListener(BackToMainMenu);
        LoadLevelData();
        CreateLevelButtons();
    }

    private void LoadLevelData()
    {
        levelDatas = new List<LevelData>();
        
        // 加载关卡0
        LevelData level0 = new LevelData();
        level0.level = 0;
        level0.levelName = "关卡 1 - 白天";
        level0.mapSuffix = "_Day";
        level0.rowCount = 5;
        level0.landRowCount = 5;
        level0.isDay = true;
        level0.plantingManagementSuffix = "_Day";
        level0.backgroundSuffix = "_Day";
        level0.zombieInitPosY = new List<float> { 1.8f, 1.0f, 0.2f, -0.6f, -1.4f };
        level0.plantCards = new List<string> { "PeaShooter", "SunFlower", "WallNut" };
        levelDatas.Add(level0);

        // 加载关卡1
        LevelData level1 = new LevelData();
        level1.level = 1;
        level1.levelName = "关卡 2 - 夜晚";
        level1.mapSuffix = "_Night";
        level1.rowCount = 5;
        level1.landRowCount = 5;
        level1.isDay = false;
        level1.plantingManagementSuffix = "_Night";
        level1.backgroundSuffix = "_Night";
        level1.zombieInitPosY = new List<float> { 1.8f, 1.0f, 0.2f, -0.6f, -1.4f };
        level1.plantCards = new List<string> { "PeaShooter", "SunFlower", "WallNut", "SnowKing" };
        levelDatas.Add(level1);

        // 加载关卡2
        LevelData level2 = new LevelData();
        level2.level = 2;
        level2.levelName = "关卡 3 - 森林";
        level2.mapSuffix = "_Forest";
        level2.rowCount = 5;
        level2.landRowCount = 5;
        level2.isDay = true;
        level2.plantingManagementSuffix = "_Forest";
        level2.backgroundSuffix = "_Forest";
        level2.zombieInitPosY = new List<float> { 1.8f, 1.0f, 0.2f, -0.6f, -1.4f };
        level2.plantCards = new List<string> { "PeaShooter", "SunFlower", "WallNut", "SnowKing", "MiaoMiao" };
        levelDatas.Add(level2);

        // 加载关卡3
        LevelData level3 = new LevelData();
        level3.level = 3;
        level3.levelName = "关卡 4 - 冰原";
        level3.mapSuffix = "_Ice";
        level3.rowCount = 5;
        level3.landRowCount = 5;
        level3.isDay = false;
        level3.plantingManagementSuffix = "_Ice";
        level3.backgroundSuffix = "_Ice";
        level3.zombieInitPosY = new List<float> { 1.8f, 1.0f, 0.2f, -0.6f, -1.4f };
        level3.plantCards = new List<string> { "PeaShooter", "SunFlower", "WallNut", "SnowKing", "MiaoMiao", "TorchWood" };
        levelDatas.Add(level3);

        // 加载关卡4
        LevelData level4 = new LevelData();
        level4.level = 4;
        level4.levelName = "关卡 5 - 寒冬";
        level4.mapSuffix = "_Winter";
        level4.rowCount = 5;
        level4.landRowCount = 5;
        level4.isDay = false;
        level4.plantingManagementSuffix = "_Winter";
        level4.backgroundSuffix = "_Winter";
        level4.zombieInitPosY = new List<float> { 1.8f, 1.0f, 0.2f, -0.6f, -1.4f };
        level4.plantCards = new List<string> { "PeaShooter", "SunFlower", "WallNut", "SnowKing", "MiaoMiao", "TorchWood", "Squash" };
        levelDatas.Add(level4);
    }

    private void CreateLevelButtons()
    {
        foreach (LevelData levelData in levelDatas)
        {
            GameObject buttonObj = Instantiate(levelButtonPrefab, levelButtonContainer);
            buttonObj.transform.localScale = Vector3.one;
            
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            buttonText.text = levelData.levelName;
            
            Button button = buttonObj.GetComponent<Button>();
            int level = levelData.level;
            button.onClick.AddListener(() => SelectLevel(level));
        }
    }

    private void SelectLevel(int level)
    {
        // 存储选择的关卡数据
        GameManagement.levelData = levelDatas.Find(data => data.level == level);
        
        // 加载游戏场景
        SceneManager.LoadScene("GameScene");
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
