using UnityEngine;
using System.Collections.Generic;

public class CaptureTriggerAdvanced : MonoBehaviour
{
    [Header("Flag")]
    public FlagController flagController;

    [Header("Base")]
    public BaseDestructibleWithHealthVisual baseScript;

    [Header("Resource Node")]
    public ResourceNode resourceNode;

    [Header("Capture Settings")]
    public float captureTime = 8f;

    [Header("Capture Type")]
    public bool countsAsCity = true;

    [Header("Structure")]
    public StructureType structureType;

    [Header("Debug")]
    public Team currentOwner = Team.Neutral;

    float captureProgress = 0f;
    bool captured = false;

    Team lastCapturingTeam = Team.Neutral;

    List<TeamMember> unitsInside = new List<TeamMember>();

    void Start()
    {
        captured = false;
        captureProgress = 0f;
        currentOwner = Team.Neutral;

        // auto pega ResourceNode se existir
        if (resourceNode == null)
        {
            resourceNode = GetComponent<ResourceNode>();
        }
    }

    void Update()
    {
        // DEBUG MANUAL
        if (captured && Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("🚗 Spawn liberado via tecla T");

            if (baseScript != null)
            {
                baseScript.OnCaptured();
            }
        }

        unitsInside.RemoveAll(u => u == null);

        HandleCapture();
    }

    // =====================================================
    // HANDLE CAPTURE
    // =====================================================
    void HandleCapture()
    {
        int teamACount = 0;
        int teamBCount = 0;

        foreach (var unit in unitsInside)
        {
            if (unit == null)
                continue;

            if (unit.team == Team.TeamA)
                teamACount++;

            if (unit.team == Team.TeamB)
                teamBCount++;
        }

        // ================= CONTESTADO =================
        if (teamACount > 0 && teamBCount > 0)
        {
            Debug.Log("⚔️ CONTESTADO");
            return;
        }

        // ================= NINGUÉM =================
        if (teamACount == 0 && teamBCount == 0)
        {
            return;
        }

        Team capturingTeam =
            teamACount > 0
            ? Team.TeamA
            : Team.TeamB;

        // evita recaptura
        if (captured && currentOwner == capturingTeam)
        {
            return;
        }

        // reset se mudou time capturando
       if (capturingTeam != lastCapturingTeam)
{
    captureProgress = 0f;

    lastCapturingTeam = capturingTeam;

    Debug.Log("🔄 Novo time capturando");
}

        // ================= PROGRESSO =================
        int count = Mathf.Max(teamACount, teamBCount);

        float speed = count;

        captureProgress +=
            (Time.deltaTime / captureTime) * speed;

        captureProgress = Mathf.Clamp01(captureProgress);

        Debug.Log(
            $"🏙️ Capturando {capturingTeam} → {captureProgress * 100f:F0}%"
        );

        // ================= CAPTURA FINAL =================
        if (captureProgress >= 1f && !captured)
        {
            Team oldOwner = currentOwner;

            // evita recaptura igual
            if (oldOwner == capturingTeam)
            {
                captured = true;
                captureProgress = 1f;
                return;
            }

            captured = true;
            captureProgress = 1f;

            currentOwner = capturingTeam;
            lastCapturingTeam = Team.Neutral;

            Debug.Log($"🏙️ BASE CAPTURADA: {currentOwner}");
            Debug.Log($"🔥 FLOW: {oldOwner} → {currentOwner}");

            // =====================================================
            // RESOURCE NODE
            // =====================================================
            if (resourceNode != null)
            {
                resourceNode.SetOwner(currentOwner);

                // REMOVE DO ANTIGO DONO
if (oldOwner == Team.TeamA)
{
    if (resourceNode.resourceType == ResourceType.Steel)
        GameManager.Instance.teamASteel--;

    if (resourceNode.resourceType == ResourceType.Oil)
        GameManager.Instance.teamAOil--;
}

if (oldOwner == Team.TeamB)
{
    if (resourceNode.resourceType == ResourceType.Steel)
        GameManager.Instance.teamBSteel--;

    if (resourceNode.resourceType == ResourceType.Oil)
        GameManager.Instance.teamBOil--;
}

                // =====================================
// RESOURCE BONUS
// =====================================
if (GameManager.Instance != null)
{
    if (resourceNode.resourceType == ResourceType.Steel)
    {
        if (currentOwner == Team.TeamA)
            GameManager.Instance.teamASteel++;

        if (currentOwner == Team.TeamB)
            GameManager.Instance.teamBSteel++;
    }

    if (resourceNode.resourceType == ResourceType.Oil)
    {
        if (currentOwner == Team.TeamA)
            GameManager.Instance.teamAOil++;

        if (currentOwner == Team.TeamB)
            GameManager.Instance.teamBOil++;
    }
}

                Debug.Log("⛏ Mina agora pertence a: " + currentOwner);
                

                // libera truck
                if (GameManager.Instance != null)
                {
                    if (currentOwner == Team.TeamA)
                    {
                        GameManager.Instance.teamATruckUnlocked = true;
                    }

                    if (currentOwner == Team.TeamB)
                    {
                        GameManager.Instance.teamBTruckUnlocked = true;
                    }

                    if (currentOwner == Team.TeamA)

if (currentOwner == Team.TeamB)

Debug.Log("🚚 Truck desbloqueado!");
                }
            }

// =====================================================
// GAME MANAGER
// =====================================================
if (GameManager.Instance != null)
{
    GameManager.Instance.RegisterStructure(
        structureType,
        currentOwner,
        oldOwner
    );
}
    
            // =====================================================
            // FLAG
            // =====================================================
            if (flagController != null)
            {
                flagController.SetOwner(currentOwner);
            }

            // =====================================================
            // BASE SCRIPT
            // =====================================================
            if (baseScript != null)
            {
                baseScript.OnCaptured();
            }

            lastCapturingTeam = Team.Neutral;
        }
    }

    // =====================================================
    // TRIGGER ENTER
    // =====================================================
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Unit"))
            return;

        TeamMember unit =
            other.GetComponent<TeamMember>() ??
            other.GetComponentInParent<TeamMember>() ??
            other.GetComponentInChildren<TeamMember>();

        if (unit != null && !unitsInside.Contains(unit))
        {
            unitsInside.Add(unit);

            Debug.Log(
                "➡️ Entrou: " +
                unit.name +
                " | " +
                unit.team
            );
        }
    }

    // =====================================================
    // TRIGGER EXIT
    // =====================================================
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Unit"))
            return;

        TeamMember unit =
            other.GetComponent<TeamMember>() ??
            other.GetComponentInParent<TeamMember>() ??
            other.GetComponentInChildren<TeamMember>();

        if (unit != null)
        {
            unitsInside.Remove(unit);

            Debug.Log("⬅️ Saiu: " + unit.name);
        }
    }

    // =====================================================
    // RESET
    // =====================================================
    public void ResetCapture()
    {
        if (countsAsCity && GameManager.Instance != null)
        {
            if (currentOwner == Team.TeamA)
            {
                GameManager.Instance.teamACities--;
            }
            else if (currentOwner == Team.TeamB)
            {
                GameManager.Instance.teamBCities--;
            }
        }

        captureProgress = 0f;
        captured = false;

        currentOwner = Team.Neutral;
        lastCapturingTeam = Team.Neutral;

        Debug.Log("🔄 Capture RESETADO");

        if (flagController != null)
        {
            flagController.ResetFlag();
        }
    }

    // =====================================================
    // GET OWNER
    // =====================================================
    public Team GetOwner()
    {
        return currentOwner;
    }
}