using Microsoft.Maui.Storage;
using Microsoft.Maui.Media;
using System.Text.Json;
using Microsoft.Maui.Controls;
namespace Assignment8;

public partial class ProfilePage : ContentPage
{
    private string profileFilePath = Path.Combine(FileSystem.AppDataDirectory, "profile.txt");
    private string profileImagePath;
    public ProfilePage()
    {
        InitializeComponent();
        LoadProfile();
    }

    private async void LoadProfile()
    {
        if (File.Exists(profileFilePath))
        {
            string json = await File.ReadAllTextAsync(profileFilePath);
            var profile = JsonSerializer.Deserialize<Profile>(json);

            if (profile != null)
            {
                NameEntry.Text = profile.Name;
                SurnameEntry.Text = profile.Surname;
                EmailEntry.Text = profile.Email;
                BioEntry.Text = profile.Bio;
                profileImagePath = profile.ProfilePicture;

                if (!string.IsNullOrEmpty(profileImagePath) && File.Exists(profileImagePath))
                {
                    ProfileImage.Source = ImageSource.FromFile(profileImagePath);
                }
            }
        }
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var profile = new Profile
        {
            Name = NameEntry.Text,
            Surname = SurnameEntry.Text,
            Email = EmailEntry.Text,
            Bio = BioEntry.Text,
            ProfilePicture = profileImagePath
        };

        string json = JsonSerializer.Serialize(profile);
        await File.WriteAllTextAsync(profileFilePath, json);
        await DisplayAlert("Success", "Profile saved successfully!", "OK");
    }

    private async void OnAddProfilePictureClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions { FileTypes = FilePickerFileType.Images });
        if (result != null)
        {
            profileImagePath = result.FullPath;
            ProfileImage.Source = ImageSource.FromFile(profileImagePath);
        }
    }

}