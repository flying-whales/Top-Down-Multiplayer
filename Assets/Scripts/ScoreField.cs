using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ScoreField : NetworkBehaviour
{
    [SerializeField] NumberField ScoreDisplay;

    // Start is called before the first frame update
    [Networked(OnChanged = nameof(NetworkedScoreChanged))]
    public int NetworkedScore { get; set; } = 0;


    private static void NetworkedScoreChanged(Changed<ScoreField> changed) {
        // Here you would add code to update the player's healthbar.
        Debug.Log($"Score changed to: {changed.Behaviour.NetworkedScore}");
        changed.Behaviour.ScoreDisplay.SetNumber(changed.Behaviour.NetworkedScore);
    }

    public void HitEnemy()
    {
        NetworkedScore += 10;
    }
}
