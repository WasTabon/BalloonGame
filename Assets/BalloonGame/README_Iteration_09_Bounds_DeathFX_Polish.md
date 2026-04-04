# Iteration 9 — World Bounds, Slow-Mo Death, UI Polish

## Что в этой итерации
- **WorldBounds.cs** — новый. Невидимые стены по бокам экрана (EdgeCollider2D), следуют за камерой. Препятствия отскакивают от стен → больше хаоса.
- **DeathSequence.cs** — новый. Slow-mo при смерти: timeScale 1.0 → 0.2 → пауза → Game Over popup. Драматичная смерть.
- **BalloonTrail.cs** — новый. Лёгкий розовый trail за шариком во время полёта.
- **GameCamera.cs** — обновлён: плавный zoom-out при высоком score (10 → 12 ortho size к 100 очкам).
- **GameplayManager.cs** — обновлён: death вызывает DeathSequence вместо мгновенного game over.
- **Editor/SetupGameScene.cs** — обновлён: добавляет WorldBounds, DeathSequence, BalloonTrail.
- **Editor/SetupMainMenuScene.cs** — обновлён: добавляет декоративный фон на MainMenu.

## Что изменилось с предыдущей итерации
- Добавлены: `WorldBounds.cs`, `DeathSequence.cs`, `BalloonTrail.cs`
- Обновлены: `GameCamera.cs`, `GameplayManager.cs`, `Editor/SetupGameScene.cs`, `Editor/SetupMainMenuScene.cs`
- Не изменены: все остальные скрипты

## Настройка

### 1. Скопируй/замени файлы
- `Assets/BalloonGame/Scripts/WorldBounds.cs` — **новый**
- `Assets/BalloonGame/Scripts/DeathSequence.cs` — **новый**
- `Assets/BalloonGame/Scripts/BalloonTrail.cs` — **новый**
- `Assets/BalloonGame/Scripts/GameCamera.cs` — **замена**
- `Assets/BalloonGame/Scripts/GameplayManager.cs` — **замена**
- `Assets/BalloonGame/Scripts/Editor/SetupGameScene.cs` — **замена**
- `Assets/BalloonGame/Scripts/Editor/SetupMainMenuScene.cs` — **замена**

### 2. Открой сцену Game
- Меню: **BalloonGame → (Iteration 9) Setup Game Scene — Bounds + DeathFX + Polish**
- **Ctrl+S**

### 3. Открой сцену MainMenu
- Меню: **BalloonGame → (Iteration 2) Setup MainMenu Scene**
- **Ctrl+S**

## Как тестировать

### Тест 1 — World Bounds
1. Играй и наблюдай за препятствиями
2. **Ожидаемый результат**:
   - Препятствия отскакивают от невидимых стен по бокам экрана
   - Больше хаотичной физики
   - Препятствия не улетают за пределы видимости

### Тест 2 — Slow-Mo Death
1. Позволь препятствию коснуться шарика
2. **Ожидаемый результат**:
   - Время замедляется (slow-mo)
   - Взрыв частиц виден в замедленном режиме
   - Через ~0.7с появляется Game Over popup
   - Время возвращается к нормальному

### Тест 3 — Balloon Trail
1. Наблюдай за шариком во время полёта
2. **Ожидаемый результат**: розовый полупрозрачный trail за шариком

### Тест 4 — Camera Zoom-Out
1. Набери 50+ очков
2. **Ожидаемый результат**: камера плавно отдаляется, видно больше пространства

### Тест 5 — MainMenu Background
1. Открой MainMenu
2. **Ожидаемый результат**: декоративные точки с параллаксом на фоне меню

## Структура на сцене Game (после editor скрипта)
```
Main Camera                [Camera, GameCamera.cs (zoom-out), ScreenShake.cs]
EventSystem
ScrollingBackground        [ScrollingBackground.cs]
BackgroundGrid             [BackgroundGrid.cs]
WorldBounds                [WorldBounds.cs — невидимые стены]
DeathSequence              [DeathSequence.cs — slow-mo]
Balloon                    [Balloon.cs, BalloonTrail.cs (trail)]
Shield                     [Shield.cs, ShieldVisuals.cs]
GameplayManager            [GameplayManager.cs]
ObstacleSpawner            [ObstacleSpawner.cs]
DifficultyManager          [DifficultyManager.cs]
ParticleManager            [ParticleManager.cs]
TapToStart                 [TapToStart.cs]
GameCanvas                 [Canvas, GameUI.cs]
```

## Структура на сцене MainMenu (после editor скрипта)
```
Main Camera
EventSystem
GameManager                [GameManager.cs]
MenuBackground             [ScrollingBackground.cs — декоративный фон]
MainMenuCanvas             [Canvas, MainMenuUI.cs]
```

## Ожидаемый результат итерации
Полированная игра: невидимые стены создают больше хаоса, смерть драматичная с slow-mo, шарик оставляет trail, камера плавно отдаляется при высоком score, MainMenu с декоративным фоном.
