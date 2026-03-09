using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{
    public int level;   //��ǰ�ؿ����
    private LevelController levelController;
    public static LevelData levelData;   //��ǰ�ؿ�����
    public bool showDialog = true;  // 是否显示剧情对话

    public List<GameObject> awakeList;  //�������б������ڿ�������������Ѹû��ѵĶ���

    public GameObject endMenuPanel;   //��Ϸ�������
    public GameObject background;   //��������
    public GameObject zombieManagement;   //��ʬ��������
    public GameObject uiManagement;   //UI��������

    private void Awake()
    {
        // 暂时硬编码为Level0Controller，因为levelData还未初始化
        levelController = 
            (LevelController)gameObject.AddComponent(System.Type.GetType("Level0Controller, Assembly-CSharp"));
        levelController.init();
        // 现在levelData已初始化，可以获取当前关卡
        int currentLevel = levelData.level;

        //���ر���ͼƬ
        background.GetComponent<SpriteRenderer>().sprite =
            Resources.Load<Sprite>("Sprites/Background/Background" + levelData.mapSuffix);
        //���ñ�������
        background.GetComponent<BGMusicControl>()
            .changeMusic("Music" + levelData.backgroundSuffix);

        //���ض�Ӧ����ֲ�������
        GameObject pm = Instantiate(
            Resources.Load<GameObject>(
                "Prefabs/PlantingManagement/PlantingManagement" + levelData.plantingManagementSuffix),
            new Vector3(0, 0, 0),
            Quaternion.Euler(0, 0, 0)
        );
        pm.name = "Planting Management";

        //����UI
        uiManagement.GetComponent<UIManagement>().initUI();

        // 初始化PlayerManager
        GameObject playerManager = new GameObject("PlayerManager");
        PlayerManager playerManagerComp = playerManager.AddComponent<PlayerManager>();
        playerManagerComp.playerPrefab = Resources.Load<GameObject>("Prefabs/Player/player");
        Debug.Log("Player prefab loaded: " + (playerManagerComp.playerPrefab != null));
        Debug.Log("PlayerManager initialized");
        
        // 根据设置决定是否显示剧情对话
        if (showDialog)
        {
            //���ضԻ����
            Instantiate(Resources.Load<UnityEngine.Object>("Prefabs/UI/DialogPanel/DialogPanel-Level" + currentLevel),
                        new Vector3(0, 0, 0),
                        Quaternion.Euler(0, 0, 0),
                        GameObject.Find("TopCanvas").transform);
        }
        else
        {
            // 直接激活所有游戏元素
            awakeAll();
        }
    }

    public void awakeAll()
    {
        foreach (GameObject gameObject in awakeList)
        {
            gameObject.SetActive(true);
        }
        uiManagement.GetComponent<UIManagement>().appear();
        zombieManagement.GetComponent<ZombieManagement>().activate();
        levelController.activate();
    }

    public void gameOver()
    {
        endMenuPanel.GetComponent<EndMenu>().gameOver();
    }

    public void win()
    {
        endMenuPanel.GetComponent<EndMenu>().win();
    }
}