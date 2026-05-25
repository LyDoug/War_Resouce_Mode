using UnityEngine;

public enum Team
{
    Neutral,
    TeamA,
    TeamB
}

public class TeamMember : MonoBehaviour
{
    [Header("Team")]
    public Team team = Team.Neutral;

    [Header("Colors")]
    public Color neutralColor = Color.gray;
    public Color teamAColor = Color.blue;
    public Color teamBColor = Color.red;

    [Header("Renderers (optional)")]
    public Renderer[] renderers;

    void Awake()
    {
        if (renderers == null || renderers.Length == 0)
        {
            renderers = GetComponentsInChildren<Renderer>();
        }

        ApplyColor();
    }

    // ================= VISUAL =================
    public void ApplyColor()
    {
        Color color = neutralColor;

        switch (team)
        {
            case Team.TeamA:
                color = teamAColor;
                break;

            case Team.TeamB:
                color = teamBColor;
                break;

            case Team.Neutral:
                color = neutralColor;
                break;
        }

        foreach (var rend in renderers)
        {
            if (rend == null) continue;

            if (rend.material.HasProperty("_BaseColor"))
                rend.material.SetColor("_BaseColor", color);
            else
                rend.material.color = color;
        }
    }

    // ================= RUNTIME =================
    public void SetTeam(Team newTeam)
    {
        team = newTeam;
        ApplyColor();
    }
}