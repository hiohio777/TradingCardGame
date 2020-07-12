using System;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    [SerializeField] private LocalisationText message = null;
    [SerializeField] private Button OK = null, NO = null;

    private Action actOK;
    private Action actNO;

    public static MessagePanel SimpleMessage(Transform parent, string text) =>
    Instantiate(Resources.Load<MessagePanel>("SimpleMessage")).Build(parent, text);

    public static MessagePanel MessageWithChoice(Transform parent, string text, Action actOK = null, Action actNO = null) =>
    Instantiate(Resources.Load<MessagePanel>("MessageWithChoice")).Build(parent, text, actOK, actNO);

    private MessagePanel Build(Transform parent, string text, Action actOK = null, Action actNO = null)
    {
        transform.SetParent(parent, false);
        transform.SetAsLastSibling();
        message.SetKey(text);
        (this.actOK, this.actNO) = (actOK, actNO);

        if (OK != null) OK.onClick.AddListener(OnOk);
        if (NO != null) NO.onClick.AddListener(OnNO);

        return this;
    }

    public void OnOk()
    {
        actOK?.Invoke();
        Destroy(gameObject);
    }

    public void OnNO()
    {
        actNO?.Invoke();
        Destroy(gameObject);
    }
}
