using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text damageText = null;

    private int damage = 0;

    private Coroutine textCor = null;

    private Action DestroyCallback = null;

    public void ShowDamageText(int _damage, Action _destroyCallback = null)
    {
        damage = _damage;
        damageText.text = $"{damage}";

        damageText.gameObject.SetActive(true);
        DestroyCallback = _destroyCallback;
        if (textCor != null )
        {
            StopCoroutine(textCor);
            textCor = null;
        }

        textCor = StartCoroutine(DamageShowCor());
    }

    private IEnumerator DamageShowCor()
    {
        while(damageText.color.a > 0.1f)
        {
            Color temp = damageText.color;

            temp.a -= 0.05f;

            damageText.color = temp;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        DestroyText();

        DestroyCallback?.Invoke();

    }

    private void DestroyText()
    {
        Destroy(gameObject);
    }
}
