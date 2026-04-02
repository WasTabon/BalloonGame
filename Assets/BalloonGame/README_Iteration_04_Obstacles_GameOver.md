# Iteration 4 — Obstacles, Spawning, Collisions, Game Over

## Что в этой итерации
- **Obstacle.cs** — компонент препятствия. Уничтожается при выходе за нижнюю границу камеры.
- **ObstacleSpawner.cs** — спавнит препятствия выше камеры. 3 формы: квадрат, прямоугольник-линия, круг. Случайная масса, скорость, вращение, цвет. Интервал спавна уменьшается со временем.
- **GameOverPopup.cs** — попап проигрыша: GAME OVER, SCORE, BEST, NEW BEST! индикатор, кнопки RESTART и MENU. Анимации (scale + fade + elastic для new best).
- **GameplayManager.cs** — обновлён: добавлена связь с GameOverPopup, показывает попап при смерти.
- **Editor/SetupGameScene.cs** — обновлён: добавляет ObstacleSpawner, GameOverPopup, генерирует спрайты для препятствий.

## Что изменилось с предыдущей итерации
- Добавлены: `Obstacle.cs`, `ObstacleSpawner.cs`, `GameOverPopup.cs`
- Обновлены: `GameplayManager.cs`, `Editor/SetupGameScene.cs`
- Не изменены: `Balloon.cs`, `Shield.cs`, `GameCamera.cs`, `GameUI.cs`, `PausePopup.cs`, все скрипты из итераций 1-2
- Генерируются новые спрайты: `ObstacleSquare.png`, `ObstacleRect.png`, `ObstacleCircle.png`

## Настройка

### 1. Скопируй/замени файлы
- `Assets/BalloonGame/Scripts/Obstacle.cs` — **новый**
- `Assets/BalloonGame/Scripts/ObstacleSpawner.cs` — **новый**
- `Assets/BalloonGame/Scripts/GameOverPopup.cs` — **новый**
- `Assets/BalloonGame/Scripts/GameplayManager.cs` — **замена**
- `Assets/BalloonGame/Scripts/Editor/SetupGameScene.cs` — **замена**

### 2. Открой сцену Game
- Открой `Assets/Scenes/Game.unity`

### 3. Запусти editor скрипт
- Меню: **BalloonGame → (Iteration 4) Setup Game Scene — Obstacles + GameOver**
- Скрипт создаст спрайты для препятствий, добавит ObstacleSpawner и GameOverPopup

### 4. Готово!

## Физика столкновений
- **Щит (kinematic) ↔ Препятствие (dynamic)** → щит толкает препятствие
- **Препятствие ↔ Препятствие** → физическое отскакивание между собой
- **Препятствие ↔ Шарик (trigger)** → OnTriggerEnter2D → Game Over

## Как тестировать

### Тест 1 — Полный flow
1. Bootstrap → MainMenu → PLAY
2. **Ожидаемый результат**: шарик летит вверх, препятствия спавнятся сверху

### Тест 2 — Препятствия
1. Смотри как падают препятствия
2. **Ожидаемый результат**:
   - Разные формы: квадраты, прямоугольники, круги
   - Разные размеры и цвета
   - Падают с гравитацией + случайная начальная скорость и вращение
   - Отталкиваются друг от друга

### Тест 3 — Щит толкает препятствия
1. Двигай щит к препятствию
2. **Ожидаемый результат**: щит физически толкает препятствие, оно отлетает

### Тест 4 — Game Over
1. Позволь препятствию коснуться шарика
2. **Ожидаемый результат**:
   - Появляется GAME OVER попап с анимацией
   - Показывает SCORE и BEST
   - Если новый рекорд — "NEW BEST!" с elastic-анимацией
   - RESTART → перезагрузка Game
   - MENU → возврат в MainMenu

### Тест 5 — Уничтожение за экраном
1. Посмотри в Hierarchy во время игры
2. **Ожидаемый результат**: объекты Obstacle уничтожаются когда уходят ниже камеры

## Замена спрайтов
Спрайты генерируются с `pixelsPerUnit = 128`. Чтобы заменить:
1. Замени PNG файлы в `Assets/BalloonGame/Sprites/`
2. В импорт-настройках нового спрайта поставь **Pixels Per Unit = 128**
3. Размеры задаются через `localScale` в ObstacleSpawner, так что скейл не полетит

## Структура на сцене Game (после editor скрипта)
```
Main Camera                [Camera, GameCamera.cs]
EventSystem
Balloon                    [SpriteRenderer, Rigidbody2D kinematic, CircleCollider2D trigger, Balloon.cs]
Shield                     [SpriteRenderer, Rigidbody2D kinematic, CircleCollider2D, Shield.cs]
GameplayManager            [GameplayManager.cs]
ObstacleSpawner            [ObstacleSpawner.cs]
GameCanvas                 [Canvas, GameUI.cs]
  ├── ScoreText            [TMP "0"]
  ├── PauseButton          [Button "| |"]
  ├── PausePopup           [скрыт]
  │    ├── DimBg
  │    └── Panel
  │         ├── PausedTitle
  │         ├── ResumeButton
  │         └── MenuButton
  └── GameOverPopup        [скрыт]
       ├── DimBg
       └── Panel
            ├── GameOverTitle  [TMP "GAME OVER"]
            ├── ScoreLabel     [TMP "SCORE"]
            ├── ScoreValue     [TMP "0"]
            ├── BestLabel      [TMP "BEST"]
            ├── BestValue      [TMP "0"]
            ├── NewBestText    [TMP "NEW BEST!", скрыт]
            ├── RestartButton  [Button "RESTART"]
            └── MenuButton     [Button "MENU"]
```

## Ожидаемый результат итерации
Полностью играбельный core loop: шарик летит вверх, препятствия спавнятся и падают с физикой, щит толкает их, при касании шарика — Game Over с попапом, Restart мгновенный. Каждый забег уникальный благодаря случайной генерации.
