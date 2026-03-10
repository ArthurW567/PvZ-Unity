using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;
    public Button level5Button;

    private List<LevelData> levelDatas;

    private void Start()
    {
        // 绑定按钮点击事件
        if (level1Button != null) level1Button.onClick.AddListener(() => SelectLevel(0));
        if (level2Button != null) level2Button.onClick.AddListener(() => SelectLevel(1));
        if (level3Button != null) level3Button.onClick.AddListener(() => SelectLevel(2));
        if (level4Button != null) level4Button.onClick.AddListener(() => SelectLevel(3));
        if (level5Button != null) level5Button.onClick.AddListener(() => SelectLevel(4));
        
        LoadLevelData();
    }

    private void LoadLevelData()
    {
        levelDatas = new List<LevelData>();
        
        // 创建5个关卡数据
        for (int i = 0; i < 5; i++)
        {
            LevelData levelData = new LevelData();
            levelData.level = i;
            levelData.levelName = "关卡 " + (i + 1);
            levelData.mapSuffix = "_Day";
            levelData.rowCount = 5;
            levelData.landRowCount = 5;
            levelData.isDay = true;
            levelData.plantingManagementSuffix = "_Day";
            levelData.backgroundSuffix = "_Day";
            levelData.zombieInitPosY = new List<float> { 1.8f, 1.0f, 0.2f, -0.6f, -1.4f };
            levelData.plantCards = new List<string> { "PeaShooter", "SunFlower", "WallNut" };
            levelDatas.Add(levelData);
        }
    }

    public void SelectLevel(int level)
    {
        // 存储选择的关卡数据
        GameManagement.levelData = levelDatas[level];
        
        // 加载游戏场景
        SceneManager.LoadScene("GameScene");
    }
}
