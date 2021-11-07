using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAiController
{

    //The parent of this AI controller
    IMobAi parent { get; set; }
    
    //Target of the mob
    IMobAi target { get; set; }

    //Vision range
    int visionRange { get; }

    //Takeover control of a mob
    void Takeover(IMobAi parent);

    //Checks vision for new targets
    void CheckVision(List<IMobAi> view);

    //Checks vision to see if current target is in view
    void CheckTarget();

    //Performs an action
    void PerformAction();

    //Forget about the current target
    void ForgetTarget();

}
