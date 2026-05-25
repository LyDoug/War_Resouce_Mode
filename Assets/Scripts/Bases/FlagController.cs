using UnityEngine;

public class FlagController : MonoBehaviour
{
    [Header("Team")]
    public TeamMember teamMember;

    [Header("Animation")]
    public Transform flagVisual; // objeto que sobe
    public float raisedHeight = 3f;
    public float loweredHeight = 0f;
    public float moveSpeed = 3f;

    float targetHeight;

    void Awake()
    {
        if (teamMember == null)
            teamMember = GetComponent<TeamMember>();

        if (flagVisual == null)
            flagVisual = transform;

        targetHeight = loweredHeight;
    }

    void Update()
    {
        Vector3 pos = flagVisual.localPosition;
        pos.y = Mathf.Lerp(pos.y, targetHeight, Time.deltaTime * moveSpeed);
        flagVisual.localPosition = pos;
    }

    // ================= CAPTURA =================
    public void SetOwner(Team team)
    {
        if (teamMember != null)
        {
            teamMember.SetTeam(team); // 🔥 MUDA COR
        }

        // anima subida
        targetHeight = raisedHeight;

        Debug.Log("🚩 Flag atualizada para: " + team);
    }

    // opcional: reset
    public void ResetFlag()
    {
        if (teamMember != null)
            teamMember.SetTeam(Team.Neutral);

        targetHeight = loweredHeight;
    }
}