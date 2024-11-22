using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{
    public UnityEvent onCheckPointTriggered; // Evento de Unity para notificar al manager
    private bool checkPointActivo = true;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        ActivarCheckPoint(); // Activar el primer checkpoint al iniciar
    }

    private void OnDrawGizmos()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Gizmos.color = checkPointActivo ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
            Vector3 colliderCenter = transform.position + transform.rotation * Vector3.Scale(boxCollider.center, transform.lossyScale);
            Vector3 colliderSize = Vector3.Scale(boxCollider.size, transform.lossyScale);
            Gizmos.DrawCube(colliderCenter, colliderSize);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(colliderCenter, colliderSize);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (checkPointActivo && other.CompareTag("Buggy"))
        {
            DesactivarCheckPoint();
            onCheckPointTriggered?.Invoke();  // Disparar el evento al pasar por el checkpoint
        }
    }

    public void ActivarCheckPoint()
    {
        checkPointActivo = true;
        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
    }

    public void DesactivarCheckPoint()
    {
        checkPointActivo = false;
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }
}