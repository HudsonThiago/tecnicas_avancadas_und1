using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * Classe base que define os atributos básicos de uma entidade no jogo.
 * Pode ser usada como base para jogadores, inimigos, NPCs ou qualquer objeto
 * que precise de características comuns, como movimentação e vida.
 */
public class Entity : MonoBehaviour
{
    public float moveSpeed;
    public float health;
    public float maxHealth;

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        healthInfo();
    }

    public void getHealth(float heal)
    {
        health += heal;
        if (health > maxHealth) health = maxHealth;
        healthInfo();
    }

    private void healthInfo()
    {
        if(TryGetComponent(out Player player))
        {
            Image healthBar = player.playerUI.healthBar;
            TextMeshProUGUI healthLabel = player.playerUI.healthLabel;
            if (healthBar)
            {
                healthBar.fillAmount = health / maxHealth;
                healthLabel.text = string.Format("{0}/{1}", health, maxHealth);
            }
        }
        if (gameObject.CompareTag("Enemy"))
        {
            if(health <=0 ) Destroy(gameObject);
        }
    }
}
