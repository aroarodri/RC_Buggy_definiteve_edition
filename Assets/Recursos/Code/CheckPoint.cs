using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways] // Permite que este script se ejecute en el editor
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
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        if (boxCollider != null)
        {
            // Configurar la matriz del Gizmo para reflejar la posición, rotación y escala del BoxCollider
            Gizmos.matrix = Matrix4x4.TRS(
                transform.TransformPoint(boxCollider.center), // Centro del BoxCollider en el espacio global
                transform.rotation,                           // Rotación del objeto
                transform.lossyScale                          // Escala global
            );

            // Configurar el color en función del estado del checkpoint
            Gizmos.color = checkPointActivo ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(Vector3.zero, boxCollider.size);

            // Dibujar el wireframe del BoxCollider
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, boxCollider.size);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (checkPointActivo && other.CompareTag("Buggy"))
        {
            DesactivarCheckPoint();
            onCheckPointTriggered?.Invoke(); // Disparar el evento al pasar por el checkpoint
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
