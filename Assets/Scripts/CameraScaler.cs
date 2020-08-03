using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    [SerializeField]
    private Transform[] TopTransforms = new Transform[0];
    [SerializeField]
    private Transform[] BottomTransforms = new Transform[0];

    void Awake()
    {
#if !UNITY_WEBGL
        Camera camera = this.gameObject.GetComponent<Camera>();

        float width = Display.displays[0].renderingWidth;
        float height = Display.displays[0].renderingHeight;
        if (width > height)
        {
            return;
        }

        float ratio = height / width;
        camera.orthographicSize *= ratio;

        for (int index = 0; index < this.TopTransforms.Length; ++index)
        {
            Vector3 position = this.TopTransforms[index].position;
            position.y *= ratio;
            this.TopTransforms[index].position = position;
        }

        for (int index = 0; index < this.BottomTransforms.Length; ++index)
        {
            Vector3 position = this.BottomTransforms[index].position;
            position.y *= ratio;
            this.BottomTransforms[index].position = position;
        }
#endif
    }
}
