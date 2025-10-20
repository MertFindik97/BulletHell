using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] Health playerHealth;

    void Start()
    {
        if (playerHealth == null)
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    
    void Update()
{
    if (playerHealth == null) return;

    float ratio = (float)playerHealth.CurrentHP / playerHealth.MaxHP;
    fillImage.fillAmount = ratio;

    if (Input.GetKeyDown(KeyCode.K))
    {
        playerHealth.TakeDamage(1);
        Debug.Log("HP: " + playerHealth.CurrentHP);
    }
}

}
