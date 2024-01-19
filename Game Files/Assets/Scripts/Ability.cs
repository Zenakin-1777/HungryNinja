using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public TextMeshProUGUI abilityText;
    public GameManager gameManager;
    private Rigidbody[] goodRb;
    public bool onCooldown;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        abilityText.text = "Ability Ready";
        abilityText.color = new Color32(0x77, 0xFA, 0x72, 0xFF);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !onCooldown && gameManager.isGameActive)
        {
            StartCoroutine(ActivateAbility());
            abilityText.text = "Ability on Cooldown";
            abilityText.color = new Color32(0xFF, 0x2F, 0x4B, 0xFF);
        }
    }

    IEnumerator ActivateAbility()
    {
        GameObject[] good = GameObject.FindGameObjectsWithTag("Good");
        goodRb = new Rigidbody[good.Length];

        for (int i = 0; i < good.Length; i++)
        {
            goodRb[i] = good[i].GetComponent<Rigidbody>();
            goodRb[i].constraints = RigidbodyConstraints.FreezePosition;
        }
        onCooldown = true;
        yield return new WaitForSeconds(10f);
        onCooldown = false;
        abilityText.text = "Ability Ready";
        abilityText.color = new Color32(0x77, 0xFA, 0x72, 0xFF);
    }
}
