# Iteration 3 — Core Gameplay: Balloon, Camera, Shield

## Что в этой итерации
- **Balloon.cs** — шарик автоматически летит вверх с лёгким горизонтальным покачиванием. Rigidbody2D kinematic, CircleCollider2D trigger (для обнаружения столкновений с препятствиями).
- **Shield.cs** — щит следует за пальцем/мышью через Rigidbody2D.MovePosition с lerp. CircleCollider2D (не trigger — будет физически толкать препятствия). Проверка IsPointerOverUI.
- **GameCamera.cs** — плавно следует за шариком, шарик отображается чуть ниже центра экрана.
- **GameplayManager.cs** — управление состоянием игры, подсчёт score на основе высоты шарика.
- **GameUI.cs** — отображение score вверху экрана, кнопка паузы с анимациями появления.
- **PausePopup.cs** — попап паузы с dim background, кнопки Resume и Menu. Анимации работают при timeScale=0.
- **Editor/SetupGameScene.cs** — editor скрипт для настройки Game сцены. Генерирует circle спрайты для шарика и щита.

## Что изменилось с предыдущей итерации
- Добавлены 7 новых скриптов
- Генерируются 2 спрайта в Assets/BalloonGame/Sprites/
- Сцены Bootstrap и MainMenu не изменены

## Настройка

### 1. Скопируй файлы в проект
- `Assets/BalloonGame/Scripts/Balloon.cs`
- `Assets/BalloonGame/Scripts/Shield.cs`
- `Assets/BalloonGame/Scripts/GameCamera.cs`
- `Assets/BalloonGame/Scripts/GameplayManager.cs`
- `Assets/BalloonGame/Scripts/GameUI.cs`
- `Assets/BalloonGame/Scripts/PausePopup.cs`
- `Assets/BalloonGame/Scripts/Editor/SetupGameScene.cs`

### 2. Открой сцену Game
- Открой `Assets/Scenes/Game.unity`

### 3. Запусти editor скрипт
- Меню: **BalloonGame → (Iteration 3) Setup Game Scene**
- Скрипт автоматически создаст circle спрайты в Assets/BalloonGame/Sprites/

### 4. Готово!

## Как тестировать

### Тест 1 — Полный flow
1. Открой сцену **Bootstrap** → Play
2. Bootstrap загрузка → MainMenu → нажми PLAY
3. **Ожидаемый результат**: переход на Game сцену

### Тест 2 — Прямой запуск Game (без GameManager)
1. Открой сцену **Game** → Play
2. **Примечание**: будет Assert ошибка т.к. GameManager не существует. Для тестирования лучше запускать из Bootstrap или MainMenu.

### Тест 3 — Шарик и камера
1. Запусти через Bootstrap → MainMenu → Play
2. **Ожидаемый результат**:
   - Розовый шарик автоматически летит вверх
   - Шарик слегка покачивается по горизонтали
   - Камера плавно следует за шариком
   - Шарик находится чуть ниже центра экрана

### Тест 4 — Щит
1. Нажми и держи палец/мышь на экране
2. **Ожидаемый результат**:
   - Голубой полупрозрачный щит (больше шарика) плавно следует за курсором
   - Щит движется с лёгким сглаживанием (не телепортируется)

### Тест 5 — Score
1. Наблюдай за числом вверху экрана
2. **Ожидаемый результат**: score увеличивается по мере набора высоты шариком

### Тест 6 — Пауза
1. Нажми кнопку "| |" в левом верхнем углу
2. **Ожидаемый результат**:
   - Игра замирает
   - Появляется попап PAUSED с анимацией
   - RESUME — продолжает игру
   - MENU — возвращает в главное меню

## Структура на сцене Game (после editor скрипта)
```
Main Camera                [Camera, GameCamera.cs]
EventSystem
Balloon                    [SpriteRenderer розовый, Rigidbody2D kinematic, CircleCollider2D trigger, Balloon.cs]
Shield                     [SpriteRenderer голубой, Rigidbody2D kinematic, CircleCollider2D, Shield.cs]
GameplayManager            [GameplayManager.cs]
GameCanvas                 [Canvas, GameUI.cs]
  ├── ScoreText            [TMP "0", fontSize 72]
  ├── PauseButton          [Button "| |", верх слева]
  └── PausePopup           [скрыт]
       ├── DimBg           [Image чёрный 60%, CanvasGroup]
       └── Panel           [Image тёмный]
            ├── PausedTitle    [TMP "PAUSED"]
            ├── ResumeButton   [Button зелёный "RESUME"]
            └── MenuButton     [Button красный "MENU"]
```

## Ожидаемый результат итерации
Играбельная сцена: шарик летит вверх с покачиванием, камера следует, щит управляется пальцем/мышью, score считается от высоты, пауза работает с Resume и Menu. Препятствий пока нет — они будут в следующей итерации.
