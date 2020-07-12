using UnityEngine;

[CreateAssetMenu(fileName = "ClassCardScriptable", menuName = "Data/ClassCard", order = 52)]
public class ClassCardScriptable : ScriptableObject, IClassCard
{
    [SerializeField, Space(10)] private Sprite icon = null;
    [SerializeField, Space(10)] private ClassCardEnum type;
    public Sprite Icon => icon;
    public string Name => type.ToString();
    public string Description => $"{Name}_description";
    public ClassCardEnum Type => type;
}
