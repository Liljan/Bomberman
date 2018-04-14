using UnityEngine;
using UnityEditor;

using System;
using System.IO;

class InputAction
{
    public int id;
    public string name;
    string negativeButton;
    string positiveButton;

    public int axis;

    public InputAction(int id, string name, string negativeButton, string positiveButton, int axis)
    {
        this.id = id;
        this.name = name;
        this.negativeButton = negativeButton;
        this.positiveButton = positiveButton;
        this.axis = axis;
    }

    public string ToString()
    {
        string s = "";

        s += "  - serializedVersion: 3\n";
        s += "    m_Name: " + name + "_" + id + "\n";
        s += "    descriptiveName: \n";
        s += "    descriptiveNegativeName: \n";
        s += "    negativeButton: \n";
        s += "    positiveButton: " + id + "\n";
        s += "    altNegativeButton: \n";
        s += "    altPositiveButton: \n";
        s += "    gravity: " + 1000 + "\n";
        s += "    dead: " + 0.001f + "\n";
        s += "    sensitivity: " + 1000 + "\n";
        s += "    snap: " + 0 + "\n";
        s += "    invert: " + 0 + "\n";
        s += "    type: " + 0 + "\n";
        s += "    axis: " + axis + "\n";
        s += "    joyNum: " + id + "\n";

        return s;
    }
}

class InputMappingCSGenerator
{
    public int amount;

    private string fields;

    public InputMappingCSGenerator(int amount)
    {
        this.amount = amount;
        fields = "";
    }

    public void AddField(string name)
    {
        fields += "public static string[] ";
        fields += name;
        fields += "= { ";

        for (int i = 1; i <= amount-1; i++)
        {
            fields += "\"" + name + "_" + i + "\", ";
        }

        fields += "\"" + name + "_" + amount + "\" };";
        fields += "\n";
    }

    public void AddLineBreak()
    {
        fields += "\n";
    }

    public string GetString(string fileName)
    {
        string result = "";

        result += "public static class " + fileName + "\n";
        result += "{\n";

        result += fields;

        result += "}\n\n";

        return result;
    }
}

public class JoystickInputGenerator : EditorWindow
{
    private string _fileName = "InputManager.asset";
    private int _numerOfContollers = 1;

    [MenuItem("Window/Generate Joystick Input")]
    public static void ShowWindow()
    {
        GetWindow<JoystickInputGenerator>("Generate Joystick Input");
    }

    public void OnGUI()
    {
        GUILayout.Label("File name", EditorStyles.boldLabel);

        _fileName = EditorGUILayout.TextField("File name", _fileName);

        GUILayout.Label("Joystick Input Generator", EditorStyles.boldLabel);

        _numerOfContollers = EditorGUILayout.IntField("Amount of players", _numerOfContollers);

        if (GUILayout.Button("Generate Input file"))
        {
           /* if (File.Exists(_fileName))
            {
                Debug.Log("Error: File already exists.");
                return;
            }
            */

            StreamWriter sr = File.CreateText(_fileName);
            sr.WriteLine("%YAML 1.1");
            sr.WriteLine("%TAG !u! tag:unity3d.com,2011:");
            sr.WriteLine("--- !u!13 &1");
            sr.WriteLine("InputManager:");
            sr.WriteLine("  m_ObjectHideFlags: 0");
            sr.WriteLine("  serializedVersion: 2");
            sr.WriteLine("  m_Axes:");

            for (int i = 0; i < _numerOfContollers; i++)
            {
                InputAction Jump = new InputAction(i + 1, "Jump", "", "1", 0);
                sr.Write(Jump.ToString());
            }

            sr.Close();

            StreamWriter cssr = File.CreateText("InputMappz.cs");
            InputMappingCSGenerator csgen = new InputMappingCSGenerator(_numerOfContollers);

            csgen.AddField("Jump");
            csgen.AddLineBreak();

            cssr.Write( csgen.GetString("InputMappz") );
            cssr.Close();
        }
    }
}
