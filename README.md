# AirPad 📱💻
[![GitHub release](https://img.shields.io/github/v/release/Ahmd-Naser/AirPad?color=blue&logo=github)](https://github.com/Ahmd-Naser/AirPad/releases/latest)

AirPad is an open-source tool that transforms your smartphone into a wireless, low-latency touchpad and remote control for your Windows PC over a local network. 

## ✨ Features
- **Zero Configuration:** Automatically finds an open network port and generates a QR Code. Just scan and connect!
- **Low Latency:** Real-time communication powered by ASP.NET Core SignalR.
- **Smart Gestures:**
  - 👆 **1 Finger Drag:** Move cursor
  - 👆 **1 Finger Tap:** Left Click
  - ✌️ **2 Fingers Tap:** Right Click
  - ✌️ **2 Fingers Drag:** Vertical & Horizontal Scrolling
- **Stealth Mode:** Runs cleanly in the Windows System Tray without any console windows.
- **Screen Wake Lock:** Prevents your phone screen from sleeping while in use.

## 🚀 Installation & Usage

1. Go to the [Releases](../../releases) page.
2. Download the latest `AirPad-v1.0.0.zip`.
3. Extract the ZIP file. **(Make sure the `wwwroot` folder stays next to the `.exe`)**.
4. Run `AirPad.Server.exe`.
5. A window will appear with a QR Code. Scan it with your phone's camera.
6. The app will minimize to your System Tray (next to the clock). Right-click the icon to show the QR code again or exit.



## 🛠️ Building from Source

```bash
git clone https://github.com/yourusername/AirPad.git
cd AirPad/AirPad.Server
dotnet build
dotnet run
```

## 🏗️ Architecture & Tech Stack
- **Backend:** C#, ASP.NET Core 8, SignalR
- **Desktop OS Integration:** Windows Forms (System Tray Integration), P/Invoke (`user32.dll` for cursor simulation)
- **Frontend (Mobile Client):** Vanilla JavaScript (Touch Events API), HTML5, CSS3

## 🤝 Contributing
Contributions, issues, and feature requests are welcome! Feel free to check the [issues page](../../issues).

## 📝 License
This project is open-source and available under the MIT License.
