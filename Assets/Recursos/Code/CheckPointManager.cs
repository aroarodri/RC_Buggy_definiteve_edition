using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Importar TextMeshPro

public class CheckPointManager : MonoBehaviour
{
    // Lista de checkpoints en orden
    [SerializeField] private List<CheckPoint> checkPoints;

    // Índice del checkpoint activo
    [SerializeField] private int currentIndex = 0;

    // Contador de vueltas
    [SerializeField] private int lapsCompleted = 0;

    // Número máximo de vueltas
    [SerializeField] private int maxLaps = 3;

    [SerializeField] private TextMeshProUGUI lapsText;

    void Start()
    {
        // Inicializar checkpoints
        ResetCheckPoints();

        if (checkPoints.Count > 0)
        {
            // Activamos el primer checkpoint al inicio
            checkPoints[0].ActivarCheckPoint();
        }

        // Suscribir a los eventos de cada checkpoint en orden
        for (int i = 0; i < checkPoints.Count; i++)
        {
            int index = i; // Necesitamos capturar el índice en una variable local debido a la naturaleza de las lambdas
            checkPoints[i].onCheckPointTriggered.AddListener(() => HandleCheckPointTriggered(index));
        }

        // Actualizar texto inicial de vueltas
        UpdateLapsText();
    }

    void OnDestroy()
    {
        // Desuscribirse de los eventos para evitar fugas de memoria
        foreach (var checkpoint in checkPoints)
        {
            checkpoint.onCheckPointTriggered.RemoveListener(() => HandleCheckPointTriggered(checkpoint.GetHashCode()));
        }
    }

    private void HandleCheckPointTriggered(int checkpointIndex)
    {
        // Verificar si el checkpoint activado es el siguiente en el orden
        if (checkpointIndex == currentIndex)
        {
            currentIndex++;

            // Si hemos completado todos los checkpoints, incrementamos el contador de vueltas
            if (currentIndex >= checkPoints.Count)
            {
                // Completar una vuelta
                lapsCompleted++;
                Debug.Log("Vuelta completada");

                // Actualizar el texto de vueltas
                UpdateLapsText();

                // Comprobar si se ha alcanzado el número máximo de vueltas
                if (lapsCompleted >= maxLaps)
                {
                    Debug.Log("¡Máximo número de vueltas alcanzado!");
                    EndRace();
                    return;
                }

                ResetCheckPoints(); // Reiniciar checkpoints
                currentIndex = 0;  // Reiniciar el índice
            }

            // Activar el siguiente checkpoint
            if (currentIndex < checkPoints.Count)
            {
                checkPoints[currentIndex].ActivarCheckPoint();
            }
        }
    }

    private void ResetCheckPoints()
    {
        // Desactivar todos los checkpoints para reiniciar
        foreach (var checkpoint in checkPoints)
        {
            checkpoint.DesactivarCheckPoint();
        }
    }

    private void UpdateLapsText()
    {
        // Actualizar el texto de vueltas
        if (lapsText != null)
        {
            lapsText.text = $"{lapsCompleted}/{maxLaps}";
        }
    }

    private void EndRace()
    {
        // Acciones a realizar cuando se alcanzan las vueltas máximas
        Debug.Log("La carrera ha terminado.");
    }
}
