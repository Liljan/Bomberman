using UnityEngine;
using UnityEngine.UI;

public class InGameHUD : MonoBehaviour {

    public int ID = 1;
    public Text NameText;
    public Text HealthText;
    public Text BombText;

	// Use this for initialization
	void Start () {
        UIEvents.Instance().UpdateHealth += SetHealthText;
        UIEvents.Instance().UpdateBombs += SetBombText;
    }

    void SetHealthText(int ID, int health)
    {
        if (this.ID != ID)
            return;

        HealthText.text = "Health: " + health;
    }

    void SetBombText(int ID, int bombs)
    {
        if (this.ID != ID)
            return;

        BombText.text = "Bombs: " + bombs;
    }
}
