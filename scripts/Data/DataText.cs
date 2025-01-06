namespace Craiel.Essentials.Data;

public struct DataText
{
    public DataText(string value)
    {
        this.Value = value;
        this.Variables = DataTextUtils.EvaluateVariables(value);
    }
    
    public readonly string Value;
    public DataTextVariable[] Variables;
}