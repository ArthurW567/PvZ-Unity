# Player 设置说明

## 功能概述
- 近战主角，使用双手大剑进行攻击
- WASD自由移动，不受网格限制
- 自动攻击进入范围的僵尸
- 操作与排兵布阵分离

## 预制体设置
1. 在Unity编辑器中打开 `Assets/Resources/Prefabs/Player/Player.prefab`
2. 确保以下组件存在：
   - Rigidbody2D：重力缩放设为0
   - BoxCollider2D：大小设为(1,1)
   - SpriteRenderer：添加一个主角精灵
   - Player.cs：脚本组件

3. 标签设置：
   - 将Player预制体的标签设为"Player"

4. 碰撞设置：
   - 在Project Settings -> Physics2D中，设置碰撞矩阵，确保Player与植物、僵尸之间没有碰撞

## 游戏管理
- PlayerManager会在游戏开始时自动生成Player实例
- Player会在(5, 0)位置生成

## 控制
- 移动：WASD键
- 攻击：自动攻击进入范围的僵尸
- 造兵：使用鼠标，与Player操作分离

## 参数调整
在Player.cs脚本中可以调整以下参数：
- moveSpeed：移动速度
- attackRange：攻击范围
- attackDamage：攻击伤害
- attackCooldown：攻击冷却时间