# Discord RPC Manager

**Discord RPC Manager** is a lightweight desktop application built with **WPF (C#)** to create and manage your custom **Discord Rich Presence** without writing a single line of code.

---

## 🚀 How To Install

> [Download DiscordRPCManager.zip](https://github.com/emiryuksel/DiscordRPCManager/releases/download/v1.0.0/DiscordRPCManager-v1.0.0.zip)  

- Download and unzip, then double-click **DiscordRPCManager.exe** to run.  
- Make sure Discord is open. Enter your settings, then start.

---

## ⚙️ How to Use

1. **Enter your Discord Application ID**
2. Fill out:
   - `Details` & `State`
   - `Large Image Key` + `Text`
   - `Small Image Key` + `Text`
   - `Button Label` + `URL`
3. Click **Connect** to initialize your presence
4. Click **Start Presence** to broadcast it on Discord
5. Optionally click **Save** to store your setup

---

## 💾 Data Storage

- Configurations are saved locally in `activity.json`
- Automatically loaded on startup

---

## ✨ Features

- 🎯 **Easy Presence Setup**  
  Define details, state, image keys, and a button label + URL with a user-friendly interface.

- 🖼️ **Image Support**  
  Set **large/small image keys** and tooltips directly from your Discord Developer Portal assets.

- 🧠 **Smart Save & Restore**  
  Automatically remembers your last activity (saved as JSON locally).

- 📦 **Tray Mode**  
  Runs silently in the background – just minimize to tray!

- 🌓 **Lightweight UI**  
  Clean, responsive interface with consistent styling.

---

## 🛠️ Built With

- WPF (.NET)
- [Lachee's Discord RPC Library](https://github.com/Lachee/discord-rpc-csharp)
- C# 10+

---

## 📌 Note

This is an early version. If you find any bugs or have suggestions, feel free to [open an issue](https://github.com/emiryuksel/DiscordRPCManager/issues).

---

## 📜 License

MIT – _free to use, modify, and distribute._
