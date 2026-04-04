# Iteration 7 — Audio, SFX, Music, Haptics

## Что в этой итерации
- **MusicManager.cs** — новый. Singleton, DontDestroyOnLoad. Загружает музыку через Addressables ("GameMusic"), loop, fade in/out. Паузится при паузе игры. Учитывает SoundEnabled.
- **SFXManager.cs** — новый. Singleton, DontDestroyOnLoad. Генерирует все звуки программно через AudioClip.Create (без файлов):
  - shield_hit — короткий "тынк"
  - balloon_pop — низкий "пуф"
  - score_tick — тихий "тик" при каждом очке
  - milestone — восходящий "дзынь" каждые 10 очков
  - button_click — мягкий клик для всех кнопок
  - game_over — нисходящий звук при смерти
- **HapticManager.cs** — новый. Singleton, DontDestroyOnLoad. iOS хаптика (Light/Medium/Heavy).
- **HapticPlugin.mm** — нативный iOS плагин для тактильного feedback.

## Обновлённые скрипты
- **Shield.cs** — SFX + Haptic Light при столкновении с препятствием
- **Balloon.cs** — SFX (pop + game_over) + Haptic Heavy при смерти
- **GameUI.cs** — SFX score_tick при каждом очке, milestone при ×10 + Haptic Medium
- **BootstrapUI.cs** — загружает музыку после Addressables
- **MainMenuUI.cs** — SFX button_click для Play и Settings
- **SettingsPopup.cs** — SFX для toggle и close, обновляет громкость музыки
- **GameOverPopup.cs** — SFX button_click для Restart и Menu
- **PausePopup.cs** — SFX для кнопок, music pause/resume
- **Editor/SetupBootstrapScene.cs** — добавляет MusicManager, SFXManager, HapticManager на Bootstrap
- **Editor/SetupGameScene.cs** — добавлен menu item для Iteration 7

## Что изменилось с предыдущей итерации
- Добавлены: `MusicManager.cs`, `SFXManager.cs`, `HapticManager.cs`, `Plugins/iOS/HapticPlugin.mm`
- Обновлены: `Shield.cs`, `Balloon.cs`, `GameUI.cs`, `BootstrapUI.cs`, `MainMenuUI.cs`, `SettingsPopup.cs`, `GameOverPopup.cs`, `PausePopup.cs`, оба editor скрипта
- Не изменены: `Obstacle.cs`, `ObstacleSpawner.cs`, `DifficultyManager.cs`, `GameplayManager.cs`, `GameCamera.cs`, `ScreenShake.cs`, `ParticleManager.cs`, `ShieldVisuals.cs`, `SceneLoader.cs`, `AddressableLoader.cs`, `GameManager.cs`

## Настройка

### 1. Скопируй/замени файлы
- `Assets/BalloonGame/Scripts/MusicManager.cs` — **новый**
- `Assets/BalloonGame/Scripts/SFXManager.cs` — **новый**
- `Assets/BalloonGame/Scripts/HapticManager.cs` — **новый**
- `Assets/BalloonGame/Plugins/iOS/HapticPlugin.mm` — **новый**
- `Assets/BalloonGame/Scripts/Shield.cs` — **замена**
- `Assets/BalloonGame/Scripts/Balloon.cs` — **замена**
- `Assets/BalloonGame/Scripts/GameUI.cs` — **замена**
- `Assets/BalloonGame/Scripts/BootstrapUI.cs` — **замена**
- `Assets/BalloonGame/Scripts/MainMenuUI.cs` — **замена**
- `Assets/BalloonGame/Scripts/SettingsPopup.cs` — **замена**
- `Assets/BalloonGame/Scripts/GameOverPopup.cs` — **замена**
- `Assets/BalloonGame/Scripts/PausePopup.cs` — **замена**
- `Assets/BalloonGame/Scripts/Editor/SetupBootstrapScene.cs` — **замена**
- `Assets/BalloonGame/Scripts/Editor/SetupGameScene.cs` — **замена**

### 2. Открой сцену Bootstrap
- Меню: **BalloonGame → (Iteration 1) Setup Bootstrap Scene**
- Это добавит MusicManager, SFXManager, HapticManager на Bootstrap сцену
- **Ctrl+S** — сохрани

### 3. Готово!
Game сцена не требует повторной настройки — все audio/haptic менеджеры живут на Bootstrap с DontDestroyOnLoad.

## Как тестировать

### Тест 1 — Полный flow со звуком
1. Bootstrap → MainMenu → Play → Game
2. **Ожидаемый результат**:
   - Музыка начинает играть после загрузки (fade in)
   - Клик по PLAY — звук клика
   - В игре: тик при каждом очке, тынк при ударе щита, дзынь при milestone
   - Game Over — пуф + нисходящий звук

### Тест 2 — Sound toggle
1. MainMenu → Settings → SOUND: OFF
2. **Ожидаемый результат**: музыка fade out, все SFX отключены
3. SOUND: ON → музыка fade in, SFX работают

### Тест 3 — Пауза музыки
1. В игре нажми паузу
2. **Ожидаемый результат**: музыка паузится
3. Resume → музыка продолжает

### Тест 4 — Хаптика (только на iOS устройстве)
1. Столкновение щита → лёгкая вибрация
2. Milestone → средняя вибрация
3. Game Over → сильная вибрация

## Структура на сцене Bootstrap (после editor скрипта)
```
Main Camera
EventSystem
AddressableLoader          [AddressableLoader.cs, DontDestroyOnLoad]
SceneLoader                [SceneLoader.cs, DontDestroyOnLoad]
  └── FadeCanvas
MusicManager               [MusicManager.cs, DontDestroyOnLoad]
SFXManager                 [SFXManager.cs, DontDestroyOnLoad]
HapticManager              [HapticManager.cs, DontDestroyOnLoad]
BootstrapCanvas            [BootstrapUI.cs]
```

## Ожидаемый результат итерации
Полный audio feedback: программно сгенерированные SFX для каждого действия (без аудиофайлов), музыка через Addressables с fade и паузой, iOS хаптика. Sound toggle в настройках управляет всем. Все звуки синтезированы — приятные, но схематические (можно заменить на реальные звуки позже).
