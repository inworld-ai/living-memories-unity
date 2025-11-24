using UnityEngine;
using UnityEngine.VFX;

public class MemoryVFXDriver : MonoBehaviour
{
    public VisualEffect vfx;
    public RectTransform frameRect;   // UI相框
    public Camera uiCamera;           // 渲染UI的摄像机（或主相机）
    public Texture2D sourceTex;       // 粒子取色图（生成前的“记忆图片”）
    public Texture2D spawnMask;       // 发射遮罩，可选

    public float startBurstRate = 500000f; // 例：50万/秒
    public float burstSeconds = 1.5f;
    public float attraction = 28f;

    void Start()
    {
        // 把 UI 相框中心转换到世界坐标（如果用World空间VFX）
        Vector3 worldTarget = GetFrameWorldCenter();
        vfx.SetVector3("TargetPosition", worldTarget);

        if (sourceTex) vfx.SetTexture("SourceTex", sourceTex);
        if (spawnMask) vfx.SetTexture("SpawnMask", spawnMask);

        vfx.SetFloat("AttractionForce", attraction);
        vfx.SetFloat("BurstSeconds", burstSeconds);

        // 启动：先高密度喷发，再逐渐减弱
        vfx.SendEvent("OnPlay"); // 在图里接收这个事件，触发Burst
    }

    Vector3 GetFrameWorldCenter()
    {
        Vector3[] corners = new Vector3[4];
        frameRect.GetWorldCorners(corners);
        Vector3 worldCenter = (corners[0] + corners[2]) * 0.5f;
        return worldCenter;
    }
}