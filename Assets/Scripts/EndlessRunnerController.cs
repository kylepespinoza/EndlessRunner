using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRunnerController : EndlessRunnerElement
{


    public void OnPlayerGetCollectible()
    {
        App.model.score++;
        UpdateScoreText();
    }

    public virtual void OnNotification(string p_event_path, Object p_target, params Object[] p_data)
    {
        if (p_event_path == EndlessRunnerNotification.ScoreIncreased)
        {
            App.model.score++;
            UpdateScoreText();
            Destroy(p_data[0]);
        }
    }

    private void UpdateScoreText()
    {
        App.view.scoreNumber.text = App.model.score.ToString();
    }
}

