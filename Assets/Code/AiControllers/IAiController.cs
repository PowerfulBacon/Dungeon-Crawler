using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAiController
{

    //The parent of this AI controller
    Mob parent { get; set; }
    
    //Target of the mob
    Mob target { get; set; }

    //Vision range
    int visionRange { get; }

    //Takeover control of a mob
    void Takeover(Mob parent);
    
    //Checks vision for new targets
    void CheckVision(List<Mob> view);

    //Checks vision to see if current target is in view
    void CheckTarget();

    //Performs an action
    void PerformAction();

    //Forget about the current target
    void ForgetTarget();

}
