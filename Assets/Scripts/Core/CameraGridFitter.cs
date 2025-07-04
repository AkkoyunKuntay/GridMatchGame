using UnityEngine;
using Zenject;

[RequireComponent(typeof(Camera))]
public class CameraGridFitter : MonoBehaviour
{
    [Inject] private GridService gridManager;
    [SerializeField] private float padding = 0.5f;

    private Camera _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();
        if (gridManager == null)
            Debug.LogError("CameraGridFitter: GridManager reference is null!");
    }

    void LateUpdate()
    {
        if (_cam == null || gridManager == null) return;

        float w = gridManager.width;
        float h = gridManager.height;
        Vector2 center = new Vector2(w * 0.5f - 0.5f, h * 0.5f - 0.5f);

        _cam.transform.position = new Vector3(center.x, center.y, _cam.transform.position.z);

        float aspect = (float)Screen.width / Screen.height;
        float halfW = w * 0.5f;
        float halfH = h * 0.5f;

        float sizeV = halfH;
        float sizeH = halfW / aspect;
        _cam.orthographicSize = Mathf.Max(sizeV, sizeH) + padding;
    }
}
