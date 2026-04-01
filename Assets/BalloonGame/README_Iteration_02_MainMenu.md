# Iteration 2 — Main Menu

## Что в этой итерации
- **GameManager.cs** — Singleton, DontDestroyOnLoad. Хранит BestScore и SoundEnabled в PlayerPrefs. Переподписывается при смене сцены.
- **MainMenuUI.cs** — UI контроллер главного меню. Анимации появления: title slide+fade, best score fade, play button bounce + pulse, settings button.
- **SettingsPopup.cs** — Попап настроек с анимацией открытия (scale+dim) и закрытия. Sound toggle с punch-эффектом.
- **Editor/SetupMainMenuScene.cs** — Editor скрипт для автоматической настройки MainMenu сцены.

## Что изменилось с предыдущей итерации
- Добавлены 4 новых скрипта
- Bootstrap сцена не изменена
- Файлы из итерации 1 не тронуты

## Настройка

### 1. Скопируй файлы в проект
- `Assets/BalloonGame/Scripts/GameManager.cs`
- `Assets/BalloonGame/Scripts/MainMenuUI.cs`
- `Assets/BalloonGame/Scripts/SettingsPopup.cs`
- `Assets/BalloonGame/Scripts/Editor/SetupMainMenuScene.cs`

### 2. Открой сцену MainMenu
- Открой `Assets/Scenes/MainMenu.unity`

### 3. Запусти editor скрипт
- Меню: **BalloonGame → (Iteration 2) Setup MainMenu Scene**

### 4. Готово!

## Как тестировать

### Тест 1 — Полный flow (из Bootstrap)
1. Открой сцену **Bootstrap**
2. Play
3. **Ожидаемый результат**:
   - Bootstrap загрузка → fade → MainMenu
   - Заголовок "BalloonGame" появляется с анимацией (slide сверху + fade)
   - "BEST: 0" появляется с fade
   - Кнопка PLAY появляется с bounce и начинает пульсировать
   - Кнопка ⚙ появляется

### Тест 2 — Прямой запуск MainMenu
1. Открой сцену **MainMenu**
2. Play
3. **Ожидаемый результат**: то же что выше (GameManager создаётся на этой сцене)

### Тест 3 — Settings popup
1. Нажми кнопку ⚙
2. **Ожидаемый результат**:
   - Появляется затемнение (dim) с fade
   - Панель настроек появляется с scale-анимацией (bounce)
   - "SOUND: ON" кнопка
3. Нажми SOUND — текст меняется на "SOUND: OFF", кнопка punch-эффект
4. Нажми CLOSE — панель закрывается с обратной анимацией

### Тест 4 — Play button
1. Нажми PLAY
2. **Ожидаемый результат**:
   - Кнопка делает press-анимацию (scale down → up)
   - Fade to black → переход на Game сцену (пустая)

## Структура на сцене MainMenu (после editor скрипта)
```
Main Camera
EventSystem
GameManager                    [GameManager.cs]
MainMenuCanvas                 [Canvas, MainMenuUI.cs]
  ├── TitleText                [TMP "BalloonGame", fontSize 80]
  ├── BestScoreText            [TMP "BEST: 0", fontSize 36]
  ├── PlayButton               [Button, зелёный]
  │    └── PlayText            [TMP "PLAY"]
  ├── SettingsButton           [Button, правый верхний угол]
  │    └── SettingsText        [TMP "⚙"]
  └── SettingsPopup            [неактивен по умолчанию]
       ├── DimBg               [Image чёрный 60%, CanvasGroup]
       └── Panel               [Image тёмный]
            ├── SettingsTitle  [TMP "SETTINGS"]
            ├── SoundToggleButton [Button]
            │    └── SoundToggleText [TMP "SOUND: ON"]
            └── CloseButton    [Button, красный]
                 └── CloseText [TMP "CLOSE"]
```

## Ожидаемый результат итерации
Полностью работающее главное меню с анимациями: Bootstrap → MainMenu с заголовком, best score, кнопкой Play (переход на Game), кнопкой Settings (popup с toggle звука). Все переходы с fade-анимацией.
