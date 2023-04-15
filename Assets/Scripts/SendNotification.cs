using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendNotification : MonoBehaviour
{
    static TMP_Text notificationText;
    public static void Notify(string notification)
    {
        notificationText = GameObject.FindGameObjectWithTag("NotificationText").GetComponent<TMP_Text>();
        notificationText.text = notification;
        notificationText.transform.parent.GetComponent<CanvasGroup>().alpha = 1.0f;
        StaticCoroutine.DoCoroutine(delayClose());
    }

    static IEnumerator delayClose()
    {
        yield return new WaitForSeconds(3);
        StaticCoroutine.DoCoroutine(closeNotification());
    }

    static IEnumerator closeNotification()
    {
        yield return new WaitForSeconds(0.01f);
        if (notificationText.transform.parent.GetComponent<CanvasGroup>().alpha > 0)
        {
            notificationText.transform.parent.GetComponent<CanvasGroup>().alpha -= 0.01f;
            StaticCoroutine.DoCoroutine(closeNotification());
        }
    }
}
