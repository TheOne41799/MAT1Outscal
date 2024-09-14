

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyAndTail : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerOne")
        {
            if (GameManager.Instance.CurrentGameState == GameState.IN_GAME_UI_ONE_PLAYER)
            {
                if (collision.gameObject.GetComponent<PlayerController>().IsShieldPowerupActive) return;
                GameManager.Instance.GameOverOnePlayerMode();
            }

            else if (GameManager.Instance.CurrentGameState == GameState.IN_GAME_UI_TWO_PLAYER)
            {
                if (this.gameObject.tag == "PlayerOneBodyOrTail")
                {
                    if (collision.gameObject.GetComponent<PlayerController>().IsShieldPowerupActive) return;
                    GameManager.Instance.PlayerTwoWinsInTwoPlayerMode();
                }
                else
                {
                    GameManager.Instance.PlayerOneWinsInTwoPlayerMode();
                }
            }
        }


        if (collision.gameObject.tag == "PlayerTwo")
        {
            if (GameManager.Instance.CurrentGameState == GameState.IN_GAME_UI_TWO_PLAYER)
            {
                if (this.gameObject.tag == "PlayerOneBodyOrTail")
                {
                    GameManager.Instance.PlayerTwoWinsInTwoPlayerMode();
                }
                else
                {
                    if (collision.gameObject.GetComponent<PlayerController>().IsShieldPowerupActive) return;
                    GameManager.Instance.PlayerOneWinsInTwoPlayerMode();
                }
            }
        }
    }
}














/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyAndTail : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerOne")
        {
            if(GameManager.Instance.CurrentGameState == GameState.IN_GAME_UI_ONE_PLAYER)
            {
                if (collision.gameObject.GetComponent<PlayerController>().IsShieldPowerupActive) return;
                GameManager.Instance.GameOverOnePlayerMode();
            }

            else if(GameManager.Instance.CurrentGameState == GameState.IN_GAME_UI_TWO_PLAYER)
            {
                if (this.gameObject.tag == "PlayerOneBodyOrTail")
                {
                    if (collision.gameObject.GetComponent<PlayerController>().IsShieldPowerupActive) return;
                    GameManager.Instance.PlayerTwoWinsInTwoPlayerMode();
                }
                else
                {
                    GameManager.Instance.PlayerOneWinsInTwoPlayerMode();
                }
            }
        }


        if (collision.gameObject.tag == "PlayerTwo")
        {
            if (GameManager.Instance.CurrentGameState == GameState.IN_GAME_UI_TWO_PLAYER)
            {
                if (this.gameObject.tag == "PlayerOneBodyOrTail")
                {
                    GameManager.Instance.PlayerTwoWinsInTwoPlayerMode();
                }
                else
                {
                    if (collision.gameObject.GetComponent<PlayerController>().IsShieldPowerupActive) return;
                    GameManager.Instance.PlayerOneWinsInTwoPlayerMode();
                }
            }
        }
    }
}
*/