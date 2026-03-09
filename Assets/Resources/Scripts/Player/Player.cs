using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackRange = 2f;
    public float attackDamage = 50f;
    public float attackCooldown = 1f;
    
    private float lastAttackTime = 0f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 targetPosition;
    private bool isMoving = false;
    
    // 存储场景中所有植物格子的位置
    private List<Vector2> plantGridPositions = new List<Vector2>();
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        // 动态获取场景中所有植物格子的位置
        UpdatePlantGridPositions();
        
        // 初始化目标位置为最近的植物格子
        if (plantGridPositions.Count > 0)
        {
            targetPosition = FindNearestGridPosition(transform.position);
            transform.position = targetPosition;
            Debug.Log("Player Start() called");
            Debug.Log("Initial position: " + transform.position);
            Debug.Log("Found " + plantGridPositions.Count + " plant grids");
        }
        else
        {
            Debug.LogWarning("No plant grids found in the scene");
        }
    }
    
    // 更新植物格子位置列表
    private void UpdatePlantGridPositions()
    {
        plantGridPositions.Clear();
        
        // 查找场景中所有带有PlantGrid组件的游戏对象
        PlantGrid[] plantGrids = FindObjectsOfType<PlantGrid>();
        
        foreach (PlantGrid grid in plantGrids)
        {
            // 添加格子的世界坐标到列表
            plantGridPositions.Add(grid.transform.position);
        }
    }
    
    void Update()
    {
        // 定期更新植物格子位置列表，以适应网格的变化
        UpdatePlantGridPositions();
        
        // 处理移动输入
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveToNearestGrid(new Vector2(0, 1));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                MoveToNearestGrid(new Vector2(0, -1));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                MoveToNearestGrid(new Vector2(-1, 0));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MoveToNearestGrid(new Vector2(1, 0));
            }
        }
        
        // 移动到目标位置
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            // 检查是否到达目标位置
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;
                isMoving = false;
                Debug.Log("Reached target position: " + targetPosition);
            }
        }
        
        // 自动攻击
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }
    
    void MoveToNearestGrid(Vector2 direction)
    {
        // 找到当前位置所在的植物格子
        Vector2 currentGrid = FindNearestGridPosition(transform.position);
        
        // 计算目标位置（当前格子加上方向）
        Vector2 targetGrid = currentGrid + direction * 1.0f; // 1.0是格子之间的水平和垂直距离
        
        // 找到离目标位置最近的植物格子
        Vector2 nearestGrid = FindNearestGridPosition(targetGrid);
        
        // 检查目标位置是否与当前位置不同
        if (nearestGrid != currentGrid)
        {
            // 检查是否是相邻的格子（距离应该接近1.0）
            float distance = Vector2.Distance(currentGrid, nearestGrid);
            if (distance < 1.5f) // 允许一定的误差
            {
                targetPosition = nearestGrid;
                isMoving = true;
                Debug.Log("Moving to grid position: " + nearestGrid);
            }
            else
            {
                Debug.Log("No valid grid position in that direction");
            }
        }
        else
        {
            Debug.Log("No valid grid position in that direction");
        }
    }
    
    Vector2 FindNearestGridPosition(Vector2 position)
    {
        if (plantGridPositions.Count == 0)
        {
            return position;
        }
        
        Vector2 nearest = plantGridPositions[0];
        float minDistance = Vector2.Distance(position, nearest);
        
        foreach (Vector2 gridPos in plantGridPositions)
        {
            float distance = Vector2.Distance(position, gridPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = gridPos;
            }
        }
        
        return nearest;
    }
    
    void Attack()
    {
        // 检测范围内的敌人
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Zombie"));
        
        if (hitEnemies.Length > 0)
        {
            // 播放攻击动画
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
            
            // 对范围内的敌人造成伤害
            foreach (Collider2D enemy in hitEnemies)
            {
                Zombie zombie = enemy.GetComponent<Zombie>();
                if (zombie != null)
                {
                    zombie.beAttacked((int)attackDamage);
                }
            }
            
            lastAttackTime = Time.time;
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // 绘制攻击范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}