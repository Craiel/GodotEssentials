namespace Craiel.Essentials.Extensions;

using Godot;

public static class LabelExtensions
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void ClearTextAndHide(this Label label)
    {
        label.Text = string.Empty;
        label.Hide();
    }
    
    public static void ClearTextAndHide(this RichTextLabel label)
    {
        label.Text = string.Empty;
        label.Hide();
    }

    public static void SetTextAndShow(this Label label, string text)
    {
        label.Text = text;
        label.Show();
    }
    
    public static void SetTextAndShow(this RichTextLabel label, string text)
    {
        label.Text = text;
        label.Show();
    }
}