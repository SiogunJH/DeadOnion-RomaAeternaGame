using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationPlayer : MonoBehaviourSingleton<AnimationPlayer>
{
    [SerializeField]
    private GameObject _guyLeftPosition;
    [SerializeField]
    private GameObject _guyRightPosition;

    [SerializeField]
    private float _moveIntoViewSpeed = 1;
    [SerializeField]
    private float _delayBetweenFrames = 0.5f;
    [SerializeField]
    private float _delayAfterAnimation = 1f;

    public UnityEvent OnAnimationFinished;
    public void PlayAnimation()
    {
        CameraManager.Instance.SwitchAnimationCamera();
        StartCoroutine(CheckIfCameraInPosition());
    }
    private IEnumerator CheckIfCameraInPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.33f);
            if (CameraManager.Instance.AnimationCameraInPosition())
                break;
        }
        PrepareAssets();
    }
    private void PrepareAssets()
    {
        //duplicate
        GameObject casterBillboard = Instantiate(CombatManager.Instance.CurrentCombatant.Billboard, gameObject.transform);
        Dictionary<Character, GameObject> affectedCharacters = new();

        foreach (var c in CombatAbilityExecutor.AffectedCharacters)
        {
            Debug.Assert(c != null, "Character is null!");
            Debug.Assert(c.Billboard != null, "Billboard is null!");

            affectedCharacters.Add(c, Instantiate(c.Billboard, gameObject.transform));
        }

        //position
        casterBillboard.transform.position = (_guyLeftPosition.transform.position + (Vector3.left * 3));
        foreach (var x in affectedCharacters)
        {
            x.Value.transform.position = (_guyRightPosition.transform.position + (Vector3.right * 3));
        }

        //set first sprite
        casterBillboard.GetComponentInChildren<SpriteRenderer>().sprite = CombatManager.Instance.MostRecentAbility.CasterFrame1;
        foreach (var x in affectedCharacters)
        {
            x.Value.GetComponentInChildren<SpriteRenderer>().sprite = x.Key.HurtSprite; //yes I just call getComponent in a loop, what are you going to do about it?
        }
        StartCoroutine(MoveSpritesIntoView(casterBillboard, affectedCharacters));
    }
    private IEnumerator MoveSpritesIntoView(GameObject casterBillboard, Dictionary<Character, GameObject> affectedCharacters)
    {
        while (casterBillboard.transform.position != _guyLeftPosition.transform.position)
        {
            yield return null;
            casterBillboard.transform.position += (Vector3.right * _moveIntoViewSpeed) * Time.deltaTime;
            foreach (var x in affectedCharacters)
            {
                x.Value.transform.position += (Vector3.left * _moveIntoViewSpeed) * Time.deltaTime;
            }
            if (casterBillboard.transform.position.x > _guyLeftPosition.transform.position.x)
            {
                casterBillboard.transform.position = _guyLeftPosition.transform.position;
                foreach (var x in affectedCharacters)
                {
                    x.Value.transform.position = _guyRightPosition.transform.position;
                }
                break;
            }
        }
        StartAnimation(casterBillboard, affectedCharacters);
    }
    private void StartAnimation(GameObject casterBillboard, Dictionary<Character, GameObject> affectedCharacters)
    {
        ////////////////////////////////////////////////////////////
        // HERE YOU SHOULD CALL UI AND SPAWN SOME NUMBERS I GUESS //
        ////////////////////////////////////////////////////////////
        //Also play some sounds I guess
        if (CombatManager.Instance.MostRecentAbility.CasterFrame2 != null)
        {
            StartCoroutine(FinishedAnimationDelay(casterBillboard, affectedCharacters));
        }
        else
        {
            StartCoroutine(Sprite1And2Delay(casterBillboard, affectedCharacters));
        }
    }
    private IEnumerator Sprite1And2Delay(GameObject casterBillboard, Dictionary<Character, GameObject> affectedCharacters)
    {
        yield return new WaitForSeconds(_delayBetweenFrames);
        StartCoroutine(PlaySprite2(casterBillboard, affectedCharacters));
    }
    private IEnumerator PlaySprite2(GameObject casterBillboard, Dictionary<Character, GameObject> affectedCharacters)
    {
        casterBillboard.GetComponentInChildren<SpriteRenderer>().sprite = CombatManager.Instance.MostRecentAbility.CasterFrame2;
        yield return new WaitForSeconds(_delayAfterAnimation);
        FinishAnimation(casterBillboard, affectedCharacters);
    }
    private IEnumerator FinishedAnimationDelay(GameObject casterBillboard, Dictionary<Character, GameObject> affectedCharacters)
    {
        yield return new WaitForSeconds(_delayAfterAnimation);
        FinishAnimation(casterBillboard, affectedCharacters);
    }
    private void FinishAnimation(GameObject casterBillboard, Dictionary<Character, GameObject> affectedCharacters)
    {
        Destroy(casterBillboard);
        foreach (var x in affectedCharacters)
        {
            Destroy(x.Value);
        }
        CameraManager.Instance.SwitchMainCamera();
        OnAnimationFinished.Invoke();
    }
}
