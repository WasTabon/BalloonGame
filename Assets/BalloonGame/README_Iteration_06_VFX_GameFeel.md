# Iteration 6 — VFX, Particles, Screen Shake, Game Feel

## Что в этой итерации
- **ScreenShake.cs** — новый. Тряска камеры через DOTween: Light (столкновение щита), Medium (milestone), Heavy (game over).
- **ParticleManager.cs** — новый. Программно создаёт все системы частиц (Built-in 2D):
  - ShieldHit — искры в точке контакта щита с препятствием
  - BalloonPop — взрыв шарика при смерти (розовые частицы)
  - ScoreMilestone — золотые частицы каждые 10 очков
- **ShieldVisuals.cs** — новый. Trail за щитом + punch scale + flash при столкновении.
- **Shield.cs** — обновлён: вызывает ScreenShake, ParticleManager, ShieldVisuals при столкновении с препятствием.
- **Balloon.cs** — обновлён: взрыв частиц + тряска + скрытие спрайта при смерти.
- **GameUI.cs** — обновлён: milestone эффект каждые 10 очков (золотой flash текста + частицы + тряска).
- **Editor/SetupGameScene.cs** — обновлён: добавляет ScreenShake на камеру, ShieldVisuals на щит, ParticleManager на сцену.

## Что изменилось с предыдущей итерации
- Добавлены: `ScreenShake.cs`, `ParticleManager.cs`, `ShieldVisuals.cs`
- Обновлены: `Shield.cs`, `Balloon.cs`, `GameUI.cs`, `Editor/SetupGameScene.cs`
- Не изменены: `Obstacle.cs`, `ObstacleSpawner.cs`, `DifficultyManager.cs`, `GameplayManager.cs`, `GameCamera.cs`, `PausePopup.cs`, `GameOverPopup.cs`, все скрипты из итераций 1-2

## Настройка

### 1. Скопируй/замени файлы
- `Assets/BalloonGame/Scripts/ScreenShake.cs` — **новый**
- `Assets/BalloonGame/Scripts/ParticleManager.cs` — **новый**
- `Assets/BalloonGame/Scripts/ShieldVisuals.cs` — **новый**
- `Assets/BalloonGame/Scripts/Shield.cs` — **замена**
- `Assets/BalloonGame/Scripts/Balloon.cs` — **замена**
- `Assets/BalloonGame/Scripts/GameUI.cs` — **замена**
- `Assets/BalloonGame/Scripts/Editor/SetupGameScene.cs` — **замена**

### 2. Открой сцену Game

### 3. Запусти editor скрипт
- Меню: **BalloonGame → (Iteration 6) Setup Game Scene — VFX + Game Feel**

### 4. Ctrl+S — сохрани сцену

## Как тестировать

### Тест 1 — Столкновение щита
1. Двигай щит в препятствие
2. **Ожидаемый результат**:
   - Искры/частицы в точке контакта (цвет препятствия)
   - Лёгкая тряска камеры
   - Щит делает punch scale + белый flash
   - За щитом остаётся лёгкий trail

### Тест 2 — Game Over
1. Позволь препятствию коснуться шарика
2. **Ожидаемый результат**:
   - Шарик исчезает
   - Взрыв розовых частиц
   - Сильная тряска камеры
   - Появляется Game Over попап

### Тест 3 — Score Milestone
1. Набери 10 очков
2. **Ожидаемый результат**:
   - Текст score становится золотым на мгновение
   - Увеличенный punch scale текста
   - Золотые частицы вверху экрана
   - Средняя тряска камеры
3. Повторяется на 20, 30, 40...

### Тест 4 — Trail щита
1. Двигай щит быстро
2. **Ожидаемый результат**: голубой trail за щитом

## Все VFX эффекты
| Событие | Частицы | Тряска | Другое |
|---------|---------|--------|--------|
| Щит ↔ Препятствие | Искры в цвет препятствия | Light | Punch scale + flash |
| Game Over | Розовый взрыв (25 частиц) | Heavy | Шарик исчезает |
| Score Milestone (×10) | Золотые частицы | Medium | Текст gold flash |

## Структура на сцене Game (после editor скрипта)
```
Main Camera                [Camera, GameCamera.cs, ScreenShake.cs]
EventSystem
Balloon                    [Balloon.cs — взрыв при смерти]
Shield                     [Shield.cs, ShieldVisuals.cs, TrailRenderer]
GameplayManager            [GameplayManager.cs]
ObstacleSpawner            [ObstacleSpawner.cs]
DifficultyManager          [DifficultyManager.cs]
ParticleManager            [ParticleManager.cs]
GameCanvas                 [Canvas, GameUI.cs — milestone эффекты]
  ├── ScoreText
  ├── PauseButton
  ├── PausePopup
  └── GameOverPopup
```

## Ожидаемый результат итерации
Игра ощущается значительно сочнее: каждое столкновение сопровождается частицами и тряской, Game Over — эффектный взрыв, milestone — праздничные частицы. Щит оставляет trail, делает flash при ударе. Все частицы создаются программно, ничего дополнительно настраивать не нужно.
