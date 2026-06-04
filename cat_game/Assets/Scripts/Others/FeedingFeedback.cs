using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace cats
{
    public class FeedingFeedback : MonoBehaviour
    {
        [SerializeField] private AudioClip _feedSound;
        [SerializeField] private Sprite[] _particleSprites;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private float _particleSize = 60f;
        [SerializeField] private float _floatDistance = 80f;
        [SerializeField] private float _popDuration = 0.4f;
        [SerializeField] private float _holdDuration = 0.3f;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _delayBetween = 0.12f;
        [SerializeField] private float _overshoot = 1.25f;

        public void Play(FoodItem food)
        {
            if (_feedSound != null)
                AudioManager.Instance.PlaySfx(_feedSound);

            StartCoroutine(SpawnSequence());
        }

        private IEnumerator SpawnSequence()
        {
            if (_canvas == null || _spawnPoints == null || _spawnPoints.Length == 0)
                yield break;

            Camera cam = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : _canvas.worldCamera;

            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                if (_spawnPoints[i] == null) continue;

                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, _spawnPoints[i].position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)_canvas.transform, screenPos, cam, out Vector2 localPos);

                StartCoroutine(AnimateParticle(localPos));
                yield return new WaitForSeconds(_delayBetween);
            }
        }

        private IEnumerator AnimateParticle(Vector2 spawnPos)
        {
            GameObject go = new GameObject("Particle");
            go.transform.SetParent(_canvas.transform, false);
            go.transform.SetAsLastSibling();

            RectTransform rect = go.AddComponent<RectTransform>();
            rect.sizeDelta = Vector2.one * _particleSize;
            rect.anchoredPosition = spawnPos;
            rect.localScale = Vector3.zero;

            Image img = go.AddComponent<Image>();
            img.raycastTarget = false;
            if (_particleSprites != null && _particleSprites.Length > 0)
                img.sprite = _particleSprites[Random.Range(0, _particleSprites.Length)];

            CanvasGroup cg = go.AddComponent<CanvasGroup>();
            cg.alpha = 1f;

            yield return StartCoroutine(PopIn(rect));
            yield return StartCoroutine(FloatAndFade(rect, cg, spawnPos));

            Destroy(go);
        }

        private IEnumerator PopIn(RectTransform rect)
        {
            float elapsed = 0f;
            while (elapsed < _popDuration)
            {
                float t = elapsed / _popDuration;
                rect.localScale = Vector3.one * SpringEase(t, _overshoot);
                elapsed += Time.deltaTime;
                yield return null;
            }
            rect.localScale = Vector3.one;
        }

        private IEnumerator FloatAndFade(RectTransform rect, CanvasGroup cg, Vector2 from)
        {
            yield return new WaitForSeconds(_holdDuration);

            float elapsed = 0f;
            Vector2 to = from + Vector2.up * _floatDistance;

            while (elapsed < _fadeDuration)
            {
                float t = elapsed / _fadeDuration;
                rect.anchoredPosition = Vector2.Lerp(from, to, t);
                rect.localScale = Vector3.one * Mathf.Lerp(1f, 0.6f, t);
                cg.alpha = Mathf.Lerp(1f, 0f, t);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        private float SpringEase(float t, float overshoot)
        {
            return 1f + (overshoot - 1f) * Mathf.Sin(t * Mathf.PI) * Mathf.Exp(-t * 3f);
        }
    }
}
