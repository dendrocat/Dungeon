using UnityEngine;

[RequireComponent(typeof(MouseLook), typeof(PlayerMover))]
public class Player : Person
{
    public Health Health => p_Health;
}

