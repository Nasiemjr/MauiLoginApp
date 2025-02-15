using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace MauiLoginApp.ViewModels;

public class LoginPageViewModel : INotifyPropertyChanged
{
    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public LoginPageViewModel()
    {
        LoginCommand = new RelayCommand(async () => await ExecuteLoginCommand(), CanExecuteLoginCommand);
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
                LoginCommand.NotifyCanExecuteChanged();
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
                LoginCommand.NotifyCanExecuteChanged();
            }
        }
    }

    #endregion Observable Properties

    #region Private Member Variables

    private string _username;

    private string _password;

    #endregion Private Member Variables

    #region Commands
    public RelayCommand LoginCommand { get; private set; }

    #endregion Commands

    #region Command Functions

    /// <summary>
    /// Login the user
    /// </summary>
    private async Task ExecuteLoginCommand()
    {
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

    /// <summary>
    /// Can function for ExecuteLoginCommand
    /// </summary>
    private bool CanExecuteLoginCommand()
    {
        // If the username is empty
        if(string.IsNullOrWhiteSpace(Username))
        {
            return false;
        }

        // If the password is empty
        if (string.IsNullOrWhiteSpace(Password))
        {
            return false;
        }

        return true;
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