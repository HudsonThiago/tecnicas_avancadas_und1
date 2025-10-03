using Assets.scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/*
 * Classe base que define os atributos b�sicos de uma entidade no jogo.
 * Pode ser usada como base para jogadores, inimigos, NPCs ou qualquer objeto
 * que precise de caracter�sticas comuns, como movimenta��o e vida.
 */
public class Entity : MonoBehaviour
{
    public float moveSpeed;
    public float health;
    public float maxHealth;
}
