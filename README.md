
# 生成器系统

* **插件未开源**

## 作者信息
Copyright FirePlume, All Rights Reserved. Email: fireplume@126.com
 
作者网址：
[Bilibili](https://space.bilibili.com/395084718)、
[YouTube](https://www.youtube.com/@FirePlume126)、
[GitHub](https://www.github.com/FirePlume126)

**[返回目录](https://www.github.com/FirePlume126/FP_Readme#Directory)**

<a name="fpspawnersystem"></a>
## FPSpawnerSystem

生成器系统

---

**生成器目录**

- [编辑器模块](#fpspawnersystem_fpspawnersystemeditor)：此插件的编辑器模块
	- [此模块的主要类](#fpspawnersystemeditor_mainclass)
	- [实体管理器](#fpspawnersystemeditor_entitymanager)：在指定区域计算并生成实体数据
		- [刷新散布数据](#fpspawnersystemeditor_refreshscatterdata)：移除旧散布数据并生成新散布数据
		- [清除散布数据](#fpspawnersystemeditor_clearscatterdata)：清除散布数据，强行停止散布
		- [应用调试数据](#fpspawnersystemeditor_applydebugdata)：应用自定义的数据
		- [应用散布数据](#fpspawnersystemeditor_applyscatterdata)：生成实体并清空散布数据，应用后实体将不受此插件的[运行时模块](#fpspawnersystem_fpspawnersystem)管理
		- [管理器设置](#fpspawnersystemeditor_managersettings)：针对运行时状态的设置，用来控制实体管理器激活
	- [散布策略](#fpspawnersystemeditor_scatterstrategy)：在派生类实现生成实体变换的逻辑
		- [网格散布](#fpspawnersystemeditor_gridscatter)：根据实体数量将散布区域等分为多个网格区域，并在每个网格区域内随机偏移生成实体数据
		- [随机散布](#fpspawnersystemeditor_randomscatter)：在场景随机生成实体数据

* [运行时模块](#fpspawnersystem_fpspawnersystem)：此插件的运行时模块
	- [此模块的主要类](#fpspawnersystem_mainclass)
	- [实体数据管理器](#fpspawnersyste_entitydatamanager)：管理编辑器状态下[实体管理器](#fpspawnersystemeditor_entitymanager)生成的实体数据
	- [生成器管理子系统](#)：生成实体并对其进行管理
		- [分区网格管理器](#)：管理世界网格划分，根据激活源位置在异步线程计算需要加载/卸载的网格和实体
			- [分区网格计算](#)：在异步线程计算需要加载/卸载的网格单元
		- [激活源](#) 激活源会作为运行时动态激活实体的锚点
		- [生成器保存数据](#) 生成器保存数据
	- [实体数据](#) 实体数据
		- [实体接口](#)：通过此接口获取实体数据，添加给生成器生成的实体Actor
		- [实体属性集](#)：
		- [LOD](#)：
- [项目设置](#fpspawnersyste-projectsettings)：此插件的项目设置
* [函数库](#fpspawnersyste-functionlibrary)：此插件的函数库

---

<a name="fpspawnersystem_fpspawnersystemeditor"></a>
### FPSpawnerSystemEditor

生成器系统编辑器，在指定区域计算并生成实体数据

<a name="fpspawnersystemeditor_mainclass"></a>
#### 此模块的主要类

|类名|描述|
|:-:|:-:|
|FPSpawnerEntityManagerEdActor|[实体管理器](#fpspawnersystemeditor_entitymanager)，在指定区域计算并生成实体数据，此Actor仅在编辑器有效，放在场景大纲中使用|
|FPSpawnerScatterBase|[散布策略](#fpspawnersystemeditor_scatterstrategy)基类，在派生类实现生成实体变换的逻辑|
|FPSpawnerScatter_Grid|[网格散布](#fpspawnersystemeditor_gridscatter)，根据实体数量将散布区域等分为多个网格区域，并在每个网格区域内随机偏移生成实体数据|
|FPSpawnerScatter_Random|[随机散布](#fpspawnersystemeditor_randomscatter)，在场景随机生成实体数据|
|FPSpawnerDebugComponent|生成器调试组件，管理调试网格，选中网格时临时生成目标实体，实现[应用调试数据](#fpspawnersystemeditor_applydebugdata)的逻辑|
|FPSpawnerBlockingVolume|生成器阻挡体，禁止在此区域生成实体，此Actor仅在编辑器有效，放在场景大纲中使用|	
|FPSpawnerActivationSourceEdActor|激活源编辑器Actor，编辑器用来测试激活实体的锚点，此Actor仅在编辑器有效，放在场景大纲中使用|

<a name="fpspawnersystemeditor_entitymanager"></a>
#### 实体管理器

实体管理器，在指定区域计算并生成实体数据，生成的实体数据保存在[实体数据管理器](#fpspawnersyste_entitydatamanager)，此Actor仅在编辑器有效，放在场景大纲中使用。

![FPSpawnerSystem_EntityManager](https://github.com/FirePlume126/FP_SpawnerSystem/blob/main/Images/FPSpawnerSystem_EntityManager.png)

<a name="fpspawnersystemeditor_refreshscatterdata"></a>
* **刷新散布数据**

移除旧散布数据并生成新散布数据。在**刷新散布数据**前，需先添加[散布策略](#fpspawnersystemeditor_scatterstrategy)算法，设置??实体数据及生成数量，并调整盒体组件范围，确保在指定区域内生成新的实体数据。

![FPSpawnerSystem_RefreshScatterData](https://github.com/FirePlume126/FP_SpawnerSystem/blob/main/Images/FPSpawnerSystem_RefreshScatterData.png)

<a name="fpspawnersystemeditor_clearscatterdata"></a>
* **清除散布数据**

清除散布数据，强行停止散布

<a name="fpspawnersystemeditor_applydebugdata"></a>
* **应用调试数据**

应用自定义的数据，[刷新散布数据](#fpspawnersystemeditor_refreshscatterdata)后，让`bDebugScatterData = true`且`bCustomScatterData = true`时，可以通过选中**调试网格**(图片中的箭头，选中时会变成目标实体)自定义实体的变换。

![FPSpawnerSystem_ApplyDebugData](https://github.com/FirePlume126/FP_SpawnerSystem/blob/main/Images/FPSpawnerSystem_ApplyDebugData.png)

自定义实体的变换后，**应用调试数据**的按钮将变为可编辑状态，应用后就可以使用自定义实体的变换。

![FPSpawnerSystem_ApplyDebugData_1](https://github.com/FirePlume126/FP_SpawnerSystem/blob/main/Images/FPSpawnerSystem_ApplyDebugData_1.png)

<a name="fpspawnersystemeditor_applyscatterdata"></a>
* **应用散布数据**

生成实体并清空散布数据，应用后实体将不受此插件的[运行时模块](#fpspawnersystem_fpspawnersystem)管理。
在网络游戏环境中，尽量避免通过[运行时模块](#fpspawnersystem_fpspawnersystem)管理大量网格体实体，以防止因频繁同步导致网络流量过大。<br>
如果??实体数据是`Actor`，会直接生成Actor。<br>
如果??实体数据是`Mesh`，参考**世界分区**，在`Mesh`所在的网格单元中生成一个`FPSpawnerScatterMeshAttachActor`，用于统一管理该单元内由插件生成的所有网格实体，网格单元的大小可通过[项目设置](#fpspawnersyste-projectsettings)中的InstancedMeshGridSize设置。

![FPSpawnerSystem_ApplyScatterData](https://github.com/FirePlume126/FP_SpawnerSystem/blob/main/Images/FPSpawnerSystem_ApplyScatterData.png)

<a name="fpspawnersystemeditor_managersettings"></a>
* **管理器设置**

针对运行时状态的设置，用来控制实体管理器激活

```c++
// 实体管理器设置
USTRUCT(BlueprintType)
struct FFPSpawnerEntityManagerSettings
{
	GENERATED_BODY()

public:

	// 启动游戏时自动激活，bAutoActivate为false且AreaName为空时，无法使用此散布区域实体数据，所以AreaName为空时，bAutoActivate只能为true
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	bool bAutoActivate = true;

	// 管理器名称，可以通过此名称手动控制管理器
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	FName ManagerName = NAME_None;
};
```

1、`bAutoActivate = true`时，启动游戏时自动激活此实体管理器管理的实体。当实体激活时，实体附近存在??激活源时生成实体

2、`ManagerName`不为空时，可以通过[函数库](#fpspawnersyste-functionlibrary)的函数TryModifyEntityManagerState()手动控制管理器

```c++
// 实体管理器请求类型
UENUM(BlueprintType)
enum class EFPSpawnerEntityManagerRequestType : uint8
{
	// 停用实体管理器
	None,

	// 激活实体管理器
	Activate,

	// 重置实体管理器，实体状态也会被重置
	Reset,

	// 移除实体管理器
	Remove,
};

// 尝试修改实体管理器状态
// @param InManagerName 实体管理器名称
// @param InRequestType 实体管理器请求类型
UFUNCTION(BlueprintCallable, BlueprintAuthorityOnly, meta = (WorldContext = "WorldContextObject"), Category = "FPSpawner")
static bool TryModifyEntityManagerState(const UObject* WorldContextObject, const FName& InManagerName, const EFPSpawnerEntityManagerRequestType InRequestType);
```

<a name="fpspawnersystemeditor_scatterstrategy"></a>
#### 散布策略

散布策略是生成器系统编辑器模块的核心逻辑部分，用于定义实体在场景中如何分布。
开发者通过继承该基类并在派生类中实现具体的散布行为，可以灵活地扩展不同类型的散布策略。<br>
每帧最大散布对象数可以通过[项目设置](#fpspawnersyste-projectsettings)中的MaxScatterObjectsPerTick设置，用于控制性能消耗，防止一次性生成大量对象导致卡顿或掉帧。

以下为派生类需要使用的成员变量和方法：
```c++
// 散布模式
UPROPERTY(EditAnywhere)
FFPSpawnerScatterMode ScatterMode = FFPSpawnerScatterMode::Ground;

// 跟踪参数，在地面生成时检测有效表面；在空中生成时检测无效表面
UPROPERTY(EditAnywhere)
FFPSpawnerTraceParams TraceParams;

// 区域数据
FFPSpawnerAreaData AreaData;

// 实体数据列表
TArray<FFPSpawnerScatterEntityData> EntityDataList;

// 计算散布数据，派生类需重写此方法以实现具体散布逻辑
virtual void ComputeScatterData() {};

// 完成散布策略，计算完成后调用。派生类可选择性重写以清理资源
virtual void CompleteScatterStrategy();

// 更新散布实体数据，将计算结果提交到实体数据管理器
void UpdateScatterEntityData(const FFPSpawnerScatterData& NewScatterData);
```

<a name="fpspawnersystemeditor_traceparameters"></a>
跟踪参数(TraceParameters)

用于判断是否可以生成实体。在地面生成时检测有效表面；在空中生成时检测无效表面

![FPSpawnerSystem_TraceParameters](https://github.com/FirePlume126/FP_SpawnerSystem/blob/main/Images/FPSpawnerSystem_TraceParameters.png)

|属性名称|类型|描述|
|:-:|:-:|:-:|
|CollisionChannels|`TArray<TEnumAsByte<ECollisionChannel>>`|碰撞检测通道列表，用于判断地面或障碍物|
|bTraceComplex|`bool`|是否使用复杂碰撞进行检测|
|IgnoredActors|`TArray<TObjectPtr<AActor>>`|在追踪时忽略的Actor列表|
|PhysMaterials|`TArray<TObjectPtr<UPhysicalMaterial>>`|允许生成的物理材质类型|
|MaxSlopeAngle|`float`|最大允许斜坡角度，超过该角度的表面将被视为无效|
|NavFilterClass|`TSubclassOf<UNavigationQueryFilter>`|导航查询过滤器类，可以仅在导航区域生成|

以下为相关枚举结构体定义：
```c++
// 散布模式
UENUM(BlueprintType)
enum class FFPSpawnerScatterMode : uint8
{
	// 在地面生成实体
	Ground,

	// 在地面生成实体，旋转对齐法线方向
	GroundAligned,

	// 在空中生成实体
	Air,
};

// 生成区域数据
USTRUCT(BlueprintType)
struct FFPSpawnerAreaData
{
	GENERATED_BODY()

public:

	// 区域中心
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	FVector Origin = FVector::ZeroVector;

	// 盒子的范围(半尺寸)
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	FVector Extent = FVector::ZeroVector;

	// 区域旋转
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	FRotator Rotation = FRotator::ZeroRotator;
};

// 散布实体数据
USTRUCT(BlueprintType)
struct FFPSpawnerScatterEntityData
{
	GENERATED_BODY()

public:

	// 实体数量
	UPROPERTY(EditAnywhere, BlueprintReadWrite, meta = (ClampMin = 1))
	int32 EntityNumber = 10;

	// 实体数据
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	TSoftObjectPtr<UFPSpawnerEntityData> EntityData = nullptr;
};

// 生成器散布数据
USTRUCT()
struct FFPSpawnerScatterData
{
	GENERATED_BODY()

public:

	// 变换
	UPROPERTY()
	FTransform Transform = FTransform::Identity;

	// 实体数据
	UPROPERTY()
	TSoftObjectPtr<UFPSpawnerEntityData> EntityData = nullptr;
};
```

<a name="fpspawnersystemeditor_gridscatter"></a>
* **网格散布**

根据实体数量将散布区域等分为多个网格区域，并在每个网格区域内随机偏移生成实体数据。

![FPSpawnerSystem_GridScatter](https://github.com/FirePlume126/FP_SpawnerSystem/blob/main/Images/FPSpawnerSystem_GridScatter.png)

|属性名称|类型|描述|
|:-:|:-:|:-:|
|RandomSeed|`int32`|随机种子，用于控制偏移的随机性。相同种子可复现相同的分布结果|
|MaxGridOffsetRatio|`float`|实体在网格区域内最大偏移比例，值越小实体分布越整齐|
|ScatterMode|`FFPSpawnerScatterMode`|散布模式，控制实体生成时的空间定位策略|
|HeightOffsetRange|`FVector2D`|对高度(Z轴)偏移范围，会受到缩放影响。ScatterMode为GroundAligned时，相对地面法线方向偏移范围|
|RotationYawRange|`FVector2D`|实体绕Yaw(Z轴)的随机旋转角度范围|
|RotationPitchRange|`FVector2D`|实体绕Pitch(Y轴)的随机旋转角度范围|
|RotationRollRange|`FVector2D`|实体绕Roll(X轴)的随机旋转角度范围|
|ScaleRange|`FVector2D`|实体缩放范围|
|TraceParams|[跟踪参数](#fpspawnersystemeditor_traceparameters)|用于判断是否可以生成实体。在地面生成时检测有效表面；在空中生成时检测无效表面|

<a name="fpspawnersystemeditor_randomscatter"></a>
* **随机散布**

在场景随机生成实体数据。

![FPSpawnerSystem_RandomScatter](https://github.com/FirePlume126/FP_SpawnerSystem/blob/main/Images/FPSpawnerSystem_RandomScatter.png)

|属性名称|类型|描述|
|:-:|:-:|:-:|
|RandomSeed|`int32`|随机种子，用于控制偏移的随机性。相同种子可复现相同的分布结果|
|ScatterMode|`FFPSpawnerScatterMode`|散布模式，控制实体生成时的空间定位策略|
|BoxExtent|`FVector`|每个实体的盒体范围(半尺寸)，会受到缩放影响|
|RotationYawRange|`FVector2D`|实体绕Yaw(Z轴)的随机旋转角度范围|
|RotationPitchRange|`FVector2D`|实体绕Pitch(Y轴)的随机旋转角度范围|
|RotationRollRange|`FVector2D`|实体绕Roll(X轴)的随机旋转角度范围|
|ScaleRange|`FVector2D`|实体缩放范围|
|TraceParams|[跟踪参数](#fpspawnersystemeditor_traceparameters)|用于判断是否可以生成实体。在地面生成时检测有效表面；在空中生成时检测无效表面|

<a name="fpspawnersystem_fpspawnersystem"></a>
### FPSpawnerSystem

生成器系统

<a name="fpspawnersystem_mainclass"></a>
#### 此模块的主要类

|类名|描述|
|:-:|:-:|
|FPSpawnerEntityDataManager|[实体数据管理器](#fpspawnersyste_entitydatamanager)，管理编辑器状态下[实体管理器](#fpspawnersystemeditor_entitymanager)生成的实体数据|
|FPSpawnerManagerSubsystem|生成器管理子系统，仅在存在于服务器，生成实体并对其进行管理|
|FPSpawnerPartitionGridManager|分区网格管理器，管理世界网格划分，根据激活源位置在异步线程计算需要加载/卸载的网格和实体|
|FPSpawnerPartitionGridCalculation|分区网格计算，在异步线程计算需要加载/卸载的网格单元|
|FPSpawnerRuntimeEntity|运行时实体，管理运行时的实体数据|
|FPSpawnerRuntimeEntityManager|运行时实体管理器，管理FFPSpawnerRuntimeEntity，其他地方调用实体使用弱指针TWeakPtr|
|FPSpawnerRuntimeCell|运行时单元格，管理单元格区域内可被激活的实体|
|FPSpawnerActivationSourceInterface|激活源，实现此接口的对象会作为运行时动态激活实体的锚点，支持添加给AActor或UActorComponent|
|FPSpawnerActivationSourceComponent|激活源组件，添加此组件的Actor会作为动态激活实体的锚点；添加给APlayerController时，使用Pawn或摄像机的位置|
|FPSpawnerEntityData|生成器实体数据|
|FPSpawnerEntityAttributeSet|生成器实体属性集|
|FPSpawnerMeshEntityManager|生成器网格体实体管理器，管理生成器的所有网格体|
|FPSpawnerStaticMeshComponent|生成器静态网格体组件|
|FPSpawnerScatterMeshAttachActor|[应用散布数据](#fpspawnersystemeditor_applyscatterdata)时，散布网格实体附着的Actor|
|FPSpawnerEntityComponent|生成器生成的实体自动添加此组件(不可手动添加)，服务器监听卸载、管理属性集，客户端处理LOD|
|FPSpawnerEntityInterface|生成器实体接口，通过此接口获取实体数据，添加给生成器生成的实体Actor|

<a name="fpspawnersyste_entitydatamanager"></a>
#### 实体数据管理器

实体数据管理器，管理编辑器状态下[实体管理器](#fpspawnersystemeditor_entitymanager)生成的实体数据。
如果场景中没有**实体数据管理器**，添加[实体管理器](#fpspawnersystemeditor_entitymanager)时，会自动添加**实体数据管理器**，**实体数据管理器**仅存在于服务器，场景仅能添加一个(程序控制)。
当场景中存在**实体数据管理器**，运行时会初始化**生成器管理子系统**

<a name="fpspawnersyste-projectsettings"></a>
### 项目设置

![FPSpawnerSystem_Settings](https://github.com/FirePlume126/FP_SpawnerSystem/blob/main/Images/FPSpawnerSystem_Settings.png)

```c++
// 默认分区网格设置
UPROPERTY(config, EditAnywhere, Category = "Config")
FFPSpawnerPartitionGridSettings DefaultPartitionGridSettings;

// 默认激活实体设置
UPROPERTY(config, EditAnywhere, Category = "Config")
FFPSpawnerActivateEntitySettings DefaultActivateEntitySettings;

// 启用LOD
UPROPERTY(config, EditAnywhere, Category = "Config|LOD Settings")
bool bEnableLOD = true;

// LOD数组
UPROPERTY(config, EditAnywhere, meta = (EditCondition = "bEnableLOD"), Category = "Config|LOD Settings")
FFPSpawnerLODArray LODArray;

// 视野区域外的LOD比例，等于1时不计算视野角度
UPROPERTY(config, EditAnywhere, meta = (EditCondition = "bEnableLOD", ClampMin = 0.0, ClampMax = 1.0), Category = "Config|LOD Settings")
float OutOfViewLODScale = 0.1f;

#if WITH_EDITORONLY_DATA

// 每帧最大散布对象数
UPROPERTY(config, EditAnywhere, meta = (ClampMin = 1), Category = "Config|Editor")
int32 MaxScatterObjectsPerTick = 100;

//  应用散布数据时，世界分区的散布网格单元大小，开启世界分区才有效
UPROPERTY(config, EditAnywhere, meta = (ClampMin = 1), Category = "Config|Editor")
uint32 InstancedMeshGridSize = 25600;
#endif

// 分区网格设置
USTRUCT(BlueprintType)
struct FFPSpawnerPartitionGridSettings
{
	GENERATED_BODY()

public:

	// 单元格大小
	UPROPERTY(EditAnywhere, meta = (ClampMin = 1600.0, ForceUnits = "cm"))
	float CellSize = 6400.0f;

	// 加载范围，激活源球形范围接触到网格单元时激活它，加载此范围内的实体
	UPROPERTY(EditAnywhere, meta = (ClampMin = 0.0, ForceUnits = "cm"))
	float LoadingRange = 10000.0f;

	// 卸载范围，激活源球形范围远离网格单元时卸载它，卸载此范围外卸载实体
	UPROPERTY(EditAnywhere, meta = (ClampMin = 0.0, ForceUnits = "cm"))
	float UnloadingRange = 12800.0f;

	// 排除范围，仅排除初次生成的实体，防止在激活源太近的位置刷新实体
	UPROPERTY(EditAnywhere, meta = (ClampMin = 0.0, ForceUnits = "cm"))
	float ExclusionRange = 500.0f;

	// 启用高度限制
	UPROPERTY(EditAnywhere)
	bool bEnableHeightLimit = false;

	// 单元格高度，限制单元格的高度
	UPROPERTY(EditAnywhere, meta = (ClampMin = 1600.0, ForceUnits = "cm", EditCondition = "bEnableHeightLimit"))
	float CellHeight = 3200.0f;

	// 加载高度，限制加载的高度
	UPROPERTY(EditAnywhere, meta = (ClampMin = 0.0, ForceUnits = "cm", EditCondition = "bEnableHeightLimit"))
	float LoadingHeight = 6400.0f;

	// 异步计算加载分区网格时，每帧最大计算时间(毫秒)
	UPROPERTY(EditAnywhere, meta = (ClampMin = 1.0, ForceUnits = "ms"))
	float MaxCalcTimePerFrame = 5.0f;

	//  异步计算加载分区网格时，每帧最大时间(毫秒)
	UPROPERTY(EditAnywhere, meta = (ClampMin = 1.0, ForceUnits = "ms"))
	float MaxFrameTime = 10.0f;

	// 读取网格单元的实体时，每帧最大加载网格单元的数量，等于0时直接异步线程处理所有加载网格单元
	UPROPERTY(EditAnywhere, BlueprintReadWrite, meta = (ClampMin = 0))
	int32 MaxLoadingNumberPerTick = 0;

	// 是否有效
	FORCEINLINE bool IsValid() const;

	// 获取单元格大小
	FORCEINLINE FVector GetCellSize() const;

	// 获取加载范围单元格
	FORCEINLINE FIntVector GetLoadingRangeCells() const;

	// 获取卸载范围单元格
	FORCEINLINE FIntVector GetUnloadingRangeCells() const;
};

// 激活实体设置
USTRUCT(BlueprintType)
struct FFPSpawnerActivateEntitySettings
{
	GENERATED_BODY()

public:

	// 每帧最大处理实体数据的数量
	UPROPERTY(EditAnywhere, BlueprintReadWrite, meta = (ClampMin = 1))
	int32 MaxActivateNumberPerTick = 1024;

	// 每帧最大生成实体的数量
	UPROPERTY(EditAnywhere, BlueprintReadWrite, meta = (ClampMin = 1))
	int32 MaxSpawnNumberPerTick = 64;

	// 最大生成Actor的数量
	UPROPERTY(EditAnywhere, BlueprintReadWrite, meta = (ClampMin = 1))
	int32 MaxActorNumber = 2560;

	// 最大生成网格体的数量
	UPROPERTY(EditAnywhere, BlueprintReadWrite, meta = (ClampMin = 1))
	int32 MaxMeshNumber = 25600;
};
```

<a name="fpspawnersyste-functionlibrary"></a>
### 函数库

函数库`UFPSpawnerFunctionLibrary`

```c++
// 初始化生成器子系统，AFPSpawnerEntityDataManager::bAutoInitSpawner=false时调用此函数才有效
// @param NewSaveData 生成器保存的数据
UFUNCTION(BlueprintCallable, BlueprintAuthorityOnly, meta = (WorldContext = "WorldContextObject"), Category = "FPSpawner")
static bool InitSpawnerSubsystem(const UObject* WorldContextObject, const FFPSpawnerSaveData& NewSaveData);

// 应用生成器保存数据，此函数只允许成功调用一次
// @param NewSaveData 生成器保存的数据
UFUNCTION(BlueprintCallable, BlueprintAuthorityOnly, meta = (WorldContext = "WorldContextObject"), Category = "FPSpawner")
static bool ApplySpawnerSaveData(const UObject* WorldContextObject, const FFPSpawnerSaveData& NewSaveData);

// 获取生成器保存数据
// @param OutSaveData 生成器保存的数据
UFUNCTION(BlueprintCallable, BlueprintAuthorityOnly, meta = (WorldContext = "WorldContextObject"), Category = "FPSpawner")
static bool GetSpawnerSaveData(const UObject* WorldContextObject, FFPSpawnerSaveData& OutSaveData);

// 尝试修改实体管理器状态
// @param InManagerName 实体管理器名称
// @param InRequestType 实体管理器请求类型
UFUNCTION(BlueprintCallable, BlueprintAuthorityOnly, meta = (WorldContext = "WorldContextObject"), Category = "FPSpawner")
static bool TryModifyEntityManagerState(const UObject* WorldContextObject, const FName& InManagerName, const EFPSpawnerEntityManagerRequestType InRequestType);

// 尝试修改实体状态
// @param InEntityHandle 实体句柄，生成器系统的实体唯一标识
// @param InRequestType 生成器实体请求类型
UFUNCTION(BlueprintCallable, BlueprintAuthorityOnly, meta = (WorldContext = "WorldContextObject"), Category = "FPSpawner")
static bool TryModifyEntityState(const UObject* WorldContextObject, const FFPSpawnerEntityHandle& InEntityHandle, const EFPSpawnerEntityRequestType InRequestType);

// 获取实体句柄
// @param InEntity 生成器生成的实体
// @return 实体句柄，生成器系统的实体唯一标识
UFUNCTION(BlueprintPure, Category = "FPSpawner")
static FFPSpawnerEntityHandle GetEntityHandle(AActor* InEntity);

// 获取实体属性集
// @param InEntity 生成器生成的实体
UFUNCTION(BlueprintPure, Category = "FPSpawner")
static UFPSpawnerEntityAttributeSet* GetEntityAttributeSet(AActor* InEntity);
```
