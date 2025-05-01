using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FlavorHUD : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI linearVelocity;
    [SerializeField] TextMeshProUGUI textViewMatrix;
    [SerializeField] Rigidbody rb;
    private void Update()
    {
        // time
        time.text = System.DateTime.Now.AddYears(31).ToString("yyyy-MM-dd HH:mm:ss.fff");

        // linear vel
        Vector3 v = rb.linearVelocity;
        linearVelocity.text = "linear-vel:{"
            + v.x.ToString("F3") + ", "
            + v.y.ToString("F3") + ", "
            + v.z.ToString("F3") + "}";


        // view matrix
        Matrix4x4 viewMatrix = Camera.main.worldToCameraMatrix;
        string viewStr = string.Format(
            "view-m:\n{{{0}, {1}, {2}, {3}}}\n{{{4}, {5}, {6}, {7}}}\n{{{8}, {9}, {10}, {11}}}",
            Format(viewMatrix.m00), Format(viewMatrix.m01), Format(viewMatrix.m02), Format(viewMatrix.m03),
            Format(viewMatrix.m10), Format(viewMatrix.m11), Format(viewMatrix.m12), Format(viewMatrix.m13),
            Format(viewMatrix.m20), Format(viewMatrix.m21), Format(viewMatrix.m22), Format(viewMatrix.m23)
        );

        string Format(float val) => (Mathf.Floor(val * 1000f) / 1000f).ToString("F3");
        textViewMatrix.text = viewStr;
    }

}
