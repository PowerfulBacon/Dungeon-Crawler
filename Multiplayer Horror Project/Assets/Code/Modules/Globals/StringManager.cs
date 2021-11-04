using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

public static class StringManager
{

    private static StackFrame stackFrame = new StackFrame(1);

    public static string ProcessString(string input = "")
    {
        MethodBase caller = stackFrame.GetMethod();

        string output = "";
        string input_to_process = input;

        var bindingFlags = BindingFlags.Instance |
                   BindingFlags.NonPublic |
                   BindingFlags.Public;

        //Get vars
        UnityEngine.Debug.Log(caller.ReflectedType.Name);
        List<string> fieldNames = caller.ReflectedType
                .GetFields(bindingFlags)
                .Select(f => f.Name)
                .ToList();
        List<object> fieldValues = caller.ReflectedType
                .GetFields(bindingFlags)
                .Select(f => f.GetValue(caller))
                .ToList();

        for (int i = 0; i < fieldNames.Count; i ++)
        {
            UnityEngine.Debug.Log("Object: " + fieldNames[i] + "value : " + fieldValues[i]);
        }

        int sanity = 100;
        
        while (sanity > 0)
        {
            //Make sure we don't loop forever
            sanity--;
            //Put the text before hand in
            if (input_to_process.IndexOf("[") != -1 && input_to_process.IndexOf("]") != -1)
            {
                output += input_to_process.Substring(0, input_to_process.IndexOf("["));
                string vars_to_process = input_to_process.Substring(input_to_process.IndexOf("["), input_to_process.IndexOf("]") - input_to_process.IndexOf("["));
                //Replace var inside [] with value of var
                
                //Remove processed part of text
                input_to_process = input_to_process.Remove(0, input_to_process.IndexOf("]") + 1);
            }
            else
            {
                output += input_to_process;
                break;
            }
        }
        return output;
    }

}
