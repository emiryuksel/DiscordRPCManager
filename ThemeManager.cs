using System;
using System.Windows;

namespace DiscordRPCManager
{
    public static class ThemeManager
    {
        public static void ApplyTheme(string themePath)
        {
            try
            {
                var uri = new Uri(themePath, UriKind.Relative);
                var resourceDict = System.Windows.Application.LoadComponent(uri) as ResourceDictionary;

                if (resourceDict != null)
                {
                    System.Windows.Application.Current.Resources.MergedDictionaries.Clear();
                    System.Windows.Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Tema yüklenemedi: " + ex.Message);
            }
        }
    }
}