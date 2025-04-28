using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour
{
    [SerializeField] Sprite[] _spritesSS = null;
    [SerializeField] Sprite[] _spritesTD = null;
    [SerializeField] EffectByViewChange _effectByViewChange = null;

    private void OnEnable()
    {
        if (_spritesSS == null || _spritesTD == null) return;
        if (_spritesSS.Length == 0 || _spritesTD.Length == 0 || _spritesSS.Length != _spritesTD.Length) return;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        int index = Random.Range(0, _spritesSS.Length);
        spriteRenderer.sprite = _spritesSS[index];
        _effectByViewChange.AddViewChangeSprites(spriteRenderer, _spritesSS[index], _spritesTD[index]);

        DestroyImmediate(this);
    }
}