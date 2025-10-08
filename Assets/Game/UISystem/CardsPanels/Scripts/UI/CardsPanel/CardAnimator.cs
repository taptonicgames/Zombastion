using Coffee.UIExtensions;
using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardAnimator
{
    private Card _card;
    private UIParticle _uIParticle;
    private Sequence _sequence;
    private float _rotateDuration = 1f;
    private float _moveDuration = 0.4f;
    private float _modifierScale = 1.25f;
    private int _stepCount = 4;

    public CardAnimator(Card card, UIParticle uIParticle)
    {
        _card = card;
        _uIParticle = uIParticle;

        PlayStopCheeoseEffect(false);
    }

    public void PlayPickAnimation(Action callback, float delay = 0.0f)
    {
        _sequence.Kill();
        _sequence = DOTween.Sequence()
            .SetUpdate(true)
            .AppendInterval(delay)
            .Append(_card.transform
                .DOScale(Vector3.one * _modifierScale, _moveDuration * 0.5f)
                .SetEase(Ease.Linear))
            .AppendCallback(() => PlayStopCheeoseEffect(true))
            .Append(_card.transform.DOScale(Vector3.zero, _moveDuration)
            .SetEase(Ease.Linear))
            .OnComplete(() =>
            {
                PlayStopCheeoseEffect(false);
                callback();
            });
    }

    private void PlayStopCheeoseEffect(bool isPlay)
    {
        _uIParticle.gameObject.SetActive(isPlay);

        //if (isPlay)
        //    SoundManager.Instance.PlaySound(_effectClip, false, 0.5f);
    }

    public void PlayRotateAroundAnimation(Image back, Transform cardView, RawImage rawImage, Action callback, float delay = 0.0f)
    {
        _sequence.Kill();
        _sequence = DOTween.Sequence();
        _sequence.SetUpdate(true);
        _sequence.SetEase(Ease.Linear);

        _sequence.AppendInterval(delay);
        _sequence.Append(cardView.DOLocalRotate(new Vector3(0, 90, 0), _rotateDuration / _stepCount));
        _sequence.Join(rawImage.transform.DOScale(Vector3.zero, _rotateDuration / _stepCount));
        _sequence.AppendCallback(() => back.gameObject.SetActive(true));
        _sequence.Append(cardView.DOLocalRotate(new Vector3(0, 180, 0), _rotateDuration / _stepCount));
        _sequence.AppendInterval(0.1f);
        _sequence.Append(cardView.DOLocalRotate(new Vector3(0, 270, 0), _rotateDuration / _stepCount));
        _sequence.AppendCallback(() => callback());
        _sequence.AppendCallback(() => back.gameObject.SetActive(false));
        _sequence.Append(cardView.DOLocalRotate(new Vector3(0, 360, 0), _rotateDuration / _stepCount));
        _sequence.Join(rawImage.transform.DOScale(Vector3.one, _rotateDuration / _stepCount));
    }
}