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
    
    // 植物格子位置数组（基于实际预制体中的坐标）
    private Vector2[] plantGridPositions = new Vector2[]
    {
        // 第0行（最下面一行）
        new Vector2(-2.5f, -2.2f),   // Plant-0-0
        new Vector2(-1.67f, -2.2f),  // Plant-1-0
        new Vector2(-0.82f, -2.2f),  // Plant-2-0
        new Vector2(0.03f, -2.2f),   // Plant-3-0
        new Vector2(0.88f, -2.2f),   // Plant-4-0
        new Vector2(1.73f, -2.2f),   // Plant-5-0
        new Vector2(2.59f, -2.2f),   // Plant-6-0
        new Vector2(3.44f, -2.2f),   // Plant-7-0
        new Vector2(4.3f, -2.2f),    // Plant-8-0
        
        // 第1行
        new Vector2(-2.5f, -1.25f),  // Plant-0-1
        new Vector2(-1.67f, -1.25f), // Plant-1-1
        new Vector2(-0.82f, -1.25f), // Plant-2-1
        new Vector2(0.03f, -1.25f),  // Plant-3-1
        new Vector2(0.88f, -1.25f),  // Plant-4-1
        new Vector2(1.73f, -1.25f),  // Plant-5-1
        new Vector2(2.59f, -1.25f),  // Plant-6-1
        new Vector2(3.44f, -1.25f),  // Plant-7-1
        new Vector2(4.3f, -1.25f),   // Plant-8-1
        
        // 第2行
        new Vector2(-2.5f, -0.3f),   // Plant-0-2
        new Vector2(-1.67f, -0.3f),  // Plant-1-2
        new Vector2(-0.82f, -0.3f),  // Plant-2-2
        new Vector2(0.03f, -0.3f),   // Plant-3-2
        new Vector2(0.88f, -0.3f),   // Plant-4-2
        new Vector2(1.73f, -0.3f),   // Plant-5-2
        new Vector2(2.59f, -0.3f),   // Plant-6-2
        new Vector2(3.44f, -0.3f),   // Plant-7-2
        new Vector2(4.3f, -0.3f),    // Plant-8-2
        
        // 第3行
        new Vector2(-2.5f, 0.85f),   // Plant-0-3
        new Vector2(-1.67f, 0.85f),  // Plant-1-3
        new Vector2(-0.82f, 0.85f),  // Plant-2-3
        new Vector2(0.03f, 0.85f),   // Plant-3-3
        new Vector2(0.88f, 0.85f),   // Plant-4-3
        new Vector2(1.76f, 0.85f),   // Plant-5-3
        new Vector2(2.59f, 0.85f),   // Plant-6-3
        new Vector2(3.44f, 0.85f),   // Plant-7-3
        new Vector2(4.3f, 0.85f),    // Plant-8-3
        
        // 第4行（最上面一行）
        new Vector2(-2.5f, 1.85f),   // Plant-0-4
        new Vector2(-1.67f, 1.85f),  // Plant-1-4
        new Vector2(-0.82f, 1.85f),  // Plant-2-4
        new Vector2(0.03f, 1.85f),   // Plant-3-4
        new Vector2(0.88f, 1.85f),   // Plant-4-4
        new Vector2(1.73f, 1.85f),   // Plant-5-4
        new Vector2(2.59f, 1.85f),   // Plant-6-4
        new Vector2(3.44f, 1.85f),   // Plant-7-4
        new Vector2(4.3f, 1.85f)      // Plant-8-4
    };
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // 初始化目标位置为最近的植物格子
        targetPosition = FindNearestGridPosition(transform.position);
        transform.position = targetPosition;
        Debug.Log("Player Start() called");
        Debug.Log("Initial position: " + transform.position);
    }
    
    void Update()
    {
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