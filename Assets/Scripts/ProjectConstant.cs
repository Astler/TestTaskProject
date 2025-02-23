using UnityEngine;

public static class ProjectConstants
{
    public static readonly Vector3 Gravity = new(0f, -9.81f, 0f);
    public const float ProjectileSize = 0.25f;
    public const float VelocityDecreaseFactor = 0.5f;

    public const float MinSensitivity = 0.01f;
    public const float DefaultSensitivity = 1f;
    public const float MaxSensitivity = 2f;

    public const int MinPower = 1;
    public const int DefaultPower = 50;
    public const int MaxPower = 100;

    public const float DefaultPlaneSize = 10f;
}