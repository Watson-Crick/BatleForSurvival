/// <summary>
/// 枪械武器类型
/// </summary>
public enum GunType
{
    AssaultRifle,
    HuntingRifle,
    Revolver,
    Shotgun,
    WoodenBow,
    WoodenSpear,
    StoneHatchet
}

/// <summary>
/// 材质类型
/// </summary>
public enum MaterialType
{
    Wood,
    Stone,
    Flesh,
    Metal
}

/// <summary>
/// AI角色类型
/// </summary>
public enum AIManagerType
{
    BOAR,
    CANNIBAL,
    NULL
}

/// <summary>
/// AI状态
/// </summary>
public enum AIState
{
    WALK,
    ENTERATTACK,
    EXITATTACK,
    ENTERRUN,
    EXITRUN,
    IDLE,
    DEATH
}

/// <summary>
/// 音频文件名称
/// </summary>
public enum ClipName
{
    /// <summary>
    /// 野猪攻击音效.
    /// </summary>
    BoarAttack,
    /// <summary>
    /// 野猪死亡音效.
    /// </summary>
    BoarDeath,
    /// <summary>
    /// 野猪受伤音效.
    /// </summary>
    BoarInjured,
    /// <summary>
    /// 丧尸攻击音效.
    /// </summary>
    ZombieAttack,
    /// <summary>
    /// 丧尸死亡音效.
    /// </summary>
    ZombieDeath,
    /// <summary>
    /// 丧尸受伤音效.
    /// </summary>
    ZombieInjured,
    /// <summary>
    /// 丧尸尖叫音效.
    /// </summary>
    ZombieScream,
    /// <summary>
    /// 子弹命中地面音效.
    /// </summary>
    BulletImpactDirt,
    /// <summary>
    /// 子弹命中身体音效.
    /// </summary>
    BulletImpactFlesh,
    /// <summary>
    /// 子弹命中金属音效.
    /// </summary>
    BulletImpactMetal,
    /// <summary>
    /// 子弹命中石头音效.
    /// </summary>
    BulletImpactStone,
    /// <summary>
    /// 子弹命中木材音效.
    /// </summary>
    BulletImpactWood,
    /// <summary>
    /// 玩家角色急促呼吸声.
    /// </summary>
    PlayerBreathingHeavy,
    /// <summary>
    /// 玩家角色受伤音效.
    /// </summary>
    PlayerHurt,
    /// <summary>
    /// 玩家角色死亡音效.
    /// </summary>
    PlayerDeath,
    /// <summary>
    /// 身体命中音效.
    /// </summary>
    BodyHit
}

/// <summary>
/// BuildPanel菜单状态
/// </summary>
public enum BuildPanelState
{
    /// <summary>
    /// 默认状态
    /// </summary>
    NORMAL,
    /// <summary>
    /// 选中内层material
    /// </summary>
    SELECT,
    /// <summary>
    /// 准备创建物体
    /// </summary>
    READY,
    /// <summary>
    /// 创建物体
    /// </summary>
    CREATE
}

/// <summary>
/// 物品放置状态
/// </summary>
public enum BuildItemState
{
    CANNOTPUT,
    CANPUT,
    HAVEPUT
}