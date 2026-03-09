# 关卡选择界面设置指南

## 场景设置

1. **创建场景**
   - 在Unity编辑器中，创建三个场景：
     - `MainMenu` - 主菜单场景
     - `LevelSelect` - 关卡选择场景
     - `GameScene` - 游戏场景（已存在）

2. **设置MainMenu场景**
   - 创建一个Canvas对象
   - 在Canvas下创建一个Text对象，设置为游戏标题
   - 创建两个Button对象：
     - "开始游戏"按钮
     - "退出游戏"按钮
   - 创建一个空GameObject，添加`MainMenuManager`脚本
   - 将按钮拖拽到脚本的对应字段中

3. **设置LevelSelect场景**
   - 创建一个Canvas对象
   - 在Canvas下创建一个Text对象，设置为"选择关卡"
   - 创建一个Vertical Layout Group对象作为关卡按钮的容器
   - 创建一个Button预制体作为关卡按钮模板
   - 创建一个"返回"按钮
   - 创建一个空GameObject，添加`LevelSelectManager`脚本
   - 将对应对象拖拽到脚本的对应字段中

4. **设置GameScene场景**
   - 确保GameManagement对象的levelData字段在场景加载时能够正确接收关卡数据

5. **在Build Settings中添加场景**
   - 打开Build Settings
   - 点击"Add Open Scenes"按钮，添加所有三个场景
   - 确保场景顺序为：MainMenu、LevelSelect、GameScene

## 脚本说明

- `MainMenuManager.cs` - 管理主菜单的逻辑，点击"开始游戏"按钮会跳转到关卡选择界面
- `LevelSelectManager.cs` - 管理关卡选择界面的逻辑，显示所有可用关卡，并处理关卡选择
- `GameManagement.cs` - 游戏管理脚本，已修改为使用从关卡选择界面传递的关卡数据

## 关卡数据

关卡选择界面会加载以下关卡数据：

1. **关卡 1 - 白天**
   - 植物：豌豆射手、向日葵、坚果墙

2. **关卡 2 - 夜晚**
   - 植物：豌豆射手、向日葵、坚果墙、冰雪国王

3. **关卡 3 - 森林**
   - 植物：豌豆射手、向日葵、坚果墙、冰雪国王、喵喵

4. **关卡 4 - 冰原**
   - 植物：豌豆射手、向日葵、坚果墙、冰雪国王、喵喵、火炬树桩

5. **关卡 5 - 寒冬**
   - 植物：豌豆射手、向日葵、坚果墙、冰雪国王、喵喵、火炬树桩、窝瓜

## 注意事项

- 确保所有必要的资源（如背景图片、音效等）都已正确导入
- 确保所有脚本都已正确添加到对应对象上
- 确保场景名称与脚本中使用的名称一致
