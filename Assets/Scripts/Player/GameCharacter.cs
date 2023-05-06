using System;
using System.Collections;
using CSFramework.Presettables;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static GameStateManager.GameState;

namespace Player
{
    /// <summary>
    /// Character manager, handling respawn, fade to black,
    /// and telling <see cref="ExperimentController"/> who <see cref="ExperimentController.XROrigin"/> is.
    /// </summary>
    public class GameCharacter: MonoBehaviour
    {
        
        [SerializeField] protected Volume darkVolume;

        public enum SpawnPoint
        {
            AtSpawn,
            AtPosition,
            AtPath
        }

        protected Terrain _activeTerrain;
        protected Vector3 _spawnPos = Vector3.zero;
        protected Vignette _vignette;


        protected virtual void OnEnable()
        {
            GameStateManager.GameStarted += OnGameStart;
            GameStateManager.GameEnded += OnGameEnd;
        }

        protected virtual void Start()
        {
            if (darkVolume == null)
            {
                Debug.LogWarning("No Post Processing set");
                return;
            }
            if (!darkVolume.profile.TryGet(out _vignette))
            {
                Debug.LogWarning("No vignette found");
                return;
            }
            _vignette.intensity.overrideState = true;
            _vignette.active = true;
            _vignette.intensity.value = 1;
            
            _activeTerrain = Terrain.activeTerrain;
            Respawn(SpawnPoint.AtPosition);
            _spawnPos = transform.position;
            CoinsController.Instance.LastCollectiblePos = _spawnPos;
            // When loading a new scene, this updates the reference correctly.
            ExperimentController.Instance.XROrigin = GetComponent<XROrigin>();
        }

        protected virtual void OnGameStart()
        {
            Respawn();
        }

        protected virtual  void OnGameEnd()
        {
            StartCoroutine(ChangeVignette(true, 2));
        }

        public virtual void Respawn(SpawnPoint spawnPoint = SpawnPoint.AtSpawn)
        {
            if (GameStateManager.State == Playing)
            {
                // doesn't work it seems
                StartCoroutine(ChangeVignette(false,2));
            }
            
            if (_activeTerrain != null)
            {
                Vector3 pos;
                switch (spawnPoint)
                {
                    case SpawnPoint.AtPath:
                        pos = CoinsController.Instance.LastCollectiblePos;
                        break;
                    case SpawnPoint.AtPosition:
                        pos = transform.position;
                        break;
                    case SpawnPoint.AtSpawn:
                        pos = _spawnPos;
                        break;
                    default:
                        pos = Vector3.zero;
                        break;
                }
                pos.y = _activeTerrain.SampleHeight(pos) + _activeTerrain.transform.position.y + 0.2f;
                transform.position = pos;
            }
        }


        /// <summary>
        /// Make the vignette fade down or up depending on <paramref name="toBlack"/>.
        /// </summary>
        /// <param name="toBlack">whether to fade down or up.</param>
        /// <param name="fadeSpeed">Speed in seconds for fade.</param>
        private IEnumerator ChangeVignette(bool toBlack, float fadeSpeed)
        {
            if (_vignette == null)
            {
                yield break;
            }
            _vignette.active = true;
            var fadeTo = toBlack ? 1 : 0;
            float fadeAmount;
            while (Math.Abs(_vignette.intensity.value - fadeTo) > 0.05f)
            {
                if (toBlack)
                {
                    fadeAmount = Mathf.Min(1, _vignette.intensity.value + (1/fadeSpeed * Time.deltaTime));
                }
                else
                {
                    fadeAmount = Mathf.Max(0, _vignette.intensity.value - (1/fadeSpeed * Time.deltaTime));
                }
                _vignette.intensity.value = fadeAmount;
                yield return null;
            }

            _vignette.intensity.value = fadeTo;
            if (! toBlack)
            {
                _vignette.active = false;
            }
        }
        

        protected virtual  void OnDisable()
        {
            GameStateManager.GameStarted -= OnGameStart;
            GameStateManager.GameEnded -= OnGameEnd;
        }
    }
}