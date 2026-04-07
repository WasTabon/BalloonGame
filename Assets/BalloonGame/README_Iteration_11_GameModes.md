# Iteration 11 — Game Modes System + Time Attack

## Что в этой итерации
- **GameMode.cs** — новый. Enum: Classic, TimeAttack, ShieldRush.
- **GameModeManager.cs** — новый. Singleton, DontDestroyOnLoad. Хранит выбранный режим.
- **ModeSelectPopup.cs** — новый. Попап выбора режима на MainMenu. 3 кнопки с описаниями и best scores per mode.
- **GameManager.cs** — обновлён: хранит best score для каждого режима отдельно (PlayerPrefs).
- **MainMenuUI.cs** — обновлён: Play открывает ModeSelectPopup вместо прямого перехода.
- **DifficultyManager.cs** — обновлён: Time Attack стартует с высокой сложности (+50 score offset), Shield Rush — спавн x3.
- **GameUI.cs** — обновлён: таймер для Time Attack (30с, обратный отсчёт, красный при <10с), mode label вверху.
- **Editor скрипты** — обновлены: Bootstrap добавляет GameModeManager, Game добавляет timer + mode label, MainMenu добавляет ModeSelectPopup.

## Что изменилось с предыдущей итерации
- Добавлены: `GameMode.cs`, `GameModeManager.cs`, `ModeSelectPopup.cs`
- Обновлены: `GameManager.cs`, `MainMenuUI.cs`, `DifficultyManager.cs`, `GameUI.cs`, все 3 editor скрипта
- Не изменены: все остальные скрипты

## Настройка

### 1. Скопируй/замени файлы
- `Assets/BalloonGame/Scripts/GameMode.cs` — **новый**
- `Assets/BalloonGame/Scripts/GameModeManager.cs` — **новый**
- `Assets/BalloonGame/Scripts/ModeSelectPopup.cs` — **новый**
- `Assets/BalloonGame/Scripts/GameManager.cs` — **замена**
- `Assets/BalloonGame/Scripts/MainMenuUI.cs` — **замена**
- `Assets/BalloonGame/Scripts/DifficultyManager.cs` — **замена**
- `Assets/BalloonGame/Scripts/GameUI.cs` — **замена**
- `Assets/BalloonGame/Scripts/Editor/SetupBootstrapScene.cs` — **замена**
- `Assets/BalloonGame/Scripts/Editor/SetupMainMenuScene.cs` — **замена**
- `Assets/BalloonGame/Scripts/Editor/SetupGameScene.cs` — **замена**

### 2. Настрой все 3 сцены

**Bootstrap:**
- Открой Bootstrap → **BalloonGame → (Iteration 1) Setup Bootstrap Scene** → Ctrl+S

**MainMenu:**
- Открой MainMenu → **BalloonGame → (Iteration 2) Setup MainMenu Scene** → Ctrl+S

**Game:**
- Открой Game → **BalloonGame → (Iteration 11) Setup Game Scene — Game Modes** → Ctrl+S

## Как тестировать

### Тест 1 — Mode Select
1. MainMenu → нажми PLAY
2. **Ожидаемый результат**:
   - Появляется попап SELECT MODE с анимацией
   - 3 кнопки: CLASSIC (зелёная), TIME ATTACK (оранжевая), SHIELD RUSH (синяя)
   - Каждая с описанием и best score

### Тест 2 — Classic Mode
1. Выбери CLASSIC
2. **Ожидаемый результат**: обычная игра, как раньше. Mode label "CLASSIC" вверху.

### Тест 3 — Time Attack
1. Выбери TIME ATTACK
2. **Ожидаемый результат**:
   - Mode label "TIME ATTACK"
   - Таймер "30" под score
   - После TAP TO START таймер тикает
   - Сложность сразу высокая (все паттерны доступны)
   - Когда <10с — таймер красный и пульсирует
   - На 0 — Game Over

### Тест 4 — Shield Rush (пока без большого щита)
1. Выбери SHIELD RUSH
2. **Ожидаемый результат**:
   - Mode label "SHIELD RUSH"
   - Препятствия спавнятся ~3x чаще
   - Много хаоса

### Тест 5 — Best Scores per Mode
1. Поиграй в разных режимах, набери score
2. Вернись в MainMenu → PLAY
3. **Ожидаемый результат**: best score показан для каждого режима отдельно

## Режимы

| Режим | Описание | Таймер | Сложность |
|-------|----------|--------|-----------|
| Classic | Стандартный бесконечный | Нет | Плавная прогрессия от 0 |
| Time Attack | 30 секунд, макс score | 30с обратный отсчёт | Стартует с score 50 (высокая) |
| Shield Rush | Огромный щит, тонны объектов | Нет | Спавн x3, масса x0.6 |

## Ожидаемый результат итерации
Система режимов: выбор на MainMenu, per-mode best scores, Time Attack с таймером и высокой сложностью, Shield Rush с частым спавном. Следующая итерация — большой щит для Shield Rush + финальная полировка.
