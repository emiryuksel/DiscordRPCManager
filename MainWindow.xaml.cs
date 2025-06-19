using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Drawing;
using Forms = System.Windows.Forms; // Forms için alias
using WPF = System.Windows;         // WPF için alias
using DiscordRPC;
using DiscordRPC.Logging;
using DiscordRPCManager.Models;

namespace DiscordRPCManager
{
    public partial class MainWindow : WPF.Window
    {


        private bool isDarkTheme = false;
        private const string DarkThemePath = "Themes/DarkTheme.xaml";
        private const string LightThemePath = "Themes/LightTheme.xaml";


        private Forms.NotifyIcon? trayIcon;
        private DiscordRpcClient? client;
        private const string SaveFilePath = "activity.json";

        public MainWindow()
        {
            InitializeComponent();
            LoadPreviousActivity();
        }

        private void ConnectToDiscord_Click(object sender, RoutedEventArgs e)
        {
            string appId = txtAppId.Text.Trim();

            if (string.IsNullOrWhiteSpace(appId))
            {
                WPF.MessageBox.Show("Please enter a valid Discord Application ID.");
                return;
            }

            client = new DiscordRpcClient(appId)
            {
                Logger = new ConsoleLogger { Level = LogLevel.Warning }
            };
            client.Initialize();

            WPF.MessageBox.Show("Connected to Discord.");
        }

        private void SendToDiscord_Click(object sender, RoutedEventArgs e)
        {
            if (client == null)
            {
                WPF.MessageBox.Show("Please connect to Discord first.");
                return;
            }

            var buttons = new List<DiscordRPC.Button>();
            if (Uri.IsWellFormedUriString(txtButtonUrl.Text, UriKind.Absolute))
            {
                buttons.Add(new DiscordRPC.Button
                {
                    Label = txtButtonLabel.Text,
                    Url = txtButtonUrl.Text
                });
            }

            client.SetPresence(new RichPresence
            {
                Details = txtDetails.Text,
                State = txtState.Text,
                Assets = new Assets
                {
                    LargeImageKey = txtLargeImageKey.Text,
                    LargeImageText = txtLargeImageText.Text,
                    SmallImageKey = txtSmallImageKey.Text,
                    SmallImageText = txtSmallImageText.Text
                },
                Buttons = buttons.ToArray()
            });

            WPF.MessageBox.Show("Presence updated.");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var activity = new ActivityModel
            {
                AppId = txtAppId.Text,
                Details = txtDetails.Text,
                State = txtState.Text,
                LargeImageKey = txtLargeImageKey.Text,
                LargeImageText = txtLargeImageText.Text,
                SmallImageKey = txtSmallImageKey.Text,
                SmallImageText = txtSmallImageText.Text,
                ButtonLabel = txtButtonLabel.Text,
                ButtonUrl = txtButtonUrl.Text
            };

            try
            {
                string json = JsonSerializer.Serialize(activity, new JsonSerializerOptions { WriteIndented = true });

                // JSON geçerli mi kontrolü
                JsonDocument.Parse(json); // parse edilebiliyorsa geçerli
                File.WriteAllText(SaveFilePath, json);
                WPF.MessageBox.Show("Activity saved.");
            }
            catch (Exception ex)
            {
                WPF.MessageBox.Show("JSON formatı hatalı veya kaydedilemedi:\n\n" + ex.Message);
            }
        }


        private void LoadPreviousActivity()
        {
            if (!File.Exists(SaveFilePath)) return;

            try
            {
                var json = File.ReadAllText(SaveFilePath);
                var activity = JsonSerializer.Deserialize<ActivityModel>(json);

                if (activity == null) return;

                txtAppId.Text = activity.AppId;
                txtDetails.Text = activity.Details;
                txtState.Text = activity.State;
                txtLargeImageKey.Text = activity.LargeImageKey;
                txtLargeImageText.Text = activity.LargeImageText;
                txtSmallImageKey.Text = activity.SmallImageKey;
                txtSmallImageText.Text = activity.SmallImageText;
                txtButtonLabel.Text = activity.ButtonLabel;
                txtButtonUrl.Text = activity.ButtonUrl;
            }
            catch (Exception ex)
            {
                WPF.MessageBox.Show("Failed to load previous activity:\n" + ex.Message);
            }
        }

        private void ClearPresence_Click(object sender, RoutedEventArgs e)
        {
            client?.ClearPresence();
            WPF.MessageBox.Show("Presence cleared.");
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            if (trayIcon == null)
            {
                var contextMenu = new Forms.ContextMenuStrip();
                contextMenu.Items.Add("Restore", null, (s, ev) =>
                {
                    Show();
                    WindowState = WindowState.Normal;
                    trayIcon.Visible = false;
                });

                contextMenu.Items.Add("Exit", null, (s, ev) =>
                {
                    trayIcon.Visible = false;
                    trayIcon.Dispose();
                    WPF.Application.Current.Shutdown();
                });

                trayIcon = new Forms.NotifyIcon
                {
                    Icon = SystemIcons.Application,
                    Visible = true,
                    Text = "Discord RPC Manager",
                    BalloonTipTitle = "Running in Background",
                    BalloonTipText = "Right-click the icon for options.",
                    ContextMenuStrip = contextMenu
                };

                trayIcon.DoubleClick += (_, _) =>
                {
                    Show();
                    WindowState = WindowState.Normal;
                    trayIcon.Visible = false;
                };
            }

        }
        private void SaveActivityToJson()
        {
            var activity = new ActivityModel
            {
                AppId = txtAppId.Text,
                Details = txtDetails.Text,
                State = txtState.Text,
                LargeImageKey = txtLargeImageKey.Text,
                LargeImageText = txtLargeImageText.Text,
                SmallImageKey = txtSmallImageKey.Text,
                SmallImageText = txtSmallImageText.Text,
                ButtonLabel = txtButtonLabel.Text,
                ButtonUrl = txtButtonUrl.Text
            };

            try
            {
                File.WriteAllText(SaveFilePath, JsonSerializer.Serialize(activity, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Failed to save activity on close:\n" + ex.Message);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            SaveActivityToJson(); // Kapanmadan önce veriyi kaydet

            trayIcon?.Dispose();
            WPF.Application.Current.Shutdown();
        }

        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
{
    try
    {
        var themePath = isDarkTheme ? LightThemePath : DarkThemePath;
        var uri = new Uri(themePath, UriKind.Relative);
        var resourceDict = WPF.Application.LoadComponent(uri) as ResourceDictionary;

        if (resourceDict != null)
        {
            WPF.Application.Current.Resources.MergedDictionaries.Clear();
            WPF.Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            isDarkTheme = !isDarkTheme;
        }
    }
    catch (Exception ex)
    {
        System.Windows.MessageBox.Show("Tema değiştirilemedi: " + ex.Message);
    }
}


    }
}
