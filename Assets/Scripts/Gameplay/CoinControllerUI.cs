using CSFramework.Presettables;
using TMPro;
using UnityEngine;
using static GameStateManager.GameState;

namespace Gameplay
{
    public class CoinControllerUI : MonoBehaviour
    {
        [Header("Coins Experiment Control options")]
        [SerializeField] private TextMeshProUGUI collectiblesLabel;
        [SerializeField] private TMP_InputField collectiblesInput;

        private CoinsController _coinsController;

        private void OnEnable()
        {
            GameStateManager.GameStateChanged += OnGameStateChanged;
        }

        private void Start()
        {
            _coinsController = CoinsController.Instance;

            if (collectiblesLabel == null || collectiblesInput == null) return;
            
            var c = _coinsController.NumberOfCollectiblesToPickUp > 0 ? _coinsController.NumberOfCollectiblesToPickUp.ToString() : "∞";
            collectiblesInput.text = c;
            collectiblesLabel.text = "0 / ";
        }

        private void Update()
        {
            if (GameStateManager.State == Playing)
            {
                if (collectiblesLabel != null)
                {
                    collectiblesLabel.text = _coinsController.PickedUpCollectibles.ToString() + " / ";
                }
            }
            
        }

        public void CollectiblesInputChange(string input)
        {
            if (int.TryParse(input, out var i)) _coinsController.NumberOfCollectiblesToPickUp = i;
        }
        
        private void OnGameStateChanged(GameStateManager.GameState state)
        {
            switch (state)
            {
                case Menu:
                    collectiblesInput.interactable = true;
                    collectiblesLabel.text = (_coinsController != null ? _coinsController.PickedUpCollectibles.ToString() : 0) + " / ";
                    break;
                
                case Playing:
                    collectiblesInput.interactable = false;
                    var c = _coinsController.NumberOfCollectiblesToPickUp > 0 ? _coinsController.NumberOfCollectiblesToPickUp.ToString() : "∞";
                    collectiblesInput.text = c;
                    break;
            }
        }
        
        private void OnDisable()
        {
            GameStateManager.GameStateChanged -= OnGameStateChanged;
        }
    }
}