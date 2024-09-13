using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyAndTail : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerOne")
        {
            if (this.gameObject.tag == "PlayerOneBodyOrTail")
            {
                GameManager.Instance.PlayerTwoWinsInTwoPlayerMode();
            }
            else
            {
                GameManager.Instance.PlayerOneWinsInTwoPlayerMode();
            }
        }


        if (collision.gameObject.tag == "PlayerTwo")
        {
            if (this.gameObject.tag == "PlayerOneBodyOrTail")
            {
                GameManager.Instance.PlayerTwoWinsInTwoPlayerMode();
            }
            else
            {
                GameManager.Instance.PlayerOneWinsInTwoPlayerMode();
            }
        }
    }
}
