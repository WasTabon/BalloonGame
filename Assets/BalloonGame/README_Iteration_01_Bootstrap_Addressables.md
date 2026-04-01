# Iteration 1 — Bootstrap + Addressables + Scene Flow

## Что в этой итерации
- **AddressableLoader.cs** — Singleton, DontDestroyOnLoad. Загружает музыку через Addressables с Cloudflare R2. На Android — фейковая загрузка (без реального скачивания).
- **SceneLoader.cs** — Singleton, DontDestroyOnLoad. Загрузка сцен с плавным fade-переходом (чёрный экран) через DOTween.
- **BootstrapUI.cs** — UI для Bootstrap сцены: заголовок, прогресс бар, статус текст, кнопка Retry.
- **Editor/SetupBootstrapScene.cs** — Editor скрипт для автоматической настройки Bootstrap сцены.

## Что изменилось с предыдущей итерации
Первая итерация — изменений нет.

## Пошаговая настройка

### 1. Создай проект (если ещё нет)
- Unity 2022.3.62f, Built-in 2D Renderer, вертикальная ориентация

### 2. Установи DOTween Free
- Window → Asset Store → найди "DOTween" (Demigiant) → Import
- После импорта появится окно Setup — нажми **Setup DOTween**
- Если не появилось: Tools → Demigiant → DOTween Utility Panel → Setup

### 3. Установи TextMeshPro
- Если при первом использовании TMP появится окно "TMP Essentials" — нажми **Import TMP Essentials**

### 4. Установи Addressables
- Window → Package Manager → кнопка **+** → Add package by name
- Введи: `com.unity.addressables`
- Дождись установки (версия 1.22.3+)

### 5. Настрой Addressables

#### 5.1 Создай Addressables Settings
- Window → Asset Management → Addressables → Groups
- Если попросит создать Settings — нажми **Create Addressables Settings**

#### 5.2 Создай Profile для Remote загрузки
- Window → Asset Management → Addressables → Profiles
- Создай новый профиль или отредактируй Default:
  - **Remote.BuildPath**: `ServerData/[BuildTarget]`
  - **Remote.LoadPath**: `https://YOUR_BUCKET.r2.cloudflarestorage.com/[BuildTarget]` ← замени на свой URL

#### 5.3 Настрой Group для музыки
- В окне Addressables Groups:
  - Нажми **Create** → **Packed Assets** → назови "Music"
  - Выбери группу Music → в Inspector:
    - **Build Path**: Remote.BuildPath
    - **Load Path**: Remote.LoadPath
    - **Bundle Mode**: Pack Together

#### 5.4 Добавь музыкальный файл
- Положи свой .wav файл куда угодно в Assets (например `Assets/BalloonGame/Audio/`)
- Выбери файл → в Inspector поставь галку **Addressable**
- В поле адреса введи: `GameMusic`
- Перетащи его в группу **Music**
- В Labels добавь лейбл `music` (создай если нет)

#### 5.5 Настрой Remote Catalog (для обновлений)
- Window → Asset Management → Addressables → Settings (или найди `AddressableAssetSettings` в Project)
- **Build Remote Catalog**: ✅ включи
- **Build Path**: Remote.BuildPath
- **Load Path**: Remote.LoadPath

#### 5.6 Собери Addressables
- Window → Asset Management → Addressables → Groups
- Build → **New Build** → **Default Build Script**
- В папке `ServerData/` появятся файлы — их нужно залить на Cloudflare R2

#### 5.7 Залей на Cloudflare R2
- Создай бакет в Cloudflare R2
- Загрузи ВСЕ файлы из `ServerData/[BuildTarget]/` в бакет
- URL бакета должен совпадать с Remote.LoadPath из профиля

> **Примечание**: Для тестирования в Editor можно пока пропустить шаги 5.2–5.7. Addressables в Editor по умолчанию используют FastMode и берут файлы локально. Загрузка произойдёт моментально.

### 6. Создай сцены
- Создай 3 сцены:
  - `Assets/Scenes/Bootstrap.unity`
  - `Assets/Scenes/MainMenu.unity`
  - `Assets/Scenes/Game.unity`

### 7. Добавь сцены в Build Settings
- File → Build Settings
- Добавь сцены в таком порядке:
  - 0: Bootstrap
  - 1: MainMenu
  - 2: Game

### 8. Настрой Bootstrap сцену
- Открой сцену **Bootstrap**
- Меню: **BalloonGame → (Iteration 1) Setup Bootstrap Scene**
- Готово! Все объекты созданы и связаны

### 9. Скопируй файлы в проект
- `Assets/BalloonGame/Scripts/AddressableLoader.cs`
- `Assets/BalloonGame/Scripts/SceneLoader.cs`
- `Assets/BalloonGame/Scripts/BootstrapUI.cs`
- `Assets/BalloonGame/Scripts/Editor/SetupBootstrapScene.cs`

## Как тестировать

### Тест 1 — Успешная загрузка (Editor)
1. Открой сцену Bootstrap
2. Нажми Play
3. **Ожидаемый результат**:
   - Появляется заголовок "BalloonGame" с fade-in
   - Прогресс бар заполняется
   - Статус меняется: Checking connection → Initializing → Checking for updates → Ready!
   - Экран fade-to-black и переход на сцену MainMenu
   - MainMenu пустая — это нормально

### Тест 2 — Ошибка загрузки
1. Отключи интернет
2. Нажми Play
3. **Ожидаемый результат**:
   - Статус: "No internet connection" (красным)
   - Появляется кнопка Retry с bounce-анимацией
   - Нажми Retry → повторная попытка

## Структура на сцене Bootstrap (после editor скрипта)
```
Main Camera
EventSystem
AddressableLoader          [AddressableLoader.cs]
SceneLoader                [SceneLoader.cs]
  └── FadeCanvas           [Canvas, sortingOrder=999]
       └── FadePanel       [Image black, CanvasGroup alpha=0]
BootstrapCanvas            [Canvas, BootstrapUI.cs]
  ├── TitleText            [TMP "BalloonGame"]
  ├── ProgressBarBg        [Image dark]
  │    └── ProgressBarFill [Image filled, white]
  ├── StatusText           [TMP]
  └── RetryButton          [Button]
       └── RetryText       [TMP "RETRY"]
```

## Ожидаемый результат итерации
Полностью работающий flow: Bootstrap → загрузка ресурсов через Addressables → fade-переход → MainMenu (пустая сцена). При ошибке — кнопка Retry. На Android — фейковая загрузка без музыки.
