using System;
using System.Threading;
using UnityEngine;

public class FunctionTimer
{
    public static FunctionTimer Create(Action action, float timer) 
    {
        FunctionTimer functionTimer = new FunctionTimer(action, timer);

        GameObject gameObject = new GameObject("FunctionTimer", typeof(MonoBehaviourHook));
        gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.Update;

        return functionTimer;
    }
    //Dummy class to have access to MonoBehaviour functions
    private class MonoBehaviourHook : MonoBehaviour
    {
        public Action onUpdate;
            private void Update()
        { 
            if(onUpdate != null) onUpdate();
        }
    }
    private Action action;
    private float timer;
    private bool isDestroyed;

    private FunctionTimer(Action action, float timer)
    {
        this.action = action;
        this.timer = timer;
        isDestroyed = false;
    }

    public void Update()
    {
        if (!isDestroyed)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                action();
                DestroySelf();
            }
        }
    }

    private void DestroySelf()
    { 
        isDestroyed = true;
    }
}
