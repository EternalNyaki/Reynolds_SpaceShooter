using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletMethod
{
    protected float _startTime;
    protected float _duration;
    protected float _interval;
    protected BulletPattern _pattern;

    protected virtual void Pattern()
    {

    }

    public IEnumerator Run()
    {
        if (_startTime != 0)
        {
            yield return new WaitForSeconds(_startTime);
        }

        float timeAtStart = Time.time;
        //This is a do-while instead of a standard while to allow a duration of 0 to have the pattern run exactly once
        do
        {
            Pattern();
            yield return new WaitForSeconds(_interval);
        } while (timeAtStart + _duration >= Time.time);
    }

    //For testing, will be removed for the final product
    public void SetPattern(BulletPattern pattern)
    {
        _pattern = pattern;
    }
}
