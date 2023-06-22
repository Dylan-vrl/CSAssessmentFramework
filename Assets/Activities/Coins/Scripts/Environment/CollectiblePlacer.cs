using System;
using System.Collections.Generic;
using UnityEngine;
using CSFramework.Core;
using CSFramework.Presets;
using Gameplay;
using PathCreation;

namespace CSFramework.Presettables
{
    public class CollectiblePlacer : Extension<PathCreator, CollectiblePlacerPreset>
    {
        [SerializeField] private PathCreator pathCreator;
        [SerializeField] private GameObject holder;

        private const float MinSpacing = 1f;
        private float _currentDist;
        private Queue<Collectible> _collectibles = new();

        private float _spacing;
        
        public override PresettableCategory GetCategory() => PresettableCategory.Environment;

        private void OnEnable()
        {
            GameStateManager.GameStarted += OnGameStart;
        }

        private void OnGameStart()
        {
            CreatePath();
        }

        private void Start()
        {
            CreatePath();
        }

        /// <summary>
        /// Creates and places the first visible collectibles on path, populating <see cref="_collectibles"/>,
        /// and starting to listen each coin's <see cref="Collectible.OnPickedUp"/>.
        /// </summary>
        private void CreatePath()
        {
            // Clearing possible existing path
            foreach (Collectible col in _collectibles)
            {
                col.OnPickedUp -= OnCollectiblePickup;
                Destroy(col.gameObject);
            }
            _collectibles.Clear();
            
            _spacing = Mathf.Max(MinSpacing, Preset.Spacing);
            //place the visible coins
            if (pathCreator != null && Preset.Collectible != null && holder != null)
            {
                for (var i = 0; i < Preset.NumberOfVisibleCollectibles; i++)
                {
                    Vector3 point = pathCreator.path.GetPointAtDistance(i*_spacing, EndOfPathInstruction.Reverse);
                    Collectible instCol = Instantiate(Preset.Collectible, point, Quaternion.identity, holder.transform);
                    if (i == 0)
                    {
                        instCol.SetInteractable(true);
                    }
                    else
                    {
                        instCol.SetInteractable(false);
                    }
                    instCol.OnPickedUp += OnCollectiblePickup;
                    _collectibles.Enqueue(instCol);
                }

                
                _currentDist = Preset.NumberOfVisibleCollectibles * _spacing;
            }
            else
            {
                Debug.LogWarning("Collectible Placement not possible as setup is not complete.");
            }
        }

        /// <summary>
        /// Places the <see cref="Collectible"/> <paramref name="sender"/> to next point in line.
        /// </summary>
        private void OnCollectiblePickup(object sender, EventArgs e)
        {
            Collectible pickedCollectible = _collectibles.Dequeue();
            
            Vector3 point = pathCreator.path.GetPointAtDistance(_currentDist, EndOfPathInstruction.Reverse);
            // Rotating is also possible, but not needed in our case
            //Quaternion rot = pathCreator.path.GetRotationAtDistance(_currentDist);
            Transform collectibleTransform = pickedCollectible.transform;
            collectibleTransform.position = point;
            //collectibleTransform.rotation = rot;
            
            pickedCollectible.ReEnable();
            _collectibles.Enqueue(pickedCollectible);
            
            _collectibles.Peek().SetInteractable(true);
            _currentDist += _spacing;
        }

        private void OnDisable()
        {
            foreach (Collectible col in _collectibles)
            {
                col.OnPickedUp -= OnCollectiblePickup;
            }

            GameStateManager.GameStarted -= OnGameStart;
        }
    }
}