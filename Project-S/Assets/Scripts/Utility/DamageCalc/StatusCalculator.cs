using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusCalculator 
{

    protected KnightStatus status;

    public StatusCalculator(KnightStatus _status) {
        status = _status;
    }

    public virtual void Calc(){
    }

}
