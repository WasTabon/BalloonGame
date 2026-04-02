# Iteration 5 — Spawn Patterns, Obstacle Variety, Difficulty Progression

## Что в этой итерации
- **DifficultyManager.cs** — новый. Управляет прогрессией: скорость шарика, частота спавна, скорость/масса препятствий, разблокировка паттернов по score.
- **ObstacleSpawner.cs** — полная переработка. 6 паттернов спавна вместо одиночных объектов.
- **Balloon.cs** — скорость теперь берётся из DifficultyManager (растёт от 3 до 6).
- **GameplayManager.cs** — обновляет DifficultyManager при изменении score.
- **Editor/SetupGameScene.cs** — добавляет DifficultyManager на сцену.

## Что изменилось с предыдущей итерации
- Добавлен: `DifficultyManager.cs`
- Обновлены: `ObstacleSpawner.cs`, `Balloon.cs`, `GameplayManager.cs`, `Editor/SetupGameScene.cs`
- Не изменены: `Obstacle.cs`, `Shield.cs`, `GameCamera.cs`, `GameUI.cs`, `PausePopup.cs`, `GameOverPopup.cs`, все скрипты из итераций 1-2

## Паттерны спавна

| Паттерн | Разблокировка | Описание |
|---------|---------------|----------|
| **Single** | Score 0+ | Одиночный случайный объект сверху |
| **Line** | Score 10+ | Горизонтальная линия из 3-5 блоков с одним проходом |
| **Rain** | Score 20+ | 5-8 мелких объектов одновременно |
| **Side** | Score 30+ | 1-3 объекта летят горизонтально с боков |
| **Narrow** | Score 50+ | Два длинных прямоугольника с узким проходом |
| **Trap** | Score 70+ | Комбо: линия + боковые объекты |

## Прогрессия сложности (0 → 100 score)
- Скорость шарика: 3 → 6
- Интервал спавна: 1.2s → 0.4s
- Скорость препятствий: 1 → 4
- Масса препятствий: 0.5 → 4

## Настройка

### 1. Скопируй/замени файлы
- `Assets/BalloonGame/Scripts/DifficultyManager.cs` — **новый**
- `Assets/BalloonGame/Scripts/ObstacleSpawner.cs` — **замена**
- `Assets/BalloonGame/Scripts/Balloon.cs` — **замена**
- `Assets/BalloonGame/Scripts/GameplayManager.cs` — **замена**
- `Assets/BalloonGame/Scripts/Editor/SetupGameScene.cs` — **замена**

### 2. Открой сцену Game

### 3. Запусти editor скрипт
- Меню: **BalloonGame → (Iteration 5) Setup Game Scene — Patterns + Difficulty**

## Как тестировать

### Тест 1 — Полный flow
1. Bootstrap → MainMenu → PLAY
2. **Ожидаемый результат**: игра запускается, препятствия спавнятся

### Тест 2 — Прогрессия паттернов
1. Играй и набирай score
2. **Ожидаемый результат**:
   - Score 0-9: только одиночные объекты
   - Score 10+: появляются линии блоков с проходом
   - Score 20+: дождь из мелких объектов
   - Score 30+: объекты летят с боков
   - Score 50+: узкие проходы
   - Score 70+: комбо-ловушки

### Тест 3 — Увеличение скорости
1. Наблюдай за скоростью шарика
2. **Ожидаемый результат**: шарик постепенно ускоряется, препятствия становятся быстрее и тяжелее

### Тест 4 — Хаотичная физика
1. Используй щит чтобы толкать препятствия
2. **Ожидаемый результат**: объекты отскакивают друг от друга, создают цепные реакции

## Структура на сцене Game (после editor скрипта)
```
Main Camera                [Camera, GameCamera.cs]
EventSystem
Balloon                    [Balloon.cs — скорость из DifficultyManager]
Shield                     [Shield.cs]
GameplayManager            [GameplayManager.cs]
ObstacleSpawner            [ObstacleSpawner.cs — 6 паттернов]
DifficultyManager          [DifficultyManager.cs — прогрессия]
GameCanvas                 [Canvas, GameUI.cs]
  ├── ScoreText
  ├── PauseButton
  ├── PausePopup
  └── GameOverPopup
```

## Ожидаемый результат итерации
Разнообразный геймплей с 6 паттернами спавна, которые разблокируются по мере набора очков. Сложность плавно растёт: шарик ускоряется, препятствия становятся быстрее и тяжелее, паттерны — сложнее. Каждый забег уникальный.
