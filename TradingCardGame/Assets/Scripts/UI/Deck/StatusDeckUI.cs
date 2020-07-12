using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Показывает статус колоды(в случае если она собрана неправильно)
/// </summary>
public class StatusDeckUI : MonoBehaviour
{
    [SerializeField] private Image fon;
    [SerializeField, Space(10)] private Text statusText;
}
