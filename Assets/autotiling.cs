using UnityEngine;

[ExecuteAlways]
public class URPAutoTileByScale : MonoBehaviour
{
    public Renderer rend;
    public Vector2 tilesPerUnit = new Vector2(1f, 1f); // repeats per world unit on X,Z
    public int materialIndex = 0;
    private static readonly int BaseMap_ST = Shader.PropertyToID("_BaseMap_ST");

    Vector3 lastScale;

    void OnEnable() { Apply(); }
    void Update() { if (transform.lossyScale != lastScale) Apply(); }

    void Apply()
    {
        if (!rend) rend = GetComponent<Renderer>();
        if (!rend) return;

        var mats = Application.isPlaying ? rend.materials : rend.sharedMaterials;
        if (materialIndex < 0 || materialIndex >= mats.Length) return;

        var s = transform.lossyScale;
        // Assumes you want tiling along X (U) and Z (V) like floors/walls.
        var scale = new Vector4(s.x * tilesPerUnit.x, s.z * tilesPerUnit.y, 0f, 0f);
        // _BaseMap_ST packs tiling.xy and offset.zw for URP Lit
        mats[materialIndex].SetVector(BaseMap_ST, scale);
        lastScale = s;
    }
}