# Trust Calibration Lab

Интерактивная учебная лаборатория для курса **Secure Software Development**. Проект помогает студентам критически оценивать комментарии AI code reviewer: проверять утверждения по исходному коду, формулировать собственное решение, указывать доказательства и понимать, где AI-ревью требует проверки.

> Это polished functional prototype для учебной демонстрации, а не production-платформа.

## Возможности

- Каталог из трёх учебных case files.
- Read-only C# code viewer с номерами строк и переходом из AI-комментария к строке.
- Оценка комментариев: True, False, Partially true.
- Confidence: Low, Medium, High.
- Evidence type и обязательное текстовое объяснение.
- Сохранение draft review в SQLite.
- Progress indicator и финальная отправка review.
- Results report с accuracy, feedback и confidence calibration.
- Server-side answer key, который не передаётся студенту до submission.
- Independent vulnerability findings и CWE matching API.
- Светлая editorial-тема с charcoal-текстом и красным акцентом.

## Стек

**Frontend:** React 19, TypeScript, Vite, React Router, CSS.

**Backend:** ASP.NET Core Web API, .NET 10, Entity Framework Core, SQLite.

## Структура

```text
frontend/
  src/api/                 HTTP-клиент
  src/main.tsx             catalog, workspace и results flow
  src/*.css                дизайн-система и темы
backend/TrustCalibrationLab.Api/
  Controllers/             API endpoints
  Data/                    DbContext, seed и answer key
  Models/                  cases, submissions, answers, findings
  Dockerfile               Docker-сборка API
Dockerfile                 Dockerfile для Render из корня
DEPLOYMENT.md              подробная инструкция деплоя
```

## Требования

- Node.js 20+;
- npm;
- .NET 10 SDK;
- Git.

SQLite-сервер отдельно не нужен: используется NuGet-пакет `Microsoft.EntityFrameworkCore.Sqlite`.

```powershell
node --version
npm --version
dotnet --version
```

## Локальный запуск

Backend и frontend запускаются в отдельных терминалах.

### Backend

```powershell
cd "C:\Calibrating trust in an AI reviewer during secure development\backend\TrustCalibrationLab.Api"
dotnet restore
dotnet run
```

API: `http://localhost:5070`

### Frontend

```powershell
cd "C:\Calibrating trust in an AI reviewer during secure development\frontend"
npm install
npm run dev
```

Frontend: `http://localhost:5173`

Vite проксирует `/api/*` на `http://localhost:5070`. При первом запуске backend создаёт SQLite database `trust-calibration-lab.db` и seed data.

## Переменные окружения

Шаблон находится в [frontend/.env.example](./frontend/.env.example).

Для production frontend:

```env
VITE_API_BASE_URL=https://your-api.onrender.com
```

Указывайте origin backend без `/api` и без завершающего slash.

Для backend:

```text
Cors__Origins=https://your-frontend.vercel.app
```

Несколько origin можно указать через запятую.

## API

```text
GET  /api/health
GET  /api/cases
GET  /api/cases/{id}
GET  /api/cases/{id}/files
GET  /api/cases/{id}/files/{fileId}
GET  /api/cases/{id}/comments
```

```text
POST /api/submissions
GET  /api/submissions/{id}
PUT  /api/submissions/{id}/answers/{commentId}
POST /api/submissions/{id}/submit
GET  /api/submissions/{id}/results
```

```text
GET    /api/submissions/{id}/findings
POST   /api/submissions/{id}/findings
DELETE /api/submissions/{id}/findings/{findingId}
```

## Учебный workflow

1. Студент выбирает case.
2. Открывает файл и AI review comment.
3. Переходит к указанной строке кода.
4. Выбирает verdict, confidence и evidence.
5. Пишет объяснение и сохраняет оценку.
6. Проверяет все комментарии и отправляет review.
7. Получает results report.

Правильные ответы находятся только в backend answer key. До submission frontend получает только данные, необходимые для анализа комментария.

Case 3 содержит SQL Injection (CWE-89), IDOR (CWE-639), Path Traversal (CWE-22), Weak Password Hashing (CWE-328/CWE-916) и Hardcoded JWT Secret (CWE-798). IDOR намеренно не сообщается AI reviewer.

## Сборка

```powershell
cd frontend
npm ci
npm run build
```

```powershell
cd backend/TrustCalibrationLab.Api
dotnet restore
dotnet build
```

## Deployment без Railway

Рекомендуемая схема:

```text
GitHub → frontend на Vercel
GitHub → backend на Render
```

1. Запушьте репозиторий в GitHub.
2. В Render создайте Docker Web Service из корня репозитория.
3. Используйте корневой [Dockerfile](./Dockerfile).
4. Укажите health check `/api/health`.
5. В Vercel импортируйте репозиторий с Root Directory `frontend`.
6. Добавьте `VITE_API_BASE_URL` с URL Render API.
7. В Render добавьте `Cors__Origins` с URL Vercel.
8. Сделайте redeploy обоих сервисов.

Полная инструкция с troubleshooting находится в [DEPLOYMENT.md](./DEPLOYMENT.md).

## Ограничения

- Нет authentication, user management и teacher dashboard.
- Anonymous session используется только для прототипа.
- SQLite может быть ephemeral на бесплатных hosting-платформах.
- Нет реального AI integration, Semgrep и Docker execution пользовательского кода.
- LO3 и LO4 курса не реализованы как полноценные assessment activities.

Для production-платформы потребуются PostgreSQL, migrations, authentication, secrets management, audit logging и server-side authorization.

## Лицензия

См. [LICENSE](./LICENSE).
