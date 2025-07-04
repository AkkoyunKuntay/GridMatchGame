using UnityEngine;
using Zenject;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class NodeView : MonoBehaviour
{
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite xSprite;

    SpriteRenderer sr;
    [Inject] GridPresenter presenter;
    public bool IsMarked { get; private set; }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();        
        if (sr) sr.sprite = emptySprite;
    }

    public void SetPresenter(GridPresenter p) => presenter = p;

    public void SetMarked(bool marked)
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>() ?? GetComponentInChildren<SpriteRenderer>(true);

        IsMarked = marked;
        sr.sprite = marked ? xSprite : emptySprite;
    }

    void OnMouseUpAsButton() => presenter?.OnNodeClicked(this);

    public int X => Mathf.RoundToInt(transform.parent
                                     ? transform.parent.localPosition.x
                                     : transform.localPosition.x);
    public int Y => Mathf.RoundToInt(transform.parent
                                     ? transform.parent.localPosition.y
                                     : transform.localPosition.y);
}
