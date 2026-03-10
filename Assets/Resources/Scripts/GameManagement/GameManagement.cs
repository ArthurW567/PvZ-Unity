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
        // 检查levelData是否为null，如果为null则使用默认值
        if (levelData == null)
        {
            Debug.LogWarning("levelData is null, using default level 0");
            levelData = new LevelData();
            levelData.level = 0;
            levelData.mapSuffix = "_Day";
            levelData.backgroundSuffix = "_Day";
            levelData.plantingManagementSuffix = "_Day";
        }
        
        // 使用levelData中的关卡数据
        int currentLevel = levelData.level;
        
        // 创建对应关卡的LevelController
        levelController = 
            (LevelController)gameObject.AddComponent(System.Type.GetType("Level" + currentLevel + "Controller, Assembly-CSharp"));
        levelController.init();

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
        playerManagerComp.playerPrefab = Resources.Load<GameObject>("Prefabs/Player/Player");
        Debug.Log("Player prefab loaded: " + (playerManagerComp.playerPrefab != null));
        if (playerManagerComp.playerPrefab == null)
        {
            // 尝试加载Friend文件夹中的player预制体
            playerManagerComp.playerPrefab = Resources.Load<GameObject>("Prefabs/Friend/player");
            Debug.Log("Player prefab loaded from Friend folder: " + (playerManagerComp.playerPrefab != null));
        }
        // 确保玩家预制件的标签和层级正确
        if (playerManagerComp.playerPrefab != null)
        {
            playerManagerComp.playerPrefab.tag = "Player";
            playerManagerComp.playerPrefab.layer = 11; // 设置为Player层
            Debug.Log("Player prefab tag: " + playerManagerComp.playerPrefab.tag + ", layer: " + playerManagerComp.playerPrefab.layer);
        }
        Debug.Log("PlayerManager initialized");
        
        // 等待Player生成后设置其血条引用
        StartCoroutine(SetPlayerHealthBarReference(playerManagerComp));
        
        IEnumerator SetPlayerHealthBarReference(PlayerManager pm) {
            yield return new WaitForSeconds(0.1f); // 等待一帧让Player生成
            GameObject player = pm.GetPlayer();
            if (player != null) {
                Player playerScript = player.GetComponent<Player>();
                if (playerScript != null) {
                    // 查找 MiddleCanvas 中的"PlayerHealthBar"血条UI对象（递归查找所有子层级）
                    GameObject middleCanvas = GameObject.Find("MiddleCanvas");
                    GameObject healthBarObj = null;
                    
                    if (middleCanvas != null) {
                        healthBarObj = FindGameObjectInChildren(middleCanvas, "PlayerHealthBar");
                    }
                    
                    // 如果在 MiddleCanvas 中找不到，尝试在场景中直接查找
                    if (healthBarObj == null) {
                        healthBarObj = GameObject.Find("PlayerHealthBar");
                    }
                    
                    if (healthBarObj != null) {
                        playerScript.healthBarObject = healthBarObj;
                        Debug.Log("Health bar object assigned to player");
                    } else {
                        Debug.LogWarning("No PlayerHealthBar object found in MiddleCanvas or scene");
                    }
                }
            }
        }
        
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
    
    // 递归查找子对象
    private GameObject FindGameObjectInChildren(GameObject parent, string name)
    {
        if (parent.name == name)
        {
            return parent;
        }
        
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject child = FindGameObjectInChildren(parent.transform.GetChild(i).gameObject, name);
            if (child != null)
            {
                return child;
            }
        }
        
        return null;
    }
}