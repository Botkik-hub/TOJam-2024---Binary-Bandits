using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpawnPoint : MonoBehaviour
{
    public void Move()
    {
        Player player = FindObjectOfType<Player>();

        player.ChangeSpawnPoint(transform.position);
    }
}
