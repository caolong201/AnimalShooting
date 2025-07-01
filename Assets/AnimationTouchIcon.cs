using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationTouchIcon : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float vibrateRange; //振動幅
    [SerializeField] private float vibrateSpeed;               //振動速度

    enum AnimationPattern {
        Field,
        Animal
    };
    [SerializeField]
    AnimationPattern SpeechIconPattern = AnimationPattern.Field;

    Tween tween;
    // Use this for initialization
    void Awake(){
        transform.localScale = new Vector3(0.1f, 0.1f, 1.0f);
    }
    void Start () {
        IconAnimation();
    }

    public void IconAnimation(){
        //吹き出しタイプによってアニメーション処理変更

        switch(SpeechIconPattern.ToString()){
            case "Field":
                StartCoroutine(ScaleIcon());
                break;
            case "Animal":
                if(!this)
                StartCoroutine(PopIcon());
                break;
            default:
                break;
        }
    }

    IEnumerator ScaleIcon(){
        DOTween.Sequence().Append(transform.DOPunchScale(new Vector3(1f, 1f, 1f)*0.01f, 0.5f, 1, 0.1f))
         .Join(transform.DOJump(transform.position,1f,1,0.5f));
        yield return new WaitForSeconds(2.0f); 
        StartCoroutine(ScaleIcon());
        yield break;
    }

    IEnumerator PopIcon(){
        DOTween.Sequence().Append(transform.DOJump(transform.position,1f,1,0.5f));
        yield return new WaitForSeconds(2.0f); 
        StartCoroutine(PopIcon());
        yield break;
    }

}
