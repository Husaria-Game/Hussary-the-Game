using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExplosionTest : MonoBehaviour, IPointerDownHandler
{
    public GameObject explosionGameObject;
    private static ExplosionTest explosionTest = null;
    public CanvasGroup canvasGroup;
    public Text damageNumberText;

    public void OnPointerDown(PointerEventData eventData)
    {

        //disable all other previews
        if (explosionTest != null)
        {
            explosionTest.explosionGameObject.SetActive(false);
        }
        explosionTest = this;
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("input.middle");
            StartCoroutine(ReceiveDamage());
        }
    }

    public void setDamageVisual(int damage)
    {
        StartCoroutine(ReceiveDamage());
    }

    public IEnumerator ReceiveDamage()
    {
        explosionGameObject.SetActive(true);
        canvasGroup.alpha = 0.1f;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2);
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        explosionGameObject.SetActive(false);
        // after the effect is shown it gets destroyed.
        bool shouldDie = false;
        if (shouldDie)
        {
            Destroy(this.gameObject);
        }
    }
}
