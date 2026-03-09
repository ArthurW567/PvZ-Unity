using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject player;
    
    void Start()
    {
        Debug.Log("PlayerManager Start() called");
        Debug.Log("Player prefab: " + playerPrefab);
        
        if (playerPrefab != null)
        {
            // 在游戏开始时生成Player实例，放在中间位置
            player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Debug.Log("Player instantiated: " + player);
            Debug.Log("Player position: " + player.transform.position);
        }
        else
        {
            Debug.LogError("Player prefab is null!");
        }
    }
    
    public GameObject GetPlayer()
    {
        return player;
    }
}