using UnityEngine;

public class Flow : MonoBehaviour
{
    private SpriteRenderer[] renderers;
    private Transform flowTransform;
    private Color color;
    private GameObject flowIndicator;
    public Color Color
    {
        get { return color; }
        set
        {
            color = value;
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.color = color;
            }
        }
    }
    private void Awake()
    {
        flowTransform = transform;
        renderers = GetComponentsInChildren<SpriteRenderer>(true);
        flowIndicator = transform.GetChild(0).gameObject;
    }
    public void Rotation(float angle)
    {
        Vector3 eulerangles = flowTransform.localEulerAngles;
        eulerangles.z = angle;
        flowTransform.localEulerAngles = eulerangles;
    }
    public void FlowON()
    {
        flowIndicator.SetActive(true);
    }
    public void FlowOFF()
    {
        flowIndicator.SetActive(false);
    }
}
