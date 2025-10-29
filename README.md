## Mini Punball

A tiny Unity prototype inspired by HABBY’s PunBall — a fast, arcade “shooting balls” roguelite where ricochets clear enemies and waves push down each turn. This repo focuses on the core loop and simple content so you can play, tweak, and extend.

### What’s inside
- Core shooting-and-bounce gameplay loop (aim, shoot, ricochet, resolve wave)
- 2D physics-based ball behavior and enemy blocks
- Basic content organized into Prefabs, ScriptableObjects, and Scenes
- Text rendering via TextMeshPro; tween utilities via DOTween (Demigiant)

Note: This is a learning/demo project, not a full clone. Meta-progression, monetization, and live-ops systems are intentionally out of scope.

## Requirements
- Unity 2022.3.62f3 (LTS) or newer in the 2022.3 LTS line
- Windows/macOS for editor; mobile targets optional

## Getting started
1. Clone or download this repository.
2. Open the project folder in Unity 2022.3.62f3.
3. Open a scene under `Assets/Scenes/` (for example, your main gameplay scene) and press Play.

## How to play
- Aim: move the mouse to adjust the shot direction (or drag on touch devices).
- Shoot: left-click (or release on touch) to fire the volley.
- Ricochet: balls bounce off walls and hit enemies; each turn/wave resolves after balls return or despawn.
- Survive: don’t let enemies push down to the bottom of the board.

## Controls
- Desktop: mouse to aim, left-click to shoot, Esc to pause/back (if wired).
- Mobile: drag to aim, release to shoot; back button to pause (platform dependent).

## Project layout
- `Assets/Scripts/` — gameplay logic and helpers
- `Assets/Prefabs/` — balls, enemies/blocks, VFX, and UI prefabs
- `Assets/Scenes/` — playable scenes
- `Assets/ScriptableObjects/` — tunable data (stats, waves, configs)
- `Assets/3rdParty/` — TextMeshPro, and `Plugins/Demigiant/` (DOTween)

## Build
Unity (Editor):
- Open File > Build Settings…
- Pick your target platform (PC/Mac/Linux, iOS, Android)
- Add your main scene(s) to Scenes In Build
- Build (or Build And Run)

Mobile notes (brief):
- Android: Switch Platform to Android, set Player Settings (package name, icons, IL2CPP if desired), then Build.
- iOS: Switch Platform to iOS, set Player Settings, Build to Xcode, then archive on a Mac.

## Tech
- Unity 3D (LTS 2022.3)
- TextMeshPro (UI)
- DOTween (tweening; Demigiant)

## Attribution
This project is an unofficial, fan-made homage to PunBall by HABBY. All trademarks and copyrights for PunBall and related materials are the property of their respective owners. This repository is for educational and non-commercial purposes and is not affiliated with or endorsed by HABBY.

## License
No license specified yet. If you plan to use this project beyond personal learning, consider adding an open-source license (e.g., MIT) to the repository.

