using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public List<CheckPoint> checkPoints;  // Lista de checkpoints en orden
    private int currentIndex = 0;         // Índice del checkpoint activo
    private int lapsCompleted = 0;        // Contador de vueltas

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
                lapsCompleted++;  // Completar una vuelta
                Debug.LogError($"Vuelta completada: {lapsCompleted}");
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

    public int GetLapsCompleted()
    {
        return lapsCompleted;
    }
}