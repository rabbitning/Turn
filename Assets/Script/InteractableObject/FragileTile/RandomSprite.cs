using UnityEngine;
// using UnityEditor;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(EffectByViewChange))]
public class RandomSprite : MonoBehaviour
{
    [SerializeField] Sprite[] _spritesSS = null;
    [SerializeField] Sprite[] _spritesTD = null;

    private void OnEnable()
    {
        // if (!PrefabUtility.IsPartOfPrefabInstance(this)) return;

        if (_spritesSS == null || _spritesTD == null) return;
        if (_spritesSS.Length == 0 || _spritesTD.Length == 0 || _spritesSS.Length != _spritesTD.Length) return;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        EffectByViewChange effectByViewChange = GetComponent<EffectByViewChange>();

        int index = Random.Range(0, _spritesSS.Length);
        spriteRenderer.sprite = _spritesSS[index];
        effectByViewChange.AddViewChangeSprites(spriteRenderer, _spritesSS[index], _spritesTD[index]);

        DestroyImmediate(this);
        // EditorUtility.SetDirty(this);
    }
}