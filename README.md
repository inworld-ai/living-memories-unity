# Living Memories - Unity Template Project

[![Unity Version](https://img.shields.io/badge/Unity-6000.0.37f1-blue.svg)](https://unity.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

A Unity template project demonstrating AI-powered interactive memory companions using Inworld AI and Runway ML integrations.

## Quick Links
- [Overview](#overview)
- [Features](#features)
- [Quick Start](#quick-start)
- [API Configuration](#api-configuration-guide)
- [Template Documentation](#template-documentation)
- [Troubleshooting](#troubleshooting)
- [Support & Contact](#support--contact)

## Overview

**Living Memories** is a template project that showcases how to build interactive AI experiences where users can:
- Upload photos and bring them to life with AI-generated video
- Have natural conversations with AI characters based on those memories
- Create immersive memory experiences with animated videos and lip-sync

This project serves as a starting point for developers who want to build AI-powered applications combining:
- **Inworld AI** - For character AI and natural language conversations
- **Runway ML** - For image-to-video generation and lip-sync animation
- **UseAPI.net** - As a proxy service for Runway ML API calls

## Features

### Template Demos

The project includes two main template scenes located in `Assets/Templates/Scenes/`:

1. **Memory Companion** (`MemoryCompanion.unity`)
   - **Configure AI Voice**: Visit [Inworld TTS Playground](https://platform.inworld.ai/v2/workspaces/YOUR_WORKSPACE/tts-playground) to clone a custom voice or select a preset, then copy the Voice ID
   - **Customize Character**: Define personality, knowledge, and intents via graph nodes in `Assets/InworldRuntime/Data/GraphNodes/SampleCharacter/`, or create new graphs and link them to the `InworldGraphExecutor` in the scene
   - **Launch & Upload**: Start the Unity project and upload a photo to transform into an animated video
   - **Interactive Conversation**: Engage in natural dialogue with your AI memory companion
   - **Emotional AI Responses**: Experience context-aware character interactions powered by Inworld AI

2. **Lip Sync Demo** (`LipSync.unity`)
   - **Configure AI Voice**: Visit [Inworld TTS Playground](https://platform.inworld.ai/v2/workspaces/YOUR_WORKSPACE/tts-playground) to clone a custom voice or select a preset, then copy the Voice ID
   - **Launch & Create**: Start the Unity project and upload a portrait-style photo
   - **Generate Speech**: Enter dialogue text and generate a lip-synced video of the person speaking using Runway ML

### Key Components

- **API Controllers** - Ready-to-use integrations for third-party AI services
- **Flow Controllers** - Scene management and state handling
- **File Browser** - Built-in image selection with Simple File Browser

---

## Quick Start

### Prerequisites

- **Unity 6000.0.37f1** or later
- Windows, macOS, or Linux
- Valid API keys for:
  - [Inworld AI](https://www.inworld.ai/) (Required)
  - [Runway ML](https://runwayml.com/) (Optional, for video generation and lip-sync)
  - [UseAPI.net](https://useapi.net/) (Optional, as a proxy for Runway ML)

### Installation

#### Step 1: Clone the Repository

```bash
git clone https://github.com/inworld-ai/living-memories-unity.git
cd LivingMemories
```

#### Step 2: Open in Unity

1. Launch **Unity Hub**
2. Click **"Open"** and select the project folder
3. Wait for Unity to import all assets (5-10 minutes)

#### Step 3: Download Required Dependencies

When you first open the project, a **Dependency Downloader** window will appear automatically. If not, open it via:

```
Unity Menu Bar > Window > Inworld > Dependency Downloader
```

![Dependency Downloader](docs/images/dependency-downloader.png)

Click the buttons to download:
- **Plugins** - Required DLLs for Inworld runtime
- **StreamingAssets** - AI models for speech recognition, TTS, etc.

> **Important**: These files are excluded from the repository due to their size. The downloader will fetch them from cloud storage.

#### Step 4: Configure API Keys

**For Memory Companion Scene:**
1. Open `Assets/Templates/Scenes/MemoryCompanion.unity`
2. In the Hierarchy, find the object with `APIController_Memory` script
3. In the Inspector, enter your **Inworld API Key** and **Runway API Key**

**For Lip Sync Scene:**
1. Open `Assets/Templates/Scenes/LipSync.unity`
2. Find the object with `APIController_Lipsync` script
3. Enter your **Inworld API Key**, **UseAPI Token** and **Runway Account Email**

#### Step 5: Test

1. Press Play in Unity Editor
2. Click "Setup" to initialize the AI connection
3. Upload a photo (Memory Companion) or start chatting (Lip Sync)
4. Enjoy your interactive memory experience!

---

## API Configuration Guide

### 1. Inworld AI (Required for Both Scenes)

**Get Your API Key:**
1. Go to [platform.inworld.ai](https://platform.inworld.ai/) and sign up or log in
2. Navigate to your workspace
3. Go to **Runtime** → **Get API Keys**
4. Copy the API key

**Configure Voice (Required):**
1. Visit [Inworld Platform](https://platform.inworld.ai/)
2. Navigate to your workspace → **TTS Playground**
3. Use **Voice Clone** to create a custom voice or select a preset from the list
4. Copy the **Voice ID** (you'll need this in Unity)

**In Unity:**
- Paste your **API Key** into the `APIController_Memory` or `APIController_Lipsync` component
- Enter your **Voice ID** in the scene's voice input field

---

### 2. Runway ML API (Required for Memory Companion Scene)

**Get Your API Key:**
1. Go to [Runway ML Dev Portal](https://dev.runwayml.com/login) and create an account
3. Create a new API key - see [Runway ML API Guide](https://docs.dev.runwayml.com/guides/using-the-api/)
4. Copy the API key

**In Unity:**
- Open `Assets/Templates/Scenes/MemoryCompanion.unity`
- Find the `RunwayImageToVideo` component in the scene
- Paste your **Runway API Key**

> **Note**: Runway ML credits are required for video generation. Each video consumes credits based on duration and quality.

---

### 3. UseAPI.net + Runway ML (Required for Lip Sync Scene)

**Get Your UseAPI Token:**
1. Go to [UseAPI.net](https://useapi.net/) and sign up
2. Navigate to **API Settings**
3. Generate an API token
4. Copy the token

**Setup Runway ML Integration:**
1. Go to [runwayml.com](https://runwayml.com/) and create an account
2. Link your Runway ML email to UseAPI.net follow the [UseAPI & LipSync Setup Guide](https://useapi.net/docs/start-here/setup-runwayml)

**In Unity:**
- Open `Assets/Templates/Scenes/LipSync.unity`
- Find the `APIController_Lipsync` component
- Enter your **Inworld API Key**
- Enter your **UseAPI Token**
- Enter your **Runway ML Account Email** 

> **Note**: UseAPI.net acts as a proxy service that simplifies calling Runway ML APIs for lip-sync generation.

---

## Template Documentation

### Memory Companion Scene

**Location**: `Assets/Templates/Scenes/MemoryCompanion.unity`

**Key Contents:**
- `FlowController_MemoryCompanion.cs` - Main scene user flow controller
- `APIController_Memory.cs` - API key management
- `Assets/InworldRuntime/Data/GraphNodes/SampleCharacter` - Customize personalities
- `RunwayImageToVideo.cs` - Video generation via Runway ML
- `PickImageWithSimpleFileBrowser.cs` - Image selection

**Customization:**

Change default video prompt:
```csharp
// In FlowController_MemoryCompanion.cs
public string defaultPrompt = "Your custom prompt here...";
```

Disable video generation and use a placeholder video for test:
- Uncheck "Use Runway Generation" in `RunwayImageToVideo` component

### Lip Sync Scene

**Location**: `Assets/Templates/Scenes/LipSync.unity`

**Key Contents:**
- `FlowController_Lipsync.cs` - Main scene controller
- `APIController_Lipsync.cs` - API configuration
- `Useapi_Runway_LipSync.cs` - UseAPI.net proxy integration
- `RunwayLipsync.cs` - Runway Account that link to your UseAPI integration

**Customization:**

Adjust video duration:
```csharp
public float videoDuration = 5.0f;  // Duration in seconds
```

---

## Project Structure

```
LivingMemories/
├── Assets/
│   ├── Editor/                      # Editor tools
│   │   ├── DependencyDownloaderWindow.cs   # Auto dependency installer
│   │   └── DependencyImporter.cs
│   ├── InworldRuntime/              # Inworld AI SDK (402 scripts)
│   │   ├── Scripts/                 # Core AI logic
│   │   ├── Prefabs/                 # Character prefabs
│   │   ├── Scenes/                  # SDK example scenes
│   │   └── Data/                    # AI configuration
│   ├── Templates/                   # Demo scenes (YOUR FOCUS)
│   │   ├── Scenes/
│   │   │   ├── MemoryCompanion.unity
│   │   │   └── LipSync.unity
│   │   ├── Scripts/                 # Template scripts
│   │   ├── Materials/               # Custom materials
│   │   ├── Textures/                # UI assets
│   │   ├── VFX/                     # Visual effects
│   │   └── Shaders/                 # Custom shaders
│   ├── Settings/                    # URP render settings
│   └── TextMesh Pro/                # UI text rendering
├── Packages/
│   └── manifest.json                # Package dependencies
├── ProjectSettings/
└── README.md                        # This file
```

---

## Troubleshooting

### Common Issues

**"Missing Plugins or StreamingAssets"**
- Open `Window > Inworld > Dependency Downloader`
- Download the required dependencies
- If auto-download fails, manually download from URLs in console

**"API Key Not Found" or "Authentication Failed"**
- Verify API keys are correct (no extra spaces)
- Check keys are active in your service dashboard
- Ensure you're using the correct key for each service
- Regenerate the API key if necessary

**"Video Generation Failed"**
- Verify Runway ML API key is correct
- Check you have sufficient API credits
- Review Console for specific error messages
- Test with a smaller/lower resolution image

**"No Audio Output"**
- Check Unity Audio settings: `Edit > Project Settings > Audio`
- Verify system audio is working
- Check the Audio Mixer: `Assets/InworldRuntime/Audio/AudioCaptureDevice.mixer`
- Ensure correct voice ID is configured

**Compilation Errors**
- Update Unity to version 6000.0.37f1
- Verify all packages are imported: `Window > Package Manager`
- Clear Library folder and reimport project
- Restart Unity Editor

**"Download Failed" or Network Errors**
- Check internet connection
- Verify you're not behind a restrictive firewall
- Try manual download from the URLs provided
- Check proxy settings if on corporate network

---

## Dependencies

This project uses the following Unity packages (defined in `Packages/manifest.json`):

- **Unity Input System** (1.12.0) - Modern input handling
- **Universal Render Pipeline** (17.0.3) - High-quality rendering
- **Newtonsoft.Json** (3.2.2) - JSON serialization
- **Simple File Browser** - File selection UI
- **TextMesh Pro** - UI text rendering
- **Inworld AI SDK** - Character AI and conversation

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

**Copyright (c) 2025 Inworld AI**

### Third-Party Components

This project integrates with third-party services and SDKs:


- **Runway ML**: [Terms of Service](https://runwayml.com/terms/)
- **UseAPI.net**: [Website](https://useapi.net/)

When using this template, you must comply with the respective terms of service for each integrated service.

---

## Resources

- [Inworld AI Documentation](https://docs.inworld.ai/)
- [Runway ML API Docs](https://docs.runwayml.com/)
- [Unity Manual](https://docs.unity3d.com/)
- [Universal Render Pipeline](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest)

---

## Tips & Best Practices

### Performance Optimization

- Compress images before upload (recommended max: 2048x2048)
- Use async/await for all API calls
- Dispose unused textures and videos properly
- Pool UI elements when possible

### API Usage

- Monitor API credit usage regularly
- Implement rate limiting for production
- Cache responses when appropriate
- Set reasonable timeout values (e.g., 30-60 seconds for video generation)

### Error Handling

- Always validate user inputs before API calls
- Provide clear error messages to users
- Implement fallbacks if services fail
- Log errors for debugging

### Security

- **Never** commit API keys to version control
- Use environment variables in production
- Rotate API keys regularly
- Implement proper authentication for production apps

---

## Support & Contact

- **GitHub Issues**: [Report bugs or request features](https://github.com/inworld-ai/LivingMemories/issues)
**General Questions**: For general inquiries and support, please email us at support@inworld.ai



