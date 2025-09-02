using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace menu;

public static class PopupExtensions 
{
    public static Task<bool?> ShowPopupAsync(this Page page, ContentPage popup)
    {
        var tcs = new TaskCompletionSource<bool?>();

        var doneButton = new Button { Text = "Valider" };
        var cancelButton = new Button { Text = "Annuler" };

        doneButton.Clicked += (s, e) =>
        {
            page.Navigation.PopModalAsync();
            tcs.SetResult(true);
        };

        cancelButton.Clicked += (s, e) =>
        {
            page.Navigation.PopModalAsync();
            tcs.SetResult(false);
        };

        var buttons = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 20,
            Children = { doneButton, cancelButton }
        };

        ((StackLayout)popup.Content).Children.Add(buttons);

        page.Navigation.PushModalAsync(popup);
        return tcs.Task;
    }

}