using UnityEngine;

public class TeamSwitcher : MonoBehaviour
{
    private TeamMember player;

    void Start()
    {
        // tenta pegar automaticamente no próprio objeto
        player = GetComponent<TeamMember>();

        // se não tiver, procura na cena
        if (player == null)
        {
            player = FindObjectOfType<TeamMember>();
        }

        if (player == null)
        {
            Debug.LogError("❌ Nenhum TeamMember encontrado na cena!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetTeam(Team.TeamA);
            
            
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetTeam(Team.TeamB);

        if (Input.GetKeyDown(KeyCode.Alpha0))
            SetTeam(Team.Neutral);
    }

    void SetTeam(Team newTeam)
    {
        if (player == null) return;

        player.SetTeam(newTeam);

        Debug.Log("🔄 Time alterado para: " + newTeam);
    }

}