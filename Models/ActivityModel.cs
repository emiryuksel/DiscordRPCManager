namespace DiscordRPCManager.Models
{
    public class ActivityModel
    {
        public string AppId { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string LargeImageKey { get; set; } = string.Empty;
        public string LargeImageText { get; set; } = string.Empty;
        public string SmallImageKey { get; set; } = string.Empty;
        public string SmallImageText { get; set; } = string.Empty;
        public string ButtonLabel { get; set; } = string.Empty;
        public string ButtonUrl { get; set; } = string.Empty;
    }
}