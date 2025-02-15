using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiLoginApp.ViewModels;

public class LoginPageViewModel : INotifyPropertyChanged
{
    private string _username;
    public string Username
    {
        get { return _username; }
        set
        {
            if (_username != value)
            {
                _username = value;
                OnPropertyChanged();
            }
        }
    }
    private string _password;
    public string Password
    {
        get { return _password; }
        set
        {
            if (_password != value)
            {
                _password = value;
                OnPropertyChanged();
            }
        }
    }
    public ICommand LoginCommand { get; }
    public LoginPageViewModel()
    {
        LoginCommand = new Command(async () => await ExecuteLoginCommand());
    }
    private async Task ExecuteLoginCommand()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "Ok");
            return;
        }
        // Addional validation code omitted
        // ...
        var dto = new LoginDto
        {
            Username = Username,
            Password = Password
        };
        var httpClient = new HttpClient();
        var result = await httpClient.PostAsJsonAsync("https://mydomain.com/login", dto);
        if (!result.IsSuccessStatusCode)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "Ok");
            return;
        }
        await Shell.Current.GoToAsync("homepage");
    }
    // Implement INotifyPropertyChanged interface
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}