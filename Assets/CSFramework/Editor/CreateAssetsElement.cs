using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CSFramework.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Int32;

public class CreateAssetsElement : MonoBehaviour
{
	private const string BASE_TEMPLATE_PATH = "Assets/CSFramework/ScriptTemplates/";
	public static VisualElement Draw(PresettableCategory category)
	{
		var root = new VisualElement();
		var nameField = new TextField(
			maxLength: 25,
			multiline: false,
			isPasswordField: false,
			maskChar: '*',
			label: "Name")
		{
			value = "MyPresettable"
		};

		var extendedField = new TextField(
			maxLength: MaxValue,
			multiline: false,
			isPasswordField: false,
			maskChar: '*',
			label: "Extended")
		{
			value = "MyExtended"
		};

		root.Add(nameField);
		root.Add(extendedField);

		var buttons = new VisualElement
		{
			style =
			{
				flexDirection = FlexDirection.Row
			}
		};
		var extensionButton = new Button
		{
			text = "Create Extension",
		};
		extensionButton.clickable.clicked += () => GenerateExtensionAndPreset(nameField.value, extendedField.value, category);
		var presettableButton = new Button
		{
			text = "Create Presettable"
		};
		presettableButton.clickable.clicked += () => GeneratePresettableAndPreset(nameField.value,  category);

		SetCreateExtensionButtonEnable(extensionButton, nameField.value, extendedField.value);
		SetCreatePresettableButtonEnable(presettableButton, nameField.value);
		
		nameField.RegisterValueChangedCallback(evt =>
			OnNameFieldChanged(presettableButton, extensionButton, extendedField.value, evt));
		extendedField.RegisterValueChangedCallback(evt =>
			OnExtendedFieldChanged(extensionButton, nameField.value, evt));

		buttons.Add(extensionButton);
		buttons.Add(presettableButton);
		root.Add(buttons);

		return root;
	}

	private static void OnNameFieldChanged(
		Button presettableButton,
		Button extensionButton,
		string extended,
		ChangeEvent<string> evt)
	{
		SetCreatePresettableButtonEnable(presettableButton, evt.newValue);
		SetCreateExtensionButtonEnable(extensionButton, evt.newValue, extended);
	}

	private static void OnExtendedFieldChanged(Button extensionButton, string name, ChangeEvent<string> evt) =>
		SetCreateExtensionButtonEnable(extensionButton, name, evt.newValue);
	

	private static void SetCreatePresettableButtonEnable(
		Button presettableButton,
		string name)
	{
		var nameAlreadyExists = AppDomain.CurrentDomain
			.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Any(t => t.Name == name && t.Namespace is "CSFramework.Presettables");
		presettableButton.SetEnabled(
			name.Length > 0 &&
			char.IsLetter(name[0]) &&
			name.All(char.IsLetterOrDigit) &&
			!nameAlreadyExists
		);
	}

	private static void SetCreateExtensionButtonEnable(
		Button extensionButton, 
		string name,
		string extended)
	{
		var nameAlreadyExists = AppDomain.CurrentDomain
			.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Any(t => t.Name == name && t.Namespace is "CSFramework.Extensions");

		var extensionExists = AppDomain.CurrentDomain
			.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Any(t => t.Name == extended);

		extensionButton.SetEnabled(
			name.Length > 0 &&
			char.IsLetter(name[0]) &&
			name.All(char.IsLetterOrDigit) &&
			!nameAlreadyExists &&
			extended.Length > 0 &&
			char.IsLetter(extended[0]) &&
			extended.All(char.IsLetterOrDigit) &&
			extensionExists
		);
	}

	private static void GeneratePresettableAndPreset(string name, PresettableCategory category)
	{
		GeneratePresettable(name, category);
		GeneratePreset(name, category);
	}

	private static void GenerateExtensionAndPreset(string name, string extended, PresettableCategory category)
	{
		GenerateExtension(name,extended,category);
		GeneratePreset(name, category);
	}

	private static void GeneratePresettable(string name, PresettableCategory category)
	{
		GenerateFromTemplate(
			templatePath: BASE_TEMPLATE_PATH + "Presettable.txt",
			replacements: new Dictionary<string, string>()
			{
				{ "#NAME#", name },
				{ "#CATEGORY#", category.ToString() }
			},
			filePath: $"{Application.dataPath}/CSFramework/Scripts/{category}/{name}.cs"
		);
	}

	private static void GenerateExtension(string name, string extended, PresettableCategory category)
	{
		GenerateFromTemplate(
			templatePath: BASE_TEMPLATE_PATH + "Extension.txt",
			replacements: new Dictionary<string, string>()
			{
				{ "#NAME#", name },
				{ "#EXTENDED#", extended },
				{ "#EXTENDED_LOWER#", extended.ToLower() },
				{ "#CATEGORY#", category.ToString() }
			},
			filePath: $"{Application.dataPath}/CSFramework/Scripts/{category}/{name}.cs"
		);
	}
	
	private static void GeneratePreset(string name, PresettableCategory category)
	{
		GenerateFromTemplate(
			templatePath: BASE_TEMPLATE_PATH + "Preset.txt",
			replacements: new Dictionary<string, string>()
			{
				{ "#NAME#", name },
				{ "#CATEGORY#", category.ToString() }
			},
			filePath: $"{Application.dataPath}/CSFramework/Scripts/{category}/{name}Preset.cs"
		);
	}
	
	private static void GenerateFromTemplate(
		string templatePath,
		Dictionary<String, String> replacements,
		string filePath
	)
	{
		//Loading the template text file which has some code already in it.
		TextAsset templateTextFile = AssetDatabase.LoadAssetAtPath(
			templatePath,
			typeof(TextAsset)
		) as TextAsset;
		
		var contents = "";
		if (templateTextFile != null)
		{
			contents = templateTextFile.text;
			foreach (var (key, value) in replacements)
			{
				contents = contents.Replace(key, value);
			}
		}
		else
		{
			Debug.LogError($"Can't find the script template file! Is it at the path '{templatePath}'?");
		}

		if (File.Exists(filePath))
		{
			Debug.LogError($"File already existing at path '{filePath}', can't overwrite it.");
		}
		else
		{
			// Create extension
			using (StreamWriter sw = new StreamWriter(filePath))
			{
				sw.Write(contents);
			}

			//Refresh the Asset Database
			AssetDatabase.Refresh();
			Debug.Log($"File successfully created at path: {filePath}");
		}
	}
}
