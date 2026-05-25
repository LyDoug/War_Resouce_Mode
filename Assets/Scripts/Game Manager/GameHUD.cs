using TMPro;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [Header("Resources")]
    public TMP_Text oilText;
    public TMP_Text steelText;

    [Header("Structures")]
    public TMP_Text mineText;
    public TMP_Text factoryText;
    public TMP_Text cityText;

    [Header("Tickets")]
    public TMP_Text ticketsText;

    void Update()
    {
        if (GameManager.Instance == null)
            return;

        // =====================================
        // RESOURCES
        // =====================================
        if (oilText != null)
        {
            oilText.text =
                "OIL " +
                GameManager.Instance.teamAOil;
        }

        if (steelText != null)
        {
            steelText.text =
                "STEEL " +
                GameManager.Instance.teamASteel;
        }

        // =====================================
        // STRUCTURES
        // =====================================
        if (mineText != null)
        {
            mineText.text =
                "MINE " +
                GameManager.Instance.teamAMines;
        }

        if (factoryText != null)
        {
            factoryText.text =
                "FACTORY " +
                GameManager.Instance.teamAFactories;
        }

        if (cityText != null)
        {
            cityText.text =
                "CITY " +
                GameManager.Instance.teamACities;
        }

        // =====================================
        // TICKETS
        // =====================================
        if (ticketsText != null)
        {
            ticketsText.text =
    "A: " +
    Mathf.RoundToInt(GameManager.Instance.teamAResources)
    +
    " | B: " +
    Mathf.RoundToInt(GameManager.Instance.teamBResources);
        }
    }
}