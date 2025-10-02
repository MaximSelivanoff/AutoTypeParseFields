# AutoTypeParseFields Plugin / Плагин AutoTypeParseFields

## English

**AutoTypeParseFields** is a KeePass 2.x plugin that extends the standard Auto-Type functionality by adding support for custom `{PARSE:...}` tags. These tags allow you to extract and transform parts of standard and custom entry fields directly in Auto-Type templates.

### Supported Fields

* Standard fields: TITLE, USERNAME, PASSWORD, URL, NOTES
* Custom fields (user-defined strings)

### Supported Operations

* `WORD:N` - returns the N-th word from the field (1-based index)
* `BEFORE:<String>` - returns the substring before a specified character or string

**Example:**

```
Title = "Ivan Sergeevich Petrov"
{PARSE:TITLE:WORD:2} -> "Sergeevich"
Username = "ivan.petrov@example.com"
{PARSE:USERNAME:BEFORE:@} -> "ivan.petrov"
```

### Installation

1. Place `AutoTypeParseFields.dll` and/or `AutoTypeParseFields.plgx` into KeePass `Plugins` folder.
2. Restart KeePass and check **Tools → Plugins**.

### Notes

* All operations occur in memory; no sensitive data is saved outside KeePass.
* PLGX files are optional, DLL is recommended for security reasons.

---

## Русский

**AutoTypeParseFields** — плагин для KeePass 2.x, расширяющий стандартный механизм автотайпа (Auto-Type) с поддержкой кастомных тегов `{PARSE:...}`. Теги позволяют извлекать и преобразовывать части стандартных и пользовательских полей записи прямо в шаблонах автотайпа.

### Поддерживаемые поля

* Стандартные: TITLE, USERNAME, PASSWORD, URL, NOTES
* Пользовательские поля (Custom String Fields)

### Поддерживаемые операции

* `WORD:N` — возвращает N-е слово из значения поля (нумерация с 1)
* `BEFORE:<String>` — возвращает часть значения до указанного символа или подстроки

**Пример:**

```
Title = "Иван Сергеевич Петров"
{PARSE:TITLE:WORD:2} -> "Сергеевич"
Username = "ivan.petrov@example.com"
{PARSE:USERNAME:BEFORE:@} -> "ivan.petrov"
```

### Установка

1. Скопируйте `AutoTypeParseFields.dll` и/или `AutoTypeParseFields.plgx` в папку `Plugins` KeePass.
2. Перезапустите KeePass и проверьте **Инструменты → Плагины**.

### Примечания

* Все операции выполняются в памяти; данные не сохраняются вне KeePass.
* PLGX файлы опциональны, для безопасности рекомендуется использовать DLL.
