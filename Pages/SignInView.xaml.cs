namespace menu.Pages;

public partial class SignInView : ContentPage
{
    public SignInView(SignInViewModel Viewmodel)
    {
        InitializeComponent();
        BindingContext = Viewmodel;
        if (BindingContext is SignInViewModel vm)
        {
            vm.ResetFields();
        }
    }
}