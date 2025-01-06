namespace Craiel.Essentials.Data;

using System.Linq;
using System.Text.RegularExpressions;
using Collections;
using Extensions;
using Godot;
using Array = System.Array;
using Colors = Godot.Colors;

public static class DataTextUtils
{
    private const string VariableRegexString = @"\{(\w+)\}";
    private static readonly Regex VariableRegex = new(VariableRegexString, RegexOptions.Compiled);
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static DataTextVariable[] EvaluateVariables(string value)
    {
        var matches = VariableRegex.Matches(value);
        if (matches.Count == 0)
        {
            return Array.Empty<DataTextVariable>();
        }

        var variables = TempList<DataTextVariable>.Allocate();
        foreach (Match match in matches)
        {
            variables.Add(new DataTextVariable
            {
                Name = match.Groups[1].Value,
                Index = match.Index,
                Length = match.Length
            });
        }
        
        return variables.OrderByDescending(x => x.Index).ToArray();
    }

    public static string Format(this DataText text, params DataTextVariableFormatInstruction[] instructions)
    {
        if (text.Variables.IsNullOrEmpty())
        {
            return text.Value;
        }

        string result = text.Value;
        for (var iV = 0; iV < text.Variables.Length; iV++)
        {
            var variable = text.Variables[iV];
            int instructionIndex = -1;
            for (var iI = 0; iI < instructions.Length; iI++)
            {
                if (instructions[iI].Name == variable.Name)
                {
                    instructionIndex = iI;
                    break;
                }
            }

            if (instructionIndex < 0)
            {
                // No instruction for this variable, apply the default "error" style
                result = ApplyInstruction(result, variable, new()
                {
                    Value = "#VAR_" + variable.Name.ToUpperInvariant(),
                    Type = DataTextVariableFormatType.Colored,
                    Color = Colors.Magenta
                });
                
                continue;
            }

            result = ApplyInstruction(result, variable, instructions[instructionIndex]);
        }
        
        return result;
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static string ApplyInstruction(string text, DataTextVariable variable, DataTextVariableFormatInstruction instruction)
    {
        var result = text.Substring(0, variable.Index);
        switch (instruction.Type)
        {
            case DataTextVariableFormatType.Colored:
            {
                result += $"[color={instruction.Color.ToHtml()}]{instruction.Value}[/color]";
                break;
            }

            default:
            {
                result += instruction.Value;
                break;
            }
        }
        
        result += text.Substring(variable.Index + variable.Length);
        return result;
    }
}