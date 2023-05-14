using System.Collections.Generic;
using System.Linq;
using CSFramework.Core;
using CSFramework.Presets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionHandler2 : PresettableMonoBehaviour<LocomotionHandlerPreset>
{
    private List<LocomotionProvider> leftActiveLocomotionProviders = new();
    private List<LocomotionProvider> rightActiveLocomotionProviders = new();

    private List<ICustomLocomotionProvider> leftCustomLocomotionProviders = new();
    private List<ICustomLocomotionProvider> rightCustomLocomotionProviders = new();

    private void Start()
    {
        leftActiveLocomotionProviders = Preset.LeftActiveLocomotionProviders;
        rightActiveLocomotionProviders = Preset.RightActiveLocomotionProviders;

        CleanAllProviders();
        ValidateActiveProviders();
        SpawnAllActiveProviders();
        
        InitCustomProviders();
        UpdateActionRefs();
    }

    private static void CleanAllProviders()
    {
        foreach (var provider in FindObjectsOfType<LocomotionProvider>())
        {
            Destroy(provider.gameObject);
        }
    }
    
    private void SpawnAllActiveProviders()
    {
        foreach (var leftProvider in leftActiveLocomotionProviders)
        {
            Instantiate(leftProvider, transform);
        }
        
        foreach (var rightProvider in rightActiveLocomotionProviders)
        {
            Instantiate(rightProvider, transform);
        }
    }

    private void ValidateActiveProviders()
    {
        leftActiveLocomotionProviders = leftActiveLocomotionProviders.Where(provider =>
            {
                var valid = provider != null && provider.TryGetComponent(out ICustomLocomotionProvider _);
                if (!valid)
                {
                    Debug.LogWarning($"{nameof(LocomotionHandler2)}: a {nameof(LocomotionProvider)} has been removed from the " +
                                     $"left hand list because it doesn't extend {nameof(ICustomLocomotionProvider)}.");
                }
                return valid;
            }
        ).ToList();
        
        rightActiveLocomotionProviders = rightActiveLocomotionProviders.Where(provider =>
            {
                var valid = provider != null && provider.TryGetComponent(out ICustomLocomotionProvider _);
                if (!valid)
                {
                    Debug.LogWarning($"{nameof(LocomotionHandler2)}: a {nameof(LocomotionProvider)} has been removed from the " +
                                     $"right hand list because it doesn't extend {nameof(ICustomLocomotionProvider)}.");
                }
                return valid;
            }
        ).ToList();
    }

    private void InitCustomProviders()
    {
        leftCustomLocomotionProviders = leftActiveLocomotionProviders
            .Select(provider => provider.GetComponent<ICustomLocomotionProvider>())
            .ToList();
        
        rightCustomLocomotionProviders = rightActiveLocomotionProviders
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

    public override PresettableCategory GetCategory() => PresettableCategory.Locomotion;
}
