# Iteration 12 — Shield Rush + Final Polish

## Что в этой итерации
- **Shield.cs** — обновлён: в Shield Rush щит увеличивается с 1.2x до 2.2x scale. Параметры настраиваемые в Inspector.
- **ShieldVisuals.cs** — обновлён: trail width подстраивается под размер щита (актуально для Shield Rush).
- **GameOverPopup.cs** — обновлён: показывает название режима (CLASSIC / TIME ATTACK / SHIELD RUSH).
- **GameplayManager.cs** — обновлён: использует per-mode best score в Game Over.
- **Editor/SetupGameScene.cs** — обновлён: modeNameText в GameOverPopup, Iteration 12 menu item.

## Что изменилось с предыдущей итерации
- Обновлены: `Shield.cs`, `ShieldVisuals.cs`, `GameOverPopup.cs`, `GameplayManager.cs`, `Editor/SetupGameScene.cs`
- Не изменены: все остальные скрипты

## Настройка

### 1. Скопируй/замени файлы
- `Assets/BalloonGame/Scripts/Shield.cs` — **замена**
- `Assets/BalloonGame/Scripts/ShieldVisuals.cs` — **замена**
- `Assets/BalloonGame/Scripts/GameOverPopup.cs` — **замена**
- `Assets/BalloonGame/Scripts/GameplayManager.cs` — **замена**
- `Assets/BalloonGame/Scripts/Editor/SetupGameScene.cs` — **замена**

### 2. Открой сцену Game
- Меню: **BalloonGame → (Iteration 12) Setup Game Scene — Shield Rush + Polish**
- **Ctrl+S**

## Как тестировать

### Тест 1 — Shield Rush — Большой щит
1. MainMenu → PLAY → SHIELD RUSH
2. **Ожидаемый результат**:
   - Щит значительно больше обычного (~2x)
   - Trail за щитом шире
   - Препятствия спавнятся очень часто (x3)
   - Масса препятствий уменьшена — щит легко их разбрасывает
   - Хаос и веселье!

### Тест 2 — Classic — нормальный щит
1. PLAY → CLASSIC
2. **Ожидаемый результат**: щит обычного размера, как раньше

### Тест 3 — Time Attack
1. PLAY → TIME ATTACK
2. **Ожидаемый результат**: щит обычного размера, таймер 30с, высокая сложность

### Тест 4 — Game Over mode name
1. Проиграй в любом режиме
2. **Ожидаемый результат**: Game Over popup показывает название режима (например "SHIELD RUSH")

### Тест 5 — Per-mode best scores
1. Набери разные score в разных режимах
2. MainMenu → PLAY → смотри best scores
3. **Ожидаемый результат**: best score отдельный для каждого режима

## Финальные характеристики режимов

| Параметр | Classic | Time Attack | Shield Rush |
|----------|---------|-------------|-------------|
| Таймер | Нет | 30с | Нет |
| Размер щита | 1.2x | 1.2x | 2.2x |
| Стартовая сложность | Score 0 | Score 50 | Score 0 |
| Интервал спавна | Обычный | Обычный (высокий score) | x0.35 (x3 чаще) |
| Масса препятствий | Обычная | Обычная | x0.6 (легче) |
| Все паттерны | С score 75 | Сразу (50+) | С score 75 |
| Условие проигрыша | Столкновение | Столкновение или время | Столкновение |

## Полный список всех режимов и скриптов

### Система режимов
| Скрипт | Описание |
|--------|----------|
| GameMode | Enum: Classic, TimeAttack, ShieldRush |
| GameModeManager | Singleton, хранит текущий режим |
| ModeSelectPopup | UI выбора режима |
| GameManager | Per-mode best scores |

### Все скрипты проекта (итого ~35)
**Bootstrap (DontDestroyOnLoad):** AddressableLoader, SceneLoader, MusicManager, SFXManager, HapticManager, GameModeManager
**MainMenu:** GameManager, MainMenuUI, SettingsPopup, ModeSelectPopup, ScrollingBackground, DecorBalloon
**Game:** GameCamera, ScreenShake, Balloon, BalloonTrail, BalloonBounce, Shield, ShieldVisuals, GameplayManager, ObstacleSpawner, Obstacle, ObstacleHitFlash, DifficultyManager, ParticleManager, TapToStart, WorldBounds, DeathSequence, ScrollingBackground, BackgroundGrid, GameUI, PausePopup, GameOverPopup, ScoreCounter
**Editor:** SetupBootstrapScene, SetupMainMenuScene, SetupGameScene

## Ожидаемый результат итерации
Полностью завершённая система режимов: Classic (стандартный), Time Attack (30с, высокая сложность), Shield Rush (большой щит, тонны объектов). Per-mode best scores, mode name на Game Over popup. Игра готова!
