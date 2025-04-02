using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        if (Camera.main == null) return;
        transform.forward = Camera.main.transform.forward;
    }
}