using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiLoginApp.ViewModels;

public class LoginPageViewModel : INotifyPropertyChanged
{
    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public LoginPageViewModel()
    {
        LoginCommand = new Command(async () => await ExecuteLoginCommand());
    }

    #endregion Constructor

    #region Events

    // Implement INotifyPropertyChanged interface
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Events

    #region Observable Properties

    /// <summary>
    /// Username
    /// </summary>
    public string Username
    {
        get => _username;
        set
        {
            if (_username != value)
            {
                _username = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Password
    /// </summary>
    public string Password
    {
        get => _password;
        set
        {
            if (_password != value)
            {
                _password = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion Observable Properties

    #region Private Member Variables

    private string _username;

    private string _password;

    #endregion Private Member Variables

    #region Commands
    public ICommand LoginCommand { get; }

    #endregion Commands

    #region Command Functions
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

    #endregion Command Functions

    #region Protected Functions

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion Protected Functions
}

class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}