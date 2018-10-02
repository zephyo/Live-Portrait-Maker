![banner](https://img.itch.zone/aW1nLzExNjU5MjQucG5n/original/eMa3fc.png "banner")

# Live Portrait Maker 🎉

Live Portrait Maker is a 2.5D dress up game with over 6.5m installs! Tap to look, double tap to dress up 😎

**Watch the [trailer](https://www.youtube.com/watch?v=6TsYSdJc1aE), or play today for free:**


<a href="https://zephyo.itch.io/live-portrait-maker" target="_blank">
  <p align="center"><img src="https://itch.io/press-kit/assets/badges/badge_color.png" 
height="60" /></p></a>
<a href="https://play.google.com/store/apps/details?id=com.zephyo.LivePortraitMaker" target="_blank">
  <p align="center">
    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/c/cd/Get_it_on_Google_play.svg/1000px-Get_it_on_Google_play.svg.png" 
height = "60" /></p></a>
  <a href="https://itunes.apple.com/us/app/live-portrait-maker/id1371293610" target="_blank">
  <p align="center"><img src="https://devimages-cdn.apple.com/app-store/marketing/guidelines/images/badge-download-on-the-app-store.svg" 
height="60" /></p></a>



Features
------
### 2.5D Character Creation
* 2D portrait moves in a way that gives 2.5D effect - adjust transform's local rotation and anchored position every frame towards a target Vector2
### Image Effects
<img src="https://i.imgur.com/PUMfZoc.png" height="300"/>

* Contains camera post-processing image effect shaders
* Contains material shaders for Image components
* Major credit to [Keijiro Takahashi](https://github.com/keijiro) for shader help
### GIF + PNG capture
<img src = "https://i.imgur.com/3k4ESj8.png" height="300">

* Crop capture area
* Call to native calls in iOS and Android for capture and external storage permissions
* Multithreaded recording, GIF/PNG encoding, and saving
### Local Save/Load
* Saves to PlayerPrefs for temporary data; persistent data path for important data (e.g. purchases)
* Saved in different, compatible texture format depending on platform

### Asset Bundling
* Stores DLC (obtained through IAP) in Asset Bundles hosted on AWS S3
### Monetization
* Uses Unity Codeless IAP for in-app purchases
* Uses ad mediation network Mopub for ads, balances between ad networks to get optimum eCPM
### Localization
* Localized into English, French, Japanese, Korean, Russian, Simplified Chinese, Spanish, Thai

Technologies
------
* C#
* Unity
* AWS S3
* Xcode
* HLSL/OpenGL
* Adobe Photoshop


Licensing
------
Copyright (C) 2018 Angela He

The artwork is under the CC-BY-NC license and cannot be used in commercial products.

The rest of the project is licensed under the terms of the MIT license.
