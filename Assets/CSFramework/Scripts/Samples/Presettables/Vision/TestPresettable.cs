using System.Collections;
using System.Collections.Generic;
using CSFramework.Core;
using CSFramework.Samples;
using UnityEngine;

public class TestPresettable : PresettableMonoBehaviour<TestPreset>
{
    public override PresettableCategory Category => PresettableCategory.Vision;
}
