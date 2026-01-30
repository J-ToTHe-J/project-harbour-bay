using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeleportWithFade : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 targetPosition = new Vector3(1.5f, 2.5f, 0f);
    public Image fadeImage;
    public float fadeSpeed = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportSequence(other.gameObject));
        }
    }

    private IEnumerator TeleportSequence(GameObject player)
    {
        // 1. Fade to Black
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // 2. Perform the Teleport
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = targetPosition;

        if (cc != null) cc.enabled = true;

        // Brief pause at black for world loading/stability
        yield return new WaitForSeconds(0.2f);

        // 3. Fade to Clear
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}