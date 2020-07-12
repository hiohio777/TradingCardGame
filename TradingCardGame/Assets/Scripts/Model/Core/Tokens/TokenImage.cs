using UnityEngine;
using UnityEngine.UI;

public class TokenImage : MonoBehaviour
{
    public string Name => name;
    public string Description => token.Description;
    public bool IsSingle => token.IsSingle;

    private ITokenData token;

    public static TokenImage CreatPrefab(ITokenData token, Transform parent) =>
        Instantiate(Resources.Load<TokenImage>($"Card/TokenImage")).SetToken(token, parent);

    private TokenImage SetToken(ITokenData token, Transform parent)
    {
        this.token = token;
        transform.SetParent(parent, false);
        transform.SetAsLastSibling();
        GetComponent<Image>().sprite = token.Icon;
        name = token.Name;

        return this;
    }
}
public interface ITokensPanel
{
    int GetCountToken(string Name);
    bool AddToken(ITokenData token, byte count);
    void DeleteToken(string Name);
}