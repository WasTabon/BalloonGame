# Iteration 8 — Background, Tap To Start, Final Balance

## Что в этой итерации
- **ScrollingBackground.cs** — новый. Параллакс-фон из точек в 3 слоя с разной скоростью и прозрачностью. Создаётся программно.
- **BackgroundGrid.cs** — новый. Тонкие горизонтальные линии-сетка, скроллятся вниз, подчёркивают ощущение скорости.
- **TapToStart.cs** — новый. При старте Game сцены шарик стоит на месте, текст "TAP TO START" пульсирует. После тапа — игра начинается.
- **Balloon.cs** — обновлён: ждёт TapToStart перед движением.
- **ObstacleSpawner.cs** — обновлён: не спавнит пока TapToStart не активирован.
- **DifficultyManager.cs** — обновлён: более плавная прогрессия (eased кривая, макс score 150 вместо 100).
- **Editor/SetupGameScene.cs** — обновлён: добавляет фон, TapToStart.

## Что изменилось с предыдущей итерации
- Добавлены: `ScrollingBackground.cs`, `BackgroundGrid.cs`, `TapToStart.cs`
- Обновлены: `Balloon.cs`, `ObstacleSpawner.cs`, `DifficultyManager.cs`, `Editor/SetupGameScene.cs`
- Не изменены: все остальные скрипты

## Настройка

### 1. Скопируй/замени файлы
- `Assets/BalloonGame/Scripts/ScrollingBackground.cs` — **новый**
- `Assets/BalloonGame/Scripts/BackgroundGrid.cs` — **новый**
- `Assets/BalloonGame/Scripts/TapToStart.cs` — **новый**
- `Assets/BalloonGame/Scripts/Balloon.cs` — **замена**
- `Assets/BalloonGame/Scripts/ObstacleSpawner.cs` — **замена**
- `Assets/BalloonGame/Scripts/DifficultyManager.cs` — **замена**
- `Assets/BalloonGame/Scripts/Editor/SetupGameScene.cs` — **замена**

### 2. Открой сцену Game

### 3. Запусти editor скрипт
- Меню: **BalloonGame → (Iteration 8) Setup Game Scene — Background + TapToStart**

### 4. Ctrl+S — сохрани

## Как тестировать

### Тест 1 — Tap To Start
1. Bootstrap → MainMenu → Play
2. **Ожидаемый результат**:
   - Шарик стоит на месте
   - "TAP TO START" пульсирует
   - Препятствия НЕ спавнятся
3. Тапни экран
4. **Ожидаемый результат**:
   - Текст fade out
   - Шарик начинает лететь
   - Препятствия начинают спавниться

### Тест 2 — Фон
1. Во время игры наблюдай за фоном
2. **Ожидаемый результат**:
   - Мелкие точки с параллаксом (3 слоя, разная скорость)
   - Тонкие горизонтальные линии скроллятся вниз
   - Создаёт ощущение движения вверх

### Тест 3 — Прогрессия баланса
1. Играй и наблюдай за сложностью
2. **Ожидаемый результат**:
   - Первые 20 очков — мягкий старт
   - Скорость растёт плавно (eased кривая, не линейно)
   - Полная сложность к 150 очкам

## Прогрессия сложности (обновлённая)
| Score | Скорость шарика | Интервал спавна | Паттерны |
|-------|-----------------|-----------------|----------|
| 0 | 3.0 | 1.2s | Single |
| 8 | 3.01 | 1.14s | + Line |
| 18 | 3.04 | 1.06s | + Rain |
| 30 | 3.12 | 0.96s | + Side |
| 50 | 3.33 | 0.80s | + Narrow |
| 75 | 3.75 | 0.60s | + Trap |
| 150 | 6.0 | 0.40s | All |

## Структура на сцене Game (после editor скрипта)
```
Main Camera                [Camera, GameCamera.cs, ScreenShake.cs]
EventSystem
ScrollingBackground        [ScrollingBackground.cs — точки с параллаксом]
BackgroundGrid             [BackgroundGrid.cs — скроллящаяся сетка]
Balloon                    [Balloon.cs — ждёт TapToStart]
Shield                     [Shield.cs, ShieldVisuals.cs]
GameplayManager            [GameplayManager.cs]
ObstacleSpawner            [ObstacleSpawner.cs — ждёт TapToStart]
DifficultyManager          [DifficultyManager.cs — плавная прогрессия]
ParticleManager            [ParticleManager.cs]
TapToStart                 [TapToStart.cs]
GameCanvas                 [Canvas, GameUI.cs]
  ├── ScoreText
  ├── PauseButton
  ├── TapToStartText       [TMP "TAP TO START", пульсирует]
  ├── PausePopup
  └── GameOverPopup
```

## Ожидаемый результат итерации
Визуально полированная игра: параллакс-фон с точками и сеткой, "TAP TO START" перед началом, плавная прогрессия сложности. Игра ощущается завершённой.
