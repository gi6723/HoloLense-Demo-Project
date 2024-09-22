# HoloLens-Demo-Project

## Project Overview
This repository showcases a **Mixed Reality** demo using **HoloLens 2** and **Unity**. The project integrates real-time eye-tracking data capture and spatial interactions using **Microsoft's Mixed Reality Toolkit (MRTK)**.

### Key Features:
- **Eye-Tracking Data**: Captures and processes gaze origin, direction, and interaction data.
- **Spatial Anchors**: Uses QR code detection to anchor holograms in the real world.
- **Data Streaming**: Streams eye-tracking data from the HoloLens to a server using MQTT and WebSockets for real-time processing.

### Technologies Used:
- **Unity 3D** with MRTK 3.0
- **Microsoft HoloLens 2**
- **MQTT Protocol** for data streaming
- **WebSocket** for bi-directional communication

### Future Goals:
- Integration into larger systems for industrial applications such as 3D printer monitoring.
- Further development on the server-side for enhanced machine interaction.

---

### **Unity Setup**:

#### **1. How to Get Unity Connected to JetBrains Rider**

1. **Install JetBrains Rider Unity Plugin**:
   - Open Unity and go to **Edit > Preferences > External Tools**.
   - In **External Script Editor**, select **JetBrains Rider** from the dropdown (or browse for it if not listed).
   - Install the **Rider Plugin for Unity** to allow smooth integration.

2. **Open Unity Project in JetBrains Rider**:
   - Once JetBrains Rider is selected as the external editor, you can right-click a script in Unity, click **Open C# Project**, and it will open in Rider.
   - Unity will now use Rider for all script editing.

---

#### **2. How to Get Unity Project Inside Repo**

1. **Add Unity Project to GitHub**:
   - After creating the Unity project on your local machine, navigate to the folder containing the Unity project.
   - Open **Git Bash** or your terminal inside the Unity project folder.
   - Initialize Git in the Unity folder:
     ```bash
     git init
     git remote add origin <GitHub-repo-URL>
     git add .
     git commit -m "Initial Unity project commit"
     git push -u origin master
     ```
2. **Push/Pull Workflow**:
   - Ensure both you and Zach follow a **push/pull** workflow. Always **pull** the latest changes from the remote repo before making edits and **push** once changes are committed.

3. **.gitignore File**:
   - Unity projects generate many unnecessary files (logs, libraries). Make sure to use a **.gitignore** file to exclude these files. You can generate a Unity-specific `.gitignore` [here](https://github.com/github/gitignore/blob/main/Unity.gitignore).

---

### **Questions/Concerns**:

#### **1. Mizzou’s Network and Tailscale Setup**:

Since Mizzou’s network is compartmentalized, **Tailscale** is a great option for creating a virtual private network (VPN) between devices. This would allow the server and HoloLens to communicate over Mizzou’s network, even with isolation.

1. **Set up Tailscale**:
   - Install Tailscale on your local development machine, HoloLens, and the server.
   - Create a **Tailscale network** where all devices join the same private mesh VPN.
   - You’ll be able to communicate directly with the server regardless of Mizzou’s compartmentalized network restrictions.

#### **2. Connecting to Shane's Server or Using a Raspberry Pi**:

If Shane’s server is already set up and functional:
- **Pros of Using Shane's Server**:
  - Saves time since it’s pre-configured.
  - You won’t need to set up a new device from scratch.
- **Cons**:
  - Potential network issues due to security or firewall settings at Mizzou.
  - Possible complexities in managing multiple projects on one server.

**Using a Raspberry Pi**:
- A Raspberry Pi running **Mosquitto** (MQTT broker) or similar software might be easier to manage and set up in your local environment or through Tailscale for secure remote access.

---

#### **3. Moonraker without Klipper**:

While **Moonraker** is generally used with **Klipper** for 3D printer management, it can still function without Klipper for monitoring and some basic command functionality.

- **Pros**: You can still use Moonraker as a lightweight API server to handle **basic monitoring** tasks and potentially repurpose it for non-3D printer-related data.
- **Cons**: You might miss out on advanced printer-related commands without Klipper. Since the demo doesn't involve printing, this might not be a significant issue.

If Moonraker feels too tied to 3D printing, you might want to explore using **custom WebSocket APIs** instead for better flexibility.

---

### **Next Steps**:
1. Set up the **GitHub repository** following the instructions above.
2. Push the **Unity project** to GitHub and ensure both you and Zach can collaborate.
3. Investigate **Tailscale** for remote device communication and decide whether to use Shane’s server or a **Raspberry Pi** for the demo project.
4. Research whether **Moonraker** meets your needs for monitoring or if another API would be better suited for your demo.

Let me know how you'd like to proceed, or if you need further clarification on any points!


