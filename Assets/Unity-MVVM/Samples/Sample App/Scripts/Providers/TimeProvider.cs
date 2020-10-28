using System;
using System.Collections;
using UnityEngine;
using UnityMVVM.Util;

public class TimeProvider : Singleton<TimeProvider>
{
    public Action<DateTime> OnTimeUpdated;

    private void Awake()
    {
        StartCoroutine(UpdateTimeRoutine());
    }

    IEnumerator UpdateTimeRoutine()
    {
        while (true)
        {
            OnTimeUpdated?.Invoke(DateTime.Now);
                
            yield return new WaitForSeconds(1f);
        }
    }


}
