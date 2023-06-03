using System.Collections.Generic;
using System.Linq;
using CSFramework.Core;
using CSFramework.Presets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionHandler : PresettableMonoBehaviour<LocomotionHandlerPreset>
{
    [SerializeField] private Transform controllersParent;

    private List<ICustomLocomotionProvider> leftCustomLocomotionProviders = new();
    private List<ICustomLocomotionProvider> rightCustomLocomotionProviders = new();

    public static Dictionary<InputActionReference, bool> ActionRefStates { get; } = new();
    
    private void Awake()
    {
        UpdateProviders(
            Preset.LeftActiveLocomotionProviders,
            Preset.RightActiveLocomotionProviders
        );
        
        SpawnControllers();
    }
    
    public void UpdateProviders(
        List<LocomotionProvider> leftProviders,
        List<LocomotionProvider> rightProviders
    )
    {
        CleanAllProviders();
        (leftProviders, rightProviders) = ValidateActiveProviders(leftProviders, rightProviders);
        SpawnAllProviders(leftProviders, rightProviders);

        InitCustomProviders(leftProviders, rightProviders);
        UpdateActionRefs();
    }
    
    private static void CleanAllProviders()
    {
        foreach (var provider in FindObjectsOfType<LocomotionProvider>())
        {
            Destroy(provider.gameObject);
        }
    }
    
    private (List<LocomotionProvider>, List<LocomotionProvider>) ValidateActiveProviders(
        List<LocomotionProvider> leftProviders,
        List<LocomotionProvider> rightProviders
    )
    {
        leftProviders = leftProviders.Where(provider =>
            {
                var valid = provider != null && provider.TryGetComponent(out ICustomLocomotionProvider _);
                if (!valid)
                {
                    Debug.LogWarning($"{nameof(LocomotionHandler)}: the {nameof(LocomotionProvider)} {(provider == null ? "null" : provider.name)} has been removed from the " +
                                     $"left hand list because it doesn't extend {nameof(ICustomLocomotionProvider)}.");
                }
                return valid;
            }
        ).ToList();
        
        rightProviders = rightProviders.Where(provider =>
            {
                var valid = provider != null && provider.TryGetComponent(out ICustomLocomotionProvider _);
                if (!valid)
                {
                    Debug.LogWarning($"{nameof(LocomotionHandler)}: the {nameof(LocomotionProvider)} {(provider == null ? "null" : provider.name)} has been removed from the " +
                                     $"right hand list because it doesn't extend {nameof(ICustomLocomotionProvider)}.");
                }
                return valid;
            }
        ).ToList();

        return (leftProviders, rightProviders);
    }
    
    private void SpawnAllProviders(
        IEnumerable<LocomotionProvider> leftProviders,
        IEnumerable<LocomotionProvider> rightProviders
    )
    {
        foreach (var provider in leftProviders.Union(rightProviders))
        {
            Instantiate(provider, transform);
        }
    }

    private void InitCustomProviders(
        IEnumerable<LocomotionProvider> leftProviders,
        IEnumerable<LocomotionProvider> rightProviders
    )
    {
        leftCustomLocomotionProviders = leftProviders
            .Select(provider => provider.GetComponent<ICustomLocomotionProvider>())
            .ToList();
        
        rightCustomLocomotionProviders = rightProviders
            .Select(provider => provider.GetComponent<ICustomLocomotionProvider>())
            .ToList();
    }

    private void UpdateActionRefs()
    {
        // Disable all action references
        FindObjectsOfType<LocomotionProvider>()
            .OfType<ICustomLocomotionProvider>()
            .ToList()
            .ForEach(provider => provider.DisableActions());
        
        // (Re-)enable active actions references for each hand
        leftCustomLocomotionProviders.ForEach(provider => provider.EnableLeftActions());
        rightCustomLocomotionProviders.ForEach(provider => provider.EnableRightActions());
    }
    
    private void SpawnControllers()
    {
        Instantiate(Preset.LeftControllerPrefab, controllersParent);
        Instantiate(Preset.RightControllerPrefab, controllersParent);
    }

    public override PresettableCategory GetCategory() => PresettableCategory.Locomotion;
}
