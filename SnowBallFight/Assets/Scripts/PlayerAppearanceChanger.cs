using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearanceChanger : MonoBehaviour
{
    [Serializable]
    private struct MaterialToTeam
    {
        public TeamType team;
        public Color materialColor;
    }
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private List<MaterialToTeam> materialToTeams;
    [SerializeField]
    private Renderer[] rendererToChangeColor;

    private void OnEnable()
    {
        playerController.PlayerTeamChanged += OnPlayerTeamChanged;
        OnPlayerTeamChanged();
    }

    private void OnDisable()
    {
        playerController.PlayerTeamChanged -= OnPlayerTeamChanged;
    }

    private void OnPlayerTeamChanged()
    {
        SetMaterial(playerController.GetTeam());
    }

    public void SetMaterial(TeamType team)
    {
        Color materialColor = materialToTeams.Find(x => x.team == team).materialColor;
        foreach(var renderer in rendererToChangeColor)
        {
             renderer.materials[6].color = materialColor;
        }
    }
}
