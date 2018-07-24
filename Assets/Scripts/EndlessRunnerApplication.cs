using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRunnerElement : MonoBehaviour
{
    public EndlessRunnerApplication App { get { return FindObjectOfType<EndlessRunnerApplication>();  }}
}

public class EndlessRunnerApplication : EndlessRunnerElement {

    public EndlessRunnerModel model;
    public EndlessRunnerView view;
    public EndlessRunnerController controller;
 
    public void Notify(string p_event_path, Object p_target, params Object[] p_data)
    {
        EndlessRunnerController[] controller_list = GetAllControllers();
        foreach(EndlessRunnerController c in controller_list)
        {
            c.OnNotification(p_event_path, p_target, p_data);
            Debug.Log(c.ToString() + " has recieved a notification from  " + p_target.ToString() + " to do " + p_event_path);
        }
    }

    public EndlessRunnerController[] GetAllControllers()
    {
        return FindObjectsOfType<EndlessRunnerController>();
    }

}
