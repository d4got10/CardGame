using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoseListener : MonoBehaviour
{
    public UnityEvent OnLose;

    public void Lose()
    {
        OnLose.Invoke();
    }
}
